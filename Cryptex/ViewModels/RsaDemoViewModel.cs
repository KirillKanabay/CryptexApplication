using System.Collections.Generic;
using Cryptex.ViewModels.RsaDemoViewModels;
using MaterialDesignExtensions.Model;

namespace Cryptex.ViewModels
{
    public class RsaDemoViewModel : BaseViewModel
    {
        private readonly StepperRsaCalculateViewModel _stepperRsaCalculateViewModel;
        private readonly StepperRsaDecryptViewModel _stepperRsaDecryptViewModel;
        private readonly StepperRsaEncryptViewModel _stepperRsaEncryptViewModel;
        private readonly StepperRsaSetPQViewModel _stepperRsaSetPQViewModel;

        public RsaDemoViewModel(StepperRsaCalculateViewModel stepperRsaCalculateViewModel,
            StepperRsaDecryptViewModel stepperRsaDecryptViewModel,
            StepperRsaEncryptViewModel stepperRsaEncryptViewModel,
            StepperRsaSetPQViewModel stepperRsaSetPqViewModel)
        {
            _stepperRsaCalculateViewModel = stepperRsaCalculateViewModel;
            _stepperRsaDecryptViewModel = stepperRsaDecryptViewModel;
            _stepperRsaEncryptViewModel = stepperRsaEncryptViewModel;
            _stepperRsaSetPQViewModel = stepperRsaSetPqViewModel;
        }

        public List<IStep> Steps =>
            new List<IStep>()
            {
                new Step()
                {
                    Header = new StepTitleHeader() {FirstLevelTitle = "Установка P и Q"},
                    Content = _stepperRsaSetPQViewModel
                },
                new Step()
                {
                    Header = new StepTitleHeader() {FirstLevelTitle = "Расчет ключей"},
                    Content = _stepperRsaCalculateViewModel
                },
                new Step()
                {
                    Header = new StepTitleHeader() {FirstLevelTitle = "Шифрование сообщения"},
                    Content = _stepperRsaEncryptViewModel
                },
                new Step()
                {
                    Header = new StepTitleHeader() {FirstLevelTitle = "Расшифровка сообщения"},
                    Content = _stepperRsaDecryptViewModel
                },
            };
    }
}