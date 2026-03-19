namespace Ticketing.Domain.ValueObjects;

public sealed record TicketDescription
{
    public const int MaxLength = 2000;

    public string Value { get; }

    public TicketDescription(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Ticket description is required.");
        }

        if (value.Length > MaxLength)
        {
            throw new ArgumentException($"Ticket description cannot exceed {MaxLength} characters.");
        }

        Value = value.Trim();
    }

    public static implicit operator string(TicketDescription description) => description.Value;
}
