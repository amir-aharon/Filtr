using Android.Graphics;
using Android.Util;
using System;
using System.IO;

namespace Filtr
{
    public class ImageHelper
    {
        public static string BitmapToBase64(Bitmap bitmap) // gets bitmap -> returns Base64 string 
        {
            string str = "";
            using (var stream = new MemoryStream())
            {
                bitmap.Compress(Bitmap.CompressFormat.Png, 0, stream);
                var bytes = stream.ToArray();
                str = Convert.ToBase64String(bytes);
            }
            return str;
        }
        public static Bitmap Base64ToBitmap(string base64String) // gets Base64 string -> returns bitmap 
        {
            byte[] imageAsBytes = Base64.Decode(base64String, Base64Flags.Default);
            return BitmapFactory.DecodeByteArray(imageAsBytes, 0, imageAsBytes.Length);
        }
        public static Bitmap CropToSquare(Bitmap bitmap) // gets bitmap -> returns bitmap in 1:1 ratio (cropped) 
        {
            int width = bitmap.Width;
            int height = bitmap.Height;
            int newWidth = (height > width) ? width : height;
            int newHeight = (height > width) ? height - (height - width) : height;
            int cropW = (width - height) / 2;
            cropW = (cropW < 0) ? 0 : cropW;
            int cropH = (height - width) / 2;
            cropH = (cropH < 0) ? 0 : cropH;
            Bitmap cropImg = Bitmap.CreateBitmap(bitmap, cropW, cropH, newWidth, newHeight);

            return cropImg;
        }
        public static Bitmap GetResizedBitmap(Bitmap image, int newHeight, int newWidth) // gets bitmap and dimensions -> return the bitmap in these dimensions 
        {
            int width = image.Width;
            int height = image.Height;
            float scaleWidth = ((float)newWidth) / width;
            float scaleHeight = ((float)newHeight) / height;
            // create a matrix for the manipulation
            Matrix matrix = new Matrix();
            // resize the bit map
            matrix.PostScale(scaleWidth, scaleHeight);
            // recreate the new Bitmap
            Bitmap resizedBitmap = Bitmap.CreateBitmap(image, 0, 0, width, height,
                    matrix, false);
            return resizedBitmap;
        }
    }
}