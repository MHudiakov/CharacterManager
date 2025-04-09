using Microsoft.EntityFrameworkCore;
using Domain.Entities;

namespace Infrastructure.Persistence;

public interface IApplicationDbContext
{
    DbSet<Character> Characters { get; }

    DbSet<Location> Locations { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}