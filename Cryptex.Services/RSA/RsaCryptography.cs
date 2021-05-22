using System;
using System.Security.Cryptography;

namespace Cryptex.Services.RSA
{
    public class RsaCryptography : IRsaCryptography
    {
        public byte[] Encrypt(byte[] dataToEncrypt, RSAParameters publicKey)
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();

            rsa.ImportParameters(publicKey);

            return rsa.Encrypt(dataToEncrypt, false);
        }

        public byte[] Decrypt(byte[] dataToDecrypt, RSAParameters privateKey)
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();

            rsa.ImportParameters(privateKey);

            return rsa.Decrypt(dataToDecrypt, false);
        }
    }
}
