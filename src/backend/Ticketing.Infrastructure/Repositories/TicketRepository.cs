using Microsoft.EntityFrameworkCore;
using Ticketing.Domain.Entities;
using Ticketing.Domain.Repositories;
using Ticketing.Infrastructure.Persistence;

namespace Ticketing.Infrastructure.Repositories;

public sealed class TicketRepository : ITicketRepository
{
    private readonly TicketingDbContext _dbContext;

    public TicketRepository(TicketingDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Ticket?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => await _dbContext.Tickets.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public async Task<IReadOnlyCollection<Ticket>> GetAllAsync(CancellationToken cancellationToken = default)
        => await _dbContext.Tickets.OrderByDescending(x => x.CreatedAt).ToListAsync(cancellationToken);

    public async Task AddAsync(Ticket ticket, CancellationToken cancellationToken = default)
        => await _dbContext.Tickets.AddAsync(ticket, cancellationToken);

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        => await _dbContext.SaveChangesAsync(cancellationToken);
}
