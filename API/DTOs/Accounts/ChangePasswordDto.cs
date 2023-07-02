using API.Utilites.Validations;
using System.ComponentModel.DataAnnotations;

namespace API.DTOs.Accounts;

public class ChangePasswordDto
{
    public string Email { get; set; }
    [Required]
    public int Otp { get; set; }
    [PasswordPolicy]
    public string NewPassword { get; set; }
    [Required(ErrorMessage = "Password not match")]
    [Compare("NewPassword")]
    public string ConfirmPassword { get; set; }
}
