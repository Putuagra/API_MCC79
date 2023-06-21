using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models;

[Table("tb_m_accounts")]
public class Account
{
    [Key]
    [Column("employee_guid")]
    public Guid EmployeeGuid { get; set; }

    [Column("password", TypeName = "nvarchar(255)")]
    public string Password { get; set; }

    [Column("is_deleted")]
    public bool IsDeleted { get; set; }

    [Column("otp", TypeName = "nvarchar(6)")]
    //blm tau boleh null apa gk
    public string Otp { get; set; }

    [Column("is_used")]
    //blm tau boleh null apa gk
    public bool IsUsed { get; set; }

    [Column("expired_time")]
    //blm tau boleh null apa gk
    public DateTime ExpiredTime { get; set; }

    [Column("created_date")]
    public DateTime CreatedDate { get; set; }

    [Column("modified_date")]
    public DateTime ModifiedDate { get; set; }
}
