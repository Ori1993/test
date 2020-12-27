using Castle.DynamicProxy;
using Dynamicproxylearning.Filter;
using Dynamicproxylearning.LogHelper;
using Dynamicproxylearning.Services;
using log4net;
using log4net.Config;
using log4net.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Dynamicproxylearning
{
    public class Startup
    {
        // <summary>
        /// log4net 仓储库
        /// </summary>
        public static ILoggerRepository repository { get; set; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            //log4net
            repository = LogManager.CreateRepository("");
            //指定配置文件，如果这里你遇到问题，应该是使用了InProcess模式，请查看Blog.Core.csproj,并删之 
            XmlConfigurator.Configure(repository, new FileInfo("Log4net.config"));//配置文件
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddControllers();
            services.AddMvc(o =>
            {
                o.Filters.Add(typeof(GlobalExceptionsFilter));
            }).SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
            //services.AddScoped<IWelcome,Welcome>();
            // 不能在控制器里使用
            //var decorator = DispatchProxy.Create<IWelcome, GenerDecorator>();
            //((GenerDecorator)decorator).TargetClass = new Welcome();
            //var tt =  decorator.SayHi();
            services.AddSingleton<ILoggerHelper, LoggerHelper>();
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
