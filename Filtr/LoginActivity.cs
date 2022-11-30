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
        Button btnToRegister, btnLogin;
        EditText etEmail, etPassword;
        View p;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            p = FindViewById<View>(Resource.Id.login_page);

            // Create your application here
            SetContentView(Resource.Layout.login_page);
            
            etEmail = p.FindViewById<EditText>(Resource.Id.etEmail);
            etPassword = p.FindViewById<EditText>(Resource.Id.etPassword);
            btnLogin = p.FindViewById<Button>(Resource.Id.btnLogin);

            btnToRegister = p.FindViewById<Button>(Resource.Id.btnFooterLink);
            btnToRegister.Click += BtnToRegister_Click;

            SetupFonts();
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

            TextView text = (TextView)FindViewById(Resource.Id.tvHeader);
            text.SetTypeface(tf, TypefaceStyle.Normal);
            btnLogin.SetTypeface(tf, TypefaceStyle.Normal);
            btnToRegister.SetTypeface(tf, TypefaceStyle.Normal);
            #endregion
            #region textfields, subhead, Footer text (Regular Poppins)
            tf = Typeface.CreateFromAsset(Assets, "Poppins-Regular.ttf");

            etPassword.SetTypeface(tf, TypefaceStyle.Normal);
            etEmail.SetTypeface(tf, TypefaceStyle.Normal);
            text = (TextView)p.FindViewById(Resource.Id.tvFooterText);
            text.SetTypeface(tf, TypefaceStyle.Normal);
            #endregion
        }
    }
}