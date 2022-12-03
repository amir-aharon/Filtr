using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Filtr
{
    public class Post : IDatabaseObject
    {
        public string id { get; private set; } // IDatabaseObject implementation
        public User creator { get; private set; } // post's creator
        public Bitmap content { get; private set; } // post's image
        public Filter filter { get; private set; } // post's implemented filter
        public List<User> likedBy { get; private set; } // list of all the users who liked this post

    }
}