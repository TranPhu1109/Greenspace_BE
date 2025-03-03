using FluentValidation;
using GreenSpace.Application;
using GreenSpace.Application.GlobalExceptionHandling;
using GreenSpace.Application.Validations;
using GreenSpace.Infrastructure;
using GreenSpace.WebAPI.Middlewares;
using Microsoft.OpenApi.Models;
using MediatR;
using Scrutor;
using System.Diagnostics;
using System.Reflection;
using System.Text.Json.Serialization;
using GreenSpace.Application.Features.User.Queries;
using GreenSpace.Application.Services.Interfaces;
using GreenSpace.WebAPI.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace GreenSpace.WebAPI;

public static class DependencyInjection
{

    public static async Task<WebApplicationBuilder> AddWebAPIServicesAsync(this WebApplicationBuilder builder)
    {

        const string serviceName = "GreenSpace_WebAPI";
        const string serviceVersion = "v1.0";
        // Add Tracing
        //builder.Services.AddOpenTelemetry()
        //    .WithTracing(cfg =>
        //        cfg.AddSource(serviceName)
        //            .ConfigureResource(resource => resource.AddService(serviceName: serviceName,
        //                serviceVersion: serviceVersion))
        //            .AddAspNetCoreInstrumentation()
        //            .AddSqlClientInstrumentation()
        //            .AddHttpClientInstrumentation()
        //            .AddOtlpExporter(ex =>
        //            {
        //                ex.Endpoint = new("http://jaeger:4317");
        //                ex.ExportProcessorType = OpenTelemetry.ExportProcessorType.Simple;
        //                ex.TimeoutMilliseconds = 30;
        //                ex.Protocol = OpenTelemetry.Exporter.OtlpExportProtocol.Grpc;
        //            })
        //            .AddConsoleExporter());
        //builder.Logging.AddSeq("http://seq:5341");
        builder.Services.AddHttpContextAccessor();
        builder.Services.AddCors(options
        => options.AddDefaultPolicy(policy
        => policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));

        builder.Services.AddControllers().AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
        builder.Services.AddHttpClient();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddControllers();
        builder.Services.AddHttpClient();
        builder.Services.AddRouting(x =>
        {
            x.LowercaseQueryStrings = true;
            x.LowercaseUrls = true;
        });

        var configuration = builder.Configuration.Get<AppSettings>() ?? throw new Exception("Null configuration");
        // DI AppSettings
        List<Assembly> assemblies = new List<Assembly>
        {
            typeof(Program).Assembly,
            Application.AssemblyReference.Assembly,
            Infrastructure.AssemblyReference.Assembly
        };
        builder.Services.AddSingleton(configuration);
        builder.Services.AddValidatorsFromAssemblies(assemblies: assemblies);
        builder.Services.AddInfrastructureServices(configuration.ConnectionStrings.DefaultConnection);
        builder.Services.AddSingleton<GlobalErrorHandlingMiddleware>();
   
        // Register To Handle Query/Command of MediatR
        builder.Services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(GetAllUserQuery).Assembly));
        builder.Services.AddScoped<IClaimsService, ClaimsService>();
        // Scan and register all interfaces --> implementations 
        builder.Services.Scan(scan => scan
         .FromAssemblies(GreenSpace.Infrastructure.AssemblyReference.Assembly,
         GreenSpace.Application.AssemblyReference.Assembly,
         AssemblyReference.Assembly)
         .AddClasses()
         .UsingRegistrationStrategy(RegistrationStrategy.Skip)
         .AsMatchingInterface()
         .WithScopedLifetime());

        //builder.Services.AddSwaggerGen(opt =>
        //{
        //    opt.SwaggerDoc("v1", new OpenApiInfo { Title = "Green Space", Version = "v1" });
        //    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        //    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
        //    opt.IncludeXmlComments(xmlPath);
        //    opt.AddSecurityDefinition(name: "Bearer", securityScheme: new OpenApiSecurityScheme
        //    {
        //        Name = "Authorization",
        //        Description = "Bearer Generated JWT-Token",
        //        In = ParameterLocation.Header,
        //        Type = SecuritySchemeType.ApiKey,
        //        Scheme = "Bearer"

        //    });
        //    opt.AddSecurityRequirement(new OpenApiSecurityRequirement
        //                {
        //                    {
        //                        new OpenApiSecurityScheme
        //                        {
        //                            Reference = new OpenApiReference
        //                            {
        //                                Type = ReferenceType.SecurityScheme,
        //                                Id = JwtBearerDefaults.AuthenticationScheme
        //                            },
        //                            Scheme = "oauth2",
        //                            Name = "Bearer",
        //                            In = ParameterLocation.Header,
        //                        }, Array.Empty<string>()
        //                    }
        //                });
        //});

        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "GreenSpace API",
                Version = "v1",
                Description = "API documentation for GreenSpace"
            });
        });

        builder.Services.AddSignalR();
        builder.Services.AddSingleton<PerformanceMiddleware>();
        builder.Services.AddSingleton<Stopwatch>();
        return builder;
    }
}