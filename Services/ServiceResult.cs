using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RobeazyCore.Services
{
    public class ServiceResult
    {
        public bool Success { get; set; }
        public List<string> Errors { get; protected set; }

        public ServiceResult()
        {
            Success = true;
        }

        public ServiceResult(string errorMsg)
        {
            AddError(errorMsg);
        }

        public ServiceResult(bool success)
        {
            Success = success;
        }

        public void AddError(string errorMsg)
        {
            Success = false;
            Errors.Add(errorMsg);
        }

        public string GetErrors(string delimiter = ", ")
        {
            return string.Join(delimiter, Errors);
        }
    }
}
