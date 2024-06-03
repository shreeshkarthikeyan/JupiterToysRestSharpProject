using Newtonsoft.Json;

namespace JupiterToysRestSharpProject.Model
{
    public class Customer
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("username")]
        public string Username { get; set; }
        [JsonProperty("firstname")]
        public string Firstname { get; set; }
        [JsonProperty("lastname")]
        public string Lastname { get; set; }
        [JsonProperty("gender")]
        public string Gender { get; set; }
        [JsonProperty("phoneNumber")]
        public string PhoneNumber { get; set; }
        [JsonProperty("addresses")]
        public List<Address> Addresses { get; set; }
        [JsonProperty("transactionHistory")]
        public List<TransactionHistory> TransactionHistory { get; set; }

    }
}
