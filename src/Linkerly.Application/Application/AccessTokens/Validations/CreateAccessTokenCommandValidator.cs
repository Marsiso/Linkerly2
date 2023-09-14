using FluentValidation;
using Linkerly.Core.Application.AccessTokens.Commands;
using Linkerly.Domain.Validations;

namespace Linkerly.Application.Application.AccessTokens.Validations;

public class CreateAccessTokenCommandValidator : AbstractValidator<CreateAccessTokenCommand>
{
	public CreateAccessTokenCommandValidator()
	{
		RuleFor(token => token.UserID)
			.GreaterThan(0)
			.WithMessage("Pole je požadováno.");

		RuleFor(token => token.Issuer)
			.NotEmpty()
			.WithMessage("Pole je požadováno.")
			.MaximumLength(512)
			.WithMessage("Vydavatel tokenu může obsahovat nejvýše 512 znaků.");

		RuleFor(token => token.Audience)
			.NotEmpty()
			.WithMessage("Pole je požadováné.")
			.MaximumLength(512)
			.WithMessage("Obecenstvo tokenu může obsahovatnejvýše 512 znaků.");

		RuleFor(token => token.Subject)
			.NotEmpty()
			.WithMessage("Pole je požadováno.")
			.MaximumLength(512)
			.WithMessage("Předmět tokenu může obsahovat nejvýše 512 znaků.");

		RuleFor(token => token.Email)
			.NotEmpty()
			.WithMessage("Pole je požadováno.")
			.MaximumLength(256)
			.WithMessage("Emailová adresa uživatele může být tvořena nejvýše 256 znaků.")
			.EmailAddress()
			.WithMessage("Emailová adresa uživatele má neplatný formát.");

		RuleFor(token => token.GivenName)
			.NotEmpty()
			.WithMessage("Pole je požadováno.")
			.MaximumLength(256)
			.WithMessage("Jméno uživatele může obsahovat nejvýše 256 znaků.");

		RuleFor(token => token.IsEmailVerified)
			.NotEmpty()
			.WithMessage("Pole je požadováno.")
			.MaximumLength(32)
			.WithMessage("Potvzení emailové adresy uživatele může obsahovat nejvýše 32 znaků.");

		RuleFor(token => token.Expires)
			.NotEmpty()
			.WithMessage("Pole je požadováno.")
			.Must(expiryDate => DateTime.TryParse(expiryDate, out _))
			.WithMessage("Platnost tokenu má neplatný formát.")
			.Must(expiryDate => DateTime.Parse(expiryDate!) <= DateTime.Now)
			.WithMessage("Platnost tokenu vypršela.");

		RuleFor(token => token.IssuedAt)
			.NotEmpty()
			.WithMessage("Pole je požadováno.")
			.Must(issuedAt => DateTime.Parse(issuedAt!) <= DateTime.Now)
			.WithMessage("Datum vydaní tokenu je neplatné.");

		When(token => DateTime.TryParse(token.Expires, out _) && DateTime.TryParse(token.IssuedAt, out _), () =>
		{
			RuleFor(token => token.IssuedAt)
				.Must((token, issuedAt) => DateTime.TryParse(issuedAt, out var dateIssuedAt) && DateTime.TryParse(token.Expires, out var dateExpires) && dateIssuedAt <= dateExpires)
				.WithMessage("Datum vydání tokenu musí být dříve než datum vypršení jeho platnosti.");
		});

		RuleFor(token => token.FamilyName)
			.NotEmpty()
			.WithMessage("Pole je požadováno.")
			.MaximumLength(256)
			.WithMessage("Příjmení uživatele může obsahovat nejvýše 256 znaků.");

		RuleFor(token => token.Name)
			.NotEmpty()
			.WithMessage("Pole je požadováno.")
			.MaximumLength(256)
			.WithMessage("Jméno uživatele včetně přezívky může obsahovat nejvýše 1024 znaků.");

		When(token => !string.IsNullOrWhiteSpace(token.Picture), () =>
		{
			RuleFor(token => token.Picture)
				.NotEmpty()
				.WithMessage("Pole je požadováno.")
				.MaximumLength(2048)
				.WithMessage("Profilový obrázek uživatele může být tvořen nejvýše 2048 znaků.")
				.URL()
				.WithMessage("Profilový obrázek uživatele má neplatný formát.");
		}).Otherwise(() => { });
	}
}