using Ticketing.Application.DTOs;

namespace Ticketing.Api.Contracts;

public sealed record CreateTicketRequest(
    string Title,
    string Description,
    string CreatedBy,
    AzureDevOpsLinkRequest? AzureDevOps);
