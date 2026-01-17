using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ElectronicsShop.Validation
{
    public class ComboBoxSelectionValidationRule : ValidationRule
    {
        public string FieldName { get; set; } = "Значение";

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value == null)
            {
                return new ValidationResult(false, $"Выберите {FieldName}");
            }

            return ValidationResult.ValidResult;
        }
    }
}
