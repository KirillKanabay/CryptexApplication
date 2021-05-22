using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cryptex.Services.RSA;
using NUnit.Framework;

namespace Cryptex.Tests
{
    [TestFixture]
    public class RsaKeyFileWorkerTests
    {
        [Test]
        public async Task CreateAndReadKey()
        {
            IRsaKeyFileWorker rsaKeyWorker = new RsaKeyFileWorker();
            var key = new RsaKeyCryptography(2048, "KirillKey");

            await rsaKeyWorker.Save(key);

            var keys = await rsaKeyWorker.LoadKeys();

            Assert.AreEqual(true, keys.Any(c => c.Name == "KirillKey"));
        }

        [Test]
        public async Task CreateAndDeleteKey()
        {
            IRsaKeyFileWorker rsaKeyWorker = new RsaKeyFileWorker();
            var key = new RsaKeyCryptography(2048, "DeletableKey");

            await rsaKeyWorker.Save(key);
            rsaKeyWorker.Delete(key);

            var keys = await rsaKeyWorker.LoadKeys();

            Assert.AreEqual(false, keys.Any(c => c.Name == "DeletableKey"));
        }
    }
}
