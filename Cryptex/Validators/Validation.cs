namespace Cryptex.Validators
{
    class Validation
    {
        private readonly IValidationRule[] _validationRules;
        public Validation(params IValidationRule[] validationRules)
        {
            _validationRules = validationRules;
        }

        public string Validate()
        {
            string validationError = null;
            foreach (var validationRule in _validationRules)
            {
                validationError = validationRule.Validate();
                if (!string.IsNullOrWhiteSpace(validationError))
                {
                    break;
                }
            }

            return validationError;
        }
    }
}
