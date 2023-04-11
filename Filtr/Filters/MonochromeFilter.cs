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
using System.Threading;
using System.Threading.Tasks;

namespace Filtr
{
    public class MonochromeFilter : Filter
    {
        public static string name = "Mono"; // filter's name
        public static Bitmap Apply(Bitmap original, int w, int h)
        {
            int gray = 0;
            ColorObject color = null;
            Color newColor = Color.Argb(255, 0, 0, 0);


            // change original size
            original = ImageHelper.GetResizedBitmap(original, w, h);
            
            
            Bitmap grayscaleBitmap = Bitmap.CreateBitmap(original.Width, original.Height, Bitmap.Config.Argb8888);

            // create a canvas object to draw the grayscale bitmap
            Canvas canvas = new Canvas(grayscaleBitmap);

            // create a color matrix to convert the colors to grayscale
            ColorMatrix colorMatrix = new ColorMatrix();
            colorMatrix.SetSaturation(0);

            // create a paint object with the color matrix
            Paint paint = new Paint();
            paint.SetColorFilter(new ColorMatrixColorFilter(colorMatrix));

            // draw the original bitmap onto the canvas with the grayscale filter
            canvas.DrawBitmap(original, 0, 0, paint);

            // return the grayscale bitmap
            return grayscaleBitmap;
        }
    }
}