using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;

namespace GistsNotes
{
    public class FilesAdapter : RecyclerView.Adapter
    {
        private readonly List<File> _files;

        public FilesAdapter(List<File> files)
        {
            _files = files;
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            var vh = holder as GistHolder;

            vh.Content.Text = _files[position].Content;
            vh.Title.Text = _files[position].Filename;
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            var itemView = LayoutInflater.From(parent.Context).
                       Inflate(Resource.Layout.CardViewFiles, parent, false);
            var vh = new GistHolder(itemView);
            return vh;
        }

        public override int ItemCount => _files.Count;

        private class GistHolder : RecyclerView.ViewHolder
        {
            public TextView Title { get; }
            public TextView Content { get; }

            public GistHolder(View itemView) : base(itemView)
            {
                Content = itemView.FindViewById<TextView>(Resource.Id.content);
                Title = itemView.FindViewById<TextView>(Resource.Id.title);
            }
        }
    }
}