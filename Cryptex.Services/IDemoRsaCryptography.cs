using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cryptex.Services
{
    public interface IDemoRsaCryptography
    {
        ulong P { get; }
        ulong Q { get; }
        ulong N { get; }
        ulong D { get; }
        uint E { get; }
        Task PSet(ulong p);
        Task QSet(ulong q);
        Task NSet();
        Task DSet();
        void ESet();
        Task<List<string>> Encrypt(string plainText);
        Task<string> Decrypt(List<string> input, int d, int n);
    }
}