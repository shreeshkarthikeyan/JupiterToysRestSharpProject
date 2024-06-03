using Newtonsoft.Json;

namespace JupiterToysRestSharpProject.Model
{
    public class Address
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("line1")]
        public string Line1 { get; set; }
        [JsonProperty("line2")]
        public string Line2 { get; set; }
        [JsonProperty("city")]
        public string City { get; set; }
        [JsonProperty("postcode")]
        public string Postcode { get; set; }
        [JsonProperty("state")]
        public string State { get; set; }
        [JsonProperty("addresstype")]
        public string AddressType { get; set; }
        [JsonProperty("deliveryName")]
        public string DeliveryName { get; set; }
    }
}