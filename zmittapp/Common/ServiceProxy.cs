using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Web.Http;
using Windows.Web.Http.Headers;
using zmittapp.DataModel;

namespace zmittapp.Common
{
    public class ServiceProxy
    {
        private const string apiUrl = "http://api.zmittapp.ch";
        private const string format = "?_format=json";

        private void SetHttpHeaders(HttpClient client){
            client.DefaultRequestHeaders.Accept.Add(new HttpMediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<string> GetSubsciptionsByUserAsync(string uid)
        {
            using (HttpClient client = new HttpClient())
            {
                SetHttpHeaders(client); 

                var result = await client.GetAsync(new Uri(String.Format("{0}/user/{1}/subscriptions{2}", apiUrl, uid, format)));
                result.EnsureSuccessStatusCode();

                return await result.Content.ReadAsStringAsync(); 
                
            }
        }

        public async Task<string> GetMenuesByRestaurantId(int restaurantId)
        {
            using (HttpClient client = new HttpClient())
            {
                SetHttpHeaders(client);
                
                var result = await client.GetAsync(new Uri(String.Format("{0}/restaurants/{1}/menuitems{2}", apiUrl, restaurantId, format)));
                result.EnsureSuccessStatusCode();
                
                return await result.Content.ReadAsStringAsync();
            }
        }

    }
}
