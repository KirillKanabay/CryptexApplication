using System;
using System.Security.Cryptography;

namespace Cryptex.Services.RSA
{
    public class RsaKeyCryptography
    {
        private readonly RSACryptoServiceProvider _cryptoServiceProvider;

        public RSAParameters PublicKey;
        public RSAParameters PrivateKey;
        public readonly string Name;

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

        public SerializableRsaKeys GetSerializableRsaKeys() => new SerializableRsaKeys(
            publicKeyXml: _cryptoServiceProvider.ToXmlString(false),
            privateKeyXml: _cryptoServiceProvider.ToXmlString(true));
    }
}