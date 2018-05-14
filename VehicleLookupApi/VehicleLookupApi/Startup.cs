using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Owin;
using Owin;
using System.Net.Http;

[assembly: OwinStartup(typeof(VehicleLookupApi.Startup))]

namespace VehicleLookupApi
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            WakeUp("http://byronweather.azurewebsites.net/"); // Avoid first time latency
        }


        public void WakeUp(string url)
        {
            var httpClient = new HttpClient();
            httpClient.GetStringAsync(url);

        }

    }
}
