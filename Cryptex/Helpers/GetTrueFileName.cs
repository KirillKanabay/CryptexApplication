using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Cryptex.Helpers
{
    public static class TrueFileName
    {
        public static string Get(string name)
        {
            var invalidChars = Path.GetInvalidFileNameChars().ToList();
            var fileNameChars = name.ToCharArray();

            for (int i = 0; i < fileNameChars.Length; ++i)
                if (invalidChars.Contains(fileNameChars[i]))
                    fileNameChars[i] = '_';

            return new string(fileNameChars);
        }
    }
}
