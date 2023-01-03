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
        public static void HomeButton(Context ctx)
        {
            Intent intent = new Intent(ctx, typeof(HomeActivity));
            intent.AddFlags(ActivityFlags.NoAnimation);
            ctx.StartActivity(intent);
        }
        public static void SearchButton(Context ctx)
        {
            Intent intent = new Intent(ctx, typeof(SearchActivity));
            intent.AddFlags(ActivityFlags.NoAnimation);
            ctx.StartActivity(intent);
        }
        public static void LikedButton(Context ctx)
        {
            Intent intent = new Intent(ctx, typeof(LikedActvity));
            intent.AddFlags(ActivityFlags.NoAnimation);
            ctx.StartActivity(intent);
        }
        public static void AccountButton(Context ctx)
        {
            Intent intent = new Intent(ctx, typeof(AccountActivity));
            intent.AddFlags(ActivityFlags.NoAnimation);
            ctx.StartActivity(intent);
        }
    }
}