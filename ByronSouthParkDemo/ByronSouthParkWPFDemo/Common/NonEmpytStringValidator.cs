using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ByronSouthParkDemo.Common
{
    public class NonEmptyStringValidator : ValidationRule
    {
        public String  DaName { get; set; }
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            ValidationResult validationResult = new ValidationResult(false, $"{DaName} cannot be empty.");
            if (value != null)
            {
                string valueAsString = value as string;
                if (!String.IsNullOrWhiteSpace( valueAsString))
                    validationResult = ValidationResult.ValidResult;
            }
            return validationResult;
        }
    }
}
