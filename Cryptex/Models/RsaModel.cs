using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using Cryptex.Helpers.Extensions;
using Cryptex.Services.Hash;
using Cryptex.Services.RSA;

namespace Cryptex.Models
{
    public class RsaModel
    {
        public event Func<object, Task> RsaModelChanged; 
        private readonly IRsaKeyFileWorker _rsaKeyFileWorker;
        private readonly IRsaCryptography _rsaCryptography;
        private readonly IHashCalculator _hashCalculator;
        private readonly UnicodeEncoding _unicodeEncoding;
        public RsaModel(IRsaKeyFileWorker rsaKeyFileWorker, IRsaCryptography rsaCryptography, IHashCalculator hashCalculator)
        {
            _rsaKeyFileWorker = rsaKeyFileWorker;
            _rsaCryptography = rsaCryptography;
            _hashCalculator = hashCalculator;
            _unicodeEncoding = new UnicodeEncoding();
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

        public async Task<string> Encrypt(string plainText, RsaKeyCryptography rkc)
        {
            try
            {
                var data = _unicodeEncoding.GetBytes(plainText);
                var publicKey = rkc.PublicKey;

                var task = Task.Run(() => _rsaCryptography.Encrypt(data, publicKey));
                var encryptedData = await task;

                return encryptedData.ToHexadecimalString();
            }
            catch (CryptographicException e)
            {
                throw new ArgumentException("Не удалось зашифровать файл. Скорее всего ваше сообщение слишком длинное.");
            }
        }

        public async Task<string> Decrypt(string encryptedText, RsaKeyCryptography rkc)
        {
            var data = encryptedText.FromHexadecimalString();
            var privateKey = rkc.PrivateKey;

            var decryptedData = await Task.Run(() => _rsaCryptography.Decrypt(data, privateKey));

            return _unicodeEncoding.GetString(decryptedData);
        }

        public async Task<string> CalculateHash(string path) => await _hashCalculator.ComputeChecksum(path);

        public async Task Export(RsaKeyCryptography rkc, string path) => await _rsaKeyFileWorker.Save(rkc, path);

        public async Task<bool> CheckSignature(RsaKeyCryptography rkc, string signature, string hash)
        {
            string decryptedText;
            try
            {
                decryptedText = await Decrypt(signature, rkc);
            }
            catch
            {
                return false;
            }

            return decryptedText.ToLower() == hash.ToLower();
        }
    }
}
