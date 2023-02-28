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

            // loading screen (only to show something if phone lags)
            SetContentView(Resource.Layout.loading);

            // connect to firestore
            if (Live.db == null)
                Live.db = GetDataBase();

            // connects to the session
            ISharedPreferences sp = GetSharedPreferences("details", Android.Content.FileCreationMode.Private);
            string id = sp.GetString("id", "");

            // if theres a user in the session (aka logged from this device)
            if (id != "")
            {
                // Set live user details
                Live.user = new User(
                        sp.GetString("id", ""),
                        sp.GetString("email", ""),
                        sp.GetString("password", ""),
                        sp.GetString("Fname", ""),
                        sp.GetString("Lname", ""),
                        sp.GetBoolean("newLikes", false));

                // update that the user has logged
                DocumentReference docRef = Live.db.Collection("users").Document(Live.user.id);
                docRef.Update("newLikes", false);

                // listener for new likes later, when user is idle
                CallAlarmManager();

                // navigate to home page
                Intent it = new Intent(this, typeof(HomeActivity));
                StartActivity(it);
            }
            // if no one isn't logged from this device
            else
            {
                // listener for new likes later, when user is idle (if a user will log in)
                CallAlarmManager();

                // navigate to landing page
                Intent it = new Intent(this, typeof(LandingPageActivity));
                StartActivity(it);
            }
        }
        public FirebaseFirestore GetDataBase() // returns a DB reference 
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
        public void CallAlarmManager() // calls the alarm manager 
        {
            // create action -> call receiver
            Intent it = new Intent(this, typeof(AlarmReceiver));

            // turn action into a pending-able type
            PendingIntent pi = PendingIntent.GetBroadcast(Application.Context, 1, it, 0);

            // declare and initialize alarm manager
            AlarmManager alarmManager = (AlarmManager)GetSystemService(AlarmService);

            // set the alarm manager -> performs the action every 5 minutes
            alarmManager.SetRepeating(
                AlarmType.ElapsedRealtimeWakeup,
                SystemClock.ElapsedRealtime() + 1000, // first call
                1000 * 5, // repeat interval in millieseconds
                pi); // action
        }
    }
}