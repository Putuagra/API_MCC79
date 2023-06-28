using API.Contracts;
using API.DTOs.Roles;
using API.Models;

namespace API.Services;

public class RoleService
{
    private readonly IRoleRepository _roleRepository;
    public RoleService(IRoleRepository roleRepository)
    {
        _roleRepository = roleRepository;
    }

    public IEnumerable<GetRoleDto>? GetRole()
    {
        var roles = _roleRepository.GetAll();
        if (!roles.Any())
        {
            return null;
        }

        var toDto = roles.Select(role => new GetRoleDto
        {
            Guid = role.Guid,
            Name = role.Name
        }).ToList();

        return toDto;
    }

    public GetRoleDto? GetRole(Guid guid)
    {
        var role = _roleRepository.GetByGuid(guid);

        if (role is null)
        {
            return null;
        }

        var toDto = new GetRoleDto
        {
            Guid = role.Guid,
            Name = role.Name
        };

        return toDto;
    }

    public GetRoleDto? CreateRole(NewRoleDto newRoleDto)
    {
        var role = new Role
        {
            Guid = new Guid(),
            Name = newRoleDto.Name,
            CreatedDate = DateTime.Now,
            ModifiedDate = DateTime.Now
        };

        var createdRole = _roleRepository.Create(role);

        if (createdRole == null)
        {
            return null;
        }

        var toDto = new GetRoleDto
        {
            Guid = createdRole.Guid,
            Name = createdRole.Name
        };

        return toDto;
    }

    public int UpdateRole(GetRoleDto getRoleDto)
    {
        var isExist = _roleRepository.IsExist(getRoleDto.Guid);

        if (!isExist)
        {
            return -1;
        }

        var getRole = _roleRepository.GetByGuid(getRoleDto.Guid);
        var role = new Role
        {
            Guid = getRoleDto.Guid,
            Name = getRoleDto.Name,
            ModifiedDate = DateTime.Now,
            CreatedDate = getRole.CreatedDate
        };

        var isUpdate = _roleRepository.Update(role);

        if (!isUpdate)
        {
            return 0;
        }

        return 1;
    }

    public int DeleteRole(Guid guid)
    {
        var isExist = _roleRepository.IsExist(guid);

        if (!isExist)
        {
            return -1;
        }

        var university = _roleRepository.GetByGuid(guid);
        var isDelete = _roleRepository.Delete(university!);
        if (!isDelete)
        {
            return 0;
        }

        return 1;
    }
}
