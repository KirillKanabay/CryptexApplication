namespace Cryptex.Validators
{
    class NotEmptyFieldValidationRule:IValidationRule
    {
        public object Value { get; }

        public NotEmptyFieldValidationRule(object value)
        {
            Value = value;
        }
        public string Validate()
        {
            return string.IsNullOrWhiteSpace((Value ?? "").ToString())
                ? "Поле не должно быть пустым"
                : null;
        }
    }
}
