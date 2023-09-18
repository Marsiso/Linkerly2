using Linkerly.Core.Application.Users.Mappings;
using Linkerly.Domain.Application.Mappings;
using Linkerly.Web;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;
var configuration = builder.Configuration;
var environment = builder.Environment;

services.AddRazorPages();
services.AddServerSideBlazor();

services.AddMudServices();
services.AddSqlite(configuration, environment);
services.AddAutoMapper(typeof(UserEntityMappingConfiguration), typeof(UserCommandMappingConfiguration));
services.AddCqrs();
services.AddGoogleCloudIdentity(configuration);

var application = builder.Build();

application.UseSqliteSeeder();
application.UseSecurityHeaders();

application.UseHttpsRedirection();

application.UseStaticFiles();

application.UseCookiePolicy();
application.UseAuthentication();

application.UseRouting();

application.MapBlazorHub();
application.MapFallbackToPage("/_Host");

application.Run();

namespace Linkerly.Web
{
    public partial class Program
    {
    }
}
