using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using GistsNotes;
using Newtonsoft.Json;

namespace GistsNotes
{
    [Serializable]
    public class DetailedGist : GistPreview
    {
        public Dictionary<string, File> Files { get; set; }
        public List<History> History { get; set; }
        public Bitmap Img { get; set; }
    }

    public class File
    {
        public string Filename { get; set; }
        public string Type { get; set; }
        public string Language { get; set; }
        public string Content { get; set; }
    }

    public class User
    {
        public string Login { get; set; }

        [JsonProperty("avatar_url", Required = Required.Always)]
        public string AvatarUrl { get; set; }
    }

    public class History
    {
        public User User { get; set; }
    }

}