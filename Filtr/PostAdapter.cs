using Android.App;
using Android.Content;
using Android.Gms.Tasks;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Firebase.Firestore;
using Firebase.Firestore.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Filtr
{
    public class PostAdapter : BaseAdapter<Post>, IOnSuccessListener
    {
        #region setup
        Context context; // caller activity
        List<Post> objects; // list to show in view
        public static string type; // tells which type of view needs to be shown

        List<int> likesCounter; // saves likes status
        List<ImageView> likeIconsDisplays; // saves like buttons by order (for setupLike query)
        List<TextView> likeCounterDisplays; // saves like counters by order (for setupAccount query)
        string queryType; // flag the type of DB query

        public PostAdapter(Context ctx, List<Post> posts) // constructor 
        {
            this.context = ctx;
            this.objects = posts;
            likesCounter = new List<int>();
            likeIconsDisplays = new List<ImageView>();
            likeCounterDisplays = new List<TextView>();
        }
        public List<Post> GetList() // returns the posts list
        {
            return objects;
        }
        public override int Count // returns the count of the posts list 
        {
            get { return this.objects.Count; }
        }
        public override Post this[int pos] // return the post in the index "pos"
        {
            get { return this.objects[pos]; }
        }
        public override long GetItemId(int pos) // return the item id (as a view) in the index "pos" 
        {
            return pos;
        }
        public override View GetView(int position, View convertView, ViewGroup parent) // create a view out of xml & data 
        {
            // home page post view
            if (type.Equals("Home"))
            {
                #region connect Views
                LayoutInflater layoutInflater = ((HomeActivity)context).LayoutInflater;
                View view = layoutInflater.Inflate(Resource.Layout.post_component_full, parent, false);
                SetupFonts((HomeActivity)context, view);
                TextView tvUser = (TextView)view.FindViewById(Resource.Id.tvUser);
                TextView tvFilter = (TextView)view.FindViewById(Resource.Id.tvFilter);
                FrameLayout btnLike = (FrameLayout)view.FindViewById(Resource.Id.btnLike);
                ImageView fullIcon = (ImageView)view.FindViewById(Resource.Id.fullIcon);
                ImageView emptyIcon = (ImageView)view.FindViewById(Resource.Id.emptyIcon);
                ImageView ivContent = (ImageView)view.FindViewById(Resource.Id.ivContent);
                #endregion

                // get post
                Post temp = objects[position];

                // if post exists
                if (temp != null)
                {
                    // show user and filter in post view
                    tvUser.Text = "By " + temp.cFname + " " + temp.cLname;
                    tvFilter.Text = "#" + temp.filter;

                    // query the post's filter when clicked
                    tvFilter.Click += (object sender, EventArgs e) =>
                    {
                        QueryFilter(context, temp.filter);
                    };

                    // query the post's user when clicked
                    tvUser.Click += (object sender, EventArgs e) =>
                    {
                        QueryUser(context, temp.creator);
                    };

                    // setup the like button visibility (check if post is liked by live user or not)
                    queryType = "SetupLike";
                    likeIconsDisplays.Add(fullIcon);
                    Live.db.Collection("posts").Document(temp.id).Get().AddOnSuccessListener(this);

                    // if there's an image to show
                    if (temp.content != null)
                    {
                        // display post's image
                        ivContent.SetImageBitmap(ImageHelper.Base64ToBitmap(temp.content));
                    }

                    // toggle like when clicked
                    btnLike.Click += (object sender, EventArgs e) =>
                    {
                        ToggleLike(sender, temp);
                    };
                }
                return view;
            }

            // liked page post view
            else if (type.Equals("Liked"))
            {
                #region connect Views
                LayoutInflater layoutInflater = ((LikedActvity)context).LayoutInflater;
                View view = layoutInflater.Inflate(Resource.Layout.post_component_full, parent, false);
                SetupFonts((LikedActvity)context, view);
                TextView tvUser = (TextView)view.FindViewById(Resource.Id.tvUser);
                TextView tvFilter = (TextView)view.FindViewById(Resource.Id.tvFilter);
                FrameLayout btnLike = (FrameLayout)view.FindViewById(Resource.Id.btnLike);
                ImageView fullIcon = (ImageView)view.FindViewById(Resource.Id.fullIcon);
                ImageView emptyIcon = (ImageView)view.FindViewById(Resource.Id.emptyIcon);
                ImageView ivContent = (ImageView)view.FindViewById(Resource.Id.ivContent);
                #endregion
                // get post
                Post temp = objects[position];

                // if post exists
                if (temp != null)
                {
                    // show user and filter in post view
                    tvUser.Text = "By " + temp.cFname + " " + temp.cLname;
                    tvFilter.Text = "#" + temp.filter;

                    // query the post's filter when clicked
                    tvFilter.Click += (object sender, EventArgs e) =>
                    {
                        QueryFilter(context, temp.filter);
                    };

                    // query the post's user when clicked
                    tvUser.Click += (object sender, EventArgs e) =>
                    {
                        QueryUser(context, temp.creator);
                    };

                    // setup the like button visibility (check if post is liked by live user or not)
                    queryType = "SetupLike";
                    likeIconsDisplays.Add(fullIcon);
                    Live.db.Collection("posts").Document(temp.id).Get().AddOnSuccessListener(this);

                    // if there's an image to show
                    if (temp.content != null)
                    {
                        // display post's image
                        ivContent.SetImageBitmap(ImageHelper.Base64ToBitmap(temp.content));
                    }

                    // toggle like when clicked
                    btnLike.Click += (object sender, EventArgs e) =>
                    {
                        ToggleLike(sender, temp);
                    };
                }
                return view;
            }

            // own post view
            else if (type.Equals("Account"))
            {
                #region connect views
                LayoutInflater layoutInflater = ((AccountActivity)context).LayoutInflater;
                View view = layoutInflater.Inflate(Resource.Layout.post_component_self, parent, false);
                SetupFonts((AccountActivity)context, view);
                TextView tvFilter = (TextView)view.FindViewById(Resource.Id.tvFilter);
                ImageView ivContent = (ImageView)view.FindViewById(Resource.Id.ivContent);
                TextView likeCount = (TextView)view.FindViewById(Resource.Id.likeCount);
                #endregion

                // get post
                Post temp = objects[position];
                
                // if post exists
                if (temp != null)
                {
                    // show filter in post view
                    tvFilter.Text = "#" + temp.filter;
                    //likeCount.Text = "" + temp.likedBy.Count + " likes"; 

                    // query the post's filter when clicked
                    tvFilter.Click += (object sender, EventArgs e) =>
                    {
                        QueryFilter(context, temp.filter);
                    };

                    // setup the like count visibility (check how much likes the post has)
                    queryType = "SetupAccount";
                    likeCounterDisplays.Add(likeCount);
                    Live.db.Collection("posts").Document(temp.id).Get().AddOnSuccessListener(this);

                    // if there's an image to show
                    if (temp.content != null)
                    {
                        ivContent.SetImageBitmap(ImageHelper.Base64ToBitmap(temp.content));
                    }
                }
                return view;
            }

            // searched by filter post view (show only creator)
            else if (type.Equals("Search_Filter"))
            {
                #region Connect Views
                LayoutInflater layoutInflater = ((SearchActivity)context).LayoutInflater;
                View view = layoutInflater.Inflate(Resource.Layout.post_component_with_user, parent, false);
                SetupFonts((SearchActivity)context, view);
                TextView tvUser = (TextView)view.FindViewById(Resource.Id.tvUser);
                FrameLayout btnLike = (FrameLayout)view.FindViewById(Resource.Id.btnLike);
                ImageView fullIcon = (ImageView)view.FindViewById(Resource.Id.fullIcon);
                ImageView emptyIcon = (ImageView)view.FindViewById(Resource.Id.emptyIcon);
                ImageView ivContent = (ImageView)view.FindViewById(Resource.Id.ivContent);
                #endregion

                // get post
                Post temp = objects[position];

                // if post exists
                if (temp != null)
                {
                    // show creator in post view
                    tvUser.Text = "By " + temp.cFname + " " + temp.cLname;

                    // query the post's creator when clicked
                    tvUser.Click += (object sender, EventArgs e) =>
                    {
                        QueryUser(context, temp.creator);
                    };

                    // setup the like button visibility (check if post is liked by live user or not)
                    queryType = "SetupLike";
                    likeIconsDisplays.Add(fullIcon);
                    Live.db.Collection("posts").Document(temp.id).Get().AddOnSuccessListener(this);

                    // if there's an image to show
                    if (temp.content != null)
                    {
                        ivContent.SetImageBitmap(ImageHelper.Base64ToBitmap(temp.content));
                    }

                    // toggle like when clicked
                    btnLike.Click += (object sender, EventArgs e) =>
                    {
                        ToggleLike(sender, temp);
                    };
                }
                return view;
            }

            // searched by user post view (show only filter)
            else if (type.Equals("Search_User"))
            {
                #region Connect Views
                LayoutInflater layoutInflater = ((SearchActivity)context).LayoutInflater;
                View view = layoutInflater.Inflate(Resource.Layout.post_component_with_filter, parent, false);
                SetupFonts((SearchActivity)context, view);
                TextView tvFilter = (TextView)view.FindViewById(Resource.Id.tvFilter);
                FrameLayout btnLike = (FrameLayout)view.FindViewById(Resource.Id.btnLike);
                ImageView fullIcon = (ImageView)view.FindViewById(Resource.Id.fullIcon);
                ImageView emptyIcon = (ImageView)view.FindViewById(Resource.Id.emptyIcon);
                ImageView ivContent = (ImageView)view.FindViewById(Resource.Id.ivContent);
                #endregion

                // get post
                Post temp = objects[position];

                // if post exists
                if (temp != null)
                {
                    // show filter in post view
                    tvFilter.Text = "#" + temp.filter;

                    // query the post's filter when clicked
                    tvFilter.Click += (object sender, EventArgs e) =>
                    {
                        QueryFilter(context, temp.filter);
                    };

                    // setup the like button visibility (check if post is liked by live user or not)
                    queryType = "SetupLike";
                    likeIconsDisplays.Add(fullIcon);
                    Live.db.Collection("posts").Document(temp.id).Get().AddOnSuccessListener(this);

                    // id there's an image to show
                    if (temp.content != null)
                    {
                        ivContent.SetImageBitmap(ImageHelper.Base64ToBitmap(temp.content));
                    }

                    // toggle like when clicked
                    btnLike.Click += (object sender, EventArgs e) =>
                    {
                        ToggleLike(sender, temp);
                    };
                }
                return view;
            }

            return null;
        }
        #endregion

        #region functions
        private void QueryFilter(Context context, string filter) // sends intent to the search page, with a query of a certain filter 
        {
            Intent it = new Intent(context, typeof(SearchActivity));
            it.PutExtra("filtersQuery", filter); // pass the searched filter
            it.PutExtra("isQuery", true); // declare query's existance
            it.PutExtra("isFilterQueried", true); // declare query's type: filter
            context.StartActivity(it);
        }
        private void QueryUser(Context context, string uid) // sends intent to the search page, with a query of a certain user 
        {
            Intent it = new Intent(context, typeof(SearchActivity));
            it.PutExtra("usersQuery", uid); // pass the searched user
            it.PutExtra("isQuery", true); // declare query's existance
            it.PutExtra("isUserQueried", true); // declare query's type: user
            context.StartActivity(it);
        }
        private void SetupFonts(Context ctx, View view) // sets the fonts of the post's components 
        {
            Typeface tf = Typeface.CreateFromAsset(ctx.Assets, "Poppins-Regular.ttf");
            TextView text = (TextView)view.FindViewById(Resource.Id.tvFilter);
            if (text != null)
                text.SetTypeface(tf, TypefaceStyle.Normal);
            text = (TextView)view.FindViewById(Resource.Id.likeCount);
            if (text != null)
                text.SetTypeface(tf, TypefaceStyle.Normal);
            text = (TextView)view.FindViewById(Resource.Id.tvUser);
            if (text != null)
                text.SetTypeface(tf, TypefaceStyle.Normal);
        }
        private void ToggleLike(object sender, Post post) // toggles on/off the like button (and update the DB)
        {
            // point to the clicked button and get reference to the like image
            FrameLayout frameLayout = (FrameLayout)sender;
            ImageView fullIcon = (ImageView)frameLayout.GetChildAt(0);

            // if unlike pressed
            if (fullIcon.Visibility == ViewStates.Visible)
            {
                // hide like image
                AnimaitonHelper.ScaleOut(context, fullIcon);

                // update unlike in the DB
                queryType = "Unlike";
                Live.db.Collection("posts").Document(post.id).Get().AddOnSuccessListener(this);
            }
            // if like pressed
            else
            {
                // show like image
                AnimaitonHelper.ScaleIn(context, fullIcon);

                // update like in DB
                queryType = "Like";
                Live.db.Collection("posts").Document(post.id).Get().AddOnSuccessListener(this);

                // inform creator about new likes
                DocumentReference docRef = Live.db.Collection("users").Document(post.creator);
                docRef.Update("newLikes", true);
            }
        }
        #endregion

        #region database
        public void OnSuccess(Java.Lang.Object result) // executes any interaction with queries from DB
        {
            // process result (a single post document)
            var snapshot = (DocumentSnapshot)result;

            // get the likedby list of the post
            JavaList likedBy = (JavaList)snapshot.Get("likedBy");

            // if post was liked
            if (queryType.Equals("Like"))
            {
                // add live user to likedby list
                likedBy.Add(Live.user.id);

                // update list to DB
                DocumentReference docRef = Live.db.Collection("posts").Document(snapshot.Id);
                docRef.Update("likedBy", likedBy);
            }

            // if post was unliked
            else if (queryType.Equals("Unlike"))
            {
                // remove live user from likedby list
                likedBy.Remove(Live.user.id);

                // update list to DB
                DocumentReference docRef = Live.db.Collection("posts").Document(snapshot.Id);
                docRef.Update("likedBy", likedBy);
            }

            // set up the like button visibility for a post
            else if (queryType.Equals("SetupLike"))
            {
                // because of the delay of working with the cloud, we can't immediately
                // update post state from DB individually while looking on a list.

                // SOLUTION: creating a binary - value list, which represents the likes
                // states in the post, in the same order of the posts themselves
                // create list by order, and when the list is complete -> update the whole listview

                if (likedBy.Contains(Live.user.id))
                {
                    // signal that the post was liked by live user
                    likesCounter.Add(1);
                }
                else
                {
                    // signal that the post wasn't liked by live user
                    likesCounter.Add(0);
                }

                // iterate through all of the posts
                foreach (Post obj in objects)
                {
                    //  if the post exists & the representing list was completed
                    if (obj != null && likesCounter.Count == likeIconsDisplays.Count)
                    {
                        // set view states of the like buttons
                        for (int i = 0; i < likesCounter.Count; i++)
                        {
                            // set like buttons visibility according to state
                            if (likeIconsDisplays[i] != null)
                            {
                                likeIconsDisplays[i].Visibility =
                                    likesCounter[i] == 1
                                    ? ViewStates.Visible
                                    : ViewStates.Invisible;
                            }
                        }
                    }
                }

            }

            // set up the like counter for a post in account page
            else if (queryType.Equals("SetupAccount"))
            {
                // because of the delay of working with the cloud, we can't immediately
                // update post state from DB individually while looking on a list.

                // SOLUTION: creating a list, which represents the count of likes
                // in the post, in the same order of the posts themselves
                // create list by order, and when the list is complete -> update the whole listview

                // add likes count by order
                likesCounter.Add(likedBy.Count);

                // iterate through all of the posts
                foreach (var obj in objects)
                {
                    //  if the post exists & the representing list was completed
                    if (obj != null && likesCounter.Count == likeCounterDisplays.Count)
                    {
                        // set like counters numbers according to state
                        for (int i = 0; i < likesCounter.Count; i++)
                        {
                            if (likeCounterDisplays[i] != null)
                            {
                                likeCounterDisplays[i].Text = likesCounter[i] + " likes";
                            }
                        }
                    }
                }
            }
        }
        #endregion
    }
}