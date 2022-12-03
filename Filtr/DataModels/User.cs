using Android.App;
using Android.Content;
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
    public class User : IDatabaseObject
    {
        public string id { get; private set; } // IDatabaseObject implementation
        public string Fname { get; private set; } // user's first name
        public string Lname { get; private set; } // user's last name
        public string password { get; private set; } // user's password
        public string email { get; private set; } // user's email address
        public List<Post> posts { get; private set; } // user's posts list
        public List<Post> likedPosts { get; private set; } // list of posts that the uaer liked
        public User (string id, string email, string password, string Fname, string Lname)
        {
            this.id = id;
            this.email = email;
            this.password = password;
            this.Fname = Fname;
            this.Lname = Lname;
            posts = new List<Post> ();
            likedPosts = new List<Post> ();
        } // full constructor
    }
}