namespace Ticketing.Application.Features.Tickets.Commands.CreateTicket;

public sealed record CreateTicketCommand(string Title, string Description, string CreatedBy);
