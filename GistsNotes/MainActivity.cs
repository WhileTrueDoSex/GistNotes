using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V4.View;
using Android.Support.V7.App;
using Android.Support.V7.Widget;
using Android.Views;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;

namespace GistsNotes
{
    [Activity(Label = "GistsNotes", WindowSoftInputMode = SoftInput.AdjustResize)]
    public class MainActivity : AppCompatActivity
    {
        private ViewPager _pager;
        private RecyclerView _recyclerView;
        private ScreenSlidePagerAdapter _screenSlidePagerAdapter;
        private AppCompatTextView _appCompatTextView;
        private List<GistPreview> _listOfLocalGists = new List<GistPreview>();

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.Main);

            _pager = FindViewById<ViewPager>(Resource.Id.pager);
            _screenSlidePagerAdapter = new ScreenSlidePagerAdapter(SupportFragmentManager);
            _pager.Adapter = _screenSlidePagerAdapter;

            _recyclerView = FindViewById<RecyclerView>(Resource.Id.rvn);
            var fabRight = FindViewById<FloatingActionButton>(Resource.Id.fab_right);
            var fabLeft = FindViewById<FloatingActionButton>(Resource.Id.fab_left);

            _appCompatTextView = FindViewById<AppCompatTextView>(Resource.Id.no_notes);

            fabRight.Click += (sender, args) =>
            {
                var gist = _screenSlidePagerAdapter.GistPreviews[_pager.CurrentItem];
                var intent = new Intent(this, typeof(NoteActivity)).PutExtra("gist", JsonConvert.SerializeObject(gist));
                StartActivity(intent);
            };

            fabLeft.Click += (sender, args) =>
            {
                StartActivity(typeof(MyGistsAndNotesActivity));
            };

            var llm = new LinearLayoutManager(this);
            _recyclerView.SetLayoutManager(llm);

            _pager.PageSelected += (sender, args) =>
            {
                var note = _screenSlidePagerAdapter.GistPreviews[_pager.CurrentItem].Notes;
                var adapter = new NotesAdapter(note);
                _recyclerView.SetAdapter(adapter);

                _appCompatTextView.Visibility = note.Count != 0 ? ViewStates.Gone : ViewStates.Visible;
            };

            var filePath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal) +
                                                     "/data.json";

            if (System.IO.File.Exists(filePath))
                _listOfLocalGists = JsonConvert.DeserializeObject<List<GistPreview>>(System.IO.File.ReadAllText(filePath));
        }

        protected override void OnNewIntent(Intent intent)
        {
            base.OnNewIntent(intent);
            var gistPreview = JsonConvert.DeserializeObject<GistPreview>(intent.GetStringExtra("gist"));
            _screenSlidePagerAdapter.GistPreviews[_pager.CurrentItem] = gistPreview;
            _screenSlidePagerAdapter.NotifyDataSetChanged();

            var adapter = new NotesAdapter(gistPreview.Notes);
            _recyclerView.SetAdapter(adapter);
            _appCompatTextView.Visibility = ViewStates.Gone;

            if (_listOfLocalGists.Any(x => x.Id == gistPreview.Id))
                _listOfLocalGists[_listOfLocalGists.FindIndex(x => x.Id == gistPreview.Id)] = gistPreview;
            else
                _listOfLocalGists.Add(gistPreview);

            System.IO.File.WriteAllText(
                System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal)
                + "/data.json",
                JsonConvert.SerializeObject(_listOfLocalGists));
        }
    }
}

