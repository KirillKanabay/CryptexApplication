using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Cryptex.Services;
using Cryptex.Services.Helpers;
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
        public async Task CheckSetP(ulong p)
        {
            IPrimeNumbersWorker pnw = new PrimeNumbersWorker();
            var drc = new DemoRsaCryptography(pnw);

            await drc.PSet(p);

            Assert.AreEqual(p, drc.P);
        }

        [Test]
        [TestCase(1u)]
        [TestCase(3u)]
        [TestCase(5u)]
        [TestCase(13u)]
        [TestCase(31u)]
        public async Task CheckSetQ(ulong q)
        {
            IPrimeNumbersWorker pnw = new PrimeNumbersWorker();
            DemoRsaCryptography drc = new DemoRsaCryptography(pnw);

            await drc.QSet(q);

            Assert.AreEqual(q, drc.Q);
        }

        [Test]
        [TestCase(4u)]
        [TestCase(6u)]
        [TestCase(8u)]
        [TestCase(24u)]
        [TestCase(32u)]
        public void CheckSetP_throwArgumentException(ulong p)
        {
            IPrimeNumbersWorker pnw = new PrimeNumbersWorker();
            DemoRsaCryptography drc = new DemoRsaCryptography(pnw);

            Assert.ThrowsAsync(typeof(ArgumentException), async () => await drc.PSet(p));
        }

        [Test]
        [TestCase(4u)]
        [TestCase(6u)]
        [TestCase(8u)]
        [TestCase(24u)]
        [TestCase(32u)]
        public void CheckSetQ_throwArgumentException(ulong q)
        {
            IPrimeNumbersWorker pnw = new PrimeNumbersWorker();
            DemoRsaCryptography drc = new DemoRsaCryptography(pnw);
            
            Assert.ThrowsAsync(typeof(ArgumentException), async () => await drc.QSet(q));
        }
        [Test]
        public async Task CheckSetN()
        {
            IPrimeNumbersWorker pnw = new PrimeNumbersWorker();
            DemoRsaCryptography drc = new DemoRsaCryptography(pnw);
            await drc.PSet(3);
            await drc.QSet(7);

            await drc.NSet();

            Assert.AreEqual(21, drc.N);
        }

        [Test]
        public async Task CheckSetD()
        {
            IPrimeNumbersWorker pnw = new PrimeNumbersWorker();
            DemoRsaCryptography drc = new DemoRsaCryptography(pnw);
            await drc.PSet(3);
            await drc.QSet(7);

            await drc.DSet();

            Assert.AreEqual(12, drc.D);
        }

        [Test]
        public async Task CheckSetE()
        {
            IPrimeNumbersWorker pnw = new PrimeNumbersWorker();
            DemoRsaCryptography drc = new DemoRsaCryptography(pnw);
            await drc.PSet(3);
            await drc.QSet(7);
            await drc.DSet();

            drc.ESet();

            Assert.AreEqual(5, drc.E);
        }
    }
}
