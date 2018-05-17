using CommonServiceLocator;
using GalaSoft.MvvmLight.Ioc;
using ByronSouthParkDemo.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ByronSouthParkDemo.DataProvider;

namespace ByronSouthParkDemo.Common
{
    public class SouthParkViewModelFactory
    {
        private static SouthParkViewModelFactory _instance;

        static SouthParkViewModelFactory()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);
            SimpleIoc.Default.Register<ViewController>();
            SimpleIoc.Default.Register<MainScreenViewModel>();
            SimpleIoc.Default.Register<SouthParkDataProvider, TestCharacterProvider>();
            SimpleIoc.Default.Register<TimeService>();

        }

        public void Start()
        {
            DaTimeService.Start();

        }
        public void Stop()
        {
            DaTimeService.Stop();
        }

        public static SouthParkViewModelFactory GetInstance()
        {
            if (_instance == null) _instance = new SouthParkViewModelFactory();

            return _instance;
        }

        public MainScreenViewModel MainScreen
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MainScreenViewModel>();
            }
        }

        public SouthParkDataProvider CharacterProvider
        {
            get
            {
                return ServiceLocator.Current.GetInstance<SouthParkDataProvider>();
            }
        }

        public TimeService DaTimeService
        {
            get
            {
                return ServiceLocator.Current.GetInstance<TimeService>();
            }
        }

        public ViewController ViewManager
        {
            get
            {
                return ServiceLocator.Current.GetInstance<ViewController>();
            }
        }
    }
}
