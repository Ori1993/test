
using apiword.IOC2;
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
        public string ApiName { get; set; } = "��֪���";
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();


            services.AddTransient<ITranTest, TranTest>();
            services.AddSingleton<ISingTest,SingTest>();
            services.AddScoped<ISconTest,SconTest>();
            services.AddScoped<IAService,AService>();

            //services.AddScoped<IMoreImplService, WelcomeChineseService>();
            //services.AddScoped<IMoreImplService, WelcomeEnglishService>();
            //SingletonFactory singletonFactory = new SingletonFactory();
            //singletonFactory.AddService<IMoreImplService>(new WelcomeChineseService(), "Chinese");
            //singletonFactory.AddService<IMoreImplService>(new WelcomeEnglishService(), "English");

            //services.AddSingleton(singletonFactory);

            services.AddScoped<WelcomeChineseService>();
            services.AddScoped<WelcomeEnglishService>();
            services.AddScoped(factory =>
            {
                Func<string, IMoreImplService> accesor = key =>
                {
                    if (key.Equals("Chinese"))
                    {
                        // ��Ϊ�����Ǵ�������ȡ����ʵ���ģ��������ǿ��Կ�����������
                        return factory.GetService<WelcomeChineseService>();
                    }
                    else if (key.Equals("English"))
                    {
                        return factory.GetService<WelcomeEnglishService>();
                    }
                    else
                    {
                        throw new ArgumentException($"Not Support key : {key}");
                    }
                };
                return accesor;
            });

            var basePath = ApplicationEnvironment.ApplicationBasePath;
            services.AddSwaggerGen(c =>
            {
                //c.SwaggerDoc("V1", new OpenApiInfo
                //{
                //    // {ApiName} �����ȫ�ֱ����������޸�
                //    Version = "V1",
                //    Title = $"{ApiName} �ӿ��ĵ�����V5.4",
                //    Description = "�ý����������֪��϶����ṩ�����нӿڣ���������ӿ�����",
                //    //Contact = new OpenApiContact { Name = ApiName, Email = "Blog.Core@xxx.com", Url = new Uri("https://www.jianshu.com/u/94102b59cc2a") },
                //    //License = new OpenApiLicense { Name = ApiName, Url = new Uri("https://www.jianshu.com/u/94102b59cc2a") }
                //});
                //c.OrderActionsBy(o => o.RelativePath);

                //var xmlPath = Path.Combine(basePath, "apiword.xml");//������Ǹո����õ�xml�ļ���
                //c.IncludeXmlComments(xmlPath, true);

                //var xmlModelPath = Path.Combine(basePath, "apiword.Model.xml");//�������Model���xml�ļ���
                //c.IncludeXmlComments(xmlModelPath);

                typeof(ApiVersions).GetEnumNames().ToList().ForEach(version =>
                {
                    c.SwaggerDoc(version, new OpenApiInfo
                    {
                        // {ApiName} �����ȫ�ֱ����������޸�
                        Version = version,
                        Title = $"{ApiName} �ӿ��ĵ�",
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

            //ֱ��ע��ĳһ����ͽӿ�
            //��ߵ���ʵ���࣬�ұߵ�As�ǽӿ�
            //����1
            builder.RegisterType<AdvertisementServices>().As<IAdvertisementServices>();


            //ע��Ҫͨ�����䴴�������
            //����2
            //var servicesDllFile = Path.Combine(basePath, "abtest.dll");
            //var assemblysServices = Assembly.LoadFrom(servicesDllFile);
            //����3
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

                //·�����ã�����Ϊ�գ���ʾֱ���ڸ�������localhost:8001�����ʸ��ļ�,ע��localhost:8001/swagger�Ƿ��ʲ����ģ�ȥlaunchSettings.json��launchUrlȥ����������뻻һ��·����ֱ��д���ּ��ɣ�����ֱ��дc.RoutePrefix = "doc";
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
