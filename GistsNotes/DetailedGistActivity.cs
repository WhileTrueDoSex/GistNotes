using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;
using Toolbar = Android.Support.V7.Widget.Toolbar;

namespace GistsNotes
{
    [Activity(Label = "DetailedGistActivity")]
    public class DetailedGistActivity : AppCompatActivity
    {
        private static ProgressBar _progressBar;
        private static ImageView _bg;
        private static Toolbar _toolbar;
        private static FilesAdapter _adapter;
        private static DetailedGist _detailedGist;
        private static RecyclerView _recyclerView;
        private static GistPreview _gistPreview;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.ScrollingActivity);
            _toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            _bg = FindViewById<ImageView>(Resource.Id.imageViewplaces);
            _progressBar = FindViewById<ProgressBar>(Resource.Id.progressBar);
            _recyclerView = FindViewById<RecyclerView>(Resource.Id.rv);

            var fab = FindViewById<FloatingActionButton>(Resource.Id.fab);

            var llm = new LinearLayoutManager(this);
            _recyclerView.SetLayoutManager(llm);
            _recyclerView.HasFixedSize = true;
            _recyclerView.NestedScrollingEnabled = false;

            _gistPreview = JsonConvert.DeserializeObject<GistPreview>(Intent.GetStringExtra("gist"));

            var intent = new Intent(this, typeof(NoteActivity)).PutExtra("gist", Intent.GetStringExtra("gist"));

            fab.Click += (sender, args) => StartActivity(intent);

            new BackgroundWork().Execute();

            _toolbar.Title = "Detailed gist";
            SetSupportActionBar(_toolbar);
        }

        private class BackgroundWork : AsyncTask<string, int, bool>
        {
            protected override void OnPreExecute()
            {
                _progressBar.Visibility = ViewStates.Visible;

                base.OnPreExecute();
            }

            protected override void OnPostExecute(bool result)
            {
                _progressBar.Visibility = ViewStates.Gone;

                _bg.SetImageBitmap(_detailedGist.Img);

                _adapter = new FilesAdapter(_detailedGist.Files.Values.ToList());
                _recyclerView.SetAdapter(_adapter);

                base.OnPostExecute(result);
            }

            protected override bool RunInBackground(params string[] @params)
            {
                _detailedGist = _gistPreview?.GetFullInfo();
                return true;
            }
        }
    }


}