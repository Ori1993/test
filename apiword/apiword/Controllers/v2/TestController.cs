using apiword.Model;
using apiword.SwaggerHelper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
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

    public class TestController : ControllerBase
    {
        private readonly IHttpContextAccessor _accessor;
        public IHttpContextAccessor HttpContextAccessor { get; set; }

        public TestController(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }


        /// <summary>
        /// 测试接口
        /// </summary>
        /// <param name="love">POSt参数模型</param>
        /// <returns></returns>
        /// [HttpPost("Test")]
        [HttpPost]
        [CustomRoute(ApiVersions.v2, "Test")]
        public string Test(Love love)
        {
            if (love.Name == "fwy")
            {
                var tt = string.Create(love.Name.Length, love.Name, (sc, v) =>
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

        [HttpGet]
        [CustomRoute(ApiVersions.v2, "Jwttest")]
        public ActionResult<IEnumerable<string>> Jwttest()
        {
            var claims = new Claim[]
                {
                    new Claim(ClaimTypes.Name,"Ori"),
                    new Claim(JwtRegisteredClaimNames.Email,"361548856@qq.com"),
                    new Claim(JwtRegisteredClaimNames.Sub,"1"),
                };
            //var token_simple = new JwtSecurityToken(claims: claims);

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("qweasdzxcqweasdzxc"));

            var token = new JwtSecurityToken(
                issuer: "https://localhost:5000",
                audience: "https://localhost:5001",
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256));

            var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);
            return new string[] { jwtToken };
        }

        [HttpGet]
        [Authorize]
        [CustomRoute(ApiVersions.v2, "Jwtget")]
        //[Authorize]
        public ActionResult<IEnumerable<string>> Jwtget(string jwtStr)
        {
            //1
            var jwtHandler = new JwtSecurityTokenHandler();
            JwtSecurityToken jwtToken = jwtHandler.ReadJwtToken(jwtStr);
            //2
            var sub = User.FindFirst(d => d.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name")?.Value;
            //3
            var name = _accessor.HttpContext.User.Identity.Name;
            var claims = _accessor.HttpContext.User.Claims;
            var claimTypeVal = (from item in claims
                                where item.Type == JwtRegisteredClaimNames.Email
                                select item.Value).ToList();
            return new string[] { JsonConvert.SerializeObject(jwtToken), sub, name, JsonConvert.SerializeObject(claimTypeVal) };
        }
    }
}