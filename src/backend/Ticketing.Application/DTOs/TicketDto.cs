namespace Ticketing.Application.DTOs;

public sealed record TicketDto(
    Guid Id,
    string Title,
    string Description,
    string Status,
    string CreatedBy,
    DateTimeOffset CreatedAt,
    DateTimeOffset? LastUpdatedAt,
    int? AzureDevOpsWorkItemId,
    string? AzureDevOpsWorkItemUrl);
