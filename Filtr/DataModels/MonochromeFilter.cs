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
    public class MonochromeFilter : Filter
    {
        public static string name = "Mono";
        public static Bitmap Apply(Bitmap original)
        {
            // Create a blank bitmap with the same dimensions as the original
            Bitmap newImg = Bitmap.CreateBitmap(original);

            // Iterate through each pixel in the bitmap
            for (int y = 0; y < original.Height; y++)
            {
                for (int x = 0; x < original.Width; x++)
                {
                    // Get the color of the 
                    ColorObject color = original.GetColor(x, y);

                    // Use a formula to convert the color to grayscale
                    int gray = (int)(color.Red() * 255 * 0.3 + color.Green() * 255 * 0.59 + color.Blue() * 255 * 0.11);

                    // Create a new grayscale color using the calculated value
                    Color newColor = Color.Argb(255, gray, gray, gray);

                    // Set the pixel in the new bitmap to the grayscale color
                    newImg.SetPixel(x, y, newColor);
                }
            }
            // Return the grayscale bitmap
            return newImg;
        }
    }
}