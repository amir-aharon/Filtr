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
using System.Threading.Tasks;

namespace Filtr
{
    [Activity(Label = "CreatorActivity")]
    public class CreatorActivity : Activity
    {
        #region setup
        ProgressBar loadingSign;
        Bitmap original, applied;
        private string appliedFilter;
        View p;
        ImageView ivContainer;
        Button btnExit, btnPost, btnNoFilter, btnMonoFilter, btnPixelFilter, btnAsciiFilter;
        HashMap filteredImages;
        const int CAMERA = 0, GALLERY = 1;
        protected override void OnCreate(Bundle savedInstanceState) 
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.creator_page);
            p = FindViewById(Resource.Id.creator_page);
            SetupFonts();
            ConnectViews();

            // cache for applied filters
            filteredImages = new HashMap();

            // set original image in cache
            filteredImages.Put("original", Live.editedBitmap);

            // check for action
            string action = Intent.GetStringExtra("action");

            if (action != null) // there was an action
            {
                if (action == "camera") // camera intent action
                {
                    Intent = new Intent(MediaStore.ActionImageCapture);
                    StartActivityForResult(Intent, CAMERA);
                }
                else if (action == "gallery") // gallery intent action
                {
                    Intent = new Intent();
                    Intent.SetType("image/*");
                    Intent.SetAction(Intent.ActionGetContent);
                    StartActivityForResult(Intent, GALLERY);
                }
                else // unknown action
                    Finish();
            }
            else // there was an action
                Finish();
        }
        private void ConnectViews() // connects to the ui components 
        {
            ivContainer = (ImageView)FindViewById(Resource.Id.ivcreatorpage);

            btnExit = p.FindViewById<Button>(Resource.Id.btnExit);
            btnPost = p.FindViewById<Button>(Resource.Id.btnPost);
            btnMonoFilter = p.FindViewById<Button>(Resource.Id.btnMonoFilter);
            btnNoFilter = p.FindViewById<Button>(Resource.Id.btnNoFilter);
            btnPixelFilter = p.FindViewById<Button>(Resource.Id.btnPixelFilter);
            btnAsciiFilter = p.FindViewById<Button>(Resource.Id.btnAsciiFilter);

            loadingSign = p.FindViewById<ProgressBar>(Resource.Id.loadingSign);
            loadingSign.Visibility = ViewStates.Visible;

            btnExit.Click += ExitPage;
            btnNoFilter.Click += NoFilter;
            btnMonoFilter.Click += MonochromeFilter;
            btnPost.Click += PostImage;
            btnPixelFilter.Click += PixelFilter;
            btnAsciiFilter.Click += AsciiFilter;
        }
        private void SetupFonts() // sets of the fonts for the ui components 
        {
            #region textfields, subhead, Footer text (Regular Poppins)
            Typeface tf = Typeface.CreateFromAsset(Assets, "Poppins-Regular.ttf");
            Button btn = (Button)p.FindViewById(Resource.Id.btnPost);
            btn.SetTypeface(tf, TypefaceStyle.Normal);
            #endregion
        }
        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data) // extracts and processes image from intent 
        {
            if (resultCode != Result.Ok || data == null) return; // check for success
            if (requestCode == GALLERY) // gallery action
            {
                // extract image from intent
                Android.Net.Uri uri = data.Data;
                var ins = ContentResolver.OpenInputStream(uri);
                original = BitmapFactory.DecodeStream(ins);
            }
            else if (requestCode == CAMERA) // camera action
            {
                // extract image from intent
                original = (Bitmap)data.Extras.Get("data");
            }
            else
                return; // unknown intent

            // crop image to square and normalize quality
            ImageHelper.CropToSquare(original);
            applied = ImageHelper.GetResizedBitmap(original, 500, 500);

            // save to cache
            filteredImages.Put("original", applied);

            // display image
            ivContainer.SetImageBitmap(original);

            // will appear if the image will be posted
            appliedFilter = "NoFilter";
        }
        #endregion

        #region events
        private void PostImage(object sender, EventArgs e) // posts image to DB 
        {
            if (applied != null) // make sure there is an image to post
            {
                // organize data in Firestore format
                HashMap post = new HashMap();
                post.Put("c_Fname", Live.user.Fname);
                post.Put("c_Lname", Live.user.Lname);
                post.Put("content", ImageHelper.BitmapToBase64(applied));
                post.Put("creator", Live.user.id);
                post.Put("filter", appliedFilter);
                post.Put("likedBy", new Java.Util.ArrayList());

                // creates new entity in posts collection
                DocumentReference postRef = Live.db.Collection("posts").Document();
                
                // sets the data in this entity
                postRef.Set(post);

                // comes back to home activity
                Intent = new Intent(this, typeof(HomeActivity));
                StartActivity(Intent);
                Finish();
            }
        }
        private void ExitPage(object sender, EventArgs e) // exits page 
        {
            Finish();
        }
        #endregion

        #region filters
        private void NoFilter(object sender, EventArgs e) // removes any filter 
        {
            // return to cached original image
            applied = (Bitmap)filteredImages.Get("original");

            // displays original image
            ivContainer.SetImageBitmap(applied);

            // update image's #
            appliedFilter = "nofilter";
        }
        private async void MonochromeFilter(object sender, EventArgs e) // applies monochrome filter 
        {
            // check if there isn't any cached monochrome image
            if ((Bitmap)filteredImages.Get("mono") == null)
            {
                // appliy the filter (algorithm)
                Bitmap bm = await Filtr.MonochromeFilter.Apply((Bitmap)filteredImages.Get("original"), 500, 500, loadingSign);
                
                // save to cache
                filteredImages.Put("mono", bm);
                //loadingSign.Visibility = ViewStates.Invisible;
            }

            // get monochrome image from cache
            applied = (Bitmap)filteredImages.Get("mono");

            // display image
            ivContainer.SetImageBitmap(applied);

            // update image's #
            appliedFilter = "monochrome";

            loadingSign.Visibility = ViewStates.Invisible;
        }
        private void PixelFilter(object sender, EventArgs e) // applies pixel filter
        {
            // check if there isn't any cached pixel image
            if ((Bitmap)filteredImages.Get("pixel") == null)
            {
                // appliy the filter (algorithm)
                Bitmap bm = Filtr.PixelFilter.Apply((Bitmap)filteredImages.Get("original"));

                // save to cache
                filteredImages.Put("pixel", bm);
            }

            // get pixel image from cache
            applied = (Bitmap)filteredImages.Get("pixel");

            // display image
            ivContainer.SetImageBitmap(applied);

            // update image's #
            appliedFilter = "pixelate";
        }
        private async Task TurnLoad()
        {
            loadingSign.Visibility = ViewStates.Visible;
            return;
        }
        private async void AsciiFilter(object sender, EventArgs e) // applies ascii filter
        {
            await TurnLoad();
            // check if there isn't any cached pixel image
            if ((Bitmap)filteredImages.Get("ascii") == null)
            {
                // appliy the filter (algorithm)
                Bitmap bm = await Filtr.AsciiFilter.Apply((Bitmap)filteredImages.Get("original"), this, loadingSign);

                // save to cache
                filteredImages.Put("ascii", bm);
            }

            // get ascii image from cache
            applied = (Bitmap)filteredImages.Get("ascii");

            // display image
            ivContainer.SetImageBitmap(applied);

            // update image's #
            appliedFilter = "ascii";
            loadingSign.Visibility= ViewStates.Invisible;
        }
        #endregion
    }
}