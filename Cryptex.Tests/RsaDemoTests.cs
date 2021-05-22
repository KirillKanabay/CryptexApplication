using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Cryptex.Services;
using Cryptex.Services.Helpers;
using Cryptex.Services.RSA;
using NUnit.Framework;
using NUnit.Framework.Internal.Commands;

namespace Cryptex.Tests
{
    [TestFixture]
    public class RsaDemoTests
    {
        [Test]
        [TestCase(1u)]
        [TestCase(3u)]
        [TestCase(5u)]
        [TestCase(13u)]
        [TestCase(31u)]
        public async Task CheckSetP(long p)
        {
            IGcdNumbersWorker gcd = new GcdNumbersWorker();
            IPrimeNumbersWorker pnw = new PrimeNumbersWorker(gcd);
            DemoRsaCryptography drc = new DemoRsaCryptography(pnw, gcd);

            await drc.PSet(p);

            Assert.AreEqual(p, drc.P);
        }

        [Test]
        [TestCase(1u)]
        [TestCase(3u)]
        [TestCase(5u)]
        [TestCase(13u)]
        [TestCase(31u)]
        public async Task CheckSetQ(long q)
        {
            IGcdNumbersWorker gcd = new GcdNumbersWorker();
            IPrimeNumbersWorker pnw = new PrimeNumbersWorker(gcd);
            DemoRsaCryptography drc = new DemoRsaCryptography(pnw, gcd);

            await drc.QSet(q);

            Assert.AreEqual(q, drc.Q);
        }

        [Test]
        [TestCase(4u)]
        [TestCase(6u)]
        [TestCase(8u)]
        [TestCase(24u)]
        [TestCase(32u)]
        public void CheckSetP_throwArgumentException(long p)
        {
            IGcdNumbersWorker gcd = new GcdNumbersWorker();
            IPrimeNumbersWorker pnw = new PrimeNumbersWorker(gcd);
            DemoRsaCryptography drc = new DemoRsaCryptography(pnw, gcd);

            Assert.ThrowsAsync(typeof(ArgumentException), async () => await drc.PSet(p));
        }

        [Test]
        [TestCase(4u)]
        [TestCase(6u)]
        [TestCase(8u)]
        [TestCase(24u)]
        [TestCase(32u)]
        public void CheckSetQ_throwArgumentException(long q)
        {
            IGcdNumbersWorker gcd = new GcdNumbersWorker();
            IPrimeNumbersWorker pnw = new PrimeNumbersWorker(gcd);
            DemoRsaCryptography drc = new DemoRsaCryptography(pnw, gcd);

            Assert.ThrowsAsync(typeof(ArgumentException), async () => await drc.QSet(q));
        }
        [Test]
        public async Task CheckSetN()
        {
            IGcdNumbersWorker gcd = new GcdNumbersWorker();
            IPrimeNumbersWorker pnw = new PrimeNumbersWorker(gcd);
            DemoRsaCryptography drc = new DemoRsaCryptography(pnw, gcd);

            await drc.PSet(3);
            await drc.QSet(7);

            await drc.NSet();

            Assert.AreEqual(21, drc.N);
        }

        [Test]
        public async Task CheckSetD()
        {
            IGcdNumbersWorker gcd = new GcdNumbersWorker();
            IPrimeNumbersWorker pnw = new PrimeNumbersWorker(gcd);
            DemoRsaCryptography drc = new DemoRsaCryptography(pnw, gcd);

            await drc.PSet(3557);
            await drc.QSet(2579);
            await drc.Calculate();

            Assert.AreEqual(6111579, drc.D);
        }

        [Test]
        public async Task CheckSetE()
        {
            IGcdNumbersWorker gcd = new GcdNumbersWorker();
            IPrimeNumbersWorker pnw = new PrimeNumbersWorker(gcd);
            DemoRsaCryptography drc = new DemoRsaCryptography(pnw, gcd);

            await drc.PSet(3);
            await drc.QSet(7);
            await drc.Calculate();

            Assert.AreEqual(5, drc.E);
        }

        [Test]
        public async Task EncryptDecryptTest()
        {
            IGcdNumbersWorker gcd = new GcdNumbersWorker();
            IPrimeNumbersWorker pnw = new PrimeNumbersWorker(gcd);
            DemoRsaCryptography drc = new DemoRsaCryptography(pnw, gcd);

            await drc.PSet(3557);
            await drc.QSet(2579);
            await drc.Calculate();

            var encryptedMessage = await drc.Encrypt("Кирилл");
            var decryptedMessage = await drc.Decrypt(encryptedMessage, drc.D, drc.N);

            Assert.AreEqual("Кирилл", decryptedMessage);
        }
    }
}
