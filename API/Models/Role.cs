﻿using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models;

[Table("tb_m_roles")]
public class Role : BasicEntity
{
    [Column("name", TypeName = "nvarchar(100)")]
    public string Name { get; set; }

    // Cardinality
    public ICollection<AccountRole>? AccountRoles { get; set; }
}
