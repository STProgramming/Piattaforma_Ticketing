using Ticketing.Application.DTOs;

namespace Ticketing.Application.Abstractions;

public interface ITicketService
{
    Task<TicketDto> CreateAsync(string title, string description, string createdBy, AzureDevOpsLinkRequest? azureDevOpsLink, CancellationToken cancellationToken = default);
    Task<TicketDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<TicketDto>> GetAllAsync(CancellationToken cancellationToken = default);
}
