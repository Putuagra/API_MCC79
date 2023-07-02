using API.Models;

namespace API.Contracts;

public interface IRoleRepository : IGeneralRepository<Role>
{
    public Role? GetByName(string role);
}
