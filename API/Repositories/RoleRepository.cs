using API.Contracts;
using API.Data;
using API.Models;

namespace API.Repositories;

public class RoleRepository : GeneralRepository<Role>, IRoleRepository
{
    public RoleRepository(BookingDbContext context) : base(context)
    {
    }

    public Role? GetByName(string name)
    {
        return _context.Set<Role>().FirstOrDefault(r => r.Name.ToLower() == name.ToLower());
    }

    public ICollection<Role>? GetByRoleGuidAR(Guid guid)
    {
        return _context.Set<Role>().Where(a => a.Guid == guid).ToList();
    }
}
