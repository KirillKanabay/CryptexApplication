using System.Security.Cryptography;

namespace Cryptex.Services.RSA
{
    public interface IRsaCryptography
    {
        byte[] Encrypt(byte[] dataToEncrypt, RSAParameters publicKey);
        byte[] Decrypt(byte[] dataToDecrypt, RSAParameters privateKey);
    }
}
