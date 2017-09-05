using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Fiver.Perf.Caching
{
    public class Startup
    {
        public void ConfigureServices(
            IServiceCollection services)
        {
            services.AddDistributedRedisCache(options =>
            {
                options.Configuration = "..."; // connection string
            });
        }

        public void Configure(
            IApplicationBuilder app, 
            IHostingEnvironment env)
        {
            app.UseWriteCaching();
            app.UseReadCaching();
        }
    }
}
