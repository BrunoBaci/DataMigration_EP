using System.ComponentModel.DataAnnotations.Schema;


/// <summary>
/// A class with Client information
/// </summary>
[Table("Clients")]
public class Client
{
    /// <summary>
    /// The client's ID
    /// </summary>
    [Column("ID")]
    public int Id { get; set; }

    /// <summary>
    /// The client's first name
    /// </summary>
    [Column("First Name")]
    public string FirstName { get; set; }

    /// <summary>
    /// The client's last name
    /// </summary>
    [Column("Last Name")]
    public string LastName { get; set; }
}