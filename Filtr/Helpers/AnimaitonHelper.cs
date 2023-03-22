using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Views.Animations;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Filtr
{
    public static class AnimaitonHelper
    {
        public static void ScaleIn(Context ctx, View v) // make like appear 
        {
            // load xml animation
            var anim = AnimationUtils.LoadAnimation(ctx, Resource.Animation.like_anim);
            
            // scale the like image to appear
            v.ScaleX = 1;
            v.ScaleY = 1;

            // make it visible
            v.Visibility = ViewStates.Visible;
            
            // execute animation
            v.StartAnimation(anim);
        }
        public static void ScaleOut(Context ctx, View v) // make like disappear 
        {
            // load xml animation
            var anim = AnimationUtils.LoadAnimation(ctx, Resource.Animation.unlike_anim);
            
            // execute animation
            v.StartAnimation(anim);

            // make like image invisible
            v.Visibility=ViewStates.Invisible;
        }
    }
}