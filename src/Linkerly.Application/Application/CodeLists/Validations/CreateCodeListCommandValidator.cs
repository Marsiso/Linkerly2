using FluentValidation;
using Linkerly.Core.Application.CodeLists.Commands;

namespace Linkerly.Application.Application.CodeLists.Validations;

public class CreateCodeListCommandValidator : AbstractValidator<CreateCodeListCommand>
{
	public CreateCodeListCommandValidator()
	{
		RuleFor(codeListItem => codeListItem.Name)
			.NotEmpty()
			.WithMessage("Pole je požadováno.")
			.MaximumLength(256)
			.WithMessage("Hodnota výběrovníku může obsahovat nejvíce 256 znaků.");
	}
}