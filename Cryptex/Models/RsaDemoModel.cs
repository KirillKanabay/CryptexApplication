using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
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

        public long D => DemoRsa.D;
        public long E => DemoRsa.E;
        public long N => DemoRsa.N;
        public long Fi => DemoRsa.Fi;

        public async Task SetP(long p)
        {
            await DemoRsa.PSet(p);
        }

        public async Task SetQ(long q)
        {
            await DemoRsa.QSet(q);
        }

        public async Task Calculate() => await DemoRsa.Calculate();
    }
}
