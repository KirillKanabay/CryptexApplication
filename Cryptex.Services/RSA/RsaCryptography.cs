using System;
using System.Security.Cryptography;

namespace Cryptex.Services.RSA
{
    public class RsaCryptography : IRsaCryptography
    {
        public string Encrypt(byte[] dataToEncrypt, RSAParameters publicKey)
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();

            rsa.ImportParameters(publicKey);

            var encryptedData = rsa.Encrypt(dataToEncrypt, false);

        }

        public string Decrypt(string encryptedText)
        {
            throw new NotImplementedException();
        }
    }
}
