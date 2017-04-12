using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace GistsNotes
{
    public class Note
    {
        public string Text { get; set; }
        public string Name { get; set; }

        public Note(string name, string text)
        {
            Name = name;
            Text = text;
        }
    }
}