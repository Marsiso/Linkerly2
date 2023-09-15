using Linkerly.Core.Application.Users.Mappings;
using Linkerly.Domain.Application.Mappings;
using Linkerly.Web;
using MudBlazor.Services;

var applicationBuilder = WebApplication.CreateBuilder(args);

applicationBuilder.Services.AddRazorPages();
applicationBuilder.Services.AddServerSideBlazor();

applicationBuilder.Services.AddMudServices();

applicationBuilder.Services.AddSqlite(applicationBuilder.Configuration, applicationBuilder.Environment);
applicationBuilder.Services.AddAutoMapper(typeof(UserEntityMappingConfiguration), typeof(UserCommandMappingConfiguration));
applicationBuilder.Services.AddCqrs();

var application = applicationBuilder.Build();

application.UseSqliteSeeder();

application.UseHttpsRedirection();

application.UseStaticFiles();

application.UseRouting();

application.MapBlazorHub();
application.MapFallbackToPage("/_Host");

application.Run();

namespace Linkerly.Web
{
	public class Program
	{
	}
}