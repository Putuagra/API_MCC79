using API.Contracts;
using API.DTOs.Accounts;
using API.Models;
using API.Utilites.Enums;

namespace API.Services;

public class AccountService
{
    private readonly IAccountRepository _accountRepository;
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IUniversityRepository _universityRepository;
    private readonly IEducationRepository _educationRepository;

    public AccountService(IAccountRepository accountRepository,
            IEmployeeRepository employeeRepository,
            IUniversityRepository universityRepository,
            IEducationRepository educationRepository)
    {
        _accountRepository = accountRepository;
        _employeeRepository = employeeRepository;
        _universityRepository = universityRepository;
        _educationRepository = educationRepository;
    }

    public IEnumerable<GetAccountDto>? GetAccount()
    {
        var accounts = _accountRepository.GetAll();
        if (!accounts.Any())
        {
            return null; // No accounts found
        }

        var toDto = accounts.Select(account => new GetAccountDto
        {
            Guid = account.Guid,
            Password = account.Password,
            IsDeleted = account.IsDeleted,
            Otp = account.Otp,
            IsUsed = account.IsUsed,
            ExpiredTime = account.ExpiredTime
        }).ToList();

        return toDto; // Accounts found
    }

    public GetAccountDto? GetAccount(Guid guid)
    {
        var account = _accountRepository.GetByGuid(guid);

        if (account is null)
        {
            return null; // No universities found
        }

        var toDto = new GetAccountDto
        {
            Guid = account.Guid,
            Password = account.Password,
            IsDeleted = account.IsDeleted,
            Otp = account.Otp,
            IsUsed = account.IsUsed,
            ExpiredTime = account.ExpiredTime
        };

        return toDto;
    }

    public GetAccountDto? CreateAccount(NewAccountDto newAccountDto)
    {
        var account = new Account
        {
            Guid = newAccountDto.Guid,
            Password = Hashing.HashPassword(newAccountDto.Password),
            IsDeleted = newAccountDto.IsDeleted,
            Otp = newAccountDto.Otp,
            IsUsed = newAccountDto.IsUsed,
            ExpiredTime = DateTime.Now,
            CreatedDate = DateTime.Now,
            ModifiedDate = DateTime.Now
        };

        var createdAccount = _accountRepository.Create(account);

        if (createdAccount == null)
        {
            return null; // Account not created
        }

        var toDto = new GetAccountDto
        {
            Guid = createdAccount.Guid,
            Password = createdAccount.Password,
            IsDeleted = createdAccount.IsDeleted,
            Otp = createdAccount.Otp,
            IsUsed = createdAccount.IsUsed,
            ExpiredTime = createdAccount.ExpiredTime
        };

        return toDto; // Account created
    }

    public int UpdateAccount(GetAccountDto getAccountDto)
    {
        var isExist = _accountRepository.IsExist(getAccountDto.Guid);

        if (!isExist)
        {
            return -1; // Account not found
        }

        var getAccount = _accountRepository.GetByGuid(getAccountDto.Guid);
        var account = new Account
        {
            Guid = getAccountDto.Guid,
            Password = Hashing.HashPassword(getAccountDto.Password),
            IsDeleted = getAccountDto.IsDeleted,
            Otp = getAccountDto.Otp,
            IsUsed = getAccountDto.IsUsed,
            ExpiredTime = getAccount.CreatedDate,
            CreatedDate = getAccount.CreatedDate,
            ModifiedDate = DateTime.Now
        };

        var isUpdate = _accountRepository.Update(account);

        if (!isUpdate)
        {
            return 0; // Account not updated
        }

        return 1; // Account updated
    }

    public int DeleteAccount(Guid guid)
    {
        var isExist = _accountRepository.IsExist(guid);

        if (!isExist)
        {
            return -1;
        }

        var account = _accountRepository.GetByGuid(guid);
        var isDelete = _accountRepository.Delete(account!);
        if (!isDelete)
        {
            return 0;
        }

        return 1;
    }

    public RegisterDto? Register(RegisterDto registerDto)
    {
        EmployeeService employeeService = new EmployeeService(_employeeRepository);
        Employee employee = new Employee
        {
            Guid = new Guid(),
            Nik = employeeService.GenerateNik(),
            FirstName = registerDto.FirstName,
            LastName = registerDto.LastName,
            BirthDate = registerDto.BirthDate,
            Gender = registerDto.Gender,
            HiringDate = registerDto.HiringDate,
            Email = registerDto.Email,
            PhoneNumber = registerDto.PhoneNumber,
            CreatedDate = DateTime.Now,
            ModifiedDate = DateTime.Now
        };

        var createdEmployee = _employeeRepository.Create(employee);
        if (createdEmployee is null)
        {
            return null;
        }

        University university = new University
        {
            Guid = new Guid(),
            Code = registerDto.UniversityCode,
            Name = registerDto.UniversityName
        };

        var createdUniversity = _universityRepository.Create(university);
        if (createdUniversity is null)
        {
            return null;
        }

        Education education = new Education
        {
            Guid = employee.Guid,
            Major = registerDto.Major,
            Degree = registerDto.Degree,
            Gpa = registerDto.Gpa,
            UniversityGuid = university.Guid
        };

        var createdEducation = _educationRepository.Create(education);
        if (createdEducation is null)
        {
            return null;
        }

        Account account = new Account
        {
            Guid = employee.Guid,
            Password = Hashing.HashPassword(registerDto.Password)
        };

        if (registerDto.Password != registerDto.ConfirmPassword)
        {
            return null;
        }

        var createdAccount = _accountRepository.Create(account);
        if (createdAccount is null)
        {
            return null;
        }


        var toDto = new RegisterDto
        {
            FirstName = createdEmployee.FirstName,
            LastName = createdEmployee.LastName,
            BirthDate = createdEmployee.BirthDate,
            Gender = createdEmployee.Gender,
            HiringDate = createdEmployee.HiringDate,
            Email = createdEmployee.Email,
            PhoneNumber = createdEmployee.PhoneNumber,
            Password = createdAccount.Password,
            Major = createdEducation.Major,
            Degree = createdEducation.Degree,
            Gpa = createdEducation.Gpa,
            UniversityCode = createdUniversity.Code,
            UniversityName = createdUniversity.Name
        };

        return toDto;
    }
}
