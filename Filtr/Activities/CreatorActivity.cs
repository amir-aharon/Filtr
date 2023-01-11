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

            #region temp
            //if (original == null)
            //{
            //    var uri = Intent.Data;
            //    if (uri is Android.Net.Uri)
            //    {
            //        BitmapFactory.Options options = new BitmapFactory.Options();
            //        options.InJustDecodeBounds = true;
            //        options.InSampleSize = 3;

            //        
            //        original = ImageHelper.CropToSquare(original);

            //        filteredImages.Put("original", original);


            //        //int size = original.Width * original.Height;
            //        //ByteBuffer byteBuffer = ByteBuffer.Allocate(size);
            //        //original.CopyPixelsToBuffer(byteBuffer);

            //        //byte[] byteArray = (byte[])byteBuffer;

            //        //Bitmap.Config configBmp = Bitmap.Config.ValueOf(original.GetConfig().Name());
            //        //Bitmap bitmap_tmp = Bitmap.CreateBitmap(original.Width, original.Height, configBmp);
            //        //ByteBuffer buffer = ByteBuffer.Wrap(byteArray);
            //        //bitmap_tmp.CopyPixelsFromBuffer(buffer);

            //        //Bitmap bmp = BitmapFactory.DecodeByteArray(byteArray, 0, byteArray.Length, options);
            //    }
            //}
            #endregion
            //if (original == null) Finish();
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
                applied = original;
                appliedFilter = "NoFilter";
            }
            else if (requestCode == CAMERA)
            {
                original = (Bitmap)data.Extras.Get("data");
                ImageHelper.CropToSquare(original);

                filteredImages.Put("original", original);
                ivContainer.SetImageBitmap(original);
                applied = original;
                appliedFilter = "NoFilter";
            }
        }
        private void ConnectViews()
        {
            ivContainer = (ImageView)FindViewById(Resource.Id.ivcreatorpage);

            btnExit = p.FindViewById<Button>(Resource.Id.btnExit);
            btnSave = p.FindViewById<Button>(Resource.Id.btnSave);
            btnPost = p.FindViewById<Button>(Resource.Id.btnPost);
            btnMonoFilter = p.FindViewById<Button>(Resource.Id.btnMonoFilter);
            btnNoFilter = p.FindViewById<Button>(Resource.Id.btnNoFilter);
            btnPixelFilter = p.FindViewById<Button>(Resource.Id.btnPixelFilter);
            btnAsciiFilter = p.FindViewById<Button>(Resource.Id.btnAsciiFilter);

            btnExit.Click += BtnExit_Click;
            btnNoFilter.Click += BtnNoFilter_Click;
            btnMonoFilter.Click += BtnMonoFilter_Click;
            btnPost.Click += BtnPost_Click;
            btnSave.Click += BtnSave_Click;
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
            if ((Bitmap)filteredImages.Get("mono") == null)
            {
                Bitmap bm = MonochromeFilter.Apply((Bitmap)filteredImages.Get("original"));
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
        }
        private void BtnExit_Click(object sender, EventArgs e)
        {
            Finish();
        }
        private void SetupFonts()
        {
            #region textfields, subhead, Footer text (Regular Poppins)
            Typeface tf = Typeface.CreateFromAsset(Assets, "Poppins-Regular.ttf");
            Button btn = (Button)p.FindViewById(Resource.Id.btnSave);
            btn.SetTypeface(tf, TypefaceStyle.Normal);
            btn = (Button)p.FindViewById(Resource.Id.btnPost);
            btn.SetTypeface(tf, TypefaceStyle.Normal);
            #endregion
        }
    }
}