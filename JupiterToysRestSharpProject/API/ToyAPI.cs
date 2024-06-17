using JupiterToysRestSharpProject.Model;
using JupiterToysRestSharpProject.Support;
using Newtonsoft.Json.Linq;
using RestSharp;
using System.Transactions;

namespace JupiterToysRestSharpProject.API
{
    public class ToyAPI : BaseAPI<Toy> 
    {
        public ToyAPI(Token token) : base(token){ }

        public string PerformCreateToyOperation(Toy toy) {
            ExceptionHandler.CheckNullArgument(new List<dynamic> { toy });
            var restClient = SetUrl(Config.readFromPropertiesFile("baseurl"));
            var restRequest = RequestOperation<Toy>(Request.POST, "/toy", toy, null);
            var restResponse = GetResponse(restClient, restRequest);
            Console.WriteLine($"Create Toy Response --> {GetContent(restResponse)}");
            dynamic data = JObject.Parse(GetContent(restResponse));
            Console.WriteLine($"Toy Id --> {data.id}");
            return data.id;
        }
        public Toy PerformGetToyOperation(string toyId) {
            ExceptionHandler.CheckNullArgument(new List<dynamic> { toyId });
            var restClient = SetUrl(Config.readFromPropertiesFile("baseurl"));
            var restRequest = RequestOperation<Toy>(Request.GET, $"/toy", null, null);
            var restResponse = GetResponse(restClient, restRequest);
            return GetListContent<Toy>(restResponse)
                    .Find(x => x.Id == Int32.Parse(toyId));
        }

        public string PerformDeleteToyOperation(string toyId) {
            ExceptionHandler.CheckNullArgument(new List<dynamic> { toyId });
            var restClient = SetUrl(Config.readFromPropertiesFile("baseurl"));
            var restRequest = RequestOperation<Toy>(Request.DELETE, $"/toy/{toyId}", null, null);
            var restResponse = GetResponse(restClient, restRequest);
            Console.WriteLine($"Delete Toy Response --> {GetContent(restResponse)}");
            dynamic data = JObject.Parse(GetContent(restResponse));
            return data.message;
        }

        public int UpdateToyStock(string toyId, dynamic updateStockCount)
        {
            ExceptionHandler.CheckNullArgument(new List<dynamic> { toyId, updateStockCount });
            var restClient = SetUrl(Config.readFromPropertiesFile("baseurl"));
            Console.WriteLine($"Get toy stock: {PerformGetToyOperation(toyId).Stock}");
            var restRequest = RequestOperation(operation: Request.PATCH, 
                                                    endpoint: $"/toy/{toyId}", 
                                                    payload: updateStockCount, 
                                                    headers: null);
            var restResponse = GetResponse(restClient, restRequest);
            //Console.WriteLine($"Update Toy Stock Response --> {GetContent(restResponse)}");
            //dynamic data = JObject.Parse(GetContent(restResponse));
            Toy responseData = GetContent<Toy>(restResponse);
            Console.WriteLine($"Toy's stock response data after updation : {responseData.Stock}");
            return responseData.Stock;
        }

        public Toy PerformGetToyByNameOperation(string toyName)
        {
            ExceptionHandler.CheckNullArgument(new List<dynamic> { toyName });
            var restClient = SetUrl(Config.readFromPropertiesFile("baseurl"));
            var restRequest = RequestOperation<Toy>(Request.GET, $"/toy", null, null);
            var restResponse = GetResponse(restClient, restRequest);
            return GetListContent<Toy>(restResponse)
                    .Find(x => x.Title.Equals(toyName));
        }
    }
}
