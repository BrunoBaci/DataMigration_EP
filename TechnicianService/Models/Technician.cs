using System.ComponentModel.DataAnnotations.Schema;


/// <summary>
/// A class with Technician information
/// </summary>
[Table("Technicians")]
public class Technician
{
    /// <summary>
    /// The technician's ID
    /// </summary>
    [Column("ID")]
    public int Id { get; set; }

    /// <summary>
    /// The technician's first name
    /// </summary>
    [Column("First Name")]
    public string FirstName { get; set; }

    /// <summary>
    /// The technician's last name
    /// </summary>
    [Column("Last Name")]
    public string LastName { get; set; }
}