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
using Firebase.Firestore;
using Java.Util;
using Firebase;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Filtr
{
    [Activity(Label = "RegisterActivity")]
    public class RegisterActivity : AppCompatActivity
    {
        Button btnToLogin, btnRegister;
        FirebaseFirestore DB;
        EditText etName, etEmail, etPassword, etConfirm;
        View p;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            DB = MainActivity.DB;

            SetContentView(Resource.Layout.register_page);
            ConnectViews();
            SetupFonts();



        }
        #region event handlers
        private void BtnToLogin_Click(object sender, EventArgs e) // sends user to login page
        {
            Intent it = new Intent(this, typeof(LoginActivity));
            StartActivity(it);
        }
        private void BtnRegister_Click(object sender, EventArgs e)
        {
            if (!HandleForm()) return;

            Intent it = new Intent(this, typeof(HomeActivity));
            StartActivity(it);
        } //TODO: Create User
        #endregion
        #region form
        private bool HandleForm() // validate & save to db
        {
            if (!IsFormValid()) return false;

            HashMap map = new HashMap();

            // process full name
            List<string> names = etName.Text.Split(" ").ToList();

            // save first name
            map.Put("Fname", names.First());

            // save last name
            names.RemoveAt(0);
            map.Put("Lname", String.Join(" ", names.ToArray()));

            // save email
            map.Put("email", etEmail.Text);

            // save password
            map.Put("password", etPassword.Text);

            // setup liked posts and own posts
            map.Put("likedPosts", new ArrayList());
            map.Put("posts", new ArrayList());

            // create document reference for firestore
            DocumentReference docRef = DB.Collection("users").Document();

            // set map in the referenced document
            docRef.Set(map);

            return true;
        }       
        private bool IsFormValid() // validates the whole form
        {
            return isNameValid() && IsEmailValid() && IsPasswordValid() && ArePasswordsMatching();
        }
        private bool ArePasswordsMatching() // checks if password confirmation is correct
        {
            // check if the password confirm is matching
            if (!etPassword.Text.Equals(etConfirm.Text))
            {
                Toast.MakeText(this, "Please confirm your password", ToastLength.Long).Show();
                return false;
            }
            // passwords match
            return true;
        }
        private bool IsPasswordValid() // validates the password
        {
            string password = etPassword.Text;

            // check if field is empty
            if (password.Trim().Equals(""))
            {
                Toast.MakeText(this, "You have to enter your password", ToastLength.Long).Show();
                return false;
            }

            // only letters and numbers
            if (!Regex.IsMatch(password, @"[A-Za-z0-9]"))
            {
                Toast.MakeText(this, "Your password can contain only letters and numbers", ToastLength.Long).Show();
                return false;
            }
            // longer than 8 digits
            if (password.Length <= 8)
            {
                Toast.MakeText(this, "Your password must be longer than 8 digits", ToastLength.Long).Show();
                return false;
            }
            // password is valid
            return true;
        }
        private bool IsEmailAvailable() // checks email evailabilit
        {
            return true; // TODO: Email Availability
        }
        private bool IsEmailValid() // validates the email
        {
            string email = etEmail.Text;

            // remove whitespaces
            email = email.Trim();

            // check if field is empty
            if (email.Equals(""))
            {
                Toast.MakeText(this, "You have to enter your email address", ToastLength.Long).Show();
                return false;
            }

            // email ends with .
            if (email.EndsWith("."))
            {
                return false;
            }

            // utilize email validation provided by .NET
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                Toast.MakeText(this, "Entered email address isn't valid", ToastLength.Long).Show();
                return false;
            }
        }
        private bool isNameValid() // validates the name
        {
            string name = etName.Text;
            
            // remove whitespaces
            name = name.Trim();
            
            // check if field is empty
            if (name.Equals(""))
            {
                Toast.MakeText(this, "You have to enter your name", ToastLength.Long).Show();
                return false;
            }

            // entered something except letters
            if (!Regex.IsMatch(name, @"^[a-zA-Z^]"))
            {
                Toast.MakeText(this, "Your name must contain only letters", ToastLength.Long).Show();
                return false;
            }
            // entered only one word
            if (name.Split(" ").Length <= 1)
            {
                Toast.MakeText(this, "Full name must contain more than one word", ToastLength.Long).Show();
                return false;
            }
            // name is valid
            return true;
        }
        #endregion
        #region setup
        private void SetupFonts() // setup the fonts for the ui components
        {
            #region Header, Button, Footer link (Semi-Bold Poppins)
            Typeface tf = Typeface.CreateFromAsset(Assets, "Poppins-SemiBold.ttf");
            TextView text = (TextView)p.FindViewById(Resource.Id.tvHeader);
            text.SetTypeface(tf, TypefaceStyle.Normal);
            btnRegister.SetTypeface(tf, TypefaceStyle.Normal);
            btnToLogin.SetTypeface(tf, TypefaceStyle.Normal);
            #endregion
            #region textfields, subhead, Footer text (Regular Poppins)
            tf = Typeface.CreateFromAsset(Assets, "Poppins-Regular.ttf");
            
            etName.SetTypeface(tf, TypefaceStyle.Normal);
            etEmail.SetTypeface(tf, TypefaceStyle.Normal);
            etPassword.SetTypeface(tf, TypefaceStyle.Normal);
            etConfirm.SetTypeface(tf, TypefaceStyle.Normal);

            text = (TextView)p.FindViewById(Resource.Id.tvSubHeader);
            text.SetTypeface(tf, TypefaceStyle.Normal);
            text = (TextView)p.FindViewById(Resource.Id.tvFooter);
            text.SetTypeface(tf, TypefaceStyle.Normal);
            #endregion
        }
        private void ConnectViews() // connects all of the components
        {
            p = FindViewById<View>(Resource.Id.register_page);

            etName = p.FindViewById<EditText>(Resource.Id.etName);
            etEmail = p.FindViewById<EditText>(Resource.Id.etEmail);
            etPassword = p.FindViewById<EditText>(Resource.Id.etPassword);
            etConfirm = p.FindViewById<EditText>(Resource.Id.etConfirm);

            btnRegister = p.FindViewById<Button>(Resource.Id.btnRegister);
            btnToLogin = p.FindViewById<Button>(Resource.Id.btnToLogin);

            btnRegister.Click += BtnRegister_Click;
            btnToLogin.Click += BtnToLogin_Click;
        }
        #endregion
    }
}