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

        
        public ObservableCollection<Restaurant> Subscriptions
        {
            get
            {
                if (User.Subscriptions == null)
                {
                    User.Subscriptions = new ObservableCollection<Restaurant>();
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

        //public User User
        //{
        //    get
        //    {
        //        return _user;
        //    }
        //    set
        //    {
        //        if (_user == value) return;

        //        _user = value;
        //       RaisePropertyChanged(() => User);
        //    } 
        //}

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

        public MainViewModel()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);
            User = new User();
            SimpleIoc.Default.Register<User>(); 
            
            //GetSubsciptionsByUser();
        }

        protected async Task GetSubsciptionsByUser()
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new HttpMediaTypeWithQualityHeaderValue("application/json"));

                var result = await client.GetAsync(new Uri(String.Format("http://api.zmittapp.ch/user/{0}/subscriptions?_format=json", User.Uid)));
                result.EnsureSuccessStatusCode();

                Subscriptions = new ObservableCollection<Restaurant>(
                   JsonConvert.DeserializeObject<IEnumerable<Restaurant>>(await result.Content.ReadAsStringAsync()));
                User.Subscriptions = Subscriptions; 
            }
        }

        public ICommand UpdateSubscriptions
        {
            get{
                return new RelayCommand(UpdatedSubscriptions); 
            }

        }

        private void UpdatedSubscriptions()
        {
            GetSubsciptionsByUser(); 
        }        
    }
}