using API.Contracts;
using API.Data;
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
    private readonly BookingDbContext _dBContext;

    public AccountService(IAccountRepository accountRepository,
            IEmployeeRepository employeeRepository,
            IUniversityRepository universityRepository,
            IEducationRepository educationRepository,
            BookingDbContext dBContext)
    {
        _accountRepository = accountRepository;
        _employeeRepository = employeeRepository;
        _universityRepository = universityRepository;
        _educationRepository = educationRepository;
        _dBContext = dBContext;
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
            ExpiredTime = getAccountDto.ExpiredTime,
            CreatedDate = getAccount!.CreatedDate,
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

    public GetRegisterDto? Register(RegisterDto registerDto)
    {
        EmployeeService employeeService = new EmployeeService(_employeeRepository);
        using var transaction = _dBContext.Database.BeginTransaction();
        try
        {
            var employee = new Employee
            {
                Guid = new Guid(),
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName ?? "",
                Gender = registerDto.Gender,
                HiringDate = registerDto.HiringDate,
                Email = registerDto.Email,
                Nik = employeeService.GenerateNik(),
                PhoneNumber = registerDto.PhoneNumber,
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now,
            };
            _employeeRepository.Create(employee);

            var account = new Account
            {
                Guid = employee.Guid,
                Password = Hashing.HashPassword(registerDto.Password),
                IsDeleted = false,
                IsUsed = false,
                Otp = 0,
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now,
                ExpiredTime = DateTime.Now.AddYears(5),
            };
            _accountRepository.Create(account);

            var universityEntity = _universityRepository.GetByCodeAndName(registerDto.UniversityCode, registerDto.UniversityName);
            if (universityEntity == null)
            {
                var university = new University
                {
                    Code = registerDto.UniversityCode,
                    Name = registerDto.UniversityName,
                    Guid = new Guid(),
                    CreatedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now,
                };
                universityEntity = _universityRepository.Create(university);
            }

            var education = new Education
            {
                Guid = employee.Guid,
                Degree = registerDto.Degree,
                Major = registerDto.Major,
                Gpa = registerDto.Gpa,
                UniversityGuid = universityEntity.Guid,
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now,
            };
            _educationRepository.Create(education);

            var toDto = new GetRegisterDto
            {
                Guid = employee.Guid,
                Email = employee.Email,
            };

            transaction.Commit();
            return toDto;
        }
        catch
        {
            transaction.Rollback();
            return null;
        }
    }


    public int ChangePassword(ChangePasswordDto changePasswordDto)
    {
        var isExist = _employeeRepository.GetByEmail(changePasswordDto.Email);
        if (isExist is null)
        {
            return -1;
        }

        var getAccount = _accountRepository.GetByGuid(isExist.Guid);
        if (getAccount.Otp != changePasswordDto.Otp)
        {
            return 0;
        }
        if (getAccount.IsUsed == true)
        {
            return 1;
        }
        if (getAccount.ExpiredTime < DateTime.Now)
        {
            return 2;
        }

        var account = new Account
        {
            Guid = getAccount.Guid,
            IsUsed = getAccount.IsUsed,
            IsDeleted = getAccount.IsDeleted,
            ModifiedDate = DateTime.Now,
            CreatedDate = getAccount!.CreatedDate,
            Otp = changePasswordDto.Otp,
            ExpiredTime = getAccount.ExpiredTime,
            Password = Hashing.HashPassword(changePasswordDto.NewPassword)
        };
        var isUpdated = _accountRepository.Update(account);
        if (!isUpdated)
        {
            return 0;
        }
        return 3;
    }

    public int ForgetPassword(ForgetPasswordDto forgetPasswordDto)
    {
        var isExist = _employeeRepository.GetByEmail(forgetPasswordDto.Email);
        if (isExist == null)
        {
            return -1;
        }

        Random rand = new Random();
        HashSet<int> uniqueDigits = new HashSet<int>();

        while (uniqueDigits.Count < 6)
        {
            int digit = rand.Next(0, 9);
            uniqueDigits.Add(digit);
        }

        int generateOtp = uniqueDigits.Aggregate(0, (acc, digit) => acc * 10 + digit);

        var relatedAccount = GetAccount(isExist.Guid);

        var updateAccountDto = new GetAccountDto
        {
            Guid = relatedAccount.Guid,
            Password = relatedAccount.Password,
            IsDeleted = relatedAccount.IsDeleted,
            Otp = generateOtp,
            IsUsed = false,
            ExpiredTime = DateTime.Now.AddMinutes(5)
        };
        var updateResult = UpdateAccount(updateAccountDto);
        if (updateResult == 0)
        {
            return 0;
        }
        return 1;
    }
}
