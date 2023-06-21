using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models;

[Table("tb_tr_account_roles")]
public class AccountRole : BasicEntity
{
    [Column("account_guid")]
    //foreign key
    public Guid AccountGuid { get; set; }

    [Column("role_guid")]
    //foreign key
    public Guid RoleGuid { get; set; }

    // Cardinality
    public Account Account { get; set; }
    public Role Role { get; set; }
}
