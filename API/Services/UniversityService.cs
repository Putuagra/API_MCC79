using API.Contracts;
using API.DTOs.Universities;
using API.Models;

namespace API.Services;

public class UniversityService
{
    private readonly IUniversityRepository _universityRepository;
    public UniversityService(IUniversityRepository universityRepository)
    {
        _universityRepository = universityRepository;
    }

    public IEnumerable<GetUniversityDto>? GetUniversity()
    {
        var universities = _universityRepository.GetAll();
        if (!universities.Any())
        {
            return null; // No universities found
        }

        var toDto = universities.Select(university => new GetUniversityDto
        {
            Guid = university.Guid,
            Code = university.Code,
            Name = university.Name
        }).ToList();

        return toDto; // Universities found
    }

    public GetUniversityDto? GetUniversity(Guid guid)
    {
        var university = _universityRepository.GetByGuid(guid);

        if (university is null)
        {
            return null; // No universities found
        }

        var toDto = new GetUniversityDto
        {
            Guid = university.Guid,
            Code = university.Code,
            Name = university.Name
        };

        return toDto;
    }

    public IEnumerable<GetUniversityDto> GetUniversity(string name)
    {
        var universities = _universityRepository.GetByName(name);

        if (!universities.Any())
        {
            return null; // University not found
        }

        var toDto = universities.Select(university => new GetUniversityDto
        {
            Guid = university.Guid,
            Code = university.Code,
            Name = university.Name
        });
        return toDto; // Universities found
    }

    public GetUniversityDto? CreateUniversity(NewUniversityDto newUniversityDto)
    {
        var university = new University
        {
            Guid = new Guid(),
            Code = newUniversityDto.Code,
            Name = newUniversityDto.Name,
            CreatedDate = DateTime.Now,
            ModifiedDate = DateTime.Now
        };

        var createdUniversity = _universityRepository.Create(university);

        if (createdUniversity == null)
        {
            return null; // University not created
        }

        var toDto = new GetUniversityDto
        {
            Guid = createdUniversity.Guid,
            Code = createdUniversity.Code,
            Name = createdUniversity.Name
        };

        return toDto; // University created
    }

    public int UpdateUniversity(UpdateUniversityDto updateUniversityDto)
    {
        var isExist = _universityRepository.IsExist(updateUniversityDto.Guid);

        if (!isExist)
        {
            return -1; // University not found
        }

        var getUniversity = _universityRepository.GetByGuid(updateUniversityDto.Guid);
        var university = new University
        {
            Guid = updateUniversityDto.Guid,
            Code = updateUniversityDto.Code,
            Name = updateUniversityDto.Name,
            ModifiedDate = DateTime.Now,
            CreatedDate = getUniversity!.CreatedDate
        };

        var isUpdate = _universityRepository.Update(university);

        if (!isUpdate)
        {
            return 0; // University not updated
        }

        return 1; // University updated
    }

    public int DeleteUniversity(Guid guid)
    {
        var isExist = _universityRepository.IsExist(guid);

        if (!isExist)
        {
            return -1;
        }

        var university = _universityRepository.GetByGuid(guid);
        var isDelete = _universityRepository.Delete(university!);
        if (!isDelete)
        {
            return 0;
        }

        return 1;
    }
}
