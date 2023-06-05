using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System.Text.Json.Serialization;
using Microsoft.Azure.Management.HealthcareApis;
using Hierh.Models;

namespace Hierh.API
{
    public class Firh
    {
        protected RestClient Client { get; set; }
        protected string Token { get; set; }
        public Firh()
        {
            Client = new RestClient("https://fiap.azurehealthcareapis.com");
        }

        protected void BeforeRequest(ref RestRequest request)
        {
            request.AddHeader("Authorization", $"Bearer {Token}");
        }

        public void Login()
        {
            var client = new RestClient("https://login.microsoftonline.com");
            var request = new RestRequest("/11dbbfe2-89b8-4549-be10-cec364e59551/oauth2/token");
            request.AlwaysMultipartFormData = true;
            request.AddParameter("grant_type", "client_credentials");
            request.AddParameter("client_id", "6731de76-14a6-49ae-97bc-6eba6914391e");
            request.AddParameter("scope", "https://graph.microsoft.com/.default");
            request.AddParameter("client_secret", "JqQX2PNo9bpM0uEihUPzyrh");
            var response = client.Execute(request, Method.Post);
            if (response.IsSuccessful)
            {
                var jtoken = JsonConvert.DeserializeObject<JToken>(response.Content);
                Token = jtoken.SelectToken("access_token").Value<string>();
            }
        }

        public List<Disease> GetDiseases(string identifier)
        {
            var request = new RestRequest("https://fiap.azurehealthcareapis.com/Patient", Method.Get);
            request.AddQueryParameter("identifier", identifier);
            BeforeRequest(ref request);
            var response = Client.Execute(request, Method.Get);
            if (response.IsSuccessful)
            {
                var response2 = JsonConvert.DeserializeObject<Patient>(response.Content);
            }
            else
            {
                var rt = new List<Disease>()
                {
                    new Disease(){ Name = "Diabete (Tipo 2)", IsHereditary = true },
                    new Disease(){ Name = "Depressão", IsHereditary = true},
                    new Disease(){ Name = "Colesterol Alto", IsHereditary = true},
                    new Disease(){ Name = "Dermatite", IsHereditary = true},
                    new Disease(){ Name = "Glaucoma", IsHereditary = true},

                };

                var rounds = new Random();
                foreach (var r in rt)
                {
                    if (rounds.Next(1, 100) % 2 == 0)
                    {
                        r.Name = null;
                    }
                }
                rt.RemoveAll(x => x.Name == null);

                return rt;
            }

            return null;
        }
    }
}
