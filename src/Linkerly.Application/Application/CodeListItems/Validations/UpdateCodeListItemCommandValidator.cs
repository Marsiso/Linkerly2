using FluentValidation;
using Linkerly.Core.Application.CodeListItems.Commands;

namespace Linkerly.Application.Application.CodeListItems.Validations;

public class UpdateCodeListItemCommandValidator : AbstractValidator<UpdateCodeListItemCommand>
{
	public UpdateCodeListItemCommandValidator()
	{
		RuleFor(codeListItem => codeListItem.CodeListItemID)
			.GreaterThan(0)
			.WithMessage("Pole je požadováno.");

		RuleFor(codeListItem => codeListItem.CodeListID)
			.GreaterThan(0)
			.WithMessage("Pole je požadováno.");

		RuleFor(codeListItem => codeListItem.Value)
			.NotEmpty()
			.WithMessage("Pole je požadováno.")
			.MaximumLength(256)
			.WithMessage("Hodnota výběrovníku může obsahovat nejvíce 256 znaků.");
	}
}