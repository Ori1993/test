using apiword.IOC2;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace apiword.Controllers
{
    public class HelloController : Controller
    {
        private readonly IEnumerable<IMoreImplService> _moreImplServices;
        private readonly SingletonFactory singletonFactory;

        // 各自定义需要的多个字段
        private readonly IMoreImplService moreImplServiceChinese;
        private readonly IMoreImplService moreImplServiceEnglish;

        private readonly Func<string, IMoreImplService> _serviceAccessor;

        //public HelloController(IEnumerable<IMoreImplService> moreImplServices)
        //{
        //    _moreImplServices = moreImplServices;

        //}

        //public HelloController(SingletonFactory singletonFactory)
        //{
        //    this.singletonFactory = singletonFactory;

        //    // 根据别名获取服务
        //    moreImplServiceChinese = singletonFactory.GetService<IMoreImplService>("Chinese");
        //    moreImplServiceEnglish = singletonFactory.GetService<IMoreImplService>("English");

        //}

        public HelloController(Func<string, IMoreImplService> serviceAccessor)
        {
            // 获取特定接口的服务访问器，然后根据别名获取
            _serviceAccessor = serviceAccessor;
            // 这里的别名，你可以配置到 appsetting.json 文件里，动态的修改获取对象实例
            // 然后再在接口中配置一个字段 string ImplementKeyName { get; }
            moreImplServiceChinese = _serviceAccessor("Chinese");
            moreImplServiceEnglish = _serviceAccessor("English");
        }
        public IActionResult Index()
        {
            return View();
        }


        [HttpGet]
        public object Get()
        {
            var result = "";
            // 调用多次输出
            foreach (var item in _moreImplServices)
            {
                result += item.SayWelocome() + "\n";
            }

            return result;
        }

        [HttpGet]
        public object Hello(string he)
        {
            //return moreImplServiceChinese.SayWelocome()+"++++"+moreImplServiceEnglish.SayWelocome();

            return moreImplServiceChinese.SayWelocome()+"+++"+he+"++" + moreImplServiceEnglish.SayWelocome();
        }


    }
}
