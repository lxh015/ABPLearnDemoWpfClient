/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocator xmlns:vm="clr-namespace:Try.Client"
                           x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"

  You can also use Blend to do all this with the tool's support.
  See http://www.galasoft.ch/mvvm
*/

using CommonServiceLocator;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using System.Configuration;
using System.ServiceModel;

namespace Try.Client.ViewModel
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    public class ViewModelLocator
    {
        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);
            this.RegisterWcfService();
            ////if (ViewModelBase.IsInDesignModeStatic)
            ////{
            ////    // Create design time view services and models
            ////    SimpleIoc.Default.Register<IDataService, DesignDataService>();
            ////}
            ////else
            ////{
            ////    // Create run time view services and models
            ////    SimpleIoc.Default.Register<IDataService, DataService>();
            ////}

            SimpleIoc.Default.Register<MainViewModel>();
        }

        public MainViewModel Main
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MainViewModel>();
            }
        }

        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }

        /// <summary>
        /// 注册wcf服务
        /// </summary>
        void RegisterWcfService()
        {
            var serverUrl = ConfigurationManager.AppSettings["ServerUrl"];
            if (string.IsNullOrEmpty(serverUrl))
                throw new System.Exception("未配置服务端访问地址");

            BasicHttpBinding binding1 = new BasicHttpBinding();
            binding1.MaxBufferPoolSize = 2147483647;
            binding1.MaxBufferSize = 2147483647;
            binding1.MaxReceivedMessageSize = 2147483647;

            if (!serverUrl.StartsWith("http://"))
                serverUrl = $"http://{serverUrl}";

            SimpleIoc.Default.Register<IStudentService>(() => new StudentServiceClient(binding1, new EndpointAddress($"{serverUrl}/StudentService.svc")));
        }

        public static T Resolve<T>()
        {
            return SimpleIoc.Default.GetInstanceWithoutCaching<T>();
        }
    }
}