using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Provider;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Firebase.Firestore;
using Java.IO;
using Java.Nio;
using Java.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace Filtr
{
    [Activity(Label = "CreatorActivity")]
    public class CreatorActivity : Activity
    {
        Bitmap original, applied;
        private string appliedFilter;
        View p;
        ImageView ivContainer;
        string photoMethod;
        Button btnExit, btnSave, btnPost, btnNoFilter, btnMonoFilter, btnPixelFilter, btnAsciiFilter;
        HashMap filteredImages;
        const int CAMERA = 0, GALLERY = 1;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.creator_page);
            p = FindViewById(Resource.Id.creator_page);
            SetupFonts();
            ConnectViews();
            filteredImages = new HashMap();

            filteredImages.Put("original", Live.editedBitmap);

            string action = Intent.GetStringExtra("action");

            if (action != null)
            {
                if (action == "camera")
                {
                    Intent = new Intent(MediaStore.ActionImageCapture);
                    StartActivityForResult(Intent, CAMERA);
                }
                if (action == "gallery")
                {
                    Intent = new Intent();
                    Intent.SetType("image/*");
                    Intent.SetAction(Intent.ActionGetContent);
                    StartActivityForResult(Intent, GALLERY);
                }
            }
        }
        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            if (resultCode != Result.Ok || data == null) return;
            if (requestCode == GALLERY)
            {
                Android.Net.Uri uri = data.Data;

                var ins = ContentResolver.OpenInputStream(uri);
                original = BitmapFactory.DecodeStream(ins);
                ImageHelper.CropToSquare(original);

                filteredImages.Put("original", original);
                ivContainer.SetImageBitmap(original);
                applied = ImageHelper.GetResizedBitmap(original, 500, 500);
                appliedFilter = "NoFilter";
            }
            else if (requestCode == CAMERA)
            {
                original = (Bitmap)data.Extras.Get("data");
                ImageHelper.CropToSquare(original);

                filteredImages.Put("original", original);
                ivContainer.SetImageBitmap(original);
                applied = ImageHelper.GetResizedBitmap(original, 500, 500);
                appliedFilter = "NoFilter";
            }
        }
        private void ConnectViews()
        {
            ivContainer = (ImageView)FindViewById(Resource.Id.ivcreatorpage);

            btnExit = p.FindViewById<Button>(Resource.Id.btnExit);
            btnPost = p.FindViewById<Button>(Resource.Id.btnPost);
            btnMonoFilter = p.FindViewById<Button>(Resource.Id.btnMonoFilter);
            btnNoFilter = p.FindViewById<Button>(Resource.Id.btnNoFilter);
            btnPixelFilter = p.FindViewById<Button>(Resource.Id.btnPixelFilter);
            btnAsciiFilter = p.FindViewById<Button>(Resource.Id.btnAsciiFilter);

            btnExit.Click += BtnExit_Click;
            btnNoFilter.Click += BtnNoFilter_Click;
            btnMonoFilter.Click += BtnMonoFilter_Click;
            btnPost.Click += BtnPost_Click;
            btnPixelFilter.Click += BtnPixelFilter_Click;
            btnAsciiFilter.Click += BtnAsciiFilter_Click;
        }

        private void BtnAsciiFilter_Click(object sender, EventArgs e)
        {
            //Console.WriteLine("OKOK");
            if ((Bitmap)filteredImages.Get("ascii") == null)
            {
                Bitmap bm = AsciiFilter.Apply((Bitmap)filteredImages.Get("original"), this);
                filteredImages.Put("ascii", bm);
            }

            applied = (Bitmap)filteredImages.Get("ascii");
            ivContainer.SetImageBitmap(applied);
            appliedFilter = "ascii";
        }

        private void BtnPixelFilter_Click(object sender, EventArgs e)
        {
            //Console.WriteLine("OKOK");
            if ((Bitmap)filteredImages.Get("pixel") == null)
            {
                Bitmap bm = PixelFilter.Apply((Bitmap)filteredImages.Get("original"));
                filteredImages.Put("pixel", bm);
            }

            applied = (Bitmap)filteredImages.Get("pixel");
            ivContainer.SetImageBitmap(applied);
            appliedFilter = "pixelate";
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            SaveImageToExternalStorage(applied);
        }

        private void BtnPost_Click(object sender, EventArgs e)
        {
            if (applied != null)
            {
                HashMap post = new HashMap();
                post.Put("c_Fname", Live.user.Fname);
                post.Put("c_Lname", Live.user.Lname);
                post.Put("content", ImageHelper.BitmapToBase64(applied));
                post.Put("creator", Live.user.id);
                post.Put("filter", appliedFilter);
                post.Put("likedBy", new Java.Util.ArrayList());

                DocumentReference postRef = Live.db.Collection("posts").Document();
                postRef.Set(post);

                Intent = new Intent(this, typeof(HomeActivity));
                StartActivity(Intent);
                Finish();
            }
        }
        private void SaveImageToExternalStorage(Bitmap final)
        {
            string root = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryPictures)
                .ToString();
            Java.IO.File myDir = new Java.IO.File(root + "/saved_images");
            myDir.Mkdirs();
            System.Random gen = new System.Random();
            int n = 10000;
            n = gen.Next(n);
            string fname = "Image-" + n + ".jpg";
            Java.IO.File f = new Java.IO.File(myDir, fname);
            if (f.Exists()) f.Delete();
            try
            {
                string path = System.IO.Path.Combine(myDir.AbsolutePath.ToString(), fname);
                var fs = new FileStream(path, FileMode.Create);
                if (fs != null)
                {
                    final.Compress(Bitmap.CompressFormat.Png, 90, fs);
                }
                fs.Flush();
                fs.Close();
            }
            catch (Exception ex)
            {
                Toast.MakeText(this, ex.ToString(), ToastLength.Long).Show();
            }
        }
        private void BtnMonoFilter_Click(object sender, EventArgs e)
        {
            //Console.WriteLine("OKOK");
            if ((Bitmap)filteredImages.Get("mono") == null)
            {
                Bitmap bm = MonochromeFilter.Apply((Bitmap)filteredImages.Get("original"), 500, 500);
                filteredImages.Put("mono", bm);
            }

            applied = (Bitmap)filteredImages.Get("mono");
            ivContainer.SetImageBitmap(applied);
            appliedFilter = "monochrome";
        }
        private void BtnNoFilter_Click(object sender, EventArgs e)
        {
            applied = (Bitmap)filteredImages.Get("original");
            ivContainer.SetImageBitmap(applied);
            appliedFilter = "nofilter";
        }
        private void BtnExit_Click(object sender, EventArgs e)
        {
            Finish();
        }
        private void SetupFonts()
        {
            #region textfields, subhead, Footer text (Regular Poppins)
            Typeface tf = Typeface.CreateFromAsset(Assets, "Poppins-Regular.ttf");
            Button btn = (Button)p.FindViewById(Resource.Id.btnPost);
            btn.SetTypeface(tf, TypefaceStyle.Normal);
            #endregion
        }
    }
}