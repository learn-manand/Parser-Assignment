using Microsoft.Extensions.Options;
using Parser.API.Models;
using System.Text.RegularExpressions;

namespace Parser.API.Services
{
    public class XmlParserService: IParserService
    {
        private readonly decimal _taxRate;

        public XmlParserService(IOptions<TaxSettings> options)
        {
            _taxRate = options.Value.Rate;
        }

        public ParseResponse Parse(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                throw new Exception("Input text is required.");

            ValidateTags(input);

            var totalText = GetTagValue(input, "total");
            if (string.IsNullOrWhiteSpace(totalText))
                throw new Exception("Missing <total> tag.");

            if (!decimal.TryParse(totalText.Replace(",", "").Trim(), out var total))
            {
                throw new Exception("Invalid <total> value.");
            }

            return new ParseResponse
            {
                CostCentre = GetTagValue(input, "cost_centre") ?? "UNKNOWN",
                Total = total,
                SalesTax = Math.Round(total * _taxRate / (1 + _taxRate), 2),
                TotalExcludingTax = Math.Round(total / (1 + _taxRate), 2),
                PaymentMethod = GetTagValue(input, "payment_method"),
                Vendor = GetTagValue(input, "vendor"),
                Description = GetTagValue(input, "description"),
                Date = GetTagValue(input, "date")
            };
        }

        private void ValidateTags(string input)
        {
            var stack = new Stack<string>();

            var matches = Regex.Matches(input, @"<(/?)([a-zA-Z_]+)>");

            foreach (Match match in matches) {
                var isClosing = match.Groups[1].Value == "/";
                var tagName = match.Groups[2].Value;
                if (!isClosing)
                {
                    stack.Push(tagName);
                }
                else
                {
                    if (stack.Count == 0)
                    {
                        throw new Exception($"Unexpected closing tag </{tagName}>");
                    }
                    var openTag = stack.Pop();
                    if (!string.Equals(openTag, tagName, StringComparison.OrdinalIgnoreCase))
                    {
                        throw new Exception($"Expected </{openTag}> but found </{tagName}>");
                    }
                }
            }

            if (stack.Count > 0)
            {
                throw new Exception($"Missing closing tag for <{stack.Peek()}>");
            }
        }

        private string? GetTagValue(string input, string tagName)
        {
            var openTag = $"<{tagName}>";
            var closeTag = $"</{tagName}>";

            var start = input.IndexOf(openTag, StringComparison.OrdinalIgnoreCase);
            if (start == -1)
            {
                return null;
            }

            start += openTag.Length;

            var end = input.IndexOf(closeTag, start, StringComparison.OrdinalIgnoreCase);
            if (end == -1)
            {
                return null;
            }

            return input.Substring(start, end - start).Trim();
        }
    }
}
