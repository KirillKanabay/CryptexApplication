using System.Threading.Tasks;
using Cryptex.Services.RSA;

namespace Cryptex.Models
{
    public class RsaModel
    {
        private readonly IRsaKeyFileWorker _rsaKeyFileWorker;

        public RsaModel(IRsaKeyFileWorker rsaKeyFileWorker)
        {
            _rsaKeyFileWorker = rsaKeyFileWorker;
        }

        public async Task Load() => await _rsaKeyFileWorker.LoadKeys();
    }
}
