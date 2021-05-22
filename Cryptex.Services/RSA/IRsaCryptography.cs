namespace Cryptex.Services.RSA
{
    interface IRsaCryptography
    {
        string Encrypt(string text);
        string Decrypt(string encryptedText);
    }
}
