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
    [Activity(Label = "AccountActivity")]
    public class AccountActivity : Activity, IOnSuccessListener
    {
        #region setup
        LinearLayout navHome, navSearch, navLiked, navAccount, btnLogOut;
        Button btnPlus;
        View p;
        ListView lv;
        PostAdapter adapter;
        string photoMethod;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.account_page);

            // Create your application here

            p = FindViewById(Resource.Id.account_page);
            SetNavbarButtons();
            
            PostAdapter.type = "Account";
            lv = p.FindViewById<ListView>(Resource.Id.lv);

            btnLogOut = p.FindViewById<LinearLayout>(Resource.Id.btnLogOut);
            btnLogOut.Click += LogOut;

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
        private async void LoadPosts() // calls posts query 
        {
            Live.db.Collection("posts").WhereEqualTo("creator", Live.user.id).Get().AddOnSuccessListener(this);
        }
        public void OnSuccess(Java.Lang.Object result) // executes the query 
        {
            var snapshot = (QuerySnapshot)result;

            List<Post> posts = new List<Post>();

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

            adapter = new PostAdapter(this, posts);
            lv.Adapter = adapter;
        }
        private void LogOut(object sender, EventArgs e) // logs out of the user (session & static data)
        {
            ISharedPreferences sp = this.GetSharedPreferences("details", Android.Content.FileCreationMode.Private);
            var editor = sp.Edit();

            editor.PutString("id", "");
            editor.PutString("email","");
            editor.PutString("password", "");
            editor.PutString("Fname", "");
            editor.PutString("Lname", "");
            editor.Commit();

            Intent it = new Intent(this, typeof(RegisterActivity));
            StartActivity(it);
            Live.user = null;
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