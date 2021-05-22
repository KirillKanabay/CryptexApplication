using System.Text;

namespace Cryptex.Helpers.Extensions
{
    public static class ByteArrayExtensions
    {
        public static string ToHexadecimalString(this byte[] bytes)
        {
            StringBuilder hex = new StringBuilder(bytes.Length * 2);
            foreach (byte b in bytes)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }
    }
}
