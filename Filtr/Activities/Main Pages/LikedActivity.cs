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

namespace Filtr
{
    [Activity(Label = "LikedActivity")]
    public class LikedActvity : Activity, IOnSuccessListener
    {
        #region setup
        LinearLayout navHome, navSearch, navAccount, navLiked;
        Button btnPlus;
        View p;
        ListView lv;
        PostAdapter adapter;
        //string photoMethod;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.liked_page);

            // Create your application here

            p = FindViewById(Resource.Id.liked_page);
            SetNavbarButtons();

            // signs which type of post view to display
            PostAdapter.type = "Liked";
            lv = p.FindViewById<ListView>(Resource.Id.lv);

            LoadPosts();
            SetupFonts();
        }
        public override void OnBackPressed() // prevent jumping to previous page on back press 
        {
            return;
        }
        private void SetupFonts() // set up fonts for the ui components 
        {
            #region textfields, subhead, Footer text (Regular Poppins)
            Typeface tf = Typeface.CreateFromAsset(Assets, "Poppins-Regular.ttf");
            TextView text = (TextView)p.FindViewById(Resource.Id.homeText);
            text.SetTypeface(tf, TypefaceStyle.Normal);
            text = (TextView)p.FindViewById(Resource.Id.searchText);
            text.SetTypeface(tf, TypefaceStyle.Normal);
            text = (TextView)p.FindViewById(Resource.Id.likedText);
            text.SetTypeface(tf, TypefaceStyle.Normal);
            text = (TextView)p.FindViewById(Resource.Id.accountText);
            text.SetTypeface(tf, TypefaceStyle.Normal);

            #endregion
        }
        public void LoadPosts() // calls posts query 
        {
            Live.db.Collection("posts").WhereArrayContains("likedBy", Live.user.id).Get().AddOnSuccessListener(this);
        }
        public void OnSuccess(Java.Lang.Object result) // executes the query 
        {
            // process result
            var snapshot = (QuerySnapshot)result;

            // create new list to set later in listview
            List<Post> posts = new List<Post>();

            // fill the list
            foreach (var post in snapshot.Documents)
            {
                posts.Add(new Post(
                    post.Id,
                    post.GetString("creator"),
                    post.GetString("content"),
                    post.GetString("filter"),
                    post.GetString("c_Fname"),
                    post.GetString("c_Lname")
                    ));
            }

            // set data in listview
            adapter = new PostAdapter(this, posts);
            lv.Adapter = adapter;
        }
        #endregion

        #region navbar
        private void SetNavbarButtons() // connect navigation bar buttons 
        {
            navSearch = (LinearLayout)p.FindViewById(Resource.Id.navSearch);
            navSearch.Click += ToSearchPage;
            navAccount = (LinearLayout)p.FindViewById(Resource.Id.navAccount);
            navAccount.Click += ToAccountPage;
            navLiked = (LinearLayout)p.FindViewById(Resource.Id.navLiked);
            navLiked.Click += ToLikedPage;
            btnPlus = (Button)p.FindViewById(Resource.Id.btnPlus);
            btnPlus.Click += AddPost;
            navHome = (LinearLayout)p.FindViewById(Resource.Id.navHome);
            navHome.Click += ToHomePage;
        }
        private void AddPost(object sender, EventArgs e) // open new post dialog 
        {
            // init take a photo dialog
            Dialog d = new Dialog(this);
            d.SetContentView(Resource.Layout.take_a_photo_dialog);

            #region setup fonts
            TextView tvCamera, tvGallery;
            Typeface tf = Typeface.CreateFromAsset(Assets, "Poppins-Regular.ttf");

            tvCamera = (TextView)d.FindViewById(Resource.Id.tvCamera);
            tvGallery = (TextView)d.FindViewById(Resource.Id.tvGallery);

            tvCamera.SetTypeface(tf, TypefaceStyle.Normal);
            tvGallery.SetTypeface(tf, TypefaceStyle.Normal);
            #endregion

            // connect components
            LinearLayout llCamera, llGallery;
            llCamera = (LinearLayout)d.FindViewById(Resource.Id.llCamera);
            llGallery = (LinearLayout)d.FindViewById(Resource.Id.llGallery);

            // handle events
            llCamera.Click += ChooseCamera;
            llGallery.Click += ChooseGallery;

            // show dialog
            d.Show();
        }
        private void ToHomePage(object sender, EventArgs e) // navigate to home page
        {
            NavbarHelper.HomeButton(this);
        }
        private void ToLikedPage(object sender, EventArgs e) // navigate to liked posts page
        {
            NavbarHelper.LikedButton(this);
        }
        private void ToAccountPage(object sender, EventArgs e) // navigate to account page 
        {
            NavbarHelper.AccountButton(this);
        }
        private void ToSearchPage(object sender, EventArgs e) // navigate to search page 
        {
            NavbarHelper.SearchButton(this);
        }
        #endregion   

        #region on dialog
        private void ChooseGallery(object sender, EventArgs e) // when user chooses gallery 
        {
            // flag for creator page
            //photoMethod = "Gallery";

            // set intent to creator page
            Intent = new Intent(this, typeof(CreatorActivity));

            // inform action type
            Intent.PutExtra("action", "gallery");

            // navigate
            StartActivity(Intent);
        }
        private void ChooseCamera(object sender, EventArgs e) // when user chooses camera 
        {
            // flag for creator page
            //photoMethod = "Camera";

            // set intent to creator page
            Intent = new Intent(this, typeof(CreatorActivity));

            // inform action type
            Intent.PutExtra("action", "camera");

            // navigate
            StartActivity(Intent);
        }
        #endregion
    }
}