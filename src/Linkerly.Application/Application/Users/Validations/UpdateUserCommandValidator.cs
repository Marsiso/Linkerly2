using FluentValidation;
using Linkerly.Core.Application.Users.Commands;
using Linkerly.Domain.Validations;

namespace Linkerly.Application.Application.Users.Validations;

public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserCommandValidator()
    {
        RuleFor(user => user.UserID)
            .GreaterThan(0)
            .WithMessage("Pole je požadováno.");

        RuleFor(user => user.Identifier)
            .NotEmpty()
            .WithMessage("Pole je požadováno.")
            .MaximumLength(256)
            .WithMessage("Identifikátor uživatele může obsahovat nejvýše 512 znaků.");

        RuleFor(user => user.Email)
            .NotEmpty()
            .WithMessage("Pole je požadováno.")
            .MaximumLength(256)
            .WithMessage("Emailová adresa uživatele může obsahovat nejvýše 256 znaků.")
            .EmailAddress()
            .WithMessage("Emailová adresa uživatele má neplatný formát.");

        RuleFor(user => user.Name)
            .NotEmpty()
            .WithMessage("Pole je požadováno.")
            .MaximumLength(256)
            .WithMessage("Přezívka uživatele může obsahovat nejvýše 256 znaků.");

        RuleFor(user => user.GivenName)
            .NotEmpty()
            .WithMessage("Pole je požadováno.")
            .MaximumLength(256)
            .WithMessage("Jméno uživatele může obsahovat nejvýše 256 znaků.");

        RuleFor(user => user.FamilyName)
            .NotEmpty()
            .WithMessage("Pole je požadováno.")
            .MaximumLength(256)
            .WithMessage("Příjmení uživatele může obsahovat nejvýše 256 znaků.");

        When(user => !string.IsNullOrWhiteSpace(user.Picture), () =>
        {
            RuleFor(user => user.Picture)
                .NotEmpty()
                .WithMessage("Pole je požadováno.")
                .MaximumLength(256)
                .WithMessage("Profilová fotka uživatele může obsahovat nejvýše 2048 znaků.")
                .URL()
                .WithMessage("Profilová fotka uživatele má neplatný formát.");
        });

        When(user => !string.IsNullOrWhiteSpace(user.Locale), () =>
        {
            RuleFor(user => user.Locale)
                .NotEmpty()
                .WithMessage("Pole je požadováno.")
                .MaximumLength(32)
                .WithMessage("Kód lokalizace může obsahovat nejvýše 32 znaků.");
        });
    }
}