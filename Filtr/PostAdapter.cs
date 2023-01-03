using Android.App;
using Android.Content;
using Android.Gms.Tasks;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Firebase.Firestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Filtr
{
    public class PostAdapter : BaseAdapter<Post>, IOnSuccessListener
    {
        Context context;
        List<Post> objects;
        List<int> likesCounter;
        List<ImageView> likeIconsDisplays;
        List<TextView> likeCounterDisplays;
        public static string type;
        ImageView fullIcon;
        string queryType;
        public PostAdapter(Context ctx, List<Post> stds)
        {
            this.context = ctx;
            this.objects = stds;
            likesCounter = new List<int>();
            likeIconsDisplays = new List<ImageView>();
            likeCounterDisplays = new List<TextView>();
        }
        public List<Post> GetList()
        {
            return this.objects;
        }
        public override int Count
        {
            get { return this.objects.Count; }
        }
        public override Post this[int position]
        {
            get { return this.objects[position]; }
        }
        public override long GetItemId(int position)
        {
            return position;
        }
        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            if (type.Equals("Home"))
            {
                #region Connect Views
                LayoutInflater layoutInflater = ((HomeActivity)context).LayoutInflater;
                View view = layoutInflater.Inflate(Resource.Layout.post_component_full, parent, false);
                SetupFonts((HomeActivity)context, view);
                TextView tvUser = (TextView)view.FindViewById(Resource.Id.tvUser);
                TextView tvFilter = (TextView)view.FindViewById(Resource.Id.tvFilter);
                FrameLayout btnLike = (FrameLayout)view.FindViewById(Resource.Id.btnLike);
                fullIcon = (ImageView)view.FindViewById(Resource.Id.fullIcon);
                ImageView emptyIcon = (ImageView)view.FindViewById(Resource.Id.emptyIcon);
                ImageView ivContent = (ImageView)view.FindViewById(Resource.Id.ivContent);
                #endregion

                Post temp = objects[position];
                
                if (temp != null)
                {
                    tvUser.Text = "By " + temp.cFname + " " + temp.cLname;
                    tvFilter.Text = "#" + temp.filter;

                    tvFilter.Click += (object sender, EventArgs e) =>
                    {
                        QueryFilter(context, temp.filter);
                    };

                    queryType = "SetupHome";

                    likeIconsDisplays.Add(fullIcon);
                    Live.db.Collection("posts").Document(temp.id).Get().AddOnSuccessListener(this);

                    if (temp.content != null)
                    {
                        ivContent.SetImageBitmap(ImageHelper.Base64ToBitmap(temp.content));
                    }

                    btnLike.Click += (object sender, EventArgs e) =>
                    {
                        ToggleLike(sender, temp);
                    };
                }
                return view;
            }
            else if (type.Equals("Liked"))
            {
                #region Connect Views
                LayoutInflater layoutInflater = ((LikedActvity)context).LayoutInflater;
                View view = layoutInflater.Inflate(Resource.Layout.post_component_full, parent, false);
                SetupFonts((LikedActvity)context, view);
                TextView tvUser = (TextView)view.FindViewById(Resource.Id.tvUser);
                TextView tvFilter = (TextView)view.FindViewById(Resource.Id.tvFilter);
                FrameLayout btnLike = (FrameLayout)view.FindViewById(Resource.Id.btnLike);
                fullIcon = (ImageView)view.FindViewById(Resource.Id.fullIcon);
                ImageView emptyIcon = (ImageView)view.FindViewById(Resource.Id.emptyIcon);
                ImageView ivContent = (ImageView)view.FindViewById(Resource.Id.ivContent);
                #endregion

                Post temp = objects[position];

                if (temp != null)
                {
                    tvUser.Text = "By " + temp.cFname + " " + temp.cLname;
                    tvFilter.Text = "#" + temp.filter;

                    tvFilter.Click += (object sender, EventArgs e) =>
                    {
                        QueryFilter(context, temp.filter);
                    };

                    queryType = "SetupHome";

                    likeIconsDisplays.Add(fullIcon);
                    Live.db.Collection("posts").Document(temp.id).Get().AddOnSuccessListener(this);

                    if (temp.content != null)
                    {
                        ivContent.SetImageBitmap(ImageHelper.Base64ToBitmap(temp.content));
                    }

                    btnLike.Click += (object sender, EventArgs e) =>
                    {
                        ToggleLike(sender, temp);
                    };
                }
                return view;
            }
            else if (type.Equals("Account"))
            {
                #region
                LayoutInflater layoutInflater = ((AccountActivity)context).LayoutInflater;
                View view = layoutInflater.Inflate(Resource.Layout.post_component_self, parent, false);
                SetupFonts((AccountActivity)context, view);
                TextView tvFilter = (TextView)view.FindViewById(Resource.Id.tvFilter);
                ImageView ivContent = (ImageView)view.FindViewById(Resource.Id.ivContent);
                TextView likeCount = (TextView)view.FindViewById(Resource.Id.likeCount);
                #endregion

                Post temp = objects[position];
                if (temp != null)
                {
                    tvFilter.Text = "#" + temp.filter;
                    //likeCount.Text = "" + temp.likedBy.Count + " likes"; 

                    tvFilter.Click += (object sender, EventArgs e) =>
                    {
                        QueryFilter(context, temp.filter);
                    };

                    queryType = "SetupAccount";

                    likeCounterDisplays.Add(likeCount);
                    Live.db.Collection("posts").Document(temp.id).Get().AddOnSuccessListener(this);

                    if (temp.content != null)
                    {
                        ivContent.SetImageBitmap(ImageHelper.Base64ToBitmap(temp.content));
                    }
                }
                return view;
            }
            else if (type.Equals("Search_Filter"))
            {
                #region Connect Views
                LayoutInflater layoutInflater = ((SearchActivity)context).LayoutInflater;
                View view = layoutInflater.Inflate(Resource.Layout.post_component_with_user, parent, false);
                SetupFonts((SearchActivity)context, view);
                TextView tvUser = (TextView)view.FindViewById(Resource.Id.tvUser);
                FrameLayout btnLike = (FrameLayout)view.FindViewById(Resource.Id.btnLike);
                fullIcon = (ImageView)view.FindViewById(Resource.Id.fullIcon);
                ImageView emptyIcon = (ImageView)view.FindViewById(Resource.Id.emptyIcon);
                ImageView ivContent = (ImageView)view.FindViewById(Resource.Id.ivContent);
                #endregion

                Post temp = objects[position];

                if (temp != null)
                {
                    tvUser.Text = "By " + temp.cFname + " " + temp.cLname;

                    queryType = "SetupHome";

                    likeIconsDisplays.Add(fullIcon);
                    Live.db.Collection("posts").Document(temp.id).Get().AddOnSuccessListener(this);

                    if (temp.content != null)
                    {
                        ivContent.SetImageBitmap(ImageHelper.Base64ToBitmap(temp.content));
                    }

                    btnLike.Click += (object sender, EventArgs e) =>
                    {
                        ToggleLike(sender, temp);
                    };
                }
                return view;
            }

            return null;
        }

        private void QueryFilter(Context context, string filter)
        {
            Intent it = new Intent(context, typeof(SearchActivity));
            it.PutExtra("filterQuery", filter);
            context.StartActivity(it);
        }

        private void SetupFonts(Context ctx, View view)
        {
            Typeface tf = Typeface.CreateFromAsset(ctx.Assets, "Poppins-Regular.ttf");
            TextView text = (TextView)view.FindViewById(Resource.Id.tvFilter);
            if (text != null )
                text.SetTypeface(tf, TypefaceStyle.Normal);
            text = (TextView)view.FindViewById(Resource.Id.likeCount);
            if (text != null)
                text.SetTypeface(tf, TypefaceStyle.Normal);
            text = (TextView)view.FindViewById(Resource.Id.tvUser);
            if (text != null)
                text.SetTypeface(tf, TypefaceStyle.Normal);
        }
        private void ToggleLike(object sender, Post post)
        {
            FrameLayout frameLayout = (FrameLayout)sender;
            ImageView fullIcon = (ImageView)frameLayout.GetChildAt(0);

            // unlike pressed
            if (fullIcon.Visibility == ViewStates.Visible)
            {
                AnimaitonHelper.ScaleOut(context, fullIcon);
                queryType = "Unlike";
                Live.db.Collection("posts").Document(post.id).Get().AddOnSuccessListener(this);
            } 
            // like pressed
            else
            {
                AnimaitonHelper.ScaleIn(context, fullIcon);
                queryType = "Like"; 
                Live.db.Collection("posts").Document(post.id).Get().AddOnSuccessListener(this);
            }
        }
        public void OnSuccess(Java.Lang.Object result)
        {
            var snapshot = (DocumentSnapshot)result;

            JavaList likedBy = (JavaList)snapshot.Get("likedBy");

            if (queryType.Equals("Like"))
            {
                likedBy.Add(Live.user.id);
                DocumentReference docRef = Live.db.Collection("posts").Document(snapshot.Id);
                docRef.Update("likedBy", likedBy);
            }
            else if (queryType.Equals("Unlike"))
            {
                likedBy.Remove(Live.user.id);
                DocumentReference docRef = Live.db.Collection("posts").Document(snapshot.Id);
                docRef.Update("likedBy", likedBy);
            }
            else if (queryType.Equals("SetupHome"))
            {
                if (likedBy.Contains(Live.user.id))
                {
                    likesCounter.Add(1);
                }
                else
                {
                    likesCounter.Add(0);
                }
                foreach (var obj in objects)
                {
                    if (obj != null && likesCounter.Count == likeIconsDisplays.Count)
                    {
                        for (int i = 0; i < likesCounter.Count; i++)
                        {
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
            else if (queryType.Equals("SetupAccount"))
            {
                likesCounter.Add(likedBy.Count);

                foreach (var obj in objects)
                {
                    if (obj != null && likesCounter.Count == likeCounterDisplays.Count)
                    {
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
    }
}