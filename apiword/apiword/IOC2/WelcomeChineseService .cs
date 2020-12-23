using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace apiword.IOC2
{
    public class WelcomeChineseService : IMoreImplService
    {
        public string SayWelocome()
        {
            return "欢迎";
        }
    }
}
