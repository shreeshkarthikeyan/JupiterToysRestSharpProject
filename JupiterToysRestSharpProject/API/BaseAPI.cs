using Gherkin;
using JupiterToysRestSharpProject.Support;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using RestSharp;
using RestSharp.Authenticators.OAuth2;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Security.Policy;

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

            restRequest.AddJsonBody(JsonConvert.SerializeObject(payload));
            return restRequest;
        }

        public RestResponse GetResponse(RestClient restClient, RestRequest restRequest) {
            ExceptionHandler.CheckNullArgument(new List<dynamic> { restClient, restRequest });
            return restClient.ExecuteAsync(restRequest).Result;
        }

        public string GetContent(RestResponse restResponse) {
            ExceptionHandler.CheckNullArgument(new List<dynamic> { restResponse });
            if (restResponse.StatusCode == HttpStatusCode.BadRequest)
                throw new Exception("Could not fetch the right response");

            return restResponse.Content!;
        }

        public List<T> GetContent<T>(RestResponse restResponse) {
            ExceptionHandler.CheckNullArgument(new List<dynamic> { restResponse });
            if (restResponse.StatusCode == HttpStatusCode.BadRequest)
                throw new Exception("Could not fetch the right response");

            var content = restResponse.Content;
            List<T> dtoObjects = JsonConvert.DeserializeObject<List<T>>(content);
            return dtoObjects;
        }
    }

}
