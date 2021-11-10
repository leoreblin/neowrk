using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace Neowrk.Library.Rest
{
    public class ApiException : Exception
    {
        private ApiResponse Response = new ApiResponse();

        public ApiException()
        {
            
        }

        public ApiException(string message) : base(message)
        {
            Response.Messages.Add(message);
        }

        public ApiException WithFluentValidation(ValidationResult validationResult)
        {
            foreach (var error in validationResult.Errors)
            {
                AddValidationResult(error);
            }

            return this;
        }

        private void AddValidationResult(ValidationFailure error)
        {
            if (error.PropertyName.Contains("GlobalMessageErrorKey"))
            {
                Response.Messages.Add(error.ErrorMessage);
            }
            else
            {
                Response.Fields.Add(new KeyValuePair<string, string>(error.PropertyName, error.ErrorMessage));
            }
        }

        public void AddError(string message)
        {
            Response.Messages.Add(message);
        }

        public void AddError(string key, string message)
        {
            Response.Fields.Add(new KeyValuePair<string, string>(key, message));
        }

        public bool HasError()
        {
            return !(Response.Messages.Count == 0) || !(Response.Fields.Count == 0);
        }

        public void ThrowIfHasError()
        {
            if (HasError())
            {
                throw this;
            }
        }

        public ApiResponse GetResponse()
        {
            return Response;
        }
    }
}
