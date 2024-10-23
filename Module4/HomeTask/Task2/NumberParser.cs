using System;

namespace Task2
{
    public class NumberParser : INumberParser
    {
        public int Parse(string stringValue)
        {
            if (stringValue == null)
            {
                throw new ArgumentNullException(nameof(stringValue), "Input cannot be null.");
            }

            stringValue = stringValue.Trim();

            if (stringValue.Length == 0)
            {
                throw new FormatException("Input cannot be an empty string or whitespace.");
            }

            bool isNegative = false;
            int startIndex = 0;

            if (stringValue[0] == '-')
            {
                isNegative = true;
                startIndex = 1;
            }
            else if (stringValue[0] == '+')
            {
                startIndex = 1;
            }

            long result = 0;
            for (int i = startIndex; i < stringValue.Length; i++)
            {
                char c = stringValue[i];

                if (c < '0' || c > '9')
                {
                    throw new FormatException($"Invalid character '{c}' in input string.");
                }

                int digitValue = c - '0';

                result = result * 10 + digitValue;

                if (isNegative && -result < int.MinValue)
                {
                    throw new OverflowException("Value is less than Int32.MinValue.");
                }
                else if (!isNegative && result > int.MaxValue)
                {
                    throw new OverflowException("Value is greater than Int32.MaxValue.");
                }
            }

            return isNegative ? (int)-result : (int)result;
        }
    }
}
