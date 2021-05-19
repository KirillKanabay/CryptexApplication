using Cryptex.Services;
using NUnit.Framework;

namespace Cryptex.Tests
{
    public class RsaTests
    {
        [Test]
        public void CheckEqualCryptedText()
        {
            RsaCryptography rc = new RsaCryptography();
            var encryptedData = rc.Encrypt("hello world");
            var decryptedData = rc.Decrypt(encryptedData);
            Assert.AreEqual("hello world", decryptedData);
        }
    }
}