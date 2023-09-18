using FluentValidation.Results;

namespace Linkerly.Domain.Validations;

public static class FluentValidationFailureHelpers
{
    public static Dictionary<string, string[]> DistinctErrorsByProperty(this ValidationResult? validationResult)
    {
        if (validationResult is null) return new Dictionary<string, string[]>();

        List<ValidationFailure>? validationFailures = validationResult.Errors;

        var validationFailuresByProperties = validationFailures
            .GroupBy(validationFailure => validationFailure.PropertyName,
                validationFailure => validationFailure.ErrorMessage,
                (propertyName, validationFailuresByProperty) => new
                {
                    Key = propertyName,
                    Values = validationFailuresByProperty.Distinct().ToArray()
                })
            .ToDictionary(
                group => group.Key,
                group => group.Values);

        return validationFailuresByProperties;
    }

    public static Dictionary<string, string[]> DistinctErrorsByProperty(this IEnumerable<ValidationResult>? validationResults)
    {
        if (validationResults is null) return new Dictionary<string, string[]>();

        var validationFailuresByProperties = validationResults
            .Where(validationResult => validationResult is { IsValid: false, Errors: not null, Errors.Count: > 0 })
            .SelectMany(validationResult => validationResult.Errors, (_, vf) => vf)
            .GroupBy(
                validationFailure => validationFailure.PropertyName,
                validationFailure => validationFailure.ErrorMessage,
                (propertyName, validationFailuresByProperty) => new
                {
                    Key = propertyName,
                    Values = validationFailuresByProperty.Distinct().ToArray()
                })
            .ToDictionary(
                group => group.Key,
                group => group.Values);

        return validationFailuresByProperties;
    }
}
