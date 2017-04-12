using Android.Content;
using Android.OS;
using Android.Views;
using Grantland.Widget;
using Newtonsoft.Json;
using Fragment = Android.Support.V4.App.Fragment;

namespace GistsNotes
{
    public class ScreenSlidePageFragment : Fragment
    {
        public GistPreview Gist { get; set; }

        public ScreenSlidePageFragment(GistPreview gist)
        {
            Gist = gist;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var rootView = (ViewGroup)inflater.Inflate(Resource.Layout.FragmentScreenSlidePage, container, false);
            var text = rootView.FindViewById<AutofitTextView>(Resource.Id.text);

            text.Text = !string.IsNullOrEmpty(Gist.Description) ? Gist.Description : "No description";

            text.Click += (sender, args) =>
            {
                var intent = new Intent(Context, typeof(DetailedGistActivity))
                            .PutExtra("gist", JsonConvert.SerializeObject(Gist));
                StartActivity(intent);
            };

            return rootView;
        }
    }
}