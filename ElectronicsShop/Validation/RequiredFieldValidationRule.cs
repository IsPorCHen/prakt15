using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ElectronicsShop.Validation
{
    public class RequiredFieldValidationRule : ValidationRule
    {
        public string FieldName { get; set; } = "Поле";

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string input = value as string;

            if (string.IsNullOrWhiteSpace(input))
            {
                return new ValidationResult(false, $"{FieldName} обязательно для заполнения");
            }

            return ValidationResult.ValidResult;
        }
    }
}
