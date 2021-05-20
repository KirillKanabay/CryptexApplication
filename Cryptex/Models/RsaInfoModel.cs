using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Xps.Packaging;

namespace Cryptex.Models
{
    public class RsaInfoModel
    {
        public FixedDocumentSequence Load()
        {
            using XpsDocument doc = new XpsDocument("Info/rsainfo.xps", FileAccess.Read);
            return doc.GetFixedDocumentSequence();
        }
    }
}
