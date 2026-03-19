using Ticketing.Application.Abstractions;
using Ticketing.Application.DTOs;
using Ticketing.Domain.Entities;
using Ticketing.Domain.Repositories;
using Ticketing.Domain.ValueObjects;

namespace Ticketing.Infrastructure.Services;

public sealed class TicketService : ITicketService
{
    private readonly ITicketRepository _ticketRepository;
    private readonly IAzureDevOpsService _azureDevOpsService;

    public TicketService(ITicketRepository ticketRepository, IAzureDevOpsService azureDevOpsService)
    {
        _ticketRepository = ticketRepository;
        _azureDevOpsService = azureDevOpsService;
    }

    public async Task<TicketDto> CreateAsync(string title, string description, string createdBy, AzureDevOpsLinkRequest? azureDevOpsLink, CancellationToken cancellationToken = default)
    {
        var ticket = new Ticket(Guid.NewGuid(), new TicketTitle(title), new TicketDescription(description), createdBy, DateTimeOffset.UtcNow);
        await _ticketRepository.AddAsync(ticket, cancellationToken);

        var initialDto = Map(ticket);
        var workItem = await _azureDevOpsService.CreateUserStoryForTicketAsync(initialDto, azureDevOpsLink, cancellationToken);
        if (workItem is not null)
        {
            ticket.LinkAzureDevOpsWorkItem(workItem.Id, workItem.Url);
        }

        await _ticketRepository.SaveChangesAsync(cancellationToken);
        return Map(ticket);
    }

    public async Task<TicketDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var ticket = await _ticketRepository.GetByIdAsync(id, cancellationToken);
        return ticket is null ? null : Map(ticket);
    }

    public async Task<IReadOnlyCollection<TicketDto>> GetAllAsync(CancellationToken cancellationToken = default)
        => (await _ticketRepository.GetAllAsync(cancellationToken)).Select(Map).ToArray();

    private static TicketDto Map(Ticket ticket)
        => new(
            ticket.Id,
            ticket.Title.Value,
            ticket.Description.Value,
            ticket.Status.ToString(),
            ticket.CreatedBy,
            ticket.CreatedAt,
            ticket.LastUpdatedAt,
            ticket.AzureDevOpsWorkItemId,
            ticket.AzureDevOpsWorkItemUrl);
}
