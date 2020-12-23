using apiword.Model;
using apiword.SwaggerHelper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static apiword.SwaggerHelper.CustomApiVersion;

namespace apiword.Controllers.v2
{
    /// <summary>
    /// 测试控制器
    /// </summary>
    [Route("[controller]")]
    //隐藏不显示
    //[ApiExplorerSettings(IgnoreApi = true)]
    //版本控制在哪个版本上显示
    //[ApiExplorerSettings(GroupName = "v2")]

    public class TestController : Controller
    {
        /// <summary>
        /// 测试首页
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [CustomRoute(ApiVersions.v2, "Index")]
        public IActionResult Index()
        {
            return View();
        }


        /// <summary>
        /// 测试接口
        /// </summary>
        /// <param name="love">POSt参数模型</param>
        /// <returns></returns>
        /// [HttpPost("Test")]
        [HttpPost]
        [CustomRoute(ApiVersions.v2, "Test")]
        public string Test(Love love )
        {
            if (love.Name =="fwy")
            {
                var tt = string.Create(love.Name.Length, love.Name,(sc,v)=>
                {
                    var prefixleng = v.Length * 25 / 100;
                    v.AsSpan(0, prefixleng).CopyTo(sc);
                    sc[prefixleng..].Fill('*');
                });

            }

            



            return love.Name + love.Age + "测试";
        }

        [HttpGet]
        [CustomRoute(ApiVersions.v2, "V2Test")]

        public object V2_Test(Love love)
        {
            return Ok(new { status = 220, data = "我是第二版的博客信息" });
        }
    }
}
