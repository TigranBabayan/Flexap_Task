using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Flexap_Task.Helpers
{
    public static class RoundNum
    {
        public static int RoundNumber(this int num)
        {
            int rem = num % 10;
            return rem >= 5 ? (num - rem + 10) : (num - rem);
        }
    }
}
