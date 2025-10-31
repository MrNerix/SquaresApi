using Microsoft.EntityFrameworkCore;

namespace SquaresApi.Data;

public class AppDb : DbContext
{
    public AppDb(DbContextOptions<AppDb> options) : base(options) { }

    public DbSet<PointEntity> Points => Set<PointEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Prevent duplicate coordinates
        modelBuilder.Entity<PointEntity>()
            .HasIndex(p => new { p.X, p.Y })
            .IsUnique();
    }
}

public class PointEntity
{
    public int Id { get; set; }  // simple int PK
    public int X { get; set; }
    public int Y { get; set; }
}
