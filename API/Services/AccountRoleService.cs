using API.Contracts;
using API.DTOs.AccountRoles;
using API.Models;

namespace API.Services;

public class AccountRoleService
{
    private readonly IAccountRoleRepository _accountRoleRepository;

    public AccountRoleService(IAccountRoleRepository accountRoleRepository)
    {
        _accountRoleRepository = accountRoleRepository;
    }

    public IEnumerable<GetAccountRoleDto>? GetAccountRole()
    {
        var accountRoles = _accountRoleRepository.GetAll();
        if (!accountRoles.Any())
        {
            return null;
        }

        var toDto = accountRoles.Select(accountRole => new GetAccountRoleDto
        {
            Guid = accountRole.Guid,
            AccountGuid = accountRole.AccountGuid,
            RoleGuid = accountRole.RoleGuid
        }).ToList();

        return toDto;
    }

    public GetAccountRoleDto? GetAccountRole(Guid guid)
    {
        var accountRole = _accountRoleRepository.GetByGuid(guid);
        if (accountRole is null)
        {
            return null;
        }

        var toDto = new GetAccountRoleDto
        {
            Guid = accountRole.Guid,
            AccountGuid = accountRole.AccountGuid,
            RoleGuid = accountRole.RoleGuid
        };

        return toDto;
    }

    public GetAccountRoleDto? CreateAccountRole(NewAccountRoleDto newAccountRoleDto)
    {
        var accountRole = new AccountRole
        {
            Guid = new Guid(),
            AccountGuid = newAccountRoleDto.AccountGuid,
            RoleGuid = newAccountRoleDto.RoleGuid,
            CreatedDate = DateTime.Now,
            ModifiedDate = DateTime.Now
        };

        var createdAccountRole = _accountRoleRepository.Create(accountRole);
        if (createdAccountRole is null)
        {
            return null;
        }

        var toDto = new GetAccountRoleDto
        {
            Guid = createdAccountRole.Guid,
            AccountGuid = createdAccountRole.AccountGuid,
            RoleGuid = createdAccountRole.RoleGuid
        };

        return toDto;
    }

    public int UpdateAccountRole(GetAccountRoleDto getAccountRoleDto)
    {
        var isExist = _accountRoleRepository.IsExist(getAccountRoleDto.Guid);
        if (!isExist)
        {
            return -1;
        }

        var getAccountRole = _accountRoleRepository.GetByGuid(getAccountRoleDto.Guid);

        var accountRole = new AccountRole
        {
            Guid = getAccountRoleDto.Guid,
            AccountGuid = getAccountRoleDto.AccountGuid,
            RoleGuid = getAccountRoleDto.RoleGuid,
            ModifiedDate = DateTime.Now,
            CreatedDate = getAccountRole!.CreatedDate
        };

        var isUpdate = _accountRoleRepository.Update(accountRole);
        if (!isUpdate)
        {
            return 0;
        }

        return 1;
    }

    public int DeleteAccountRole(Guid guid)
    {
        var isExist = _accountRoleRepository.IsExist(guid);
        if (!isExist)
        {
            return -1;
        }

        var accountRole = _accountRoleRepository.GetByGuid(guid);
        var isDelete = _accountRoleRepository.Delete(accountRole!);
        if (!isDelete)
        {
            return 0;
        }

        return 1;
    }
}
