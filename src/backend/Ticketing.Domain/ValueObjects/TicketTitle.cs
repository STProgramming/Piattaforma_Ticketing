namespace Ticketing.Domain.ValueObjects;

public sealed record TicketTitle
{
    public const int MaxLength = 120;

    public string Value { get; }

    public TicketTitle(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Ticket title is required.");
        }

        if (value.Length > MaxLength)
        {
            throw new ArgumentException($"Ticket title cannot exceed {MaxLength} characters.");
        }

        Value = value.Trim();
    }

    public static implicit operator string(TicketTitle title) => title.Value;
}
