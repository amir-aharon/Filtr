using Android.App;
using Android.Content;
using Android.Gms.Tasks;
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
    [BroadcastReceiver]
    public class AlarmReceiver : BroadcastReceiver, IOnSuccessListener
    {
        Context context;
        public override void OnReceive(Context context, Intent intent)
        {
            this.context = context;
            ISharedPreferences sp = context.GetSharedPreferences("details", Android.Content.FileCreationMode.Private);
            string id = sp.GetString("id", "");
            System.Diagnostics.Debug.WriteLine(id);
            if (id != "")
                Live.db.Collection("users").Document(id).Get().AddOnSuccessListener(this);


        }

        public void OnSuccess(Java.Lang.Object result)
        {
            var snapshot = (DocumentSnapshot)result;
            
            if ((bool)snapshot.Get("newLikes"))
            {
                #region Notification
                Intent i = new Intent(context, typeof(MainActivity));
                i.PutExtra("key", "new message");
                PendingIntent pendingIntent = PendingIntent.GetActivity(context, 0, i, 0);
                Notification.Builder notificationBuilder = new Notification.Builder(context)
                .SetSmallIcon(Resource.Drawable.like_full)
                .SetContentTitle("You have new likes!")
                .SetContentText("Tap here to check your account.");

                var nm = (NotificationManager)context.GetSystemService(Context.NotificationService);
                notificationBuilder.SetContentIntent(pendingIntent);

                //Build.VERSION_CODES.O - is a reference to API level 26 (Android Oreo which is Android 8)if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
                {
                    NotificationChannel notificationChannel = new NotificationChannel("abcd", "NAME", NotificationImportance.High);
                    notificationBuilder.SetChannelId("abcd");
                    nm.CreateNotificationChannel(notificationChannel);
                }
                nm.Notify(1, notificationBuilder.Build());
                #endregion

                DocumentReference docRef = Live.db.Collection("users").Document(snapshot.Id);
                docRef.Update("newLikes", false);
            }
        }
    }
}