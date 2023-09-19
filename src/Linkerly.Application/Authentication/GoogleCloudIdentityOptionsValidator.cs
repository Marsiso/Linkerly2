using FluentValidation;

namespace Linkerly.Application.Authentication;

public class GoogleCloudIdentityOptionsValidator : AbstractValidator<GoogleCloudIdentityOptions>
{
    public GoogleCloudIdentityOptionsValidator()
    {
        RuleFor(options => options.ClientID)
            .NotEmpty()
            .WithMessage("Konfigurace Google Cloud Identity poskytovatele identity vyžaduje klientův identifikátor.");

        RuleFor(options => options.ClientSecret)
            .NotEmpty()
            .WithMessage("Konfigurace Google Cloud Identity poskytovatele identity vyžaduje klientovo tajemství.");

        RuleFor(options => options.CallbackPath)
            .NotEmpty()
            .WithMessage("Konfigurace Google Cloud Identity poskytovatele identity vyžaduje URL zpětného volání.");
    }
}
