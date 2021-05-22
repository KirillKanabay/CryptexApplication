using System.Security.Cryptography;

namespace Cryptex.Services.RSA
{
    interface IRsaCryptography
    {
        string Encrypt(byte[] dataToEncrypt, RSAParameters publicKey);
        string Decrypt(byte[] dataToDecrypt, RSAParameters privateKey);
    }
}
