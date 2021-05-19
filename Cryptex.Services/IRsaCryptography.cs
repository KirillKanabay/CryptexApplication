using System.Threading.Tasks;

namespace Cryptex.Services
{
    interface IRsaCryptography
    {
        string Encrypt(string text);
        string Decrypt(string encryptedText);
    }
}
