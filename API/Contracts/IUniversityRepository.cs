using API.Models;

namespace API.Contracts;

public interface IUniversityRepository : IGeneralRepository<University>
{
    public IEnumerable<University> GetByName(string name);

    public University? GetByCodeAndName(string code, string name);
}
