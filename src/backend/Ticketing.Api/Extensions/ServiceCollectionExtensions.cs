using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Ticketing.Api.Options;
using Ticketing.Infrastructure.DependencyInjection;
using Ticketing.Infrastructure.Options;

namespace Ticketing.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApiServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<AzureAdOptions>(configuration.GetSection(AzureAdOptions.SectionName));
        services.Configure<AzureDevOpsOptions>(configuration.GetSection(AzureDevOpsOptions.SectionName));
        services.AddInfrastructure(configuration);
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        var azureAd = configuration.GetSection(AzureAdOptions.SectionName).Get<AzureAdOptions>();
        if (azureAd is not null && !string.IsNullOrWhiteSpace(azureAd.Instance) && !string.IsNullOrWhiteSpace(azureAd.TenantId))
        {
            var authority = $"{azureAd.Instance}{azureAd.TenantId}/v2.0";
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.Authority = authority;
                    options.Audience = string.IsNullOrWhiteSpace(azureAd.Audience) ? azureAd.ClientId : azureAd.Audience;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true
                    };
                });
            services.AddAuthorization();
        }

        return services;
    }
}
