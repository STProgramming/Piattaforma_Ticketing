using Ticketing.Domain.Enums;
using Ticketing.Domain.ValueObjects;

namespace Ticketing.Domain.Entities;

public class Ticket
{
    private Ticket() { }

    public Ticket(Guid id, TicketTitle title, TicketDescription description, string createdBy, DateTimeOffset createdAt)
    {
        Id = id;
        Title = title;
        Description = description;
        CreatedBy = createdBy;
        CreatedAt = createdAt;
        Status = TicketStatus.Open;
    }

    public Guid Id { get; private set; }
    public TicketTitle Title { get; private set; } = null!;
    public TicketDescription Description { get; private set; } = null!;
    public TicketStatus Status { get; private set; }
    public string CreatedBy { get; private set; } = string.Empty;
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset? LastUpdatedAt { get; private set; }
    public int? AzureDevOpsWorkItemId { get; private set; }
    public string? AzureDevOpsWorkItemUrl { get; private set; }

    public void MoveTo(TicketStatus newStatus)
    {
        Status = newStatus;
        LastUpdatedAt = DateTimeOffset.UtcNow;
    }

    public void LinkAzureDevOpsWorkItem(int workItemId, string workItemUrl)
    {
        if (workItemId <= 0)
        {
            throw new ArgumentException("Azure DevOps work item id must be greater than zero.");
        }

        if (string.IsNullOrWhiteSpace(workItemUrl))
        {
            throw new ArgumentException("Azure DevOps work item url is required.");
        }

        AzureDevOpsWorkItemId = workItemId;
        AzureDevOpsWorkItemUrl = workItemUrl;
        LastUpdatedAt = DateTimeOffset.UtcNow;
    }
}
