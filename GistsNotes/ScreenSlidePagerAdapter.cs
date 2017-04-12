using System.Collections.Generic;
using Android.Support.V4.App;
using Java.Lang;
using Fragment = Android.Support.V4.App.Fragment;
using FragmentManager = Android.Support.V4.App.FragmentManager;

namespace GistsNotes
{
    public class ScreenSlidePagerAdapter : FragmentStatePagerAdapter
    {
        public List<GistPreview> GistPreviews { get; set; }

        public override int Count => GistPreviews.Count;

        public ScreenSlidePagerAdapter(FragmentManager fm)
            : base(fm)
        {
            GistPreviews = Gists.GetPublicGists();
        }

        public override Fragment GetItem(int position)
        {
            return new ScreenSlidePageFragment(GistPreviews[position]);
        }

        public override int GetItemPosition(Object @object)
        {
            return PositionNone;
        }
    }
    
}