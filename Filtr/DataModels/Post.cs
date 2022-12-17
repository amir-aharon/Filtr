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
        public string creator { get; private set; } // post's creator
        public string cFname { get; private set; } // post's creator
        public string cLname { get; private set; } // post's creator
        public string content { get; private set; } // post's image
        public string filter { get; private set; } // post's implemented filter

        public Post(string id, string creator, string f)
        {
            this.id = id;
            this.creator = creator;
            filter = f;
            content = null;
        }
        public Post(string id, string creator, string content, string filter, string fname, string lname)
        {
            this.id = id;
            this.creator = creator;
            this.filter = filter;
            this.content = content;
            cFname = fname;
            cLname = lname;
        }
    }
}