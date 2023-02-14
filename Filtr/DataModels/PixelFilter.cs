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
    public class PixelFilter : Filter
    {
        public string name { get; private set; } // filter's name
        public static Bitmap Apply(Bitmap original)
        {
            // Create a blank bitmap with the same dimensions as the original
            original = ImageHelper.GetResizedBitmap(original, 128, 128);
            original = ImageHelper.GetResizedBitmap(original, 500, 500);
            //Bitmap newImg = Bitmap.CreateBitmap(original.Width, original.Height, Bitmap.Config.Argb8888);

            

            return original;
        }
    }
}