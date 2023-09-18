using FluentValidation;
using Linkerly.Application.Authentication;
using Linkerly.Application.Helpers;
using Linkerly.Application.Validations;
using Linkerly.Core.Application.Users.Queries;
using Linkerly.Data;
using Linkerly.Domain.Validations;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Linkerly.Web;

public static class ConfigurationExtensions
{
    public static IServiceCollection AddSqlite(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
    {
        services.AddSingleton<IValidator<CloudContextOptions>, CloudContextOptionsValidator>();

        services.AddOptions<CloudContextOptions>()
            .Bind(configuration.GetSection(CloudContextOptions.SectionName))
            .ValidateFluently()
            .ValidateOnStart();

        var databaseContextOptions = configuration.GetSection(CloudContextOptions.SectionName).Get<CloudContextOptions>();

        ArgumentNullException.ThrowIfNull(databaseContextOptions);

        services.AddHttpContextAccessor();
        services.AddScoped<HttpContextAccessor>();

        services.AddScoped<ISaveChangesInterceptor, Auditor>();

        services.AddDbContext<CloudContext>(options =>
        {
            var source = Path.Combine(databaseContextOptions.Location, databaseContextOptions.FileName);

            var connectionStringBase = $"Data Source={source};";

            var connectionString = new SqliteConnectionStringBuilder(connectionStringBase)
            {
                Mode = SqliteOpenMode.ReadWriteCreate
            }.ToString();

            options.UseQueryTrackingBehavior(QueryTrackingBehavior.TrackAll);
            options.UseSqlite(connectionString);

            if (environment.IsDevelopment())
            {
                options.EnableDetailedErrors();
                options.EnableSensitiveDataLogging();
            }
        });

        return services;
    }

    public static IServiceCollection AddCqrs(this IServiceCollection services)
    {
        services.AddMediatR(configuration => configuration.RegisterServicesFromAssembly(typeof(GetUserQuery).Assembly));
        services.AddValidatorsFromAssembly(typeof(FluentValidationPipelineBehaviour<,>).Assembly, ServiceLifetime.Singleton);
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(FluentValidationPipelineBehaviour<,>));

        return services;
    }

    public static WebApplication UseSqliteSeeder(this WebApplication application)
    {
        var services = application.Services;
        var environment = application.Environment;

        using var serviceScope = services.CreateScope();

        var databaseContext = serviceScope.ServiceProvider.GetRequiredService<CloudContext>();

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

    public static IServiceCollection AddGoogleCloudIdentity(this IServiceCollection services, IConfiguration configuration)
    {
        var configurationSection = configuration.GetSection(GoogleCloudIdentityOptions.SegmentName);

        ArgumentNullException.ThrowIfNull(configurationSection);

        var identityProviderOptions = configurationSection.Get<GoogleCloudIdentityOptions>();

        ArgumentNullException.ThrowIfNull(identityProviderOptions);

        services.AddSingleton<IValidator<GoogleCloudIdentityOptions>, GoogleCloudIdentityOptionsValidator>();

        services.AddOptions<GoogleCloudIdentityOptions>()
            .Bind(configurationSection)
            .ValidateFluently()
            .ValidateOnStart();

        services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie();

        services.AddAuthentication()
            .AddGoogle(options =>
            {
                options.ClientId = identityProviderOptions.ClientID;
                options.ClientSecret = identityProviderOptions.ClientSecret;
                options.CallbackPath = identityProviderOptions.CallbackPath;
                options.SaveTokens = true;
                options.ClaimActions.MapAll();
            });

        services.AddHttpContextAccessor();
        services.AddScoped<HttpContextAccessor>();

        services.AddHttpClient();
        services.AddScoped<HttpClient>();

        services.AddScoped<AuthenticationStateProvider, BlazorAuthenticationStateProvider>();

        return services;
    }

    public static WebApplication UseSecurityHeaders(this WebApplication application)
    {
        var environment = application.Environment;

        if (!environment.IsDevelopment()) application.UseSecurityHeaders(SecurityHeaderHelpers.GetHeaderPolicyCollection());

        return application;
    }
}
