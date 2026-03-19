using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Ticketing.Infrastructure.Options;
using Ticketing.Application.Abstractions;
using Ticketing.Application.DTOs;

namespace Ticketing.Infrastructure.Services;

public sealed class AzureDevOpsService : IAzureDevOpsService
{
    private readonly HttpClient _httpClient;
    private readonly AzureDevOpsOptions _options;

    public AzureDevOpsService(HttpClient httpClient, AzureDevOpsOptions options)
    {
        _httpClient = httpClient;
        _options = options;
    }

    public async Task<AzureDevOpsWorkItemDto?> CreateUserStoryForTicketAsync(TicketDto ticket, AzureDevOpsLinkRequest? linkRequest, CancellationToken cancellationToken = default)
    {
        if (linkRequest is null || !linkRequest.CreateUserStory)
        {
            return null;
        }

        if (string.IsNullOrWhiteSpace(_options.Organization) || string.IsNullOrWhiteSpace(_options.Project) || string.IsNullOrWhiteSpace(_options.PersonalAccessToken))
        {
            throw new ArgumentException("Azure DevOps configuration is missing. Set Organization, Project and PersonalAccessToken.");
        }

        var request = new HttpRequestMessage(
            HttpMethod.Post,
            $"https://dev.azure.com/{_options.Organization}/{_options.Project}/_apis/wit/workitems/$User%20Story?api-version=7.1-preview.3");

        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json-patch+json"));
        request.Headers.Authorization = new AuthenticationHeaderValue(
            "Basic",
            Convert.ToBase64String(Encoding.ASCII.GetBytes($":{_options.PersonalAccessToken}")));

        var operations = new object[]
        {
            new { op = "add", path = "/fields/System.Title", value = linkRequest.Title ?? $"Ticket {ticket.Id} - {ticket.Title}" },
            new { op = "add", path = "/fields/System.Description", value = linkRequest.Description ?? ticket.Description },
            new { op = "add", path = "/fields/System.AreaPath", value = string.IsNullOrWhiteSpace(linkRequest.AreaPath) ? _options.DefaultAreaPath : linkRequest.AreaPath },
            new { op = "add", path = "/fields/System.IterationPath", value = string.IsNullOrWhiteSpace(linkRequest.IterationPath) ? _options.DefaultIterationPath : linkRequest.IterationPath },
            new { op = "add", path = "/fields/System.AssignedTo", value = linkRequest.AssignedTo ?? ticket.CreatedBy }
        };

        request.Content = new StringContent(JsonSerializer.Serialize(operations), Encoding.UTF8, "application/json-patch+json");

        using var response = await _httpClient.SendAsync(request, cancellationToken);
        var body = await response.Content.ReadAsStringAsync(cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            throw new InvalidOperationException($"Azure DevOps work item creation failed: {(int)response.StatusCode} {body}");
        }

        using var document = JsonDocument.Parse(body);
        var id = document.RootElement.GetProperty("id").GetInt32();
        var url = document.RootElement.TryGetProperty("url", out var urlElement)
            ? urlElement.GetString() ?? string.Empty
            : string.Empty;

        return new AzureDevOpsWorkItemDto(id, url);
    }
}
