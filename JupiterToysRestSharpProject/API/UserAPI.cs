using JupiterToysRestSharpProject.Model;
using JupiterToysRestSharpProject.Support;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace JupiterToysRestSharpProject.API
{
    public class UserAPI : BaseAPI<Customer>
    {
        public UserAPI(Token token) : base(token) { }

        public string performCreateUserOperation(Customer customer) {
            ExceptionHandler.CheckNullArgument(new List<dynamic> { customer });
            var restClient = SetUrl(Config.readFromPropertiesFile("customerbaseurl"));
            var restRequest = RequestOperation<Customer>(operation: Request.POST, endpoint: "/customer", payload: customer, headers: null);
            var restResponse = GetResponse(restClient, restRequest);
            Console.WriteLine($"Create Customer Response --> {GetContent(restResponse)}");
            dynamic data = JObject.Parse(GetContent(restResponse));
            Console.WriteLine($"Customer Id --> {data.id}");
            return data.id;
        }

        public List<string> addToysToCartOperation(string customerId, TransactionHistory toysAddToCart) {
            ExceptionHandler.CheckNullArgument(new List<dynamic> { customerId, toysAddToCart });
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

        public void UpdatePaymentStatus(string transactionId, TransactionHistory paymentStatusUpdate) {
            ExceptionHandler.CheckNullArgument(new List<dynamic> { transactionId, paymentStatusUpdate });
            var restClient = SetUrl(Config.readFromPropertiesFile("customerbaseurl"));
            var restRequest = RequestOperation<TransactionHistory>(operation: Request.PATCH, endpoint: $"/transaction/{transactionId}", payload: paymentStatusUpdate, headers: null);
            var restResponse = GetResponse(restClient, restRequest);
            Console.WriteLine($"Update Payment Status Response --> {GetContent(restResponse)}");
        }

        public Customer UpdateCustomerAddress(string customerId, Customer customerDetails)
        {
            ExceptionHandler.CheckNullArgument(new List<dynamic> { customerId, customerDetails });
            var restClient = SetUrl(Config.readFromPropertiesFile("customerbaseurl"));
            var restRequest = RequestOperation<Customer>(operation: Request.PATCH, endpoint: $"/customer/{customerId}", payload: customerDetails, headers: null);
            var restResponse = GetResponse(restClient, restRequest);
            //Console.WriteLine($"Update Customer Address Response --> {GetContent(restResponse)}");
            return GetContent<Customer>(restResponse);
        }

        public string DeleteCustomer(string customerId)
        {
            ExceptionHandler.CheckNullArgument(new List<dynamic> { customerId });
            var restClient = SetUrl(Config.readFromPropertiesFile("customerbaseurl"));
            var restRequest = RequestOperation<Customer>(operation: Request.DELETE, endpoint: $"/customer/{customerId}", null, headers: null);
            var restResponse = GetResponse(restClient, restRequest);
            Console.WriteLine($"Delete Customer Response --> {GetContent(restResponse)}");
            return GetContent(restResponse);

        }
    }
}
