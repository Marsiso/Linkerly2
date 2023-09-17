using Linkerly.Core.Application.Users.Mappings;
using Linkerly.Domain.Application.Mappings;
using Linkerly.Web;
using MudBlazor.Services;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

IServiceCollection services = builder.Services;

services.AddRazorPages();
services.AddServerSideBlazor();

services.AddMudServices()
        .AddSqlite(builder.Configuration, builder.Environment)
        .AddAutoMapper(typeof(UserEntityMappingConfiguration), typeof(UserCommandMappingConfiguration))
        .AddCqrs();

WebApplication application = builder.Build();

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