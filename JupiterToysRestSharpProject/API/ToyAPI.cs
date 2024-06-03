using JupiterToysRestSharpProject.Model;
using JupiterToysRestSharpProject.Support;
using Newtonsoft.Json.Linq;

namespace JupiterToysRestSharpProject.API
{
    public class ToyAPI : BaseAPI<Toy> 
    {
        public string PerformCreateToyOperation(Toy toy)
        {
            ArgumentNullException.ThrowIfNull(toy);
            var restClient = SetUrl(Config.readFromPropertiesFile("baseurl"));
            var restRequest = RequestOperation<Toy>(Request.POST, "/toy", toy, null);
            var restResponse = GetResponse(restClient, restRequest);
            Console.WriteLine($"Create Toy Response --> {GetContent(restResponse)}");
            dynamic data = JObject.Parse(GetContent(restResponse));
            Console.WriteLine($"Toy Id --> {data.id}");
            return data.id;
        }

        public Toy PerformGetToyOperation(string toyId)
        {
            ArgumentNullException.ThrowIfNull(toyId);
            /*var headers = new Dictionary<string, string>
            {
                {"Accept","application/hal+json"}
            };*/
            var restClient = SetUrl(Config.readFromPropertiesFile("baseurl"));
            var restRequest = RequestOperation<Toy>(Request.GET, $"/toy", null, null);
            var restResponse = GetResponse(restClient, restRequest);
            return GetContent<Toy>(restResponse)
                    .Find(x => x.Id == Int32.Parse(toyId));
        }

        public string PerformDeleteToyOperation(String toyId)
        {
            ArgumentNullException.ThrowIfNull(toyId);
            var restClient = SetUrl(Config.readFromPropertiesFile("baseurl"));
            var restRequest = RequestOperation<Toy>(Request.DELETE, $"/toy/{toyId}", null, null);
            var restResponse = GetResponse(restClient, restRequest);
            Console.WriteLine($"Delete Toy Response --> {GetContent(restResponse)}");
            dynamic data = JObject.Parse(GetContent(restResponse));
            return data.message;
        }
    }
}
