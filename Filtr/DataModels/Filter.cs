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
    public abstract class Filter
    {
        public Bitmap uiBackground { get; private set; } // background in app ui
        public string name { get; private set; } // filter's name
        public Filter filter { get; private set; } // instance of the filter in order to create it once
        public Bitmap Apply(Bitmap img) // apply the filter on the image (here each algorithm will be applied)
        {
            return img; // temp to override
        }
        private Filter() // private constructor
        {
        }
        public Filter GetInstance() // implement Singleton design pattern
        {
            return filter; // temp to override
        }

    }
}