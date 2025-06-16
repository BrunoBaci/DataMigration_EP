using Microsoft.EntityFrameworkCore;

public class TechnicianDbContext : DbContext
{
    public TechnicianDbContext(DbContextOptions<TechnicianDbContext> options) : base(options) { }

    public DbSet<Technician> Technicians { get; set; }
}