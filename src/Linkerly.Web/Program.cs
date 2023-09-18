using Linkerly.Core.Application.Users.Mappings;
using Linkerly.Domain.Application.Mappings;
using Linkerly.Web;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;
IConfiguration configuration = builder.Configuration;
var environment = builder.Environment;

services.AddRazorPages();
services.AddServerSideBlazor();

services.AddMudServices()
    .AddSqlite(configuration, environment)
    .AddAutoMapper(typeof(UserEntityMappingConfiguration), typeof(UserCommandMappingConfiguration))
    .AddCqrs()
    .AddGoogleCloudIdentity(configuration);

var application = builder.Build();

application.UseSqliteSeeder()
    .UseSecurityHeaders();

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
    public class Program
    {
    }
}
