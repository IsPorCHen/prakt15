using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ElectronicsShop.Validation
{
    public class DecimalFormatValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string input = value as string;

            if (string.IsNullOrEmpty(input))
            {
                return ValidationResult.ValidResult;
            }

            Regex regex = new Regex(@"^[0-9]*\.?[0-9]*$");
            if (!regex.IsMatch(input))
            {
                return new ValidationResult(false, "Разрешены только цифры и точка");
            }

            return ValidationResult.ValidResult;
        }
    }
}
