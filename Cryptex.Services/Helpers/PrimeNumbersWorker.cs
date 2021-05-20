using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace Cryptex.Services.Helpers
{
    public class PrimeNumbersWorker : IPrimeNumbersWorker
    {
        private readonly IGcdNumbersWorker _gcdNumbersWorker;
        public PrimeNumbersWorker(IGcdNumbersWorker gcdNumbersWorker)
        {
            _gcdNumbersWorker = gcdNumbersWorker;
        }
        public bool IsPrime(long num)
        {
            for (long i = 2; i <= (long)Math.Sqrt(num); i++)
            {
                if (num % i == 0)
                    return false;
            }

            return true;
        }

        public bool IsCoprime(long num1, long num2)
        {
            return _gcdNumbersWorker.Gcd((long)num1, (long)num2) == 1;
        }

        /// <summary>
        /// Метод нахождения простых чисел до maxRange алгоритмом "Решета Эратосфена"
        /// </summary>
        /// <param name="maxRange"></param>
        /// <returns></returns>
        public List<int> GetPrimes(int maxRange)
        {
            var numbers = Enumerable.Range(3, maxRange).ToList();
            
            for (var i = 0; i < numbers.Count; i++)
            {
                for (var j = 2; j < maxRange; j++)
                {
                    //удаляем кратные числа из списка
                    numbers.Remove(numbers[i] * j);
                }
            }

            return numbers;
        }

        public List<uint> GetPrimesFromFile(int maxRange)
        {
            BinaryFormatter bf = new BinaryFormatter();
            List<uint> primes = new List<uint>();
            using (var stream = new FileStream("primes.dat", FileMode.Open))
            {
                primes = (List<uint>)bf.Deserialize(stream);
            }

            if (maxRange > primes.Count)
            {
                maxRange = primes.Count;
            }
            return primes.Take(maxRange).ToList();
        }
    }
}
