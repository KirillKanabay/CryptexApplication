using System;
using System.Collections.Generic;
using System.Text;

namespace Cryptex.Helpers.Extensions
{
    public static class StringExtensions
    {
        public static byte[] FromHexadecimalString(this string hex)
        {
            int numberChars = hex.Length;
            byte[] bytes = new byte[numberChars / 2];
            for (int i = 0; i < numberChars; i += 2)
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            return bytes;
        }
    }
}
