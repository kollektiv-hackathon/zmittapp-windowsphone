using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;

namespace zmittapp.ViewModel
{
    public class ViewModelLocator
    {
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<RestaurantDetailViewModel>();
            SimpleIoc.Default.Register<RestaurantAllViewModel>();
            SimpleIoc.Default.Register<RestaurantByLocationViewModel>();
        }

        public MainViewModel Main
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MainViewModel>();
            }
        }

        public RestaurantDetailViewModel RestaurantDetail
        {
            get
            {
                return ServiceLocator.Current.GetInstance<RestaurantDetailViewModel>();
            }
        }

        public RestaurantAllViewModel RestaurantAll
        {
            get
            {
                return ServiceLocator.Current.GetInstance<RestaurantAllViewModel>();
            }
        }

        public RestaurantByLocationViewModel RestaurantByLocation
        {
            get
            {
                return ServiceLocator.Current.GetInstance<RestaurantByLocationViewModel>();
            }
        }
        
        public static void Cleanup()
        {
            SimpleIoc.Default.Unregister<MainViewModel>();
            SimpleIoc.Default.Unregister<RestaurantDetailViewModel>();
            SimpleIoc.Default.Unregister<RestaurantByLocationViewModel>();
            SimpleIoc.Default.Unregister<RestaurantAllViewModel>(); 
        }
    }
}