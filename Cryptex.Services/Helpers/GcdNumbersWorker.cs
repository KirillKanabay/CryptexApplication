using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Cryptex.Services.Helpers
{
    public class GcdNumbersWorker : IGcdNumbersWorker
    {
        public long GcdExtended(long a, long b, ref long x, ref long y)
        {
            if (a == 0)
            {
                x = 0;
                y = 1;
                return a;
            }

            long x1 = 1, y1 = 1;
            long gcd = GcdExtended(b % a, a, ref x1, ref y1);

            x = y1 - (b / a) * x1;
            y = x1;

            return gcd;
        }

        public long Gcd(long a, long b)
        {
            while (true)
            {
                if (a == b) return a;
                if (a > b)
                {
                    a = a - b;
                }
                else
                {
                    var a1 = a;
                    a = b - a;
                    b = a1;
                }
            }
        }
    }
}
