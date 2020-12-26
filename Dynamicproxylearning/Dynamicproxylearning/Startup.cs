using Castle.DynamicProxy;
using Dynamicproxylearning.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Dynamicproxylearning
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            //services.AddScoped<IWelcome,Welcome>();
            // 不能在控制器里使用
            //var decorator = DispatchProxy.Create<IWelcome, GenerDecorator>();
            //((GenerDecorator)decorator).TargetClass = new Welcome();
            //var tt =  decorator.SayHi();
            services.AddScoped<Welcome>();
            services.AddScoped<GenerDecorator>();
            services.AddScoped(provider => {
                var generator = new ProxyGenerator();
                var tatgetClass = provider.GetService<Welcome>();
                var intercepter = provider.GetService<GenerDecorator>();
                var proxy = generator.CreateInterfaceProxyWithTarget<IWelcome>(tatgetClass, intercepter);

                return proxy;
            
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
