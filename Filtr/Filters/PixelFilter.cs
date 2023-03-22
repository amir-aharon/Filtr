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
            // resize input bitmap - compression does all the work for us
            original = ImageHelper.GetResizedBitmap(original, 128, 128);

            // return the bitmap (which is a pixel matrix) to original size 
            original = ImageHelper.GetResizedBitmap(original, 500, 500);            

            // return bitmap
            return original;
        }
    }
}