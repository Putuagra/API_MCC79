﻿using API.Contracts;
using API.Data;
using API.Models;

namespace API.Repositories;
public class AccountRoleRepository : GeneralRepository<AccountRole>, IAccountRoleRepository
{
    public AccountRoleRepository(BookingDbContext context) : base(context)
    {
    }
    public IEnumerable<AccountRole>? GetByAccountGuid(Guid guid)
    {
        return _context.Set<AccountRole>().Where(ar => ar.AccountGuid == guid);
    }
}
