using System;
using System.Text;
using Cryptex.Services;
using Cryptex.Services.RSA;
using NUnit.Framework;

namespace Cryptex.Tests
{
    [TestFixture]
    public class RsaTests
    {
        [Test]
        [TestCase("Hello world!")]
        [TestCase("Кирилл Канабай 12.09.2002")]
        [TestCase("                         ")]
        [TestCase("1234567890")]
        [TestCase("!;%:?*()_*(?*()?*()?*():?*<>...")]
        public void CheckEqualEncryptedDecryptedTextTest(string plainText)
        {
            RsaCryptography rc = new RsaCryptography();
            
            RsaKeyCryptography rck = new RsaKeyCryptography(2048, "Test");
            var publicKey = rck.PublicKey;
            var privateKey = rck.PrivateKey;

            UnicodeEncoding byteConverter = new UnicodeEncoding();

            var data = byteConverter.GetBytes(plainText);

            var encryptedData = rc.Encrypt(data, publicKey);
            var decryptedData = rc.Decrypt(encryptedData, privateKey);

            var decryptedText = byteConverter.GetString(decryptedData);
            
            Assert.AreEqual(plainText, decryptedText);
        }

        [Test]
        [TestCase("Hello world!")]
        [TestCase("Кирилл Канабай 12.09.2002")]
        [TestCase("                         ")]
        [TestCase("1234567890")]
        [TestCase("!;%:?*()_*(?*()?*()?*():?*<>...")]
        public void CompareEncryptedAndPlainTextTest(string plainText)
        {
            RsaCryptography rc = new RsaCryptography();

            RsaKeyCryptography rck = new RsaKeyCryptography(2048, "Test");
            var publicKey = rck.PublicKey;
            var privateKey = rck.PrivateKey;

            UnicodeEncoding byteConverter = new UnicodeEncoding();

            var data = byteConverter.GetBytes(plainText);

            var encryptedData = rc.Encrypt(data, publicKey);

            var encryptedText = byteConverter.GetString(encryptedData);

            Assert.AreNotEqual(plainText, encryptedText);
        }
    }
}