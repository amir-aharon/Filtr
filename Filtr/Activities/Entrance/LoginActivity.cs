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
using Firebase.Firestore;
using Android.Gms.Tasks;

namespace Filtr
{
    [Activity(Label = "LoginActivity")]
    public class LoginActivity : AppCompatActivity, IOnSuccessListener
    {
        #region setup
        #region variables
        Button btnToRegister, btnLogin;
        EditText etEmail, etPassword;
        View p;
        #endregion
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Live.user = null;
            SetContentView(Resource.Id.login_page);
            ConnectViews();
            SetupFonts();
        }
        private void ConnectViews() // connects all of the components 
        {
            // get references
            p = FindViewById<View>(Resource.Id.login_page);
            etEmail = p.FindViewById<EditText>(Resource.Id.etEmail);
            etPassword = p.FindViewById<EditText>(Resource.Id.etPassword);
            btnLogin = p.FindViewById<Button>(Resource.Id.btnLogin);
            btnToRegister = p.FindViewById<Button>(Resource.Id.btnFooterLink);

            // clean view
            etEmail.Text = "";
            etPassword.Text = "";

            // set actions
            btnToRegister.Click += BtnToRegister_Click;
            btnLogin.Click += BtnLogin_Click;
        }
        private void SetupFonts() // setup the fonts for the ui components 
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
        #endregion

        #region events
        private void BtnToRegister_Click(object sender, EventArgs e) // navigates to registration page 
        {
            Intent it = new Intent(this, typeof(RegisterActivity));
            StartActivity(it);
        }
        private void BtnLogin_Click(object sender, EventArgs e) // form submit process 
        {
            CheckIfFieldsMatch();
        }
        #endregion

        #region auth
        private void CheckIfFieldsMatch() // validates user input (calling a query) 
        {
            Live.db.Collection("users")
               .WhereEqualTo("email", etEmail.Text)
               .Get().AddOnSuccessListener(this);
        }
        public void OnSuccess(Java.Lang.Object result) // executes query 
        {
            // process result
            var snapshot = (QuerySnapshot)result;
            
            // search for entity with matching email and password
            foreach (var doc in snapshot.Documents)
            {
                // if this user matches the requirements
                if (doc.Get("password").Equals(etPassword.Text))
                {
                    // set live user
                    Live.user = new User(
                        doc.Id,
                        (string)doc.Get("email"),
                        (string)doc.Get("password"),
                        (string)doc.Get("Fname"),
                        (string)doc.Get("Lname"),
                        (bool)doc.Get("newLikes"));

                    // initialize session
                    ISharedPreferences sp = this.GetSharedPreferences("details", Android.Content.FileCreationMode.Private);
                    var editor = sp.Edit();

                    // fill session data
                    editor.PutString("id", Live.user.id);
                    editor.PutString("email", Live.user.email);
                    editor.PutString("password", Live.user.password);
                    editor.PutString("Fname", Live.user.Fname);
                    editor.PutString("Lname", Live.user.Lname);
                    editor.PutBoolean("newLikes", Live.user.newLikes);
                    editor.Commit();

                    // navigate to home page
                    Intent it = new Intent(this, typeof(HomeActivity));
                    StartActivity(it);
                    return;
                }
            }
            // inform user if email and password are incorrect
            Toast.MakeText(this, "Email or password are incorrect", ToastLength.Long).Show();
        }
        #endregion
    }
}