using JupiterToysRestSharpProject.Support;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators.OAuth2;
using System.Net;

namespace JupiterToysRestSharpProject.API
{
    public class BaseAPI<T>
    {
        public string accessToken;
        Token token;
        public BaseAPI(Token token) {
            this.token = token;
            accessToken = token.GetAccessToken();
        }

        public enum Request {
            GET, POST, PUT, DELETE, PATCH
        }

        public RestClient SetUrl(string url) {
            ExceptionHandler.CheckNullArgument(new List<dynamic> { url });
            var authenticator = new OAuth2AuthorizationRequestHeaderAuthenticator(
                accessToken, "Bearer"
            );
            var options = new RestClientOptions(url)
            {
                RemoteCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true,
                Authenticator = authenticator
            };
            var restClient = new RestClient(options);
            return restClient;
        }
        
        public RestRequest RequestOperation<T>(Request operation, string endpoint, T payload, Dictionary<string, string> headers) where T: notnull {
            ExceptionHandler.CheckNullArgument(arguments: new List<dynamic> { operation, endpoint });
            RestRequest restRequest = operation switch
            {
                Request.GET => new RestRequest(endpoint, Method.Get),
                Request.POST => new RestRequest(endpoint, Method.Post),
                Request.PUT => new RestRequest(endpoint, Method.Put),
                Request.DELETE => new RestRequest(endpoint, Method.Delete),
                Request.PATCH => new RestRequest(endpoint, Method.Patch),
            };

            if (headers != null)
                restRequest.AddHeaders(headers);

            //restRequest.AddJsonBody(JsonConvert.SerializeObject(payload));
            if (payload != null)
            {
                if (payload.GetType() == typeof(string)) {
                    restRequest.AddParameter("application/json", payload, ParameterType.RequestBody);
                } else {
                    restRequest.AddParameter("application/json", JsonConvert.SerializeObject(payload), ParameterType.RequestBody);
                }
            }
            return restRequest;
        }

        public RestResponse GetResponse(RestClient restClient, RestRequest restRequest) {
            ExceptionHandler.CheckNullArgument(new List<dynamic> { restClient, restRequest });
            return restClient.ExecuteAsync(restRequest).Result;
        }

        public string GetContent(RestResponse restResponse) {
            ExceptionHandler.CheckNullArgument(new List<dynamic> { restResponse });
            if (restResponse.StatusCode == HttpStatusCode.BadRequest)
                throw new Exception("Could not fetch the response");

            return restResponse.Content!;
        }

        public T GetContent<T>(RestResponse restResponse)
        {
            ExceptionHandler.CheckNullArgument(new List<dynamic> { restResponse });
            if (restResponse.StatusCode == HttpStatusCode.BadRequest)
                throw new Exception("Could not fetch the response");

            var content = restResponse.Content;
            T dtoObjects = JsonConvert.DeserializeObject<T>(content);
            return dtoObjects;
        }

        public List<T> GetListContent<T>(RestResponse restResponse) {
            ExceptionHandler.CheckNullArgument(new List<dynamic> { restResponse });
            if (restResponse.StatusCode == HttpStatusCode.BadRequest)
                throw new Exception("Could not fetch the response");

            var content = restResponse.Content;
            List<T> dtoObjects = JsonConvert.DeserializeObject<List<T>>(content);
            return dtoObjects;
        }
    }

}
