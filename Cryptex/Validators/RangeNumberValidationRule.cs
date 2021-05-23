using System;
using System.Collections.Generic;
using System.Text;

namespace Cryptex.Validators
{
    public class RangeNumberValidationRule:IValidationRule
    {
        public object Value { get; }
        private long _start;
        private long _end;
        public RangeNumberValidationRule(object value, long start, long end)
        {
            Value = value;
            _start = start;
            _end = end;
        }
        public string Validate()
        {
            if (long.TryParse(Value.ToString(), out long num))
            {
                if (num >= _start && num <= _end)
                {
                    return string.Empty;
                }
            }
            return $"Число не входит в диапазон: {_start}-{_end}";
        }
    }
}
