using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ElectronicsShop.Validation
{
    public class OptionalDecimalValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string input = value as string;

            if (string.IsNullOrWhiteSpace(input))
            {
                return ValidationResult.ValidResult;
            }

            if (!decimal.TryParse(input, out _))
            {
                return new ValidationResult(false, "Введите корректное число");
            }

            return ValidationResult.ValidResult;
        }
    }
}
