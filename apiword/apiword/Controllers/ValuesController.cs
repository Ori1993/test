using abtest.abTest;
using apiword.Test;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace apiword.Controllers
{
    
    public class ValuesController : Controller
    {
        private readonly ISingTest sing;
        private readonly ITranTest tran; 
        private readonly ISconTest scon; 
        private readonly IAService aService;
        private readonly IAdvertisementServices advertisementServices;
        private readonly Iabtt btt;

        public ValuesController(ISingTest sing, ITranTest tran, ISconTest scon, IAService aService, IAdvertisementServices advertisementServices, Iabtt btt)
        {
            this.sing = sing;
            this.tran = tran;
            this.scon = scon;
            this.aService = aService;
            this.advertisementServices = advertisementServices;
            this.btt = btt;
        
        }
        public IActionResult Index()
        {
            return View();
        }


        [HttpGet]
        public ActionResult<IEnumerable<string>> SetTest()
        {
            sing.Age = 18;
            sing.Name = "小红";

            tran.Age = 19;
            tran.Name = "小明";

            scon.Age = 20;
            scon.Name = "小蓝";

            aService.RedisTest();


            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        [HttpGet]
        public ActionResult<int> Test()
        {
            var tt = advertisementServices.Test();
            return tt;
        }

        [HttpGet]
        public ActionResult<string> myTest()
        {
            var tt = btt.mytest();
            return tt;
        }
    }
}
