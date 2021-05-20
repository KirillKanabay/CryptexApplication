using System;
using System.Collections.Generic;
using System.Text;
using Cryptex.Services;

namespace Cryptex.Models
{
    public class RsaDemoModel
    {
        public readonly IDemoRsaCryptography DemoRsa;

        public RsaDemoModel(IDemoRsaCryptography demoRsa)
        {
            DemoRsa = demoRsa;
        }
    }
}
