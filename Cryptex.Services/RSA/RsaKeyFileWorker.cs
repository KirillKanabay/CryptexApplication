using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;

namespace Cryptex.Services.RSA
{
    [Serializable]
    public struct SerializableRsaKeys
    {
        public string PublicKeyXml;
        public string PrivateKeyXml;

        public SerializableRsaKeys(string publicKeyXml, string privateKeyXml)
        {
            PublicKeyXml = publicKeyXml;
            PrivateKeyXml = privateKeyXml;
        }
    }

    public class RsaKeyFileWorker : IRsaKeyFileWorker
    {
        private const string Dir = "keys";

        public async Task<List<RsaKeyCryptography>> LoadKeys()
        {
            if (!Directory.Exists(Dir))
            {
                Directory.CreateDirectory(Dir);
            }

            var files = Directory.GetFiles(Dir, "*.ck");
            
            var bf = new BinaryFormatter();

            var keys = new List<RsaKeyCryptography>();

            foreach (var file in files)
            {
                string keyName = Path.GetFileNameWithoutExtension(file);
                await using var fs = new FileStream(file, FileMode.Open);
                try
                {
                    var key = (SerializableRsaKeys)bf.Deserialize(fs);
                    keys.Add(new RsaKeyCryptography(key, keyName));
                }
                catch(Exception e)
                {
                    Trace.WriteLine(e);
                }
            }

            return keys;
        }

        public async Task<RsaKeyCryptography> Import(string path)
        {
            var bf = new BinaryFormatter();
            await using var fs = new FileStream(path, FileMode.Open);
            RsaKeyCryptography rkc;
            try
            {
                string keyName = Path.GetFileNameWithoutExtension(path);
                var key = (SerializableRsaKeys)bf.Deserialize(fs);
                rkc = new RsaKeyCryptography(key, keyName);
            }
            catch (Exception)
            {
                throw new Exception("Не удалось открыть ключ.");
            }

            return rkc;
        }

        public async Task Save(RsaKeyCryptography rkc)
        {
            if (!Directory.Exists(Dir))
            {
                Directory.CreateDirectory(Dir);
            }

            var bf = new BinaryFormatter();
            await using var fs = new FileStream($"{Dir}/{rkc.Name}.ck", FileMode.OpenOrCreate);
            bf.Serialize(fs, rkc.GetSerializableRsaKeys());
        }

        public async Task Save(RsaKeyCryptography rkc, string path)
        {
            if (!Directory.Exists(Dir))
            {
                Directory.CreateDirectory(Dir);
            }

            var bf = new BinaryFormatter();
            await using var fs = new FileStream(path, FileMode.OpenOrCreate);
            bf.Serialize(fs, rkc.GetSerializableRsaKeys());
        }

        public void Delete(RsaKeyCryptography rkc)
        {
            try
            {
                File.Delete($"{Dir}\\{rkc.Name}.ck");
            }
            catch (Exception)
            {
                throw new Exception("Не удалось удалить ключ.");
            }
           
        }
    }
}
