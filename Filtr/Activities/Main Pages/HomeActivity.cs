using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Filtr
{
    [Activity(Label = "HomeActivity")]
    public class HomeActivity : Activity
    {
        LinearLayout navHome, navSearch, navAccount, navLiked;
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
            SetContentView(Resource.Layout.home_page);

            // Create your application here
            
            p = FindViewById(Resource.Id.home_page);
            SetNavbarButtons();

            PostAdapter.type = "Home";
            lv = FindViewById<ListView>(Resource.Id.lv);

            ListViewExample();

            SetupFonts();
        }
        public void ListViewExample()
        {
            User u1 = new User("1", "aaa@aaa.aaa", "aaaaaaaa1", "Amir", "Aharon");
            Filter f1 = PixelFilter.filter;

            List<Post> posts = new List<Post>();
            posts.Add(new Post(u1, f1));
            posts.Add(new Post(u1));
            posts.Add(new Post(u1, f1));

            adapter = new PostAdapter(this, posts);
            lv.Adapter = adapter;
        }
        private void SetNavbarButtons()
        {
            navHome = (LinearLayout)p.FindViewById(Resource.Id.navHome);
            navSearch = (LinearLayout)p.FindViewById(Resource.Id.navSearch);
            navSearch.Click += NavSearch_Click;
            navAccount = (LinearLayout)p.FindViewById(Resource.Id.navAccount);
            navAccount.Click += NavAccount_Click;
            navLiked = (LinearLayout)p.FindViewById(Resource.Id.navLiked);
        }

        private void NavAccount_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
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
    }
}