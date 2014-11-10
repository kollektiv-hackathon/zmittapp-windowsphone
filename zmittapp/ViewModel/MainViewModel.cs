using GalaSoft.MvvmLight;
using Newtonsoft.Json;
using System;
using System.Linq; 
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Windows.Web.Http;
using Windows.Web.Http.Headers;
using zmittapp.DataModel;
using System.Windows.Input;
using zmittapp.Common;

namespace zmittapp.ViewModel
{
    public class MainViewModel : ViewModelBase
    {

        private ObservableCollection<Restaurant> _restaurants;
        private Restaurant _restaurant;
        private ICommand _subscribeCommand;
        private ICommand _unsubscribeCommand;
        private User _user; 

        public ObservableCollection<Restaurant> Restaurants
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

        public Restaurant Restaurant
        {
            get
            {
                return _restaurant;
            }
            set{
                if (_restaurant == value) return;

                _restaurant = value;
                RaisePropertyChanged(() => Restaurant);
            }
        }

        public ObservableCollection<MenuItem> MenuItems
        {
            get
            {
                return _restaurant.MenuItems;
            }
            set
            {
                if (_restaurant.MenuItems == value) return;

                _restaurant.MenuItems = value;
                RaisePropertyChanged(() => MenuItems);
            }
        }

        public User User
        {
            get
            {
                return _user;
            }
            set
            {
                if (_user == value) return;

                _user = value;
               RaisePropertyChanged(() => User);
            } 
        }

        public MainViewModel()
        {
            GetRestaurant();
            User = new User(); 
        }

        private async Task GetRestaurant()
        {
            using (HttpClient client = new HttpClient())
            {

                client.DefaultRequestHeaders.Accept.Add(new HttpMediaTypeWithQualityHeaderValue("application/json"));

                var result = await client.GetAsync(new Uri("http://api.zmittapp.ch/restaurants/?_format=json"));
                result.EnsureSuccessStatusCode();

                Restaurants = new ObservableCollection<Restaurant>(
                   JsonConvert.DeserializeObject<IEnumerable<Restaurant>>(await result.Content.ReadAsStringAsync()));
            }
        }

        public async Task GetRestaurantById(int id)
        {
            if (Restaurants.Any(o => o.Id == id))
            {
                Restaurant = Restaurants.Where(o => o.Id == id).SingleOrDefault();
            }
            else
            {
                using (HttpClient client = new HttpClient())
                {

                    client.DefaultRequestHeaders.Accept.Add(new HttpMediaTypeWithQualityHeaderValue("application/json"));

                    var result = await client.GetAsync(new Uri("http://api.zmittapp.ch/restaurants/" + id + "?_format=json"));

                    Restaurant = JsonConvert.DeserializeObject<Restaurant>(await result.Content.ReadAsStringAsync());
                }
            }
            
            await GetMenuesByRestaurantId(id);
            
        }

        public async Task GetMenuesByRestaurantId(int id)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new HttpMediaTypeWithQualityHeaderValue("application/json"));

                var result = await client.GetAsync(new Uri("http://api.zmittapp.ch/restaurants/" + id + "/menuitems?_format=json"));

                if(Restaurant != null)
                    MenuItems = new ObservableCollection<MenuItem>(
                        JsonConvert.DeserializeObject<IEnumerable<MenuItem>>(await result.Content.ReadAsStringAsync()));
                   
            }
        }

        public async Task PutSubscription()
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new HttpMediaTypeWithQualityHeaderValue("application/json"));

                var result = await client.PutAsync(new Uri("http://api.zmittapp.ch/restaurants/" + Restaurant.Id + "/subscribe/" + User.Uid),null);

                if (result.StatusCode == HttpStatusCode.NotFound)
                {
                    await PostUser(); 
                    await PutSubscription(); //TODO: refactor
                }
             }
        }

        public async Task PostUser(){
            using (HttpClient client = new HttpClient())
            {
                HttpFormUrlEncodedContent content = new HttpFormUrlEncodedContent(new []{new KeyValuePair<string, string>("uid", User.Uid.ToString())});

                var resultUserPost = await client.PostAsync(new Uri("http://api.zmittapp.ch/user/"), content);
                resultUserPost.EnsureSuccessStatusCode();
            }
        }

        public ICommand SubscribeCommand
        {
            get
            {
                if (_subscribeCommand == null)
                {
                    _subscribeCommand = new RelayCommand(SubscribeExecute, CanSubscribe);
                }
                return _subscribeCommand;
            }
        }

        private async void SubscribeExecute()
        {
            await PutSubscription(); 
        }

         private bool CanSubscribe()
        {
            //TODO: only when not subscribed yet 
             return true; 
        }
    }
}