using API.Utilites.Validations;
using API.Utilities.Enums;
using System.ComponentModel.DataAnnotations;

namespace API.DTOs.Employees;

public class NewEmployeeDto
{
    [Required]
    public string FirstName { get; set; }
    public string? LastName { get; set; }
    [Required]
    public DateTime BirthDate { get; set; }
    [Required]
    public GenderEnum Gender { get; set; }
    [Required]
    public DateTime HiringDate { get; set; }
    [Required]
    [EmailAddress]
    [EmployeeDuplicateProperty("string", "Email")]
    public string Email { get; set; }
    [Required]
    [EmployeeDuplicateProperty("string", "PhoneNumber")]
    public string PhoneNumber { get; set; }
}
