
using apiword.Test;
using Autofac;
using Autofac.Extras.DynamicProxy;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.DotNet.PlatformAbstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using static apiword.SwaggerHelper.CustomApiVersion;

namespace apiword
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public string ApiName { get; set; } = "认知诊断";
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();


            services.AddTransient<ITranTest, TranTest>();
            services.AddSingleton<ISingTest,SingTest>();
            services.AddScoped<ISconTest,SconTest>();
            services.AddScoped<IAService,AService>();
            var basePath = ApplicationEnvironment.ApplicationBasePath;
            services.AddSwaggerGen(c =>
            {
                //c.SwaggerDoc("V1", new OpenApiInfo
                //{
                //    // {ApiName} 定义成全局变量，方便修改
                //    Version = "V1",
                //    Title = $"{ApiName} 接口文档——V5.4",
                //    Description = "该界面呈现了认知诊断对外提供的所有接口，具体见各接口描述",
                //    //Contact = new OpenApiContact { Name = ApiName, Email = "Blog.Core@xxx.com", Url = new Uri("https://www.jianshu.com/u/94102b59cc2a") },
                //    //License = new OpenApiLicense { Name = ApiName, Url = new Uri("https://www.jianshu.com/u/94102b59cc2a") }
                //});
                //c.OrderActionsBy(o => o.RelativePath);

                //var xmlPath = Path.Combine(basePath, "apiword.xml");//这个就是刚刚配置的xml文件名
                //c.IncludeXmlComments(xmlPath, true);

                //var xmlModelPath = Path.Combine(basePath, "apiword.Model.xml");//这个就是Model层的xml文件名
                //c.IncludeXmlComments(xmlModelPath);

                typeof(ApiVersions).GetEnumNames().ToList().ForEach(version =>
                {
                    c.SwaggerDoc(version, new OpenApiInfo
                    {
                        // {ApiName} 定义成全局变量，方便修改
                        Version = version,
                        Title = $"{ApiName} 接口文档",
                        Description = $"{ApiName} HTTP API " + version,
                        //TermsOfService = "None",
                        Contact = new OpenApiContact { Name = "Blog.Core", Email = "Blog.Core@xxx.com", Url = new Uri( "https://www.jianshu.com/u/94102b59cc2a") }
                    });
                });
            });
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {

            var basePath = ApplicationEnvironment.ApplicationBasePath;

            //直接注册某一个类和接口
            //左边的是实现类，右边的As是接口
            //方法1
            builder.RegisterType<AdvertisementServices>().As<IAdvertisementServices>();


            //注册要通过反射创建的组件
            //方法2
            //var servicesDllFile = Path.Combine(basePath, "abtest.dll");
            //var assemblysServices = Assembly.LoadFrom(servicesDllFile);
            //方法3
            var assemblysServices = Assembly.Load("abtest");
            builder.RegisterAssemblyTypes(assemblysServices)
                      .AsImplementedInterfaces()
                      .InstancePerLifetimeScope()
                      .EnableInterfaceInterceptors();

        }





        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                //c.SwaggerEndpoint($"/swagger/V1/swagger.json", $"{ApiName} V1");

                //路径配置，设置为空，表示直接在根域名（localhost:8001）访问该文件,注意localhost:8001/swagger是访问不到的，去launchSettings.json把launchUrl去掉，如果你想换一个路径，直接写名字即可，比如直接写c.RoutePrefix = "doc";
                typeof(ApiVersions).GetEnumNames().OrderByDescending(e => e).ToList().ForEach(version =>
                {
                    c.SwaggerEndpoint($"/swagger/{version}/swagger.json", $"{ApiName} {version}");
                });
                c.RoutePrefix = "";
            });
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
