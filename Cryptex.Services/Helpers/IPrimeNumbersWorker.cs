using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cryptex.Services.Helpers
{
    public interface IPrimeNumbersWorker
    {
        bool IsPrime(long num);
        bool IsCoprime(long num1, long num2);
        List<int> GetPrimes(int maxRange);
        List<uint> GetPrimesFromFile(int maxRange);
    }
}