using System.Threading.Tasks;

namespace Cryptex.Services.Hash
{
    public interface IHashCalculator
    {
        Task<string> ComputeChecksum(string path);
    }
}