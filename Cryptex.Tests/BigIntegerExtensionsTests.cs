using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using Cryptex.Services.Helpers;
using NUnit.Framework;

namespace Cryptex.Tests
{
    [TestFixture]
    class BigIntegerExtensionsTests
    {
        [Test]
        public void PowAndModTests()
        {
            BigInteger bi = new BigInteger(4051753);
            
            bi = bi.PowAndMod(6111579, 9173503);

            BigInteger expected = new BigInteger(111111); 
            Assert.AreEqual(expected, bi);
        }
    }
}
