using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V7.View.Menu;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Views.Animations;
using Android.Widget;

namespace GistsNotes
{
    public class GistsAdapter : RecyclerView.Adapter
    {
        private readonly List<DetailedGist> _gists;
        private View _itemView;
        public override int ItemCount => _gists.Count;
        public event EventHandler<int> ItemClick;

        public GistsAdapter(List<DetailedGist> gists)
        {
            _gists = gists;
        }

        public GistsAdapter() {}

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            var vh = holder as GistHolder;

            vh.Avatar.SetImageBitmap(_gists[position].Img);
            vh.Login.Text = _gists[position].History.First().User.Login;
            vh.Description.Text = _gists[position].Description;

            var llm = new LinearLayoutManager(_itemView.Context);
            vh.Notes.SetLayoutManager(llm);

            vh.Notes.SetAdapter(new NotesAdapter(_gists[position].Notes));
        }

        private void OnClick(int position)
        {
            ItemClick?.Invoke(this, position);
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            _itemView = LayoutInflater.From(parent.Context).
                Inflate(Resource.Layout.CardViewGists, parent, false);
            var vh = new GistHolder(_itemView, OnClick);
            return vh;
        }

        private class GistHolder : RecyclerView.ViewHolder
        {
            public TextView Login { get; }
            public TextView Description { get; }
            public ImageView Avatar { get; }
            public RecyclerView Notes { get; }

            public GistHolder(View itemView, Action<int> listener) : base(itemView)
            {
                Login = itemView.FindViewById<TextView>(Resource.Id.login);
                Description = itemView.FindViewById<TextView>(Resource.Id.desc);
                Avatar = itemView.FindViewById<ImageView>(Resource.Id.avatar);
                Notes = itemView.FindViewById<RecyclerView>(Resource.Id.rv);

                itemView.Click += (sender, e) => listener(base.Position);
            }
        }
    }
}