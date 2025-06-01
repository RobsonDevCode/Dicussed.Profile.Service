using System.Text;
using Discussed.Profile.Api.Configuration;
using Discussed.Profile.Domain.Mappers;
using Discussed.Profile.Persistence.Interfaces.Mapper;
using Discussed.Profile.Domain.Services;
using Discussed.Profile.Domain.Services.Profiles;
using Discussed.Profile.Persistence.Interfaces.Factories;
using Discussed.Profile.Persistence.Interfaces.Reader;
using Discussed.Profile.Persistence.Interfaces.Writer;
using Discussed.Profile.Persistence.MySql.Factories;
using Discussed.Profile.Persistence.MySql.Reader;
using Discussed.Profile.Persistence.MySql.Writer;
using Discussed.Profile.Persistence.Neo4J.Factories;
using Discussed.Profile.Persistence.Neo4J.Reader;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace Discussed.Profile.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddDiscussedAuth(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtSettings = configuration.GetSection("JwtSettings").Get<JwtSettings>();
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme =
                options.DefaultChallengeScheme =
                    options.DefaultForbidScheme =
                        options.DefaultScheme =
                            options.DefaultSignInScheme =
                                options.DefaultSignOutScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = jwtSettings.Issuer,
                ValidateAudience = true,
                ValidAudience = jwtSettings.Audience,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key))
            };
        });
        services.AddAuthorization();
    }

    public static void AddDiscussedSwagger(this IServiceCollection services)
    {
        const string version = "v1";
        services.AddSwaggerGen(s =>
        {
            s.SwaggerDoc(version, new()
            {
                Title = "Discussed CreateProfileInput API",
                Version = version
            });
            var securityScheme = new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Description = "JWT Authorization header using the Bearer scheme.",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                Reference = new OpenApiReference
                {
                    Id = JwtBearerDefaults.AuthenticationScheme,
                    Type = ReferenceType.SecurityScheme
                }
            };

            s.AddSecurityDefinition(securityScheme.Reference.Id, securityScheme);
            s.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                { securityScheme, [] }
            });
        });
    }

    public static void AddDiscussedCors(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("AllowSpecificOrigins", policy =>
            {
                policy.WithOrigins("http://localhost:5173")
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();

                policy.WithOrigins("http://localhost:5183")
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();

                policy.WithOrigins("https://localhost:7164")
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
            });
        });
    }

    public static void AddDiscussedDependencies(this IServiceCollection services)
    {
        services.AddSingleton<IMySqlConnectionFactory, MySqlConnectionFactory>();
        services.AddSingleton<INeo4JConnectionFactory, Neo4JConnectionFactory>();
        
        services.AddScoped<IFollowingRetrievalService, FollowingRetrievalService>();
        services.AddScoped<IFollowingRemovalService, FollowingRemovalService>();
        services.AddScoped<IProfileRetrievalService, ProfileRetrievalService>();
        services.AddScoped<IProfileUpsertService, ProfileUpsertService>();
        services.AddScoped<IFollowingUpsertService, FollowingUpsertService>();
        services.AddScoped<IFollowReader, FollowReader>();
        services.AddScoped<IFollowWriter, FollowerWriter>();
        services.AddSingleton<Persistence.Interfaces.Mapper.IMapper, Persistence.Interfaces.Mapper.Mapper>();
        services.AddSingleton<Domain.Mappers.IMapper, Domain.Mappers.Mapper>();
    }

    public static void AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        var profileVersion = configuration.GetSection("Versioning:ProfileVersion").Get<int>();
        switch (profileVersion)
        {
            case 1:
                services.AddScoped<IProfileWriter, ProfileWriter>();
                services.AddScoped<IProfileReader, ProfileReader>();     
                services.AddScoped<IProfileValidationReader, ProfileValidationReader>();
                break;
            case 2:
                services.AddScoped<IProfileWriter, ProfileWriterV2>();
                services.AddScoped<IProfileReader, ProfileReaderV2>();
                break;
            
            default:
                services.AddScoped<IProfileWriter, ProfileWriter>();
                services.AddScoped<IProfileReader, ProfileReader>();
                break;
        }
    }
}