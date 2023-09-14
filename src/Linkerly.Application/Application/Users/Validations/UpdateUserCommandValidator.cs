using FluentValidation;
using Linkerly.Core.Application.Users.Commands;

namespace Linkerly.Application.Application.Users.Validations;

public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
	public UpdateUserCommandValidator()
	{
		RuleFor(folder => folder.UserID)
			.GreaterThan(0)
			.WithMessage("Pole je požadováno.");

		RuleFor(user => user.Email)
			.NotEmpty()
			.WithMessage("Pole je požadováno.")
			.MaximumLength(256)
			.WithMessage("Emailová adresa uživatele může obsahovat nejvýše 256 znaků.")
			.EmailAddress()
			.WithMessage("Emailová adresa uživatele má neplatný formát.");

		RuleFor(user => user.FirstName)
			.NotEmpty()
			.WithMessage("Pole je požadováno.")
			.MaximumLength(256)
			.WithMessage("Jméno uživatele může obsahovat nejvýše 256 znaků.");

		RuleFor(user => user.LastName)
			.NotEmpty()
			.WithMessage("Pole je požadováno.")
			.MaximumLength(256)
			.WithMessage("Příjmení uživatele může obsahovat nejvýše 256 znaků.");

		When(user => user.DateLastAccessed.HasValue, () =>
		{
			RuleFor(user => user.DateLastAccessed)
				.Must(dateLastAccessed => dateLastAccessed.HasValue && dateLastAccessed.Value <= DateTime.Now)
				.WithMessage("Datum posledního přihlášení není platné.");
		});
	}
}