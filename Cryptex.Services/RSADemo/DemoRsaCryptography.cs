using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using Cryptex.Services.Helpers;

namespace Cryptex.Services.RSA
{
    public class DemoRsaCryptography : IDemoRsaCryptography
    {
        private readonly IPrimeNumbersWorker _primeNumbersWorker;
        public IGcdNumbersWorker NumbersWorker { get; }
        private readonly List<uint> _generatedPrimeNumbers;
        public DemoRsaCryptography(IPrimeNumbersWorker primeNumbersWorker, IGcdNumbersWorker gcdNumbersWorker)
        {
            _primeNumbersWorker = primeNumbersWorker;
            NumbersWorker = gcdNumbersWorker;
            _generatedPrimeNumbers = _primeNumbersWorker.GetPrimesFromFile(25000);
        }

        public void Clear()
        {
            P = default;
            Q = default;
            N = default;
            Fi = default;
            D = default;
            E = default;
        }

        public long P { get; private set; }
        public long Q { get; private set; }
        public long N { get; private set; }
        public long Fi { get; private set; }
        public long D { get; private set; }
        public uint E { get; private set; }

        public async Task GenerateP()
        {
            P = await Task.Run(() => _generatedPrimeNumbers[new Random().Next(300, 2000)]);
        }

        public async Task GenerateQ()
        {
            Q = await Task.Run(() => _generatedPrimeNumbers[new Random().Next(300, 2000)]);
        }

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
            E = await Task.Run(() => 
                _generatedPrimeNumbers
                    .Take(2000)
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
            List<string> result = await Task.Run(()=> 
                plainText
                         .Select(index => new BigInteger(index)
                         .PowAndMod(E, N)
                         .ToString("X"))
                         .ToList());

            return result;
        }

        public async Task<string> Decrypt(List<string> input, long d, long n)
        {
            string result = await Task.Run(() =>
                {
                    return string.Concat(
                        input
                             .Where(i => i.Trim() != "") //Отсеиваем пустые строки
                             .Select(i => (char) 
                                 Convert.ToInt32(new BigInteger(Convert.ToInt32(i, 16)) //Переводим в 16-ую СС
                                .PowAndMod(d, n) //Расшифровываем
                                .ToString())));
                });

            return result;
        }
    }
}
