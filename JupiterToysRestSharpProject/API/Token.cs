using JupiterToysRestSharpProject.Support;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace JupiterToysRestSharpProject.API
{
    public class Token
    {
        public string GetAccessToken()
        {
            var client = new RestClient(Config.readFromPropertiesFile("tokenurl"));
            var request = new RestRequest("", Method.Post);
            request.AddParameter("grant_type", Config.readFromPropertiesFile("grant_type"));
            request.AddParameter("client_id", Config.readFromPropertiesFile("client_id"));
            request.AddParameter("client_secret", Config.readFromPropertiesFile("client_secret"));
            request.AddParameter("scope", Config.readFromPropertiesFile("scope"));
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            RestResponse response = client.Post(request);
            
            if(response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                throw new Exception("Could not fetch the right response");

            var content = response.Content;
            dynamic data = JObject.Parse(content);
            Console.WriteLine(data.access_token);
            return data.access_token;
        }
    }
}
