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
        public static void ScaleIn(Context ctx, View v)
        {
            var anim = AnimationUtils.LoadAnimation(ctx, Resource.Animation.like_anim);
            
            v.ScaleX = 1;
            v.ScaleY = 1;
            v.Visibility = ViewStates.Visible;
            
            v.StartAnimation(anim);
        }
        public static void ScaleOut(Context ctx, View v)
        {
            var anim = AnimationUtils.LoadAnimation(ctx, Resource.Animation.unlike_anim);
            v.StartAnimation(anim);
            v.Visibility=ViewStates.Invisible;
        }
    }
}