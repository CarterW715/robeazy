using System.Collections.Generic;
using System.Linq;

namespace RobeazyCore.Models.Validation
{
    public class RobeazyValidationResult
    {
        public bool Success { get; set; }
        public List<string> Errors { get; protected set; }

        public void AddError(string errorMsg)
        {
            Success = false;
            Errors.Add(errorMsg);
        }

        public void AddErrors(List<string> errors)
        {
            Errors = Errors.Concat(errors).ToList();
        }

        public string GetErrors(string delimiter = ", ")
        {
            return string.Join(delimiter, Errors);
        }
    }
}