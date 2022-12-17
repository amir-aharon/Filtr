using Android.App;
using Android.Content;
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
            ctx.StartActivity(intent);
        }
        public static void SearchButton(Context ctx)
        {
            Intent intent = new Intent(ctx, typeof(SearchActivity));
            ctx.StartActivity(intent);
        }
        //public static void LikedButton(Context ctx)
        //{
        //    Intent intent = new Intent(ctx, typeof(Lik));
        //    ctx.StartActivity(intent);
        //}
        public static void AccountButton(Context ctx)
        {
            Intent intent = new Intent(ctx, typeof(AccountActivity));
            ctx.StartActivity(intent);
        }
    }
}