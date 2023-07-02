using API.Contracts;
using API.Data;
using API.Models;

namespace API.Repositories;

public class EmployeeRepository : GeneralRepository<Employee>, IEmployeeRepository
{
    public EmployeeRepository(BookingDbContext context) : base(context)
    {
    }
    public Employee? GetByEmailAndPhoneNumber(string data)
    {
        return _context.Set<Employee>().FirstOrDefault(e => e.PhoneNumber == data || e.Email == data);
    }

    public Employee? GetByEmail(string email)
    {
        return _context.Set<Employee>().FirstOrDefault(e => e.Email == email);
    }

    public Employee? GetEmailLogin(string email)
    {
        return _context.Set<Employee>().SingleOrDefault(e => e.Email == email);
    }

    public string? GetLastEmployeeNik()
    {
        return _context.Set<Employee>().ToList().Select(e => e.Nik).LastOrDefault();
    }
}