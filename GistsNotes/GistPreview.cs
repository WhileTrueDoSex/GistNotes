using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace GistsNotes
{
    [Serializable]
    public class GistPreview
    {
        public string URL { get; set; }

        [JsonProperty("html_url", Required = Required.Always)]
        public string HtmlURL { get; set; }

        public string Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string Description { get; set; }
        public List<Note> Notes = new List<Note>();
    }
}