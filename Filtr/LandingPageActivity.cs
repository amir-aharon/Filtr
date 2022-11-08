using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Filtr
{
    [Activity(Label = "LandingPageActivity")]
    public class LandingPageActivity : AppCompatActivity
    {
        Button btnStart;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.landing_page);
            SetupFonts();

            btnStart = FindViewById<Button>(Resource.Id.btnLandingPageGetStarted);
            btnStart.Click += BtnStart_Click;
        }

        private void BtnStart_Click(object sender, EventArgs e)
        {
            Intent it = new Intent(this, typeof(RegisterActivity));
            StartActivity(it);
        }

        private void SetupFonts()
        {
            #region Header, Button (Semi-Bold Poppins)
            Typeface tf = Typeface.CreateFromAsset(Assets, "Poppins-SemiBold.ttf");
            TextView text = (TextView)FindViewById(Resource.Id.tvLandingPageHeader);
            Button btn = (Button)FindViewById(Resource.Id.btnLandingPageGetStarted);
            text.SetTypeface(tf, TypefaceStyle.Normal);
            btn.SetTypeface(tf, TypefaceStyle.Normal);
            #endregion
            #region Paragraph (Regular Poppins, line spacing 1.5)
            text = (TextView)FindViewById(Resource.Id.tvLandingPageParagraph);
            tf = Typeface.CreateFromAsset(Assets, "Poppins-Regular.ttf");
            text.SetLineSpacing(0, (float)1.5);
            text.SetTypeface(tf, TypefaceStyle.Normal);
            #endregion

        }
    }
}