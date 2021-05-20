namespace Cryptex.Validators
{
    public interface IValidationRule
    {
        object Value { get; }
        public string Validate();
    }
}
