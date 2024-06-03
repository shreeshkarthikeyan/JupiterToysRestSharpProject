using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JupiterToysRestSharpProject.Model
{
    public class Link
    {
        [JsonProperty("rel")]
        public string Rel { get; set; }
        [JsonProperty("href")]
        public string Href { get; set; }
        [JsonProperty("hreflang")]
        public string Hreflang { get; set; }
        [JsonProperty("media")]
        public string Media { get; set; }
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("image")]
        public string Image { get; set; }
        [JsonProperty("type")]
        public string Type { get; set; }
        [JsonProperty("deprecation")]
        public string Deprecation { get; set; }
        [JsonProperty("profile")]
        public string Profile { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
