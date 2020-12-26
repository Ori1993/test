using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dynamicproxylearning.Services
{
    public class Welcome : IWelcome
    {
       public string SayHi()
        {
            return "Hello !";
        }
    }
}
