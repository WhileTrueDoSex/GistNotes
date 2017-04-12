using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Views.Animations;
using Android.Widget;

namespace GistsNotes
{
    public class NotesAdapter : RecyclerView.Adapter
    {
        private readonly List<Note> _notes;
        public override int ItemCount => _notes.Count;

        public NotesAdapter(List<Note> notes)
        {
            _notes = notes;
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            var vh = holder as NoteHolder;

            vh.Name.Text = _notes[position].Name;
            vh.Text.Text = _notes[position].Text;

            SetAnimation(holder.ItemView, position);
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            var itemView = LayoutInflater.From(parent.Context).
                       Inflate(Resource.Layout.CardViewNotes, parent, false);
            var vh = new NoteHolder(itemView);
            return vh;
        }


        private void SetAnimation(View viewToAnimate, int position)
        {
            var animation = AnimationUtils.LoadAnimation(viewToAnimate.Context, Android.Resource.Animation.SlideInLeft);
            animation.Duration = 200;
            viewToAnimate.StartAnimation(animation);
        }

        private class NoteHolder : RecyclerView.ViewHolder
        {
            public TextView Name { get; }
            public TextView Text { get; }

            public NoteHolder(View itemView) : base(itemView)
            {
                Name = itemView.FindViewById<TextView>(Resource.Id.name_of_note);
                Text = itemView.FindViewById<TextView>(Resource.Id.text_of_note);
            }
        }
    }
}