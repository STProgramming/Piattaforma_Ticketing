namespace Ticketing.Application.DTOs;

public sealed record AzureDevOpsLinkRequest(
    bool CreateUserStory,
    string? Title,
    string? Description,
    string? AreaPath,
    string? IterationPath,
    string? AssignedTo);
