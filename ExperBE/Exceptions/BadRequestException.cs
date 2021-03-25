using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using FluentValidation.Results;
using Newtonsoft.Json;

namespace ExperBE.Exceptions
{
    public class BadRequestException : Exception
    {
        public BadRequestExceptionError Error { get; }
        public BadRequestException(ValidationResult validationResult)
        {
            Error = new BadRequestExceptionError(
                validationResult.Errors.GroupBy(e => e.PropertyName).ToDictionary(
                    e => e.Key,
                    e => e.Select(x => x.ErrorMessage)));
        }

        public BadRequestException(string property, string errorMessage)
        {
            Error = new BadRequestExceptionError(new Dictionary<string, IEnumerable<string>>
            {
                {
                    property,
                    new List<string>
                    {
                        errorMessage
                    }
                }
            });
        }
    }

    public class BadRequestExceptionError
    {
        public IDictionary<string, IEnumerable<string>> Errors { get; }
        public BadRequestExceptionError(IDictionary<string, IEnumerable<string>> errors)
        {
            Errors = errors;
        }
    }
}
