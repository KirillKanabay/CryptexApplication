using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cryptex.Services.RSA
{
    public interface IRsaKeyFileWorker
    {
        Task<List<RsaKeyCryptography>> LoadKeys();
        Task Import(string path);
        Task Save(RsaKeyCryptography rkc);
        Task Delete(RsaKeyCryptography rkc);
    }
}