using System;
using System.Security.Cryptography;

namespace Cryptex.Services.RSA
{
    public class RsaKeyCryptography
    {
        private RSACryptoServiceProvider _cryptoServiceProvider;

        public RSAParameters PublicKey;
        public RSAParameters PrivateKey;
        public string Name { get; set; }

        public RsaKeyCryptography(SerializableRsaKeys keys, string name)
        {
            Name = name;
            SetKeys(keys);
        }
        public RsaKeyCryptography(int dwKeySize, string name)
        {
            Name = name;
            _cryptoServiceProvider = new RSACryptoServiceProvider(dwKeySize);
            PublicKey = _cryptoServiceProvider.ExportParameters(false);
            PrivateKey = _cryptoServiceProvider.ExportParameters(true);
        }

        public void SetKeys(SerializableRsaKeys keys)
        {
            var csp = new RSACryptoServiceProvider();
            csp.FromXmlString(keys.PublicKeyXml);
            PublicKey = csp.ExportParameters(false);

            csp = new RSACryptoServiceProvider();
            csp.FromXmlString(keys.PrivateKeyXml);
            PrivateKey = csp.ExportParameters(true);
        }

        public SerializableRsaKeys GetSerializableRsaKeys()
        {
            if (_cryptoServiceProvider is null)
            {
                _cryptoServiceProvider = new RSACryptoServiceProvider();
                _cryptoServiceProvider.ImportParameters(PrivateKey);
            }
            return new SerializableRsaKeys(
                publicKeyXml: _cryptoServiceProvider.ToXmlString(false),
                privateKeyXml: _cryptoServiceProvider.ToXmlString(true));
        } 
    }
}