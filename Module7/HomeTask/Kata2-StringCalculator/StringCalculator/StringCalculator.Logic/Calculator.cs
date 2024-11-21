using System.Text.RegularExpressions;

namespace StringCalculator.Logic
{
    public static class Calculator
    {
        public static int Add(string numbers)
        {
            if (string.IsNullOrWhiteSpace(numbers))
                return 0;

            var delimiters = new List<string> { ",", "\n" };

            if (numbers.StartsWith("//"))
            {
                var delimiterSectionEnd = numbers.IndexOf('\n');
                var delimiterSection = numbers[2..delimiterSectionEnd];

                if (delimiterSection.StartsWith("[") && delimiterSection.EndsWith("]"))
                {
                    var matches = Regex.Matches(delimiterSection, @"\[(.*?)\]");
                    foreach (Match match in matches)
                    {
                        delimiters.Add(match.Groups[1].Value);
                    }
                }
                else
                {
                    delimiters.Add(delimiterSection);
                }

                numbers = numbers[(delimiterSectionEnd + 1)..];
            }

            var parts = numbers.Split(delimiters.ToArray(), StringSplitOptions.None);

            var intParts = parts.Select(int.Parse).ToList();
            var negatives = intParts.Where(n => n < 0).ToList();

            if (negatives.Any())
                throw new ArgumentException($"Negatives not allowed: {string.Join(", ", negatives)}");

            return intParts.Where(n => n <= 1000).Sum();
        }
    }
}
