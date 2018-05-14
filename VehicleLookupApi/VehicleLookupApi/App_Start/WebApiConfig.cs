using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json.Serialization;
using System.Web.Http.ExceptionHandling;
using Elmah.Contrib.WebApi;

namespace VehicleLookupApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            // Configure Web API to use only bearer token authentication.
            config.SuppressDefaultHostAuthentication();
            config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));

            config.Services.Add(typeof(IExceptionLogger), new ElmahExceptionLogger());

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "Version1",
                routeTemplate: "v1/Vehicles/{action}/{id}",
                defaults: new { controller = "Vehicles" , action="Get", id = RouteParameter.Optional }
            );

           config.Routes.MapHttpRoute(
                name: "Version2",
                routeTemplate: "v2/Vehicles/{action}/{id}",
                defaults: new { controller = "VehiclesV2", action = "Get", id = RouteParameter.Optional }
            );

        }
    }
}
