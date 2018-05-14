using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public static class Mapper
    {   
        public static IMapper IMap { get; private set; }

        static  Mapper()
        {
            //AutoMapper
            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<Vehicle, VehicleModelYear>()
                    .ForMember (dst => dst.make,  source => source.MapFrom( src => src.Manufacturer ));

                cfg.CreateMap<VehicleModelYear, Vehicle>()
                    .ForMember(dst => dst.Manufacturer, source => source.MapFrom(src => src.make));

                cfg.CreateMap<Vehicle, VehicleTE>();
                cfg.CreateMap<VehicleTE, Vehicle>();
            });
            IMap = config.CreateMapper();
        }

    }
}
