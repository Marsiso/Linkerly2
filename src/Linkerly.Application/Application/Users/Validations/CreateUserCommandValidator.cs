using FluentValidation;
using Linkerly.Core.Application.Users.Commands;

namespace Linkerly.Application.Application.Users.Validations;

public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(user => user.Identifier)
            .NotEmpty()
            .WithMessage("Pole je požadováno.")
            .MaximumLength(256)
            .WithMessage("Identifikátor uživatele může obsahovat nejvýše 512 znaků.");

        RuleFor(user => user.Email)
            .NotEmpty()
            .WithMessage("Pole je požadováno.")
            .MaximumLength(256)
            .WithMessage("Emailová adresa uživatele může obsahovata nejvýše 256 znaků.")
            .EmailAddress()
            .WithMessage("Emailová adresa uživatele má neplatný formát.");

        RuleFor(user => user.Name)
            .NotEmpty()
            .WithMessage("Pole je požadováno.")
            .MaximumLength(256)
            .WithMessage("Přezívka uživatele může obsahovat nejvýše 256 znaků.");

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
    }
}