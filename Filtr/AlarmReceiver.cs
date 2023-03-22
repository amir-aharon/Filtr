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
            // set activity context
            this.context = context;

            // check if there's a logged in user from this device
            ISharedPreferences sp = context.GetSharedPreferences("details", Android.Content.FileCreationMode.Private);
            string id = sp.GetString("id", "");
            System.Diagnostics.Debug.WriteLine(id);

            // if there is a logged user, check for new likes
            if (id != "")
                Live.db.Collection("users").Document(id).Get().AddOnSuccessListener(this);


        }
        public void OnSuccess(Java.Lang.Object result) // executes the query 
        {
            // process the queried data
            var snapshot = (DocumentSnapshot)result;
            
            // if there are new likes
            if ((bool)snapshot.Get("newLikes"))
            {
                #region notification
                Intent i = new Intent(context, typeof(MainActivity));
                i.PutExtra("key", "new message");
                PendingIntent pendingIntent = PendingIntent.GetActivity(context, 0, i, 0);
                Notification.Builder notificationBuilder = new Notification.Builder(context)
                .SetSmallIcon(Resource.Drawable.like_full)
                .SetContentTitle("You have new likes!")
                .SetContentText("Tap here to check your account.");

                var nm = (NotificationManager)context.GetSystemService(Context.NotificationService);
                notificationBuilder.SetContentIntent(pendingIntent);

                {
                    NotificationChannel notificationChannel = new NotificationChannel("abcd", "NAME", NotificationImportance.High);
                    notificationBuilder.SetChannelId("abcd");
                    nm.CreateNotificationChannel(notificationChannel);
                }
                nm.Notify(1, notificationBuilder.Build());
                #endregion

                // update newLikes field to false
                DocumentReference docRef = Live.db.Collection("users").Document(snapshot.Id);
                docRef.Update("newLikes", false);
            }
        }
    }
}