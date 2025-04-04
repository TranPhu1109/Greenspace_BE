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
using GreenSpace.Application.Repositories;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using CloudinaryDotNet;
using GreenSpace.Application.Services;
using MongoDB.Driver;
using MongoDB.Driver.Core.Extensions.DiagnosticSources;
using Hangfire;

namespace GreenSpace.WebAPI;

public static class DependencyInjection
{

    public static async Task<WebApplicationBuilder> AddWebAPIServicesAsync(this WebApplicationBuilder builder)
    {

        //const string serviceName = "GreenSpace_WebAPI";
        //const string serviceVersion = "v1.0";

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
            typeof(GreenSpace.Application.AssemblyReference).Assembly,
            typeof(GreenSpace.Infrastructure.AssemblyReference).Assembly
        };
        builder.Services.AddSingleton(configuration);
        builder.Services.AddValidatorsFromAssemblies(assemblies: assemblies);
        builder.Services.AddInfrastructureServices(configuration.ConnectionStrings.DefaultConnection);
        builder.Services.AddSingleton<GlobalErrorHandlingMiddleware>();

        // Register MongoDb
        var mongoUrl = MongoUrl.Create(configuration.ConnectionStrings.MongoDbConnection);
        var clientSettings = MongoClientSettings.FromUrl(mongoUrl);
        clientSettings.ClusterConfigurator = cb => cb.Subscribe(new DiagnosticsActivityEventSubscriber());
        var mongoClient = new MongoClient(clientSettings);
        builder.Services.AddSingleton(mongoClient.GetDatabase("GreenSpace"));

        // Register To Handle Query/Command of MediatR
        builder.Services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies()));
        builder.Services.AddScoped<IClaimsService, ClaimsService>();
        // Register hangfire
        builder.Services.AddHangfire(config =>
        config.UseSqlServerStorage(builder.Configuration.GetConnectionString("DefaultConnection")));
        builder.Services.AddHangfireServer();
        builder.Services.AddHttpClient();
        builder.Services.AddTransient<GhnJobService>();
        // Scan and register all interfaces --> implementations 
        builder.Services.Scan(scan => scan
         .FromAssemblies(GreenSpace.Infrastructure.AssemblyReference.Assembly,
         GreenSpace.Application.AssemblyReference.Assembly,
         AssemblyReference.Assembly)
         .AddClasses()
         .UsingRegistrationStrategy(RegistrationStrategy.Skip)
         .AsMatchingInterface()
         .WithScopedLifetime());

        builder.Services.AddSwaggerGen(opt =>
        {
            opt.SwaggerDoc("v1", new OpenApiInfo { Title = "Green Space", Version = "v1" });
            //var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            //var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            //opt.IncludeXmlComments(xmlPath);
            opt.AddSecurityDefinition(name: "Bearer", securityScheme: new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Description = "Bearer Generated JWT-Token",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"

            });
            opt.AddSecurityRequirement(new OpenApiSecurityRequirement
                        {
                            {
                                new OpenApiSecurityScheme
                                {
                                    Reference = new OpenApiReference
                                    {
                                        Type = ReferenceType.SecurityScheme,
                                        Id = JwtBearerDefaults.AuthenticationScheme
                                    },
                                    Scheme = "oauth2",
                                    Name = "Bearer",
                                    In = ParameterLocation.Header,
                                }, Array.Empty<string>()
                            }
                        });
        });
        var key = Encoding.ASCII.GetBytes(configuration.JWTOptions.Secret);
        builder.Services.AddAuthentication(x =>
        {
            x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(x =>
        {
            x.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidIssuer = configuration.JWTOptions.Issuer,
                ValidAudience = configuration.JWTOptions.Audience,
                ValidateAudience = true
            };
        });
        // Đọc cấu hình từ appsettings.json
        var cloudinarySettings = builder.Configuration.GetSection("CloudinarySettings");
        Account account = new Account(
            cloudinarySettings["CloudName"],
            cloudinarySettings["ApiKey"],
            cloudinarySettings["ApiSecret"]
        );
        Cloudinary cloudinary = new Cloudinary(account);

        builder.Services.AddSingleton(cloudinary);
        builder.Services.AddSingleton<CloudinaryService>();

        // Đọc cấu hình GHN từ appsettings.json
        builder.Services.Configure<GhnSettings>(builder.Configuration.GetSection("GHN"));

        builder.Services.AddHttpClient<ShippingService>();

        builder.Services.AddSignalR();
        builder.Services.AddSingleton<PerformanceMiddleware>();
        builder.Services.AddSingleton<Stopwatch>();
        return builder;
    }
}