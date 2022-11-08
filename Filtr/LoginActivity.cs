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
using AndroidX.AppCompat.App;
using System.Text;

namespace Filtr
{
    [Activity(Label = "LoginActivity")]
    public class LoginActivity : AppCompatActivity
    {
        Button btnToRegister;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.login_page);
            SetupFonts();

            btnToRegister = FindViewById<Button>(Resource.Id.btnLoginFooterLink);
            btnToRegister.Click += BtnToRegister_Click;
        }

        private void BtnToRegister_Click(object sender, EventArgs e)
        {
            Intent it = new Intent(this, typeof(RegisterActivity));
            StartActivity(it);
        }

        private void SetupFonts()
        {
            #region Header, Button, Footer link (Semi-Bold Poppins)
            Typeface tf = Typeface.CreateFromAsset(Assets, "Poppins-SemiBold.ttf");
            TextView text = (TextView)FindViewById(Resource.Id.tvLoginPageHeader);
            Button btn = (Button)FindViewById(Resource.Id.btnLogin);
            text.SetTypeface(tf, TypefaceStyle.Normal);
            btn.SetTypeface(tf, TypefaceStyle.Normal);
            btn = (Button)FindViewById(Resource.Id.btnLoginFooterLink);
            btn.SetTypeface(tf, TypefaceStyle.Normal);
            #endregion
            #region textfields, subhead, Footer text (Regular Poppins)
            tf = Typeface.CreateFromAsset(Assets, "Poppins-Regular.ttf");
            EditText et = (EditText)FindViewById(Resource.Id.etLoginEmail);
            et.SetTypeface(tf, TypefaceStyle.Normal);
            et = (EditText)FindViewById(Resource.Id.etLoginPass);
            et.SetTypeface(tf, TypefaceStyle.Normal);
            text = (TextView)FindViewById(Resource.Id.tvLoginFooterText);
            text.SetTypeface(tf, TypefaceStyle.Normal);
            #endregion
        }
    }
}