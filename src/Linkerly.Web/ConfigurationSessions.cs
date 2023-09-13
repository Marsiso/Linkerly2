﻿using FluentValidation;
using Linkerly.Data;
using Linkerly.Domain.Validations;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Linkerly.Web;

public static class ConfigurationSessions
{
	public static IServiceCollection AddSqlite(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
	{
		services.AddSingleton<IValidator<CloudContextOptions>, CloudContextOptionsValidator>();

		services
			.AddOptions<CloudContextOptions>()
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
			options.UseQueryTrackingBehavior(QueryTrackingBehavior.TrackAll);
			options.UseSqlite();

			var source = Path.Combine(databaseContextOptions.Location, databaseContextOptions.FileName);

			var connectionStringBase = $"Data Source={source};";

			var connectionString = new SqliteConnectionStringBuilder(connectionStringBase)
			{
				Mode = SqliteOpenMode.ReadWriteCreate
			}.ToString();

			options.UseSqlite(connectionString);

			if (environment.IsDevelopment())
			{
				options.EnableDetailedErrors();
				options.EnableSensitiveDataLogging();
			}
		});

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
}