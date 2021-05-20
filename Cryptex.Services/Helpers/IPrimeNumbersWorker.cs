using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cryptex.Services.Helpers
{
    public interface IPrimeNumbersWorker
    {
        bool IsPrime(ulong num);
        bool IsCoprime(ulong num1, ulong num2);
        List<int> GetPrimes(int maxRange);
        List<uint> GetPrimesFromFile(int maxRange);
    }
}