using AutoMapper;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public  class VehicleFactory
    {
        private bool _registered;

        IUnityContainer _ioc_Container = new UnityContainer();
        
        private  void Register()
        {
            _ioc_Container.RegisterType<VehicleRepository, VehicleDBRepository>("v1", new ContainerControlledLifetimeManager());
            _ioc_Container.RegisterType<VehicleRepository, VehicleNoSqlRepository>("v2" , new ContainerControlledLifetimeManager());
            
            _registered = true;
        }

        public  VehicleRepository GetRepository( string version)
        {

            if (_registered == false) Register() ;

            return _ioc_Container.Resolve<VehicleRepository>(version);
            
        }

    }
}
