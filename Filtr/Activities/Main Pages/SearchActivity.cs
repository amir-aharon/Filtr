using Android.App;
using Android.Content;
using Android.Gms.Tasks;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Com.Google.Android.Flexbox;
using Firebase.Firestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Filtr
{
    [Activity(Label = "SearchActivity")]
    public class SearchActivity : Activity, IOnSuccessListener
    {
        #region setup
        LinearLayout navHome, navAccount, navLiked, menuUsers, navSearch;
        Button btnPlus;
        FlexboxLayout btnSearch;
        PostAdapter adapter;
        ListView lv;
        //string photoMethod;
        View p;
        public static string queryType;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.search_page);

            p = FindViewById(Resource.Id.search_page);
            SetNavbarButtons();
            SetupFonts();

            btnSearch = (FlexboxLayout)p.FindViewById(Resource.Id.btnSearch);


            // prepare listview for later use
            lv = (ListView)p.FindViewById(Resource.Id.lv);
            lv.Visibility = ViewStates.Invisible;

            btnSearch.Click += OnSearch;

            // check if there's a need to show search results
            bool isFilterQueried = Intent.GetBooleanExtra("isFilterQueried", false);
            bool isUserQueried = Intent.GetBooleanExtra("isUserQueried", false);
            bool isQuery = Intent.GetBooleanExtra("isQuery", false);
            Query q;

            // if there was a query
            if (isQuery)
            {
                // check if it was for a filter
                if (isFilterQueried)
                {
                    queryType = "Filters";
                    Live.db.Collection("posts").WhereEqualTo("filter", Intent.GetStringExtra("filtersQuery")).Get().AddOnSuccessListener(this);
                }
                // check if it was for a user
                else if (isUserQueried)
                {
                    queryType = "Users";
                    Live.db.Collection("posts").WhereEqualTo("creator", Intent.GetStringExtra("usersQuery")).Get().AddOnSuccessListener(this);
                }
            }

            Intent.PutExtra("isFilterQueried", false);
            Intent.PutExtra("isUserQueried", false);
            Intent.PutExtra("isQuery", false);
        }
        public override void OnBackPressed() // prevent jumping to previous page on back press
        {
            return;
        }
        private void SetupFonts() // sets the fonts of the ui components
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
            #region Header, Button (Semi-Bold Poppins)
            tf = Typeface.CreateFromAsset(Assets, "Poppins-SemiBold.ttf");
            text = (TextView)p.FindViewById(Resource.Id.tvSearchBar);
            text.SetTypeface(tf, TypefaceStyle.Normal);
            #endregion
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

        #region search action
        private void OnSearch(object sender, EventArgs e)
        {
            PopupMenu menu = new PopupMenu(this, btnSearch);
            menu.MenuItemClick += MenuItemClick;
            menu.Menu.Add("NoFilter");
            menu.Menu.Add("Monochrome");
            menu.Menu.Add("Pixelate");
            menu.Gravity = GravityFlags.Center;
            menu.Show();
        }
        private void MenuItemClick(object sender, PopupMenu.MenuItemClickEventArgs e)
        {
            string filter = e.Item.ToString().ToLower();
            queryType = "Filters";
            Query q = Live.db.Collection("posts").WhereEqualTo("filter", filter);
            q.Get().AddOnSuccessListener(this);
        }
        public void OnSuccess(Java.Lang.Object result)
        {
            if (queryType.Equals("Users"))
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
                PostAdapter.type = "Search_User";
                lv.Adapter = adapter;

                lv.Visibility = ViewStates.Visible;
                FlexboxLayout llEntrance = (FlexboxLayout)p.FindViewById(Resource.Id.llEntrance);
                llEntrance.Visibility = ViewStates.Invisible;
                FlexboxLayout topBar = (FlexboxLayout)p.FindViewById(Resource.Id.topBar);
                topBar.Visibility = ViewStates.Visible;
                btnSearch.Visibility = ViewStates.Gone;

                TextView tvTopBar = (TextView)p.FindViewById(Resource.Id.tvTopBar);
                tvTopBar.Text = "By " + posts[0].cFname + " " + posts[0].cLname;
                tvTopBar.SetTextColor(Color.ParseColor("#4ecdc4"));
                Typeface tf = Typeface.CreateFromAsset(Assets, "Poppins-SemiBold.ttf");
                tvTopBar.SetTypeface(tf, TypefaceStyle.Normal);
            }
            else if (queryType.Equals("Filters"))
            {
                var snapshot = (QuerySnapshot)result;

                if (snapshot.Documents.Count == 0)
                {
                    Toast.MakeText(this, "There aren't posts with this filter", ToastLength.Short).Show();
                    return;
                }

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
                PostAdapter.type = "Search_Filter";
                lv.Adapter = adapter;

                lv.Visibility = ViewStates.Visible;
                FlexboxLayout llEntrance = (FlexboxLayout)p.FindViewById(Resource.Id.llEntrance);
                llEntrance.Visibility = ViewStates.Invisible;
                FlexboxLayout topBar = (FlexboxLayout)p.FindViewById(Resource.Id.topBar);
                topBar.Visibility = ViewStates.Visible;
                btnSearch.Visibility = ViewStates.Gone;

                TextView tvTopBar = (TextView)p.FindViewById(Resource.Id.tvTopBar);
                tvTopBar.Text = "#" + posts[0].filter;
                tvTopBar.SetTextColor(Color.ParseColor("#FFE66D"));
                Typeface tf = Typeface.CreateFromAsset(Assets, "Poppins-SemiBold.ttf");
                tvTopBar.SetTypeface(tf, TypefaceStyle.Normal);
            }
        }
        #endregion
    }

}