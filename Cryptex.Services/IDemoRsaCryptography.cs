using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cryptex.Services
{
    public interface IDemoRsaCryptography
    {
        long P { get; }
        long Q { get; }
        long N { get; }
        long Fi { get; }
        long D { get; }
        uint E { get; }
        Task PSet(long p);
        Task QSet(long q);
        Task NSet();
        Task DSet();
        Task FiSet();
        Task ESet();
        Task Calculate();
        Task<List<string>> Encrypt(string plainText);
        Task<string> Decrypt(List<string> input, long d, long n);
    }
}