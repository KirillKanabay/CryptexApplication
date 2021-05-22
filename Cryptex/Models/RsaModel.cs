using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Documents;
using Cryptex.Services.RSA;

namespace Cryptex.Models
{
    public class RsaModel
    {
        public event Func<object, Task> RsaModelChanged; 
        private readonly IRsaKeyFileWorker _rsaKeyFileWorker;

        public RsaModel(IRsaKeyFileWorker rsaKeyFileWorker)
        {
            _rsaKeyFileWorker = rsaKeyFileWorker;
        }

        public async Task<List<RsaKeyCryptography>> LoadAsync()
        {
            return await _rsaKeyFileWorker.LoadKeys();
        }

        public async Task AddAsync(RsaKeyCryptography rkc)
        {
            await _rsaKeyFileWorker.Save(rkc);
            RsaModelChanged?.Invoke(rkc);
        }

        public void Delete(RsaKeyCryptography rkc)
        {
            _rsaKeyFileWorker.Delete(rkc);
            RsaModelChanged?.Invoke(rkc);
        }

        public async Task<RsaKeyCryptography> ImportKey(string path)
        {
            return await _rsaKeyFileWorker.Import(path);
        }
    }
}
