using FluentValidation;
using Linkerly.Core.Application.Files.Commands;

namespace Linkerly.Application.Application.Files.Validations;

public class UpdateFileCommandValidator : AbstractValidator<UpdateFileCommand>
{
    public UpdateFileCommandValidator()
    {
        RuleFor(file => file.FileID)
            .GreaterThan(0)
            .WithMessage("Pole je požadováno.");

        RuleFor(file => file.FolderID)
            .GreaterThan(0)
            .WithMessage("Pole je požadováno.");

        RuleFor(file => file.ExtensionID)
            .GreaterThan(0)
            .WithMessage("Pole je požadováno.");

        RuleFor(file => file.MimeTypeID)
            .GreaterThan(0)
            .WithMessage("Pole je požadováno.");

        RuleFor(file => file.Location)
            .NotEmpty()
            .WithMessage("Pole je požadováno.");

        RuleFor(file => file.Size)
            .GreaterThan(0)
            .WithMessage("Pole je požadováno.")
            .LessThan(6L)
            .WithMessage("Velikost souboru nesmí přesahovat 1 TB.");

        RuleFor(file => file.UnsafeName)
            .NotEmpty()
            .WithMessage("Pole je požadováno.")
            .MaximumLength(256)
            .WithMessage("Název souboru může obsahovat nejvýše 256 znaků.");

        RuleFor(file => file.SafeName)
            .NotEmpty()
            .WithMessage("Pole je požadováno.")
            .MaximumLength(256)
            .WithMessage("Název souboru v souborovém systému může obsahovat nejvýše 256 znaků.");
    }
}