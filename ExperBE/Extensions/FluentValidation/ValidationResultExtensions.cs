using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExperBE.Exceptions;
using FluentValidation;
using FluentValidation.Results;

namespace ExperBE.Extensions.FluentValidation
{
    public static class ValidationResultExtensions
    {
        public static async Task ThrowIfInvalid(this Task<ValidationResult> validationResult)
        {
            var result = await validationResult;
            if (!result.IsValid)
                throw new BadRequestException(result);
        }
    }
}
