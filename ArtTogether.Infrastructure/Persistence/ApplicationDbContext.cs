using ArtTogether.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ArtTogether.Infrastructure.Persistence;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<Stroke> Strokes { get; set; }
}
