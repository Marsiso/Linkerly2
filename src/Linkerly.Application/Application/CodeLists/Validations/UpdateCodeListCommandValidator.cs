using FluentValidation;
using Linkerly.Core.Application.CodeLists.Commands;

namespace Linkerly.Application.Application.CodeLists.Validations;

public class UpdateCodeListCommandValidator : AbstractValidator<UpdateCodeListCommand>
{
    public UpdateCodeListCommandValidator()
    {
        RuleFor(codeListItem => codeListItem.CodeListID)
            .GreaterThan(0)
            .WithMessage("Pole je požadováno.");

        RuleFor(codeListItem => codeListItem.Name)
            .NotEmpty()
            .WithMessage("Pole je požadováno.")
            .MaximumLength(256)
            .WithMessage("Jméno výběrovníku může obsahovat nejvíce 256 znaků.");
    }
}
