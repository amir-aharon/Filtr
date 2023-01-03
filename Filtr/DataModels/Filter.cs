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
    public interface Filter
    {
        public static string name { get; } // filter name
        public virtual Bitmap Apply(Bitmap img) // apply the filter on the image (here each algorithm will be applied)
        {
            return img; // temp to override
        }

    }
}