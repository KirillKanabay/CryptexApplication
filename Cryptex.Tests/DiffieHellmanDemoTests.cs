using System.Threading.Tasks;
using Cryptex.Services;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace Cryptex.Tests
{
    [TestFixture]
    public class DiffieHellmanDemoTests
    {
        [Test]
        [TestCase(23,5,4,6)]
        [TestCase(7,9,4,5)]
        [TestCase(7234,933,422,5111)]
        [TestCase(1,2,3,4)]
        public async Task CheckEqualKeys(int g, int p, int privateA, int privateB)
        {
            IDemoDHCryptography dhc = new DemoDHCryptography();
            dhc.P = p;
            dhc.G = g;
            dhc.PrivateA = privateA;
            dhc.PrivateB = privateB;

            await dhc.CalculatePublicA();
            await dhc.CalculatePublicB();

            await dhc.CalculateKeyA();
            await dhc.CalculateKeyB();

            Assert.AreEqual(dhc.KeyA, dhc.KeyB);
        }
    }
}
