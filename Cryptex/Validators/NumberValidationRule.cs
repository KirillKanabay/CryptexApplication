using System.Text.RegularExpressions;

namespace Cryptex.Validators
{
    class NumberValidationRule:IValidationRule
    {
        public object Value { get; }

        public NumberValidationRule(object value)
        {
            Value = value;
        }
        public string Validate()
        {
            string validationError = null;

            Regex doubleRegex = new Regex(@"^[0-9]*[,]?[0-9]+$");
            if (!doubleRegex.IsMatch((Value ?? "").ToString()))
            {
                validationError = "Поле должно содержать число";
            }

            return validationError;
        }
    }
}
