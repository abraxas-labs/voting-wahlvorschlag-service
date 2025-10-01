// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

#if DEBUG
using System;
#endif
using System.Text.Json.Serialization;
using Eawv.Service.Authentication;
using Eawv.Service.Configuration;
using Eawv.Service.DataAccess;
using Eawv.Service.DataAccess.Entities;
using Eawv.Service.Ech.Configuration;
using Eawv.Service.Ech.DependencyInjection;
using Eawv.Service.Middleware;
using Eawv.Service.Services;
using Eawv.Service.Services.Excel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
#if DEBUG
using Microsoft.OpenApi.Models;
#endif
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure;
using Prometheus;
using Voting.Lib.Common.DependencyInjection;
using Voting.Lib.Database.Migrations;
using Voting.Lib.MalwareScanner.DependencyInjection;
using Voting.Lib.Rest.Middleware;

namespace Eawv.Service;

public class Startup
{
#if DEBUG
    private const string MajorVersionName = "v1";
#endif
    private readonly IConfiguration _configuration;
    private readonly AppConfig _appConfig;

    public Startup(IConfiguration configuration)
    {
        _configuration = configuration;
        _appConfig = configuration.Get<AppConfig>();
    }

    public virtual void ConfigureServices(IServiceCollection services)
    {
        services
            .AddControllers()
            .AddJsonOptions(x =>
            {
                x.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });

        services.AddAutoMapper(typeof(Election).Assembly);
        services.AddMemoryCache();
        services.AddHealthChecks();
        services.AddRazorPages();

        services.AddCors(_configuration);

        ConfigureAppConfigServices(services);

#if DEBUG
        ConfigureSwaggerServices(services);
#endif

        ConfigureEawvServices(services);
        ConfigureEchServices(services, _appConfig.Ech);
        ConfigureDatabaseServices(services);
        ConfigureAuthentication(services.AddVotingLibIam(_appConfig.SecureConnectApi, _appConfig.AuthStore));

        services.AddMalwareScanner(_appConfig.MalwareScanner);
    }

    public void Configure(IApplicationBuilder app)
    {
        app.UseMetricServer(_appConfig.MetricPort);

        app.UseRouting();

        app.UseMiddleware<ExceptionHandlingMiddleware>();

        app.UseCorsFromConfig();

#if DEBUG
        app.UseSwagger(c =>
        c.RouteTemplate = "documentation/{documentName}/swagger.json");

        app.UseSwaggerUI(c =>
        {
            c.RoutePrefix = "documentation";
            c.SwaggerEndpoint($"./{MajorVersionName}/swagger.json", MajorVersionName);
        });
#endif

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseMiddleware<IamLoggingHandler>();
        app.UseSerilogRequestLoggingWithTraceabilityModifiers();

        var supportedLocales = _appConfig.SupportedLocales.ToArray();
        app.UseRequestLocalization(l =>
            l.AddSupportedCultures(supportedLocales)
                .AddSupportedUICultures(supportedLocales)
                .SetDefaultCulture(supportedLocales[0]));

        app.UseHttpMetrics();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapHealthChecks("/healthz");
            endpoints.MapControllers();
        });
    }

    protected virtual void ConfigureAuthentication(AuthenticationBuilder builder)
        => builder.AddSecureConnectScheme(options =>
        {
            options.Audience = _appConfig.SecureConnect.Audience;
            options.Authority = _appConfig.SecureConnect.Authority;
            options.FetchRoleToken = true;
            options.LimitRolesToAppHeaderApps = false;
            options.ServiceAccount = _appConfig.SecureConnect.ServiceAccount;
            options.ServiceAccountPassword = _appConfig.SecureConnect.ServiceAccountPassword;
            options.TenantHeaderName = _appConfig.SecureConnect.TenantHeaderName;
            options.AppHeaderName = _appConfig.SecureConnect.AppHeaderName;
            options.ServiceAccountScopes = _appConfig.SecureConnect.Scopes;
        });

    /// <summary>
    /// Configures the applications database services.
    /// Potential enhancement could be adding the 'options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)' (default is 'TrackAll').
    /// But then all queries that use nested includes would need to be reorganized or explicitly set to '.AsTracking()' to avoid cyclic references.
    /// More details see:...
    /// <li>
    ///     <ul>EF Core: 'https://docs.microsoft.com/en-us/ef/core/what-is-new/ef-core-3.x/breaking-changes#notrackingresolution'</ul>
    ///     <ul>Performance: 'https://github.com/dotnet/efcore/issues/23558'</ul>
    /// </li>
    /// </summary>
    /// <param name="services">The applications service collection.</param>
    private void ConfigureDatabaseServices(IServiceCollection services)
    {
        services.AddDbContext<EawvContext>(options =>
        {
            if (_appConfig.Database.EnableDetailedErrors)
            {
                options.EnableDetailedErrors();
            }

            if (_appConfig.Database.EnableSensitiveDataLogging)
            {
                options.EnableSensitiveDataLogging();
            }

            options.UseNpgsql(_appConfig.Database.ConnectionString, ConfigureNpgsql);
        });

        services.AddVotingLibDatabase<EawvContext>();
    }

    /// <summary>
    /// Configures Npgsql to use single query by default.
    /// </summary>
    /// <param name="options">The Npgsql options builder.</param>
    private void ConfigureNpgsql(NpgsqlDbContextOptionsBuilder options)
    {
        options.UseQuerySplittingBehavior(QuerySplittingBehavior.SingleQuery);
    }

    private void ConfigureAppConfigServices(IServiceCollection services)
    {
        services.AddSingleton(_appConfig);
        services.AddSingleton(_appConfig.Cache);
        services.AddSingleton(_appConfig.SecureConnect);
        services.AddSingleton(_appConfig.PDFService);
        services.AddSingleton(_appConfig.NotificationService);
    }

    private void ConfigureEawvServices(IServiceCollection services)
    {
        services.AddSingleton<IDatabaseMigrator, DatabaseMigrator<EawvContext>>();
        services.AddScoped<AuthService>();
        services.AddSingleton<CacheService>();
        services.AddHttpContextAccessor();
        services.AddHttpClient();
        services.AddSystemClock();
        services.AddHttpClient<IPdfService, PdfService>(opts => opts.BaseAddress = _appConfig.PDFService.Endpoint);
        services.AddTransient<INotificationService, NotificationService>();
        services.AddTransient<EventNotificationService>();
        services.AddSingleton<RazorRenderer>();
        services.AddTransient<ITemplateService, TemplateService>();
        services.AddSingleton<ExcelService>();
        services.AddSingleton<WabstiCandidatesExcelService>();
        services.AddScoped<FileValidationService>();

        services.AddTransient<IListService, ListService>();
        services.AddTransient<ITenantService, TenantService>();
        services.AddTransient<IUserService, UserService>();
        services.AddTransient<ApplicationService>();
        services.AddTransient<DomainOfInfluenceRepository>();
        services.AddTransient<ElectionRepository>();
        services.AddTransient<InfoTextRepository>();
        services.AddTransient<BallotDocumentRepository>();
        services.AddTransient<ListRepository>();
        services.AddTransient<DomainOfInfluenceElectionRepository>();
        services.AddTransient<ListUnionRepository>();
        services.AddTransient<ListUnionRepository>();
        services.AddTransient<CandidateRepository>();
        services.AddTransient<TemplateRepository>();
        services.AddTransient<ListCommentRepository>();
        services.AddTransient<SettingRepository>();
    }

    /// <summary>
    /// Configures the eCH services.
    /// VOTING-3320: after dotnet 6 update, reference Voting.Lib.Ech.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="config">The eCH config from <see cref="AppConfig"/>.</param>
    private void ConfigureEchServices(IServiceCollection services, EchConfig config)
    {
        services.AddVotingLibEch(config);
        services.AddSingleton<EchService>();
    }

#if DEBUG
    private void ConfigureSwaggerServices(IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc(MajorVersionName, new OpenApiInfo
            {
                Version = MajorVersionName,
                Title = "EAWv-Service",
            });

            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
            {
                Description =
                    "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement()
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer",
                        },
                    },
                    Array.Empty<string>()
                },
            });
        });
    }
#endif
}
