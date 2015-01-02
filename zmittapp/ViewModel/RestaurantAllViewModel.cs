using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Web.Http;
using Windows.Web.Http.Headers;
using zmittapp.DataModel;

namespace zmittapp.ViewModel
{
    public class RestaurantAllViewModel : MainViewModel
    {
        private List<Restaurant> _restaurants;
        private List<Restaurant> _originalRestaurants;
        private string _keyword; 

        public List<Restaurant> Restaurants
        {
            get
            {
                return _restaurants;
            }
            set
            {
                if (_restaurants == value) return;

                _restaurants = value;
                RaisePropertyChanged(() => Restaurants);
            }
        }

        public List<Restaurant> OriginalRestaurants
        {
            get
            {
                return _originalRestaurants;
            }
            set
            {
                if (_originalRestaurants == value) return;

                _originalRestaurants = value;
                RaisePropertyChanged(() => OriginalRestaurants);
            }
        }

        public string Keyword
        {
            get
            {
                return _keyword;
            }
            set
            {
                if (_keyword == value) return;

                _keyword = value;

                if (value == "" || value == null)
                {
                    Restaurants = OriginalRestaurants;
                }
                else
                {
                    Restaurants = OriginalRestaurants.Where(o => o.Name.ToUpper().Contains(value.ToUpper())).ToList(); 
                }
            }
        }

        //TODO: -> ServiceProxy
        public async Task GetRestaurants()
        {
            using (HttpClient client = new HttpClient())
            {

                client.DefaultRequestHeaders.Accept.Add(new HttpMediaTypeWithQualityHeaderValue("application/json"));

                var result = await client.GetAsync(new Uri("http://api.zmittapp.ch/restaurants/?_format=json"));
                result.EnsureSuccessStatusCode();

                Restaurants =  JsonConvert.DeserializeObject<IEnumerable<Restaurant>>(await result.Content.ReadAsStringAsync()).ToList();
                
                OriginalRestaurants = Restaurants.ToList(); 
            }
        }
    }
}
