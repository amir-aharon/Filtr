using Android.App;
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
        Button btnStart;
        View p;
        Animation anim;
        ImageView empty, full;
        FrameLayout frame;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.landing_page);
            p = FindViewById<View>(Resource.Id.landing_page);

            btnStart = p.FindViewById<Button>(Resource.Id.btnGetStarted);
            anim = AnimationUtils.LoadAnimation(this, Resource.Animation.like_anim);
            btnStart.Click += BtnStart_Click;

            empty = p.FindViewById<ImageView>(Resource.Id.empty);
            full = p.FindViewById<ImageView>(Resource.Id.full);
            frame = p.FindViewById<FrameLayout>(Resource.Id.frame);

            full.ScaleX = 0;
            full.ScaleY = 0;
            full.Visibility = ViewStates.Invisible;

            frame.Click += Frame_Click;

            //empty.Click += Empty_Click;
            //full.Click += Full_Click;

            SetupFonts();
        }

        private void Frame_Click(object sender, EventArgs e)
        {
            if (full.Visibility == ViewStates.Visible)
                AnimaitonHelper.ScaleOut(this, full);
            else
                AnimaitonHelper.ScaleIn(this, full);
        }

        private void Full_Click(object sender, EventArgs e)
        {
            Toast.MakeText(this, "Full Clicked", ToastLength.Short).Show();
            
            //full.Visibility = ViewStates.Invisible;

        }

        private void Empty_Click(object sender, EventArgs e)
        {
            Toast.MakeText(this, "Empty Clicked", ToastLength.Short).Show();
            
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
    }
}