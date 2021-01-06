using apiword.Model;
using apiword.Models;
using apiword.SwaggerHelper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using static apiword.SwaggerHelper.CustomApiVersion;

namespace apiword.Controllers.v1
{
    [Route("[controller]")]
    [ApiExplorerSettings(GroupName = "v1")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        /// <summary>
        /// 程序主页面
        /// </summary>
        /// <returns></returns>
        [HttpGet("Index")]
        public IActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 程序界面二
        /// </summary>
        /// <returns></returns>
        [HttpGet("Privacy")]
        public IActionResult Privacy()
        {
            return View();
        }



        /// <summary>
        /// 测试接口
        /// </summary>
        /// <param name="Name">姓名</param>
        /// <param name="years">年龄</param>
        /// <returns></returns>
        [HttpPost("PlayTest")]
        public string PlayTest(string Name, string years )
        {
            return Name+ years + "测试成功";
        }

        /// <summary>
        /// 错误页面
        /// </summary>
        /// <returns></returns>
        [HttpPost("Error")]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }



        [HttpGet]
        [CustomRoute(ApiVersions.v1, "V2Test")]

        public object V2_Test(Love love)
        {
            return Ok(new { status = 220, data = "我是第二版的博客信息" });
        }
    }
}
