using JupiterToysRestSharpProject.Model;
using JupiterToysRestSharpProject.Support;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JupiterToysRestSharpProject.API
{
    public class UserAPI : BaseAPI<Customer>
    {
        public string performCreateUserOperation(Customer customer)
        {
            ArgumentNullException.ThrowIfNull(customer);
            var restClient = SetUrl(Config.readFromPropertiesFile("customerbaseurl"));
            var restRequest = RequestOperation<Customer>(operation: Request.POST, endpoint: "/customer", payload: customer, headers: null);
            var restResponse = GetResponse(restClient, restRequest);
            Console.WriteLine($"Create Customer Response --> {GetContent(restResponse)}");
            dynamic data = JObject.Parse(GetContent(restResponse));
            Console.WriteLine($"Customer Id --> {data.id}");
            return data.id;
        }

        public List<string> addToysToCartOperation(String customerId, TransactionHistory toysAddToCart)
        {
            ArgumentNullException.ThrowIfNull(customerId);
            ArgumentNullException.ThrowIfNull(toysAddToCart);
            var restClient = SetUrl(Config.readFromPropertiesFile("customerbaseurl"));
            var restRequest = RequestOperation<TransactionHistory>(operation: Request.PUT, endpoint: $"/customer/{customerId}/purchase", payload: toysAddToCart, headers: null);
            var restResponse = GetResponse(restClient, restRequest);
            Console.WriteLine($"Add Toys to Cart Response --> {GetContent(restResponse)}");
            dynamic data = JObject.Parse(GetContent(restResponse));
            Console.WriteLine($"Transaction Id --> {data.transaction_id}");
            Console.WriteLine($"Order Number --> {data.order_number}");
            return new List<String> { $"Transaction Id -> {data.transaction_id}", 
                                      $"Order Number -> {data.order_number}" };
        }

        public void updatePaymentStatus(String transactionId, TransactionHistory paymentStatusUpdate)
        {
            ArgumentNullException.ThrowIfNull(transactionId);
            ArgumentNullException.ThrowIfNull(paymentStatusUpdate);

            var restClient = SetUrl(Config.readFromPropertiesFile("customerbaseurl"));
            var restRequest = RequestOperation<TransactionHistory>(operation: Request.PATCH, endpoint: $"/transaction/{transactionId}", payload: paymentStatusUpdate, headers: null);
            var restResponse = GetResponse(restClient, restRequest);
            Console.WriteLine($"Update Payment Status Response --> {GetContent(restResponse)}");
        }
    }
}
