using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ElectronicsShop.Validation
{
    public class MaxLengthValidationRule : ValidationRule
    {
        public int MaxLength { get; set; }
        public string FieldName { get; set; } = "Поле";

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string input = value as string;

            if (!string.IsNullOrEmpty(input) && input.Length > MaxLength)
            {
                return new ValidationResult(false, $"{FieldName} не должно превышать {MaxLength} символов");
            }

            return ValidationResult.ValidResult;
        }
    }
}
