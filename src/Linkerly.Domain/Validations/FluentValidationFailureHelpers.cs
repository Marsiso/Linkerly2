using FluentValidation.Results;

namespace Linkerly.Domain.Validations;

public static class FluentValidationFailureHelpers
{
	public static Dictionary<string, string[]> DistinctErrorsByProperty(this ValidationResult? validationResult)
	{
		if (validationResult is null)
		{
			return new Dictionary<string, string[]>();
		}

		return validationResult.Errors
			.GroupBy(
				validationFailure => validationFailure.PropertyName,
				validationFailure => validationFailure.ErrorMessage,
				(propertyName, validationFailures) => new
				{
					Key = propertyName,
					Values = validationFailures.Distinct().ToArray()
				})
			.ToDictionary(
				group => group.Key,
				group => group.Values);
	}

	public static Dictionary<string, string[]> DistinctErrorsByProperty(this IEnumerable<ValidationResult>? validationResults)
	{
		if (validationResults is null)
		{
			return new Dictionary<string, string[]>();
		}

		return validationResults
			.Where(validationResult => validationResult is { IsValid: false, Errors: not null, Errors.Count: > 0 })
			.SelectMany(validationResult => validationResult.Errors, (_, vf) => vf)
			.GroupBy(
				validationFailure => validationFailure.PropertyName,
				validationFailure => validationFailure.ErrorMessage,
				(propertyName, validationFailures) => new
				{
					Key = propertyName,
					Values = validationFailures.Distinct().ToArray()
				})
			.ToDictionary(
				group => group.Key,
				group => group.Values);
	}
}