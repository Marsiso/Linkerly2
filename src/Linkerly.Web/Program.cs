using Linkerly.Core.Application.Users.Mappings;
using Linkerly.Domain.Application.Mappings;
using Linkerly.Web;
using MudBlazor.Services;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

IServiceCollection services = builder.Services;
IConfiguration configuration = builder.Configuration;
IWebHostEnvironment environment = builder.Environment;

services.AddRazorPages();
services.AddServerSideBlazor();

services.AddMudServices()
        .AddSqlite(configuration, environment)
        .AddAutoMapper(typeof(UserEntityMappingConfiguration), typeof(UserCommandMappingConfiguration))
        .AddCqrs()
        .AddGoogleCloudIdentity(configuration);

WebApplication application = builder.Build();

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