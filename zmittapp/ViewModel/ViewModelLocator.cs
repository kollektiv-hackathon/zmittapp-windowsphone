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
            SimpleIoc.Default.Register<RestaurantIndexViewModel>();
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

        public RestaurantIndexViewModel RestaurantIndex
        {
            get
            {
                return ServiceLocator.Current.GetInstance<RestaurantIndexViewModel>();
            }
        }
        
        public static void Cleanup()
        {
            SimpleIoc.Default.Unregister<MainViewModel>();
            SimpleIoc.Default.Unregister<RestaurantDetailViewModel>(); 
        }
    }
}