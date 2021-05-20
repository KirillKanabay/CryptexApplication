using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using Cryptex.Services.Helpers;

namespace Cryptex.Services
{
    public class DemoRsaCryptography
    {
        private readonly IPrimeNumbersWorker _primeNumbersWorker;
        
        public DemoRsaCryptography(IPrimeNumbersWorker primeNumbersWorker)
        {
            _primeNumbersWorker = primeNumbersWorker;
        }

        public ulong P { get; private set; }
        public ulong Q { get; private set; }
        public ulong N { get; private set; }
        public ulong D { get; private set; }
        public uint E { get; private set; }

        public async Task PSet(ulong p)
        {
            bool isPrime = await Task.Run(() => _primeNumbersWorker.IsPrime(p));
            if (!isPrime)
            {
                throw new ArgumentException("Число P не является простым");
            }
            P = p;
        }
        public async Task QSet(ulong q)
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
        public async Task DSet()
        {
            D = await Task.Run(() => (P - 1) * (Q - 1));
        }
        public void ESet()
        {
            E = _primeNumbersWorker
                .GetPrimesFromFile((int)D - 1)
                .AsParallel()
                .First(primeNum => _primeNumbersWorker.IsCoprime((ulong)primeNum, D));
        }

        public async Task<List<string>> Encrypt(string plainText)
        {
            List<string> result = new List<string>();

            BigInteger bi;

            for (int i = 0; i < plainText.Length; i++)
            {
                int index = plainText[i];

                bi = new BigInteger(index);
                bi = await Task.Run(() => BigInteger.Pow(bi, (int) E));

                BigInteger n = new BigInteger((int) N);

                bi = bi % n;

                result.Add(bi.ToString());
            }

            return result;
        }

        public async Task<string> Decrypt(List<string> input, int d, int n)
        {
            string result = "";

            BigInteger bi;

            foreach (string item in input)
            {
                if (item.Trim() == "")
                    continue;
                bi = new BigInteger(Convert.ToInt64(item, 16));
                bi = await Task.Run(() => BigInteger.Pow(bi, (int)d));

                BigInteger n_ = new BigInteger((int)n);

                bi = bi % n_;

                int index = Convert.ToInt32(bi.ToString());

                result += (char)index;
            }

            return result;
        }
    }
}
