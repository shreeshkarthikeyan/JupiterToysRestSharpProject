using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JupiterToysRestSharpProject.Model
{
    public class Toy
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("price")]
        public double Price { get; set; }
        [JsonProperty("category")]
        public string Category { get; set; }
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("size")]
        public string Size { get; set; }
        [JsonProperty("image")]
        public string Image { get; set; }
        [JsonProperty("stock")]
        public int Stock { get; set; }
        [JsonProperty("links")]
        public IList<Link> Links { get; set; }
    }
}
