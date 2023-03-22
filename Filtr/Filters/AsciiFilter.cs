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
using System.Threading.Tasks;

namespace Filtr
{
    public class AsciiFilter : Filter
    {
        public string name { get; private set; } // filter's name
        public static async Task<Bitmap> Apply(Bitmap original, Context ctx, ProgressBar ls)
        {
            ls.Visibility = ViewStates.Visible;
            // string of characters which present levels of intensity
            string CHARACTERS = " .:-=+*#%@";
            
            // image's dimensions
            int w = 72, h = 72;

            // resize input image to desired dimensions
            original = ImageHelper.GetResizedBitmap(original, w, h);

            // apply monochrome filter
            original = await MonochromeFilter.Apply(original, w, h, ls);

            // create representing array of strings - each string is a line in the assci image
            string[] asciiImg = new string[h];

            // iterate through image and for coresponding string array
            int index = 0;
            for (int row = 0; row < h; row++)
            {
                for (int col = 0; col < w; col++)
                {
                    // get intensity string index from pixel's intensity
                    index = (int) Math.Floor(9 * original.GetColor(col, row).Red());

                    // add the wanted characteds to the matching string
                    asciiImg[row] += CHARACTERS[index];
                }
            }

            // init display
            Bitmap img = Bitmap.CreateBitmap(1000, 1000, Bitmap.Config.Argb8888);

            // setup a canvas in order to turn string array into a bitmap
            Canvas canvas = new Canvas(img);
            canvas.DrawColor(Color.Black);
            Paint paint = new Paint();
            Typeface tf = Typeface.CreateFromAsset(ctx.Assets, "Cascadia.ttf");
            paint.LetterSpacing = 0.3f;
            paint.SetTypeface(tf);
            paint.TextSize = 16;
            paint.Color = Color.White;
            int xPos = 0;
            int yPos = 10;

            // display string array on canvas
            foreach (string row in asciiImg)
            {
                canvas.DrawText(row, xPos, yPos, paint);
                yPos += 14;
            }
            
            // return the canvas as bitmap
            return img;
        }
    }
}