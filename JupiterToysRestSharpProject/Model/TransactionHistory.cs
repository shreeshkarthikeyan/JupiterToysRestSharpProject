using Newtonsoft.Json;

namespace JupiterToysRestSharpProject.Model
{
    public class TransactionHistory
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("transactionItems")]
        public List<TransactionItem> TransactionItems { get; set; }
        [JsonProperty("date")]
        public string Date { get; set; }
        [JsonProperty("cost")]
        public double Cost { get; set; }
        [JsonProperty("paymentStatus")]
        public string PaymentStatus { get; set; }
        [JsonProperty("orderNumber")]
        public string OrderNumber { get; set; }
    }
}