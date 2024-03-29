﻿using API.Utilites.Validations;

namespace API.DTOs.Accounts;

public class NewAccountDto
{
    public Guid Guid { get; set; }
    [PasswordPolicy]
    public string Password { get; set; }
    public bool IsDeleted { get; set; }
    public int Otp { get; set; }
    public bool IsUsed { get; set; }
    public DateTime ExpiredTime { get; set; }
}
