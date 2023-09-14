using FluentValidation;
using Linkerly.Core.Application.Folders.Commands;

namespace Linkerly.Application.Application.Folders.Validations;

public class CreateFolderCommandValidator : AbstractValidator<CreateFolderCommand>
{
	public CreateFolderCommandValidator()
	{
		RuleFor(folder => folder.ParentID)
			.GreaterThan(0)
			.WithMessage("Pole je požadováno.");

		RuleFor(folder => folder.TypeID)
			.GreaterThan(0)
			.WithMessage("Pole je požadováno.");

		RuleFor(folder => folder.UserID)
			.GreaterThan(0)
			.WithMessage("Pole je požadováno.");

		RuleFor(folder => folder.Name)
			.NotEmpty()
			.WithMessage("Pole je požadováno.")
			.MaximumLength(256)
			.WithMessage("Název složky musí obsahovat nejvýše 256 znaků.");

		RuleFor(folder => folder.TotalCount)
			.GreaterThanOrEqualTo(0)
			.WithMessage("Celkový počet souborů ve složce nemůže nabývat záporných hodnot.");

		RuleFor(folder => folder.TotalSize)
			.GreaterThanOrEqualTo(0)
			.WithMessage("Celková velikost souborů ve složce nemůže nabývat záporných hodnot.");
	}
}