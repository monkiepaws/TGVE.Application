using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TGVE.WebApi.Models;

namespace TGVE.Api
{
    public class ClientsClient
    {
        readonly string Url = "http://localhost:2788/api/Clients";

        public async Task<List<Client>> GetAsync()
        {
            using (HttpClient request = new HttpClient())
            {
                var response = await request.GetAsync(this.Url);
                var stringResponse = await response.Content.ReadAsStringAsync();
                var responseObject = JsonConvert.DeserializeObject<List<Client>>(stringResponse);

                return responseObject;
            }
        }

        public async Task<List<Client>> GetAsync(int id)
        {
            using (HttpClient request = new HttpClient())
            {
                var response = await request.GetAsync($"{this.Url}/{id}");
                var stringResponse = await response.Content.ReadAsStringAsync();
                var responseObject = JsonConvert.DeserializeObject<List<Client>>(stringResponse);

                return responseObject;
            }
        }

        public async Task<Client> PostAsync(Client client)
        {
            using (HttpClient request = new HttpClient())
            {
                var dataString = JsonConvert.SerializeObject(client);
                var sendData = new StringContent(dataString, System.Text.Encoding.UTF8, "application/json");
                var response = await request.PostAsync(this.Url, sendData);

                var responseObject = JsonConvert.DeserializeObject<Client>(await response.Content.ReadAsStringAsync());

                return responseObject;
            }
        }

        public async Task<Client> PutAsync(Client client)
        {
            using (HttpClient request = new HttpClient())
            {
                var dataString = JsonConvert.SerializeObject(client,
                    new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore,
                        Formatting = Formatting.None,
                        ContractResolver = new CamelCasePropertyNamesContractResolver()
                    });
                var sendData = new StringContent(dataString, System.Text.Encoding.UTF8, "application/json");
                var response = await request.PutAsync($"{this.Url}/{client.Id}", sendData);

                var responseObject = JsonConvert.DeserializeObject<Client>(await response.Content.ReadAsStringAsync());

                return responseObject;
            }
        }

        public async Task<string> DeleteAsync(int id)
        {
            using (HttpClient request = new HttpClient())
            {
                var response = await request.DeleteAsync($"{this.Url}/{id}");
                var stringResponse = response.StatusCode.ToString();

                return stringResponse;
            }
        }
    }
}