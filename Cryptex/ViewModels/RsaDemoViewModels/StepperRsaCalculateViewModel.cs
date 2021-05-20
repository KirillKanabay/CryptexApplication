using Cryptex.Models;

namespace Cryptex.ViewModels.RsaDemoViewModels
{
    public class StepperRsaCalculateViewModel:BaseViewModel
    {
        private readonly RsaDemoModel _rsaDemoModel;

        public StepperRsaCalculateViewModel(RsaDemoModel rsaDemoModel)
        {
            _rsaDemoModel = rsaDemoModel;
        }

        public long N => _rsaDemoModel.N;
        public long Fi => _rsaDemoModel.Fi;
        public long E => _rsaDemoModel.E;
        public long D => _rsaDemoModel.D;
        public string OpenKey => $"{{e, N}} = {{{E}, {N}}}";
        public string PrivateKey => $"{{d, N}} = {{{D}, {N}}}";
    }
}
