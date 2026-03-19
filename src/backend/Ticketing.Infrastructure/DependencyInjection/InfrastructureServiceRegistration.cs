using Azure.Identity;
using Azure.Storage.Blobs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ticketing.Infrastructure.Options;
using Ticketing.Application.Abstractions;
using Ticketing.Domain.Repositories;
using Ticketing.Infrastructure.Persistence;
using Ticketing.Infrastructure.Repositories;
using Ticketing.Infrastructure.Services;

namespace Ticketing.Infrastructure.DependencyInjection;

public static class InfrastructureServiceRegistration
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("TicketingDb")
            ?? "Server=(localdb)\\mssqllocaldb;Database=TicketingDb;Trusted_Connection=True;TrustServerCertificate=True;";

        services.AddDbContext<TicketingDbContext>(options => options.UseSqlServer(connectionString));
        services.AddScoped<ITicketRepository, TicketRepository>();
        services.AddScoped<ITicketService, TicketService>();

        var azureDevOpsOptions = configuration.GetSection(AzureDevOpsOptions.SectionName).Get<AzureDevOpsOptions>() ?? new AzureDevOpsOptions();
        services.AddSingleton(azureDevOpsOptions);
        services.AddHttpClient<IAzureDevOpsService, AzureDevOpsService>();

        var blobServiceUri = configuration["AzureResources:BlobServiceUri"];
        if (!string.IsNullOrWhiteSpace(blobServiceUri))
        {
            services.AddSingleton(new BlobServiceClient(new Uri(blobServiceUri), new DefaultAzureCredential()));
            services.AddSingleton<AzureBlobFileStorage>();
        }

        return services;
    }
}
