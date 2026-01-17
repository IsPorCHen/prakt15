using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ElectronicsShop.Validation
{
    public class NonNegativeIntegerValidationRule : ValidationRule
    {
        public string FieldName { get; set; } = "Значение";

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string input = value as string;

            if (string.IsNullOrWhiteSpace(input))
            {
                return new ValidationResult(false, $"{FieldName} обязательно для заполнения");
            }

            if (!int.TryParse(input, out int result))
            {
                return new ValidationResult(false, $"{FieldName} должно быть целым числом");
            }

            if (result < 0)
            {
                return new ValidationResult(false, $"{FieldName} не может быть отрицательным");
            }

            return ValidationResult.ValidResult;
        }
    }
}
