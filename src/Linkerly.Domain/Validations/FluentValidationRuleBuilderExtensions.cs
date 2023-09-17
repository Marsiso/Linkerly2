using FluentValidation;

namespace Linkerly.Domain.Validations;

public static class FluentValidationRuleBuilderExtensions
{
    public static IRuleBuilderOptions<T, string?> URL<T>(this IRuleBuilder<T, string?> ruleBuilder)
    {
        return ruleBuilder.Must(url =>
        {
            bool valid = !string.IsNullOrWhiteSpace(url);

            if (valid)
            {
                valid = Uri.TryCreate(url, UriKind.Absolute, out Uri? uri) && (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps);
            }

            return valid;
        });
    }
}