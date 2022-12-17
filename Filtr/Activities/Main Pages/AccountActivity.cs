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
        LinearLayout navHome, navSearch, navLiked;
        View p;
        ListView lv;
        PostAdapter adapter;
        public override void OnBackPressed()
        {
            return;
        }
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.account_page);

            // Create your application here

            p = FindViewById(Resource.Id.account_page);
            SetNavbarButtons();
            
            PostAdapter.type = "Account";
            lv = FindViewById<ListView>(Resource.Id.lv);

            ListViewExampleAsync();

            SetupFonts();
        }

        
        private void SetNavbarButtons()
        {
            navHome = (LinearLayout)p.FindViewById(Resource.Id.navHome);
            navHome.Click += NavHome_Click;
            navSearch = (LinearLayout)p.FindViewById(Resource.Id.navSearch);
            navSearch.Click += NavSearch_Click;
            navLiked = (LinearLayout)p.FindViewById(Resource.Id.navLiked);
        }
        private void NavHome_Click(object sender, EventArgs e)
        {
            NavbarHelper.HomeButton(this);
        }
        private void NavSearch_Click(object sender, EventArgs e)
        {
            NavbarHelper.SearchButton(this);
        }
        private void SetupFonts()
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
        #endregion
        #region querys
        private async void ListViewExampleAsync()
        {
            Live.db.Collection("posts").WhereEqualTo("creator", Live.user.id).Get()
                .AddOnSuccessListener(this);
        }
        public void OnSuccess(Java.Lang.Object result)
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
        #endregion
    }
}