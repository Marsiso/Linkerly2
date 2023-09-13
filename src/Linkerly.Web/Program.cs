using Linkerly.Web;

var applicationBuilder = WebApplication.CreateBuilder(args);

applicationBuilder.Services.AddRazorPages();
applicationBuilder.Services.AddServerSideBlazor();

applicationBuilder.Services.AddSqlite(applicationBuilder.Configuration, applicationBuilder.Environment);

var application = applicationBuilder.Build();

application.UseSqliteSeeder();

application.UseHttpsRedirection();

application.UseStaticFiles();

application.UseRouting();

application.MapBlazorHub();
application.MapFallbackToPage("/_Host");

application.Run();

public partial class Program
{
}