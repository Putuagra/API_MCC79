using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models;

[Table("tb_m_accounts")]
public class Account : BasicEntity
{
    [Column("password", TypeName = "nvarchar(255)")]
    public string Password { get; set; }

    [Column("is_deleted")]
    public bool IsDeleted { get; set; }

    [Column("otp")]
    //blm tau boleh null apa gk
    public int Otp { get; set; }

    [Column("is_used")]
    //blm tau boleh null apa gk
    public bool IsUsed { get; set; }

    [Column("expired_time")]
    //blm tau boleh null apa gk
    public DateTime ExpiredTime { get; set; }

    // Cardinality
    public Employee? Employee { get; set; }
    public ICollection<AccountRole>? AccountRoles { get; set; }
}
