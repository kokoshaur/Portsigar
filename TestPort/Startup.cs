using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TestPort.Logical;

namespace TestPort
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseExceptionHandler("/Home/Error"); //Библиотека обхода типичных исключений
            app.UseHsts();                          //Штука для трансляции MVC
            app.UseHttpsRedirection();              //Штука для перехода на HTTPS
            app.UseStaticFiles();                   //Штука для работы с файликами
            app.UseRouting();                       //Штука для маршрутизации

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
