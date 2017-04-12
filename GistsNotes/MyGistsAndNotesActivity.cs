using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V7.App;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;

namespace GistsNotes
{
    [Activity(Label = "MyGistsAndNotes")]
    public class MyGistsAndNotesActivity : AppCompatActivity
    {
        private static RecyclerView _recyclerView;
        private static GistsAdapter _adapter;
        private static List<GistPreview> _gistPreviews;
        private static List<DetailedGist> _detailedGists;
        private static ProgressBar _progressBar;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.ListOfGists);

            _detailedGists = new List<DetailedGist>();

            _gistPreviews = JsonConvert.DeserializeObject<List<GistPreview>>(
                System.IO.File.ReadAllText(System.Environment.GetFolderPath(
                    System.Environment.SpecialFolder.Personal) +
                                       "/data.json"));

            _recyclerView = FindViewById<RecyclerView>(Resource.Id.rv);
            _progressBar = FindViewById<ProgressBar>(Resource.Id.progressBar);

            var llm = new LinearLayoutManager(this);
            _recyclerView.SetLayoutManager(llm);

            _adapter = new GistsAdapter();

            new BackgroundWork().Execute();

            _adapter.ItemClick += (sender, i) =>
            {
                var intent = new Intent(this, typeof(DetailedGistActivity)).PutExtra("gist", JsonConvert.SerializeObject(_detailedGists[i]));
                StartActivity(intent);
            };
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

                _adapter = new GistsAdapter(_detailedGists);
                _recyclerView.SetAdapter(_adapter);
                base.OnPostExecute(result);
            }

            protected override bool RunInBackground(params string[] @params)
            {
                _gistPreviews.ForEach(x => _detailedGists.Add(x?.GetFullInfo()));

                return true;
            }
        }
    }
}