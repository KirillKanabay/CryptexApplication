using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using Cryptex.Services.Helpers;

namespace Cryptex.Services
{
    public class DemoRsaCryptography : IDemoRsaCryptography
    {
        private readonly IPrimeNumbersWorker _primeNumbersWorker;
        private readonly IGcdNumbersWorker _gcdNumbersWorker;
        public DemoRsaCryptography(IPrimeNumbersWorker primeNumbersWorker, IGcdNumbersWorker gcdNumbersWorker)
        {
            _primeNumbersWorker = primeNumbersWorker;
            _gcdNumbersWorker = gcdNumbersWorker;
        }

        public long P { get; private set; }
        public long Q { get; private set; }
        public long N { get; private set; }
        public long Fi { get; private set; }
        public long D { get; private set; }
        public uint E { get; private set; }

        public async Task PSet(long p)
        {
            bool isPrime = await Task.Run(() => _primeNumbersWorker.IsPrime(p));
            if (!isPrime)
            {
                throw new ArgumentException("Число P не является простым");
            }
            P = p;
        }
        public async Task QSet(long q)
        {
            bool isPrime = await Task.Run(() => _primeNumbersWorker.IsPrime(q));
            if (!isPrime)
            {
                throw new ArgumentException("Число Q не является простым");
            }
            Q = q;
        }
        public async Task NSet()
        {
            N = await Task.Run(() => P * Q);
        }
        public async Task FiSet()
        {
            Fi = await Task.Run(() => (P - 1) * (Q - 1));
        }
        public async Task ESet()
        {
            E = await Task.Run(() => _primeNumbersWorker
                .GetPrimesFromFile(2000)
                .AsParallel()
                .First(primeNum => _primeNumbersWorker.IsCoprime((long)primeNum, Fi)));
        }

        public async Task DSet()
        {
            D = await Task.Run(() =>
            {
                long fi = (long)Fi;
                long tempD = 3;
                for (; tempD < fi - 1; tempD++)
                {
                    if ((tempD * E) % fi == 1)
                    {
                        return tempD;
                    }
                }
                return tempD;
            });
        }

        public async Task Calculate()
        {
            await NSet();
            await FiSet();
            await ESet();
            await DSet();
        }

        public async Task<List<string>> Encrypt(string plainText)
        {
            List<string> result = new List<string>();

            BigInteger bi;

            foreach (int index in plainText)
            {
                bi = new BigInteger(index);
                bi = await Task.Run(() => bi.PowAndMod(E, N));

                result.Add(bi.ToString());
            }

            return result;
        }

        public async Task<string> Decrypt(List<string> input, long d, long n)
        {
            string result = "";

            BigInteger bi;

            foreach (string item in input)
            {
                if (item.Trim() == "")
                    continue;
                bi = new BigInteger(Convert.ToInt64(item));
                bi = await Task.Run(() => bi.PowAndMod(d, n));

                int index = Convert.ToInt32(bi.ToString());

                result += (char)index;
            }

            return result;
        }
    }
}
