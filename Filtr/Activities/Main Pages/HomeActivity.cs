﻿using Android.App;
using Android.Content;
using Android.Gms.Tasks;
using Android.Graphics;
using Android.Media;
using Android.OS;
using Android.Provider;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Firebase.Firestore;
using Java.IO;
using System;
using System.Collections.Generic;
using System.IO;
using Xamarin.Essentials;

namespace Filtr
{
    [Activity(Label = "HomeActivity")]
    public class HomeActivity : Activity, IOnSuccessListener
    {
        LinearLayout navHome, navSearch, navAccount, navLiked;
        Button btnPlus;
        View p;
        ListView lv;
        PostAdapter adapter;
        string photoMethod;
        public override void OnBackPressed()
        {
            return;
        }
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.home_page);

            // Create your application here

            p = FindViewById(Resource.Id.home_page);
            SetNavbarButtons();

            PostAdapter.type = "Home";
            lv = p.FindViewById<ListView>(Resource.Id.lv);
            //Live.user = new User("Vk1Moxq8vvlsp1evNWCs", "email@address.com", "aaaa1234", "Amir", "Aharon");
            LoadPosts();

            SetupFonts();
        }
        public void LoadPosts()
        {
            Live.db.Collection("posts").WhereNotEqualTo("creator", Live.user.id).Get().AddOnSuccessListener(this);
        }
        private void SetNavbarButtons()
        {
            navSearch = (LinearLayout)p.FindViewById(Resource.Id.navSearch);
            navSearch.Click += NavSearch_Click;
            navAccount = (LinearLayout)p.FindViewById(Resource.Id.navAccount);
            navAccount.Click += NavAccount_Click;
            navLiked = (LinearLayout)p.FindViewById(Resource.Id.navLiked);
            navLiked.Click += NavLiked_Click;
            btnPlus = (Button)p.FindViewById(Resource.Id.btnPlus);
            btnPlus.Click += BtnPlus_Click;
        }

        private void BtnPlus_Click(object sender, EventArgs e)
        {
            Dialog d = new Dialog(this);
            d.SetContentView(Resource.Layout.take_a_photo_dialog);

            #region setup fonts
            TextView tvCamera, tvGallery;
            Typeface tf = Typeface.CreateFromAsset(Assets, "Poppins-Regular.ttf");

            tvCamera = (TextView)d.FindViewById(Resource.Id.tvCamera);
            tvGallery = (TextView)d.FindViewById(Resource.Id.tvGallery);

            tvCamera.SetTypeface(tf, TypefaceStyle.Normal);
            tvGallery.SetTypeface(tf, TypefaceStyle.Normal);
            #endregion

            LinearLayout llCamera, llGallery;
            llCamera = (LinearLayout)d.FindViewById(Resource.Id.llCamera);
            llGallery = (LinearLayout)d.FindViewById(Resource.Id.llGallery);

            llCamera.Click += LlCamera_Click;
            llGallery.Click += LlGallery_Click;
            d.Show();
        }

        private void LlGallery_Click(object sender, EventArgs e)
        {
            photoMethod = "Gallery";

            Intent = new Intent(this, typeof(CreatorActivity));
            //Intent.SetType("image/*");
            //Intent.SetAction(Intent.ActionGetContent);
            Intent.PutExtra("action", "gallery");
            //StartActivityForResult(Intent, 0);
            StartActivity(Intent);
        }
        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            if (resultCode != Result.Ok || data == null) return;
            if (requestCode == 0)
            {
                Android.Net.Uri uri = data.Data;

                Intent it = new Intent(this, typeof(CreatorActivity));
                
                it.SetData(uri);
                //it.PutExtra("img", bitmap);
                StartActivity(it);
            }
            else if (requestCode == 1)
            {
                Live.editedBitmap = (Bitmap)data.Extras.Get("data");
                Bitmap img = (Bitmap)data.Extras.Get("data");
                // Generate a RANDOM number  between 0 to 9999 - for the file name
                Random generator = new Random();
                int n = 10000;
                n = generator.Next(n);      // n = the genrated random number
                string ImageName = "Image-" + n + ".png";       // This will be the file name: Image-<n>.jpg

                Java.IO.File folderPath = Android.OS.Environment.ExternalStorageDirectory;  //initial path

                string directoryName = "MyAppImages";       // This is the Folder name where the picture will be written
                Java.IO.File dir = new Java.IO.File(folderPath.AbsolutePath + "/" + directoryName); //directory path
                dir.Mkdirs();       //creates directories to path

                string path = System.IO.Path.Combine(dir.Path, ImageName);    //create the whole path of: <Folder>+<imageName> + <.jpg> is the image format

                FileStream fs = new FileStream(path, FileMode.OpenOrCreate);
                img.Compress(Bitmap.CompressFormat.Png, 100, fs);      // Apply compression on seleced file <imageToSave> and save to <fs>

                MediaScannerConnection.ScanFile(Application.Context, new string[] { path }, null, null);        // Update this picture in system's Gallery 
                fs.Close();

                Intent it = new Intent(this, typeof(CreatorActivity));
                StartActivity(it);
            }
        }

        private void LlCamera_Click(object sender, EventArgs e)
        {
            photoMethod = "Camera";

            Intent = new Intent(this, typeof(CreatorActivity));
            //Intent = new Intent(MediaStore.ActionImageCapture);
            //StartActivityForResult(Intent, 1);
            Intent.PutExtra("action", "camera");
            StartActivity(Intent);

            //var result = await MediaPicker.CapturePhotoAsync();

            //var stream = await result.OpenReadAsync();


        }
        //protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        //{
        //    base.OnActivityResult(requestCode, resultCode, data);
        //    if (requestCode == 0)
        //    {
        //        if (resultCode == Result.Ok)
        //        {
        //            Bitmap image = (Bitmap)data.Extras.Get("data");

        //            //convert bitmap into byte array
        //            byte[] bitmapData;
        //            using (var stream = new MemoryStream())
        //            {
        //                image.Compress(Bitmap.CompressFormat.Png, 0, stream);
        //                bitmapData = stream.ToArray();
        //            }


        //            Intent it = new Intent(this, typeof(CreatorActivity));

        //            it.PutExtra("content", bitmapData);
        //            StartActivity(it);
        //        }
        //    }
        //}

        private void NavLiked_Click(object sender, EventArgs e)
        {
            NavbarHelper.LikedButton(this);
        }

        private void NavAccount_Click(object sender, EventArgs e)
        {
            NavbarHelper.AccountButton(this);
        }
        private void NavSearch_Click(object sender, EventArgs e)
        {
            NavbarHelper.SearchButton(this);
        }
        private void SetupFonts()
        {
            #region textfields, subhead, Footer text (Regular Poppins)
            Typeface tf = Typeface.CreateFromAsset(Assets, "Poppins-Regular.ttf");
            TextView text = (TextView)p.FindViewById(Resource.Id.homeText);
            text.SetTypeface(tf, TypefaceStyle.Normal);
            text = (TextView)p.FindViewById(Resource.Id.searchText);
            text.SetTypeface(tf, TypefaceStyle.Normal);
            text = (TextView)p.FindViewById(Resource.Id.likedText);
            text.SetTypeface(tf, TypefaceStyle.Normal);
            text = (TextView)p.FindViewById(Resource.Id.accountText);
            text.SetTypeface(tf, TypefaceStyle.Normal);

            #endregion
        }

        public void OnSuccess(Java.Lang.Object result)
        {
            var snapshot = (QuerySnapshot)result;

            List<Post> posts = new List<Post>();

            foreach (var post in snapshot.Documents)
            {
                posts.Add(new Post(
                    post.Id,
                    post.GetString("creator"),
                    post.GetString("content"),
                    post.GetString("filter"),
                    post.GetString("c_Fname"),
                    post.GetString("c_Lname")
                    ));
            }

            adapter = new PostAdapter(this, posts);
            lv.Adapter = adapter;
        }
    }
}