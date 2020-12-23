using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace apiword.Test
{

    public class AService : IAService
    {
        private ISingTest sing; ITranTest tran; ISconTest scon;
        public AService(ISingTest sing, ITranTest tran, ISconTest scon)
        {
            this.sing = sing;
            this.tran = tran;
            this.scon = scon;
        }
        public void RedisTest()
        {

        }
    }
}
