using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Linkerly.Domain.Validations;

public static class OptionsBuilderFluentValidationExtensions
{
	public static OptionsBuilder<TOptions> ValidateFluently<TOptions>(this OptionsBuilder<TOptions> optionsBuilder) where TOptions : class
	{
		optionsBuilder.Services.AddSingleton<IValidateOptions<TOptions>>(serviceProvider =>
		{
			var optionsValidator = serviceProvider.GetRequiredService<IValidator<TOptions>>();

			return new FluentValidationOptions<TOptions>(optionsBuilder.Name, optionsValidator);
		});

		return optionsBuilder;
	}
}