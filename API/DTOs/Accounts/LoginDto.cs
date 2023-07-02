using API.Utilites.Validations;
using System.ComponentModel.DataAnnotations;

namespace API.DTOs.Accounts;

public class LoginDto
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    [Required]
    [PasswordPolicy]
    public string Password { get; set; }
}
