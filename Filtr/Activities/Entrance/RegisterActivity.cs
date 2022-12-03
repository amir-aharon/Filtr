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
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using Android.Gms.Tasks;
using Android.Gms.Extensions;
using System.Threading;

namespace Filtr
{
    [Activity(Label = "RegisterActivity")]
    public class RegisterActivity : AppCompatActivity, IOnSuccessListener
    {
        #region setup
        #region variables
        Button btnToLogin, btnRegister;
        EditText etName, etEmail, etPassword, etConfirm;
        View p;
        #endregion
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            
            Live.user = null;

            SetContentView(Resource.Layout.register_page);
            ConnectViews();
            SetupFonts();
        }
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

            etEmail.Text = "";
            etPassword.Text = "";
            etName.Text = "";
            etConfirm.Text = "";

            btnRegister.Click += BtnRegister_Click;
            btnToLogin.Click += BtnToLogin_Click;
        }
        #endregion
        #region events
        private void BtnToLogin_Click(object sender, EventArgs e) // sends user to login page
        {
            Intent it = new Intent(this, typeof(LoginActivity));
            StartActivity(it);
        }
        private void BtnRegister_Click(object sender, EventArgs e)
        {
            SortCaseByEmailAvailability();
        } 
        #endregion
        #region form
        #region email availablilty
        private void SortCaseByEmailAvailability()
        {
            Live.db.Collection("users")
               .WhereEqualTo("email", etEmail.Text)
               .Get().AddOnSuccessListener(this);
        }
        public void OnSuccess(Java.Lang.Object result)
        {
            var snapshot = (QuerySnapshot)result; // get list of all docs matching the query

            if (!snapshot.IsEmpty)
                HandleForm(false);
            else
                HandleForm(true);

        }
        #endregion
        #region handlers
        private void HandleForm(bool emailIsAvaliable) // validate & save to db
        {
            if (!IsFormValid(emailIsAvaliable)) return;

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
            DocumentReference userRef = Live.db.Collection("users").Document();

            // set map in the referenced document
            userRef.Set(map);

            // setup the logged in user
            Live.user = new User(
                userRef.Id, 
                (string)map.Get("email"), 
                (string)map.Get("password"), 
                (string)map.Get("Fname"), 
                (string)map.Get("Lname"));

            Intent it = new Intent(this, typeof(HomeActivity));
            StartActivity(it);
        }
        private bool IsFormValid(bool emailIsAvailable) // validates the whole form
        {
            return IsNameValid() && IsEmailValid(emailIsAvailable) && IsPasswordValid() && ArePasswordsMatching();
        }
        #endregion
        #region string check
        private bool IsEmailValid(bool emailIsAvailable) // validates the email
        {
            if (!emailIsAvailable)
            {
                Toast.MakeText(this, "Email is already taken", ToastLength.Long).Show();
                return false;
            }
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
                // TODO: email validation
                // only one @
                // from letters only english
                // before and after @:
                //  not ending or beginning with .,
                //
            }

            // utilize email validation provided by .NET
            Regex rg = new Regex(@"[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?");
            if (rg.IsMatch(email) && Regex.Matches(email, "@").Count == 1)
            {
                return true;
            }
            Toast.MakeText(this, "Entered email address isn't valid", ToastLength.Long).Show();
            return false;
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
            if (password.Length < 8)
            {
                Toast.MakeText(this, "Your password must be 8 characters or more", ToastLength.Long).Show();
                return false;
            }
            // password is valid
            return true;
        }
        private bool IsNameValid() // validates the name
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
        #endregion
    }
}