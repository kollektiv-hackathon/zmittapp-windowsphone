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
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;

namespace zmittapp.ViewModel
{
    public class MainViewModel : ViewModelBase
    {

        private User _user;
        
        public List<Restaurant> Subscriptions
        {
            get
            {
                if (User.Subscriptions == null)
                {
                    User.Subscriptions = new List<Restaurant>();
                    User.Subscriptions.Add(new Restaurant { Name = "Keine Subscriptions",  });
                }
                return User.Subscriptions;
            }
            set
            {
                //if (User.Subscriptions == value) return;

                User.Subscriptions = value;
                RaisePropertyChanged(() => Subscriptions);
            }
        }

        public User User
        {
            get
            {
                return ServiceLocator.Current.GetInstance<User>();
            }
            set
            {
                SimpleIoc.Default.Register<User>(); 
            }
        }

        public ServiceProxy ServiceProxy
        {
            get
            {
                return ServiceLocator.Current.GetInstance<ServiceProxy>(); 
            }
            set
            {
                SimpleIoc.Default.Register<ServiceProxy>(); 
            }
        }

        public MainViewModel()
        {
            //TODO: refactor
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);
            SimpleIoc.Default.Register<User>();
            SimpleIoc.Default.Register<ServiceProxy>(); 
        }

        public ICommand UpdateSubscriptions
        {
            get{
                return new RelayCommand(UpdateSubscriptionsFunc); 
            }
        }

        private async void UpdateSubscriptionsFunc()
        {
            var restaurants = new List<Restaurant>(
                JsonConvert.DeserializeObject<IEnumerable<Restaurant>>(
                await ServiceProxy.GetSubsciptionsByUserAsync(User.Uid)));
           
            foreach(var restaurant in restaurants){
                var task2 = await ServiceProxy.GetMenuesByRestaurantId(restaurant.Id);
                restaurant.MenuItems = new List<MenuItem>(JsonConvert.DeserializeObject<IEnumerable<MenuItem>>(task2));
            }

            //TODO: Refactor
            Subscriptions = restaurants;
            User.Subscriptions = restaurants; 
            
        }        
    }
}