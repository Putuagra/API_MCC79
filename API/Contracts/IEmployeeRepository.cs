using API.Models;

namespace API.Contracts;

public interface IEmployeeRepository : IGeneralRepository<Employee>
{
    public Employee? GetByEmailAndPhoneNumber(string nama);
    public Employee? GetByEmail(string email);
}
