using FluentValidation;

namespace Linkerly.Data;

public class CloudContextOptionsValidator : AbstractValidator<CloudContextOptions>
{
	public CloudContextOptionsValidator()
	{
		RuleFor(o => o.Location)
			.NotEmpty()
			.WithMessage("Umístnění souboru databáze je požadováno.")
			.Must(Directory.Exists)
			.WithMessage("Umístění souboru databáze neexistuje.");

		RuleFor(o => o.FileName)
			.NotEmpty()
			.WithMessage("Název souboru databáze je požadován.")
			.Must(Path.HasExtension)
			.WithMessage("Název souboru databáze nemá příponu.")
			.Matches(@"^.+\.db$");
	}
}