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
        public bool newLikes {get; private set;} // flag for when a user had new likes since last time in app
        public User (string id, string email, string password, string Fname, string Lname, bool newLikes) // constructor
        {
            this.id = id;
            this.email = email;
            this.password = password;
            this.Fname = Fname;
            this.Lname = Lname;
            this.newLikes = newLikes;
        }
    }
}