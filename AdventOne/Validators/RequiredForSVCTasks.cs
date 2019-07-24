using AdventOne.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace AdventOne.Validators {

    public sealed class RequiredForSVCTasks : RequiredBase {

        protected override ValidationResult IsValid(object value, ValidationContext validationContext) {

            const string errorMessage = "This field is mandatory for SVC tasks.";
            if (value != null && validationContext.ObjectInstance != null && validationContext.ObjectType == typeof(Task)) {

                Task task = (Task)validationContext.ObjectInstance;

                if (task.RevenueType == RevenueType.SVC) {

                    if (value is String)        { if (String.IsNullOrWhiteSpace((String)value)) return new ValidationResult(errorMessage); }
                    if (IsNumericType(value))   { if (Convert.ToDecimal(value) == 0) return new ValidationResult(errorMessage); }

                    return ValidationResult.Success;

                }
                else {
                    return ValidationResult.Success;
                }

            }

            else {

                return new ValidationResult(errorMessage);

            }

        }
        
    }

}