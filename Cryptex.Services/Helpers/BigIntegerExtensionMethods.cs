using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Cryptex.Services.Helpers
{
    public static class BigIntegerExtensionMethods
    {
        public static BigInteger PowAndMod(this BigInteger bi, long power,long mod)
        {
            BigInteger result = bi;
            for (long i = 0; i < power - 1; i++)
            {
                result = (result * bi) % mod;
            }

            return result;
        }
    }
}
