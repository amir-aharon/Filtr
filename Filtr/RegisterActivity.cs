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
using AndroidX.AppCompat.App;

namespace Filtr
{
    [Activity(Label = "RegisterActivity")]
    public class RegisterActivity : AppCompatActivity
    {
        Button btnToLogin;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.register_page);
            SetupFonts();

            btnToLogin = FindViewById<Button>(Resource.Id.btnRegisterFooterLink);
            btnToLogin.Click += BtnToLogin_Click;
        }

        private void BtnToLogin_Click(object sender, EventArgs e)
        {
            Intent it = new Intent(this, typeof(LoginActivity));
            StartActivity(it);
        }

        private void SetupFonts()
        {
            #region Header, Button, Footer link (Semi-Bold Poppins)
            Typeface tf = Typeface.CreateFromAsset(Assets, "Poppins-SemiBold.ttf");
            TextView text = (TextView)FindViewById(Resource.Id.tvRegisterPageHeader);
            Button btn = (Button)FindViewById(Resource.Id.btnRegister);
            text.SetTypeface(tf, TypefaceStyle.Normal);
            btn.SetTypeface(tf, TypefaceStyle.Normal);
            btn = (Button)FindViewById(Resource.Id.btnRegisterFooterLink);
            btn.SetTypeface(tf, TypefaceStyle.Normal);
            #endregion
            #region textfields, subhead, Footer text (Regular Poppins)
            tf = Typeface.CreateFromAsset(Assets, "Poppins-Regular.ttf");
            EditText et = (EditText)FindViewById(Resource.Id.etRegisterName);
            et.SetTypeface(tf, TypefaceStyle.Normal);
            et = (EditText)FindViewById(Resource.Id.etRegisterEmail);
            et.SetTypeface(tf, TypefaceStyle.Normal);
            et = (EditText)FindViewById(Resource.Id.etRegisterPass);
            et.SetTypeface(tf, TypefaceStyle.Normal);
            et = (EditText)FindViewById(Resource.Id.etRegisterConfirm);
            et.SetTypeface(tf, TypefaceStyle.Normal);
            text = (TextView)FindViewById(Resource.Id.tvRegisterPageSubHeader);
            text.SetTypeface(tf, TypefaceStyle.Normal);
            text = (TextView)FindViewById(Resource.Id.tvRegisterFooterText);
            text.SetTypeface(tf, TypefaceStyle.Normal);
            #endregion
        }
    }
}