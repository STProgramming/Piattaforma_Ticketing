using Ticketing.Application.DTOs;

namespace Ticketing.Application.Abstractions;

public interface IAzureDevOpsService
{
    Task<AzureDevOpsWorkItemDto?> CreateUserStoryForTicketAsync(TicketDto ticket, AzureDevOpsLinkRequest? linkRequest, CancellationToken cancellationToken = default);
}
