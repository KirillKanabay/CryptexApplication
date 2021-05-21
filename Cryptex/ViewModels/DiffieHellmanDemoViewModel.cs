using System.Collections.Generic;
using Cryptex.ViewModels.DhDemoViewModels;
using MaterialDesignExtensions.Model;

namespace Cryptex.ViewModels
{
    class DiffieHellmanDemoViewModel:BaseViewModel
    {
        private readonly StepperDhCalculateGeneralKey _stepperDhCalculateGeneralKey;
        private readonly StepperDhCalculatePublicKeys _stepperDhCalculatePublicKeys;
        private readonly StepperDhSetPrivateKeysViewModel _stepperDhSetPrivateKeys;
        private readonly StepperDhSetPGViewModel _stepperDhSetPgViewModel;
        private readonly StepperDhEncryptDecryptViewModel _stepperDhEncryptDecryptViewModel;

        public List<IStep> Steps =>
            new List<IStep>()
            {
                new Step()
                {
                    Header = new StepTitleHeader() {FirstLevelTitle = "Установка P и G"},
                    Content = _stepperDhSetPgViewModel
                },
                new Step()
                {
                    Header = new StepTitleHeader() {FirstLevelTitle = "Установка секретных ключей"},
                    Content = _stepperDhSetPrivateKeys
                },
                new Step()
                {
                    Header = new StepTitleHeader() {FirstLevelTitle = "Расчет публичных ключей"},
                    Content = _stepperDhCalculatePublicKeys
                },
                new Step()
                {
                    Header = new StepTitleHeader() {FirstLevelTitle = "Расчет общего приватного ключа"},
                    Content = _stepperDhCalculateGeneralKey
                },
                new Step()
                {
                    Header = new StepTitleHeader() {FirstLevelTitle = "Шифрование - Расшифровка"},
                    Content = _stepperDhCalculateGeneralKey
                },
            };
    }
}
