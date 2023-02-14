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
    public class AsciiFilter
    {
        public string name { get; private set; } // filter's name
        public static Bitmap Apply(Bitmap original, Context ctx)
        {
            string CHARACTERS = " .:-=+*#%@";
            StringBuilder sb = new StringBuilder();
            int w = 72, h = 72;

            original = ImageHelper.GetResizedBitmap(original, w, h);
            original = MonochromeFilter.Apply(original, w, h);

            string[] asciiImg = new string[h];


            Console.WriteLine(original.Width);
            Console.WriteLine(original.Height);

            for (int row = 0; row < h; row++)
            {
                for (int col = 0; col < w; col++)
                {
                    int index = (int) Math.Floor(9 * original.GetColor(col, row).Red());
                    Console.WriteLine("ok" + index);
                    asciiImg[row] += CHARACTERS[index];
                }
            }
                    
            Bitmap img = Bitmap.CreateBitmap(1000, 1000, Bitmap.Config.Argb8888);
            for (int x = 0; x < img.Width; x++)
            {
                for (int y = 0; y < img.Height; y++)
                {
                    img.SetPixel(x, y, new Color(0, 0, 0));
                }
            }
            Canvas canvas = new Canvas(img);
            Paint paint = new Paint();
            Typeface tf = Typeface.CreateFromAsset(ctx.Assets, "Cascadia.ttf");
            paint.LetterSpacing = 0.3f;
            paint.SetTypeface(tf);
            paint.TextSize = 16;
            paint.Color = Color.White;
            int xPos = 0;
            int yPos = (int)10; //- ((paint.Descent() + paint.Ascent()) / 2));

            
            foreach (string row in asciiImg)
            {
                Console.WriteLine(row);
                canvas.DrawText(row, xPos, yPos, paint);
                yPos += 14;
            }

            return img;

            return original;
        }
    }
}