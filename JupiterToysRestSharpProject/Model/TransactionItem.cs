using Newtonsoft.Json;

namespace JupiterToysRestSharpProject.Model
{
    public class TransactionItem
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("toy")]
        public Toy ToyDetails { get; set; }
        [JsonProperty("numberOfToys")]
        public int NumberOfToys { get; set; }
        [JsonProperty("status")]
        public string Status { get; set; }
    }
}