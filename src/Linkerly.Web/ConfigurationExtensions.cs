using FluentValidation;
using Linkerly.Application.Validations;
using Linkerly.Core.Application.Users.Queries;
using Linkerly.Data;
using Linkerly.Domain.Validations;
using MediatR;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Linkerly.Web;

public static class ConfigurationExtensions
{
    public static IServiceCollection AddSqlite(this IServiceCollection services, IConfiguration configuration,
                                               IWebHostEnvironment environment)
    {
        services.AddSingleton<IValidator<CloudContextOptions>, CloudContextOptionsValidator>();

        services.AddOptions<CloudContextOptions>()
                .Bind(configuration.GetSection(CloudContextOptions.SectionName))
                .ValidateFluently()
                .ValidateOnStart();

        CloudContextOptions? contextOptions = configuration.GetSection(CloudContextOptions.SectionName).Get<CloudContextOptions>();

        ArgumentNullException.ThrowIfNull(contextOptions);

        services.AddHttpContextAccessor()
                .AddScoped<HttpContextAccessor>();

        services.AddScoped<ISaveChangesInterceptor, Auditor>();

        services.AddDbContext<CloudContext>(options =>
        {
            string source = Path.Combine(contextOptions.Location, contextOptions.FileName);

            string connectionStringBase = $"Data Source={source};";

            string connectionString = new SqliteConnectionStringBuilder(connectionStringBase)
            {
                Mode = SqliteOpenMode.ReadWriteCreate,
            }.ToString();

            options.UseQueryTrackingBehavior(QueryTrackingBehavior.TrackAll)
                   .UseSqlite(connectionString);

            if (environment.IsDevelopment())
            {
                options.EnableDetailedErrors()
                       .EnableSensitiveDataLogging();
            }
        });

        return services;
    }

    public static IServiceCollection AddCqrs(this IServiceCollection services)
    {
        services.AddMediatR(configuration => configuration.RegisterServicesFromAssembly(typeof(GetUserQuery).Assembly))
                .AddValidatorsFromAssembly(typeof(FluentValidationPipelineBehaviour<,>).Assembly, ServiceLifetime.Singleton)
                .AddTransient(typeof(IPipelineBehavior<,>), typeof(FluentValidationPipelineBehaviour<,>));

        return services;
    }

    public static WebApplication UseSqliteSeeder(this WebApplication application)
    {
        IServiceProvider services = application.Services;
        IWebHostEnvironment environment = application.Environment;

        using IServiceScope serviceScope = services.CreateScope();

        CloudContext databaseContext = serviceScope.ServiceProvider.GetRequiredService<CloudContext>();

        if (environment.IsDevelopment())
        {
            databaseContext.Database.EnsureDeleted();
            databaseContext.Database.EnsureCreated();
        }
        else
        {
            databaseContext.Database.EnsureCreated();
        }

        return application;
    }
}