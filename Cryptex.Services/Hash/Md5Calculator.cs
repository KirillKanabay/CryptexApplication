using System;
using System.IO;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace Cryptex.Services.Hash
{
    public class Md5Calculator : IHashCalculator
    {
       public async Task<string> ComputeChecksum(string path)
        {
            await using (FileStream fs = System.IO.File.OpenRead(path))
            {
                MD5 md5 = new MD5CryptoServiceProvider();
                byte[] fileData = new byte[fs.Length];
                fs.Read(fileData, 0, (int)fs.Length);
                byte[] checkSum = md5.ComputeHash(fileData);
                string result = BitConverter.ToString(checkSum).Replace("-", String.Empty);
                return result;
            }
        }
    }
}
