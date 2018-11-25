using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TGVE.WebApi.Models;

namespace TGVE.Api
{
    public class ToursClient
    {
        readonly string Url = "http://localhost:2788/api/Tours";

        public async Task<List<Tour>> GetAsync()
        {
            using (HttpClient request = new HttpClient())
            {
                var response = await request.GetAsync(this.Url);
                var stringResponse = await response.Content.ReadAsStringAsync();
                var responseObject = JsonConvert.DeserializeObject<List<Tour>>(stringResponse);

                return responseObject;
            }
        }

        public async Task<List<Tour>> GetAsync(int id)
        {
            using (HttpClient request = new HttpClient())
            {
                var response = await request.GetAsync($"{this.Url}/{id}");
                var stringResponse = await response.Content.ReadAsStringAsync();
                var responseObject = JsonConvert.DeserializeObject<List<Tour>>(stringResponse);

                return responseObject;
            }
        }

        public async Task<Tour> PostAsync(Tour tour)
        {
            using (HttpClient request = new HttpClient())
            {
                var dataString = JsonConvert.SerializeObject(tour);
                var sendData = new StringContent(dataString, System.Text.Encoding.UTF8, "application/json");
                var response = await request.PostAsync(this.Url, sendData);

                var responseObject = JsonConvert.DeserializeObject<Tour>(await response.Content.ReadAsStringAsync());

                return responseObject;
            }
        }

        public async Task<Tour> PutAsync(Tour tour)
        {
            using (HttpClient request = new HttpClient())
            {
                var dataString = JsonConvert.SerializeObject(tour);
                    //new JsonSerializerSettings
                    //{
                    //    NullValueHandling = NullValueHandling.Ignore,
                    //    Formatting = Formatting.None,
                    //    ContractResolver = new CamelCasePropertyNamesContractResolver()
                    //});
                var sendData = new StringContent(dataString, System.Text.Encoding.UTF8, "application/json");
                var response = await request.PutAsync($"{this.Url}/{tour.Id}", sendData);

                var responseObject = JsonConvert.DeserializeObject<Tour>(await response.Content.ReadAsStringAsync());

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
