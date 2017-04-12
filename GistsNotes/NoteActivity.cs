using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;

namespace GistsNotes
{
    [Activity(Label = "NoteActivity", Theme = "@style/NoteTheme", WindowSoftInputMode = SoftInput.AdjustResize)]
    public class NoteActivity : AppCompatActivity
    {
        private EditText _name;
        private EditText _text;

        private GistPreview _gistPreview;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.NoteActivity);

            SupportActionBar.SetHomeButtonEnabled(true);
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);

            _gistPreview = JsonConvert.DeserializeObject<GistPreview>(Intent.GetStringExtra("gist"));

            _name = FindViewById<EditText>(Resource.Id.note_name);
            _text = FindViewById<EditText>(Resource.Id.note_text);

            SupportActionBar.Title = "Note for a gist";
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.MenuNote, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    Finish();
                    return true;

                case Resource.Id.done:
                    _gistPreview.Notes.Add(new Note(_name.Text, _text.Text));
                    var intent = new Intent(this, typeof(MainActivity))
                                 .PutExtra("gist", JsonConvert.SerializeObject(_gistPreview));
                    intent.SetFlags(ActivityFlags.ClearTop | ActivityFlags.SingleTop);
                    StartActivity(intent);
                    return true;

                default:
                    return base.OnOptionsItemSelected(item);
            }
        }
    }
}