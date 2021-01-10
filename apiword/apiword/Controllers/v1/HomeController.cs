using apiword.Model;
using apiword.Models;
using apiword.SwaggerHelper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
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
        /// 测试玩加接口
        /// </summary>
        /// <field name="name" type="String">憨憨</field>
        /// <remarks>
        /// 接口示例:
        /// <para>给爸爸换行听见没有</para>
        ///
        ///     {
        ///        "id": 你好,
        ///        "name": "Item1",
        ///        "isComplete": true
        ///     }
        ///
        /// </remarks>
        /// <param name="Name">姓名</param>
        /// <param name="years">年龄</param>
        /// <returns>A newly created TodoItem</returns>
        /// <response code="201">返回value字符串</response>
        /// <response code="400">如果id为空</response>
        [HttpGet("PlayTest")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public string PlayTest(string Name, string years)
        {
            return Name + years + "测试成功";
        }

        /// <summary>
        /// 测试Post接口1
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        [HttpPost("PostTest")]
        public string PostTest(Info info)
        {
            return info.Name + info.age + "测试成功";
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