using API.Utilities.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models;

[Table("tb_m_employees")]
public class Employee : BasicEntity
{
    [Column("nik", TypeName = "nvarchar(6)")]
    //unique
    public string Nik { get; set; }

    [Column("first_name", TypeName = "nvarchar(100)")]
    public string FirstName { get; set; }

    [Column("last_name", TypeName = "nvarchar(100)")]
    public string? LastName { get; set; }

    [Column("birth_date")]
    public DateTime BirthDate { get; set; }

    [Column("gender")]
    //harusnya enum male female
    public GenderEnum Gender { get; set; }

    [Column("hiring_date")]
    public DateTime HiringDate { get; set; }

    [Column("email", TypeName = "nvarchar(100)")]
    //unique
    public string Email { get; set; }

    [Column("phone_number", TypeName = "nvarchar(20)")]
    //unique
    public string PhoneNumber { get; set; }

    // Cardinality
    public Education? Education { get; set; }
    public ICollection<Booking>? Bookings { get; set; }
    public Account? Account { get; set; }
}
