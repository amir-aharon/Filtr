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

namespace Filtr
{
    public static class NavbarHelper
    {
        public static void HomeButton(Context ctx) // performs home button intent 
        {
            Intent intent = new Intent(ctx, typeof(HomeActivity));
            intent.AddFlags(ActivityFlags.NoAnimation);
            ctx.StartActivity(intent);
        }
        public static void SearchButton(Context ctx) // performs search button intent 
        {
            Intent intent = new Intent(ctx, typeof(SearchActivity));
            intent.AddFlags(ActivityFlags.NoAnimation);
            ctx.StartActivity(intent);
        }
        public static void LikedButton(Context ctx) // performs liked button intent 
        {
            Intent intent = new Intent(ctx, typeof(LikedActvity));
            intent.AddFlags(ActivityFlags.NoAnimation);
            ctx.StartActivity(intent);
        }
        public static void AccountButton(Context ctx) // performs account button intent 
        {
            Intent intent = new Intent(ctx, typeof(AccountActivity));
            intent.AddFlags(ActivityFlags.NoAnimation);
            ctx.StartActivity(intent);
        }
    }
}