// Import the necessary namespace for data annotations
using System.ComponentModel.DataAnnotations;

namespace Services.Helpers
{
    // Helper class to provide validation methods for models
    public class ValidationHelper
  {
        // Internal method to validate the properties of a model object
      internal static void ModelValidation(object obj)
    {
            //Model validations // Create a validation context for the given object
            ValidationContext validationContext = new ValidationContext(obj);

            // Create a list to store validation results
            List<ValidationResult> validationResults = new List<ValidationResult>();


            // Validate the object and store the results in validationResults list
            bool isValid = Validator.TryValidateObject(obj, validationContext, validationResults, true);

            // If the object is not valid, throw an ArgumentException with the first validation error message
       if (!isValid)
      {
        throw new ArgumentException(validationResults.FirstOrDefault()?.ErrorMessage);
      }
    }
  }
}
