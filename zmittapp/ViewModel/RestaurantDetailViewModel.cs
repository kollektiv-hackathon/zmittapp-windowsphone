using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Web.Http;
using Windows.Web.Http.Headers;
using zmittapp.DataModel;

namespace zmittapp.ViewModel
{
    public class RestaurantDetailViewModel : MainViewModel
    {

        private Restaurant _restaurant;
        private ICommand _subscribeCommand;
        private ICommand _unsubscribeCommand;
        
        public User User {
            get
            {
                return base.User; 
            }
            set
            {
                base.User = value; 
            }

        }

        public Restaurant Restaurant
        {
            get
            {
                return _restaurant;
            }
            set
            {
                if (_restaurant == value) return;

                _restaurant = value;
                RaisePropertyChanged(() => Restaurant);
            }
        }

        public ObservableCollection<MenuItem> MenuItems
        {
            get
            {
                if (_restaurant != null) return _restaurant.MenuItems;

                return null;                  
            }
            set
            {
                if (_restaurant.MenuItems == value) return;

                _restaurant.MenuItems = value;
                RaisePropertyChanged(() => MenuItems);
            }
        }

        public async Task GetRestaurantById(int id)
        {
            //if (Restaurants.Any(o => o.Id == id))
            //{
            //    Restaurant = Restaurants.Where(o => o.Id == id).SingleOrDefault();
            //}
            //else
            //{
                using (HttpClient client = new HttpClient())
                {

                    client.DefaultRequestHeaders.Accept.Add(new HttpMediaTypeWithQualityHeaderValue("application/json"));

                    var result = await client.GetAsync(new Uri("http://api.zmittapp.ch/restaurants/" + id + "?_format=json"));

                    Restaurant = JsonConvert.DeserializeObject<Restaurant>(await result.Content.ReadAsStringAsync());
                }
            //}

            await GetMenuesByRestaurantId(id);
        }

        public async Task GetMenuesByRestaurantId(int id)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new HttpMediaTypeWithQualityHeaderValue("application/json"));

                var result = await client.GetAsync(new Uri("http://api.zmittapp.ch/restaurants/" + id + "/menuitems?_format=json"));

                if (Restaurant != null)
                    MenuItems = new ObservableCollection<MenuItem>(
                        JsonConvert.DeserializeObject<IEnumerable<MenuItem>>(await result.Content.ReadAsStringAsync()));
            }
        }

        public async Task PutSubscription()
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new HttpMediaTypeWithQualityHeaderValue("application/json"));

                var result = await client.PutAsync(new Uri("http://api.zmittapp.ch/restaurants/" + Restaurant.Id + "/subscribe/" + User.Uid), null);

                
               if (result.StatusCode == HttpStatusCode.NotFound)
                {
                    await PostUser();
                    result = await client.PutAsync(new Uri("http://api.zmittapp.ch/restaurants/" + Restaurant.Id + "/subscribe/" + User.Uid), null);
                }

                result.EnsureSuccessStatusCode(); 

                base.Subscriptions.Add(Restaurant);

            }
        }

        public async Task PutUnsubscription()
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new HttpMediaTypeWithQualityHeaderValue("application/json"));

                var result = await client.PutAsync(new Uri("http://api.zmittapp.ch/restaurants/" + Restaurant.Id + "/unsubscribe/" + User.Uid), null);

                if (result.StatusCode == HttpStatusCode.NotFound)
                {
                    await PostUser();
                    result = await client.PutAsync(new Uri("http://api.zmittapp.ch/restaurants/" + Restaurant.Id + "/subscribe/" + User.Uid), null);
                }
               
                result.EnsureSuccessStatusCode(); 

                base.Subscriptions.Remove(Restaurant);

            }
        }

        public async Task PostUser()
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new HttpMediaTypeWithQualityHeaderValue("application/json"));

                HttpFormUrlEncodedContent content = new HttpFormUrlEncodedContent(new[] { new KeyValuePair<string, string>("user[uid]", User.Uid.ToString()) });

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

        public ICommand UnsubscribeCommand
        {
            get
            {
                if (_subscribeCommand == null)
                {
                    _subscribeCommand = new RelayCommand(UnsubscribeExecute, CanSubscribe);
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

        private async void UnsubscribeExecute()
        {
            await PutUnsubscription();
        }

    }

}
