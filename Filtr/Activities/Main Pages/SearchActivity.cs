﻿using Android.App;
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
        LinearLayout navHome, navAccount, navLiked, menuUsers, navSearch;
        FlexboxLayout btnSearch;
        PostAdapter adapter;
        ListView lv;
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

            PostAdapter.type = "Search_Filter";
            lv = (ListView)p.FindViewById(Resource.Id.lv);
            lv.Visibility = ViewStates.Invisible;

            btnSearch.Click += BtnSearch_Click;

            string query = Intent.GetStringExtra("filterQuery");
            if (query != null)
            {
                queryType = "Filters_" + query;
                Query q = Live.db.Collection("posts").WhereEqualTo("filter", query);
                q.Get().AddOnSuccessListener(this);
            }
        }
        #region Dialog
        private void BtnSearch_Click(object sender, EventArgs e)
        {
            PopupMenu menu = new PopupMenu(this, btnSearch);
            menu.MenuItemClick += Menu_MenuItemClick;
            menu.Menu.Add("NoFilter");
            menu.Menu.Add("Monochrome");
            menu.Menu.Add("Pixelate");
            menu.Gravity = GravityFlags.Center;
            menu.Show();

            //var mainDialog = new Dialog(this);
            //mainDialog.SetContentView(Resource.Layout.search_options_dialog);
            //mainDialog.SetTitle("select");
            //mainDialog.SetCancelable(true);

            //LinearLayout btnByUser = (LinearLayout)mainDialog.FindViewById(Resource.Id.btnByUser);
            //LinearLayout btnByFilter = (LinearLayout)mainDialog.FindViewById(Resource.Id.btnByFilter);

            //Typeface tf = Typeface.CreateFromAsset(Assets, "Poppins-Regular.ttf");
            //TextView tv = (TextView)mainDialog.FindViewById(Resource.Id.tvByFilter);
            //tv.SetTypeface(tf, TypefaceStyle.Normal);
            //tv = (TextView)mainDialog.FindViewById(Resource.Id.tvByUser);
            //tv.SetTypeface(tf, TypefaceStyle.Normal);

            //btnByUser.Click += BtnByUser_Click;
            //btnByFilter.Click += BtnByFilter_Click;

            //mainDialog.Show();
        }

        private void Menu_MenuItemClick(object sender, PopupMenu.MenuItemClickEventArgs e)
        {
            string filter = e.Item.ToString();
            queryType = "Filters_" + filter;
            Query q = Live.db.Collection("posts").WhereEqualTo("filter", filter);
            q.Get().AddOnSuccessListener(this);
        }
        //private void BtnByFilter_Click(object sender, EventArgs e)
        //{
        //    Toast.MakeText(this, "Filter", ToastLength.Short).Show();

        //    var filterDialog = new Dialog(this);
        //    filterDialog.SetContentView(Resource.Layout.filters_menu_dialog);
        //    LinearLayout menuFilters = (LinearLayout)filterDialog.FindViewById(Resource.Id.menu);
        //    menuFilters.Click += MenuFilters_Click;
        //}

        //private void MenuFilters_Click(object sender, EventArgs e)
        //{
        //    // TODO: menu
        //}

        //private void BtnByUser_Click(object sender, EventArgs e)
        //{
        //    Toast.MakeText(this, "User", ToastLength.Short).Show();

        //    var userDialog = new Dialog(this);
        //    userDialog.SetContentView(Resource.Layout.users_menu_dialog);
        //    menuUsers = (LinearLayout)userDialog.FindViewById(Resource.Id.menu);
        //    menuUsers.Click += MenuUsers_Click;
        //    userDialog.Show();
        //}

        //private void MenuUsers_Click(object sender, EventArgs e)
        //{
        //    queryType = "Users";
        //    Live.db.Collection("users").Get().AddOnSuccessListener(this);
        //}
        #endregion

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
            else if (queryType.Equals("Filters_NoFilter"))
            {
                var snapshot = (QuerySnapshot)result;
                List<Post> posts = new List<Post>();
                foreach (var post in snapshot.Documents)
                {
                    if (post.GetString("creator") != Live.user.id)
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
                }
                adapter = new PostAdapter(this, posts);
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




        private void SetNavbarButtons()
        {
            navHome = (LinearLayout)p.FindViewById(Resource.Id.navHome);
            navHome.Click += NavHome_Click;
            navAccount = (LinearLayout)p.FindViewById(Resource.Id.navAccount);
            navAccount.Click += NavAccount_Click;
            navLiked = (LinearLayout)p.FindViewById(Resource.Id.navLiked);
            navLiked.Click += NavLiked_Click;
            navSearch = (LinearLayout)p.FindViewById(Resource.Id.navSearch);
            navSearch.Click += NavSearch_Click;
        }
        private void NavSearch_Click(object sender, EventArgs e)
        {
            NavbarHelper.SearchButton(this);
        }

        private void NavLiked_Click(object sender, EventArgs e)
        {
            NavbarHelper.LikedButton(this);
        }
        private void NavAccount_Click(object sender, EventArgs e)
        {
            NavbarHelper.AccountButton(this);
        }
        private void NavHome_Click(object sender, EventArgs e)
        {
            NavbarHelper.HomeButton(this);
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
            #region Header, Button (Semi-Bold Poppins)
            tf = Typeface.CreateFromAsset(Assets, "Poppins-SemiBold.ttf");
            text = (TextView)p.FindViewById(Resource.Id.tvSearchBar);
            text.SetTypeface(tf, TypefaceStyle.Normal);
            #endregion
        }
    }

}