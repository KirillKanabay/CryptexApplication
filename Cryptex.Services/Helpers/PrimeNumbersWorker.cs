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
        public bool IsPrime(ulong num)
        {
            for (ulong i = 2; i <= (ulong)Math.Sqrt(num); i++)
            {
                if (num % i == 0)
                    return false;
            }

            return true;
        }

        public bool IsCoprime(ulong num1, ulong num2)
        {
            if (num1 == num2)
            {
                return num1 == 1;
            }
            else
            {
                if (num1 > num2)
                {
                    return IsCoprime(num1 - num2, num2);
                }
                else
                {
                    return IsCoprime(num2 - num1, num1);
                }
            }
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

            return primes.Take(maxRange).ToList();
        }
    }
}
