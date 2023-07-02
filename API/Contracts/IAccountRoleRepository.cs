using API.Models;

namespace API.Contracts;

public interface IAccountRoleRepository : IGeneralRepository<AccountRole>
{
    public IEnumerable<AccountRole>? GetByAccountGuid(Guid guid);
}
