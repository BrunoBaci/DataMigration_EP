using System.ComponentModel.DataAnnotations.Schema;

[Table("WorkOrders")]
public class WorkOrder
{
    [Column("ID")]
    public int Id { get; set; }
    
    [Column("ClientId")]
    public int ClientId { get; set; }

    [Column("TechnicianId")] 
    public int TechnicianId { get; set; }

    [Column("Information")]
    public string Information { get; set; }

    [Column("Date")] 
    public DateTime Date { get; set; }
    
    [Column("Total")]
    public decimal Total { get; set; }
}