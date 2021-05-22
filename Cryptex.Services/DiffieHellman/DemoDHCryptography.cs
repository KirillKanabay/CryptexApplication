using System;
using System.Numerics;
using System.Threading.Tasks;
using Cryptex.Services.Helpers;

namespace Cryptex.Services.DiffieHellman
{
    public class DemoDHCryptography : IDemoDHCryptography
    {
        public int G { get; set; }
        public int P { get; set; }
        public int PrivateA { get; set; }
        public int PrivateB { get; set; }
        public int PublicA { get; private set; }
        public int PublicB { get; private set; }
        public int KeyA { get; set; }
        public int KeyB { get; set; }

        public async Task<int> CalculatePublicA()
        {
            if (PrivateA <= 0)
            {
                throw new ArgumentException("Не указан приватный ключ А");
            }

            if (G <= 0)
            {
                throw new ArgumentException("Не указан публичный ключ G");
            }

            if (P <= 0)
            {
                throw new ArgumentException("Не указан публичный ключ P");
            }

            PublicA = await Task.Run(()=>int.Parse(new BigInteger(G).PowAndMod(PrivateA,P).ToString()));
            return PublicA;
        }

        public async Task<int> CalculatePublicB()
        {
            if (PrivateB <= 0)
            {
                throw new ArgumentException("Не указан приватный ключ B");
            }

            if (G <= 0)
            {
                throw new ArgumentException("Не указан публичный ключ G");
            }

            if (P <= 0)
            {
                throw new ArgumentException("Не указан публичный ключ P");
            }

            PublicB = await Task.Run(() => int.Parse(new BigInteger(G).PowAndMod(PrivateB, P).ToString()));
            return PublicB;
        }

        public async Task<int> CalculateKeyA()
        {
            if (PublicB <= 0)
            {
                throw new ArgumentException("Не рассчитан публичный ключ B");
            }

            if (PrivateA <= 0)
            {
                throw new ArgumentException("Не указан приватный ключ A");
            }

            if (P <= 0)
            {
                throw new ArgumentException("Не указан публичный ключ P");
            }
            KeyA = await Task.Run(() => int.Parse(new BigInteger(PublicB).PowAndMod(PrivateA, P).ToString()));

            return KeyA;
        }

        public async Task<int> CalculateKeyB()
        {
            if (PublicA <= 0)
            {
                throw new ArgumentException("Не рассчитан публичный ключ A");
            }

            if (PrivateB <= 0)
            {
                throw new ArgumentException("Не указан приватный ключ B");
            }

            if (P <= 0)
            {
                throw new ArgumentException("Не указан публичный ключ P");
            }
            KeyB = await Task.Run(() => int.Parse(new BigInteger(PublicA).PowAndMod(PrivateB, P).ToString()));

            return KeyB;
        }
    }
}
