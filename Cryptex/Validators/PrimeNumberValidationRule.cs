using System;
using System.Collections.Generic;
using System.Text;
using Cryptex.Services.Helpers;

namespace Cryptex.Validators
{
    public class PrimeNumberValidationRule:IValidationRule
    {
        public object Value { get; }

        public PrimeNumberValidationRule(object value)
        {
            Value = value;
        }

        public string Validate()
        {
            IGcdNumbersWorker gcd = new GcdNumbersWorker();
            IPrimeNumbersWorker pnw = new PrimeNumbersWorker(gcd);

            if(long.TryParse((string) Value, out long num))
            {
                return pnw.IsPrime(num) && num >= 3 ? string.Empty : "Число не является простым";
            }

            return "Число не входит в диапазон допустимых значений";
        }
    }
}
