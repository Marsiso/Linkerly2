using FluentValidation;
using Microsoft.Extensions.Options;

namespace Linkerly.Domain.Validations;

public class FluentValidationOptions<TOptions> : IValidateOptions<TOptions> where TOptions : class
{
    public readonly IValidator<TOptions> OptionsValidator;

    public FluentValidationOptions(string name, IValidator<TOptions> optionsValidator)
    {
        Name = name;
        OptionsValidator = optionsValidator;
    }

    public string? Name { get; }

    public ValidateOptionsResult Validate(string? optionsName, TOptions options)
    {
        if (optionsName is not null && !optionsName.Equals(Name, StringComparison.OrdinalIgnoreCase)) return ValidateOptionsResult.Skip;

        ArgumentNullException.ThrowIfNull(options);

        var validationContext = new ValidationContext<TOptions>(options);

        var validationResult = OptionsValidator.Validate(validationContext);

        if (validationResult.IsValid) return ValidateOptionsResult.Success;

        var validationFailures = validationResult.DistinctErrorsByProperty();

        var failureMessage = validationFailures.Select(kvp => $"Options '{typeof(TOptions).Name}' has validation failures. Property: '{kvp.Key}' Failures: '{string.Join(" ", kvp.Value)}'.")
            .Aggregate((l, r) => string.Join(" ", l, r));

        return ValidateOptionsResult.Fail(failureMessage);
    }
}
