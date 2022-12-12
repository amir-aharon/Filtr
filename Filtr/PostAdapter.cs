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
    public class PostAdapter : BaseAdapter<Post>
    {
        Context context;
        List<Post> objects;
        public static string type;
        public PostAdapter(Context ctx, List<Post> stds)
        {
            this.context = ctx;
            this.objects = stds;
        }
        public List<Post> GetList()
        {
            return this.objects;
        }
        public override int Count
        {
            get { return this.objects.Count; }
        }
        public override Post this[int position]
        {
            get { return this.objects[position]; }
        }
        public override long GetItemId(int position)
        {
            return position;
        }
        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            if (type == "Home")
            {
                LayoutInflater layoutInflater = ((HomeActivity)context).LayoutInflater;
                View view = layoutInflater.Inflate(Resource.Layout.post_component_full, parent, false);
                TextView tvUser = (TextView)view.FindViewById(Resource.Id.tvUser);
                TextView tvFilter = (TextView)view.FindViewById(Resource.Id.tvFilter);
                FrameLayout btnLike = (FrameLayout)view.FindViewById(Resource.Id.btnLike);
                ImageView fullIcon = (ImageView)view.FindViewById(Resource.Id.fullIcon);
                ImageView emptyIcon = (ImageView)view.FindViewById(Resource.Id.emptyIcon);
                ImageView ivContent = (ImageView)view.FindViewById(Resource.Id.ivContent);

                Post temp = objects[position];
                if (temp != null)
                {
                    tvUser.Text = "By " + temp.creator.Fname + " " + temp.creator.Lname;
                    tvFilter.Text = "#" + (temp.filter != null ? PixelFilter.filter.name : "nofilter");

                    btnLike.Click += ToogleLike;
                }
                return view;
            }

            return null;
        }

        private void ToogleLike(object sender, EventArgs e)
        {
            FrameLayout frameLayout = (FrameLayout)sender;
            ImageView fullIcon = (ImageView)frameLayout.GetChildAt(0);
            ImageView emptyIcon = (ImageView)frameLayout.GetChildAt(1);

            if (fullIcon.Visibility == ViewStates.Visible)
            {
                AnimaitonHelper.ScaleOut(context, fullIcon);
            } 
            else
            {
                AnimaitonHelper.ScaleIn(context, fullIcon);
            }
        }
    }
}