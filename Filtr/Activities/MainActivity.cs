using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Widget;
using AndroidX.AppCompat.App;
using Firebase;
using Firebase.Firestore;
using System;

namespace Filtr
{
    [Activity(Label = "@string/app_name", Icon = "@drawable/app_icon", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);  
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);

            // Loading Screen
            SetContentView(Resource.Layout.loading);

            Live.db = GetDataBase();

            ISharedPreferences sp = this.GetSharedPreferences("details", Android.Content.FileCreationMode.Private);
            string id = sp.GetString("id", "");
            if (id != "")
            {
                // Set live user details
                Live.user = new User(
                        sp.GetString("id", ""),
                        sp.GetString("email", ""),
                        sp.GetString("password", ""),
                        sp.GetString("Fname", ""),
                        sp.GetString("Lname", ""));

                // Move to home page
                Intent it = new Intent(this, typeof(HomeActivity));
                StartActivity(it);
            }
            else
            {
                // Move to starting page
                Intent it = new Intent(this, typeof(LandingPageActivity));
                StartActivity(it);
            }

            
        }
        public FirebaseFirestore GetDataBase()
        {
            FirebaseFirestore db;

            var options = new FirebaseOptions.Builder()
                .SetProjectId("filtr-87054")
                .SetApplicationId("filtr-87054")
                .SetApiKey("AIzaSyAF1Z62LMGzOYTIzNARCwHem_BQrfmntFE")
                .SetDatabaseUrl("https://filtr-87054-default-rtdb.firebaseio.com")
                .SetStorageBucket("filtr-87054.appspot.com")
                .Build();

            var app = FirebaseApp.InitializeApp(this, options);
            db = FirebaseFirestore.GetInstance(app);
            return db;
        }
    }
}