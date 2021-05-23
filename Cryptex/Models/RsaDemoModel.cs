using System.Collections.Generic;
using System.Threading.Tasks;
using Cryptex.Services;
using Microsoft.VisualBasic;

namespace Cryptex.Models
{
    public class RsaDemoModel
    {
        private readonly IDemoRsaCryptography _demoRsa;
        private List<string> _encryptedMessage;

        public RsaDemoModel(IDemoRsaCryptography demoRsa)
        {
            _demoRsa = demoRsa;
        }
        public void Clear()
        {
            PlainText = default;
            EncryptedText = default;
            DecryptedText = default;
            _encryptedMessage = default;
            _demoRsa.Clear();
        }

        public string EncryptedText { get; set; }
        public string DecryptedText { get; set; }
        public long P => _demoRsa.P;
        public long Q => _demoRsa.Q;
        public long D => _demoRsa.D;
        public long E => _demoRsa.E;
        public long N => _demoRsa.N;
        public long Fi => _demoRsa.Fi;
        public string EncryptedMessage => string.Concat(_encryptedMessage);
        public string PlainText { get; set; }

        public async Task GenerateP() => await _demoRsa.GenerateP();
        public async Task GenerateQ() => await _demoRsa.GenerateQ();

        public async Task SetP(long p)
        {
            await _demoRsa.PSet(p);
        }
        public async Task SetQ(long q)
        {
            await _demoRsa.QSet(q);
        }

        public async Task Calculate() => await _demoRsa.Calculate();

        public async Task<string> Encrypt(string plainText)
        {
            PlainText = plainText;
            _encryptedMessage = await _demoRsa.Encrypt(plainText);
            return string.Concat(_encryptedMessage);
        }

        public async Task<string> Decrypt()
        {
            return await _demoRsa.Decrypt(_encryptedMessage, D, N);
        }

    }
}
