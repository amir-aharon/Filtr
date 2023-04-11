using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Firebase.Firestore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Filtr
{
    public static class Live
    {
        public static FirebaseFirestore db; // static connection for app's DB
        public static User user; // static info about the logged user
    }
}