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
    [Activity(Label = "SearchActivity")]
    public class SearchActivity : Activity
    {
        LinearLayout navHome, navSearch, navAccount, navLiked;
        View p;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.search_page);

            p = FindViewById(Resource.Id.search_page);
            SetNavbarButtons();
            SetupFonts();
        }
        private void SetNavbarButtons()
        {
            navHome = (LinearLayout)p.FindViewById(Resource.Id.navHome);
            navHome.Click += NavHome_Click;
            navAccount = (LinearLayout)p.FindViewById(Resource.Id.navAccount);
            navAccount.Click += NavAccount_Click;
            navLiked = (LinearLayout)p.FindViewById(Resource.Id.navLiked);
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