using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

public class WorkOrderDbContext : DbContext
{
    public WorkOrderDbContext(DbContextOptions<WorkOrderDbContext> options) : base(options) { }

    /// <summary>
    /// 
    /// </summary>
    public DbSet<WorkOrder> WorkOrders { get; set; }

    /// <summary>
    /// The list of clients that will be used as a comparison whether the Work Order client names are valid or not
    /// </summary>
    public DbSet<Client> Clients { get; set; }
    public DbSet<Technician> Technicians { get; set; }
}

[Table("Clients")]
public class Client
{
    [Column("ID")]
    public int Id { get; set; }

    [Column("First Name")]
    public string FirstName { get; set; }

    [Column("Last Name")]
    public string LastName { get; set; }

    public string FullName => $"{FirstName} {LastName}";
}


[Table("Technicians")]
public class Technician
{
    [Column("ID")]
    public int Id { get; set; }

    [Column("First Name")]
    public string FirstName { get; set; }

    [Column("Last Name")]
    public string LastName { get; set; }

    public string FullName => $"{FirstName} {LastName}";
}