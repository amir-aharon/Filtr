﻿using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Views.Animations;
using Android.Widget;
using AndroidX.AppCompat.App;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Filtr
{
    [Activity(Label = "LandingPageActivity")]
    public class LandingPageActivity : AppCompatActivity
    {
        #region setup
        Button btnStart;
        View p;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.landing_page);
            p = FindViewById<View>(Resource.Id.landing_page);

            btnStart = p.FindViewById<Button>(Resource.Id.btnGetStarted);
            btnStart.Click += BtnStart_Click;
            
            SetupFonts();
        }
        private void SetupFonts() // setup the fonts for the ui components
        {
            #region Header, Button (Semi-Bold Poppins)
            Typeface tf = Typeface.CreateFromAsset(Assets, "Poppins-SemiBold.ttf");
            TextView text = (TextView)FindViewById(Resource.Id.tvHeader);
            Button btn = (Button)FindViewById(Resource.Id.btnGetStarted);
            text.SetTypeface(tf, TypefaceStyle.Normal);
            btn.SetTypeface(tf, TypefaceStyle.Normal);
            #endregion
            #region Paragraph (Regular Poppins, line spacing 1.5)
            text = (TextView)FindViewById(Resource.Id.tvParagraph);
            tf = Typeface.CreateFromAsset(Assets, "Poppins-Regular.ttf");
            text.SetLineSpacing(0, (float)1.5);
            text.SetTypeface(tf, TypefaceStyle.Normal);
            #endregion

        }
        #endregion

        #region events
        private void BtnStart_Click(object sender, EventArgs e) // navigates to registration page 
        {
            Intent it = new Intent(this, typeof(RegisterActivity));
            StartActivity(it);
        }
        #endregion
    }
}