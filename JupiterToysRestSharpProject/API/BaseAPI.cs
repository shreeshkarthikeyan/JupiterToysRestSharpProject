using Gherkin;
using JupiterToysRestSharpProject.Support;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using RestSharp.Authenticators.OAuth2;
using System;
using System.Net;
using System.Security.Policy;

namespace JupiterToysRestSharpProject.API
{
    public class BaseAPI<T> where T : class
    {
        public string accessToken;
        public BaseAPI() {
            GetAccessToken();
        }

        public enum Request {
            GET, POST, PUT, DELETE, PATCH
        }

        public RestClient SetUrl(string url) {

            ArgumentNullException.ThrowIfNull(url);
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
            
            ArgumentNullException.ThrowIfNull(operation);
            ArgumentNullException.ThrowIfNull(endpoint);
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

            restRequest.AddJsonBody(JsonConvert.SerializeObject(payload));
            return restRequest;
        }

        public RestResponse GetResponse(RestClient restClient, RestRequest restRequest) {
            ArgumentNullException.ThrowIfNull(restClient);
            ArgumentNullException.ThrowIfNull(restRequest);
            return restClient.ExecuteAsync(restRequest).Result;
        }

        public string GetContent(RestResponse restResponse) {

            ArgumentNullException.ThrowIfNull(restResponse);
            if (restResponse.StatusCode == HttpStatusCode.BadRequest)
                throw new Exception("Could not fetch the right response");

            return restResponse.Content!;
        }

        public List<T> GetContent<T>(RestResponse restResponse) {
            ArgumentNullException.ThrowIfNull(restResponse);
            /*HttpStatusCode statusCode = restResponse.StatusCode;
            int numericStatusCode = (int)statusCode;
            Console.WriteLine($"Status Code: {numericStatusCode}");*/

            if (restResponse.StatusCode == HttpStatusCode.BadRequest)
                throw new Exception("Could not fetch the right response");

            var content = restResponse.Content;
            List<T> dtoObjects = JsonConvert.DeserializeObject<List<T>>(content);
            return dtoObjects;
        }
        public void GetAccessToken()
        {
            var client = new RestClient(Config.readFromPropertiesFile("tokenurl"));
            var request = new RestRequest("", Method.Post);
            request.AddParameter("grant_type", Config.readFromPropertiesFile("grant_type"));
            request.AddParameter("client_id", Config.readFromPropertiesFile("client_id"));
            request.AddParameter("client_secret", Config.readFromPropertiesFile("client_secret"));
            request.AddParameter("scope", Config.readFromPropertiesFile("scope"));
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            RestResponse response = client.Post(request);
            var content = response.Content;
            dynamic data = JObject.Parse(content);
            Console.WriteLine(data.access_token);
            accessToken = data.access_token;
        }
    }

}
