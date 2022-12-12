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
        public Bitmap uiBackground { get; private set; } // background in app ui
        public string name { get; private set; } // filter's name
        public static PixelFilter filter = new PixelFilter(); // instance of the filter in order to create it once
        public Bitmap Apply(Bitmap img) // apply the filter on the image (here each algorithm will be applied)
        {
            return img; // temp to override
        }
        private PixelFilter() // private constructor
        {
            this.name = "pixelate";

        }
        public PixelFilter GetInstance() // implement Singleton design pattern
        {
            if (filter == null)
                filter = new PixelFilter();
            return filter; // temp to override
        }
    }
}