using API.Utilites.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models;

[Table("tb_tr_bookings")]
public class Booking : BasicEntity
{
    [Column("start_date")]
    public DateTime StartDate { get; set; }

    [Column("end_date")]
    public DateTime EndDate { get; set; }

    [Column("remarks", TypeName = "nvarchar(255)")]
    //blm tau boleh null apa gk
    public string Remarks { get; set; }

    [Column("status")]
    //harusnya enum
    public StatusLevel Status { get; set; }

    [Column("room_guid")]
    //foreign key
    public Guid RoomGuid { get; set; }

    [Column("employee_guid")]
    //foreign key
    public Guid EmployeeGuid { get; set; }

    // Cardinality
    public Employee Employee { get; set; }
    public Room room { get; set; }
}
