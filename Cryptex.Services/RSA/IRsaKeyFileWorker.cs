using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cryptex.Services.RSA
{
    public interface IRsaKeyFileWorker
    {
        Task<List<RsaKeyCryptography>> LoadKeys();
        Task<RsaKeyCryptography> Import(string path);
        Task Save(RsaKeyCryptography rkc);
        Task Save(RsaKeyCryptography rkc, string path);
        void Delete(RsaKeyCryptography rkc);
    }
}