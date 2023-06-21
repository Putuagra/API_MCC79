using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models
{
    [Table("tb_m_employees")]
    public class Employee
    {
        [Key]
        [Column("guid")]
        public Guid Guid { get; set; }

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
        public int Gender { get; set; }

        [Column("hiring_date")]
        public DateTime HiringDate { get; set; }

        [Column("email", TypeName = "nvarchar(50)")]
        //unique
        public string Email { get; set; }

        [Column("phone_number", TypeName = "nvarchar(50)")]
        //unique
        public string? phone_number { get; set; }

        [Column("created_date")]
        public DateTime CreatedDate { get; set; }

        [Column("modified_date")]
        public DateTime ModifiedDate { get; set; }
    }
}
