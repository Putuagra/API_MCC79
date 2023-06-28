using API.Contracts;
using API.DTOs.Educations;
using API.Models;

namespace API.Services;

public class EducationService
{
    private readonly IEducationRepository _educationRepository;
    public EducationService(IEducationRepository educationRepository)
    {
        _educationRepository = educationRepository;
    }

    public IEnumerable<EducationDto>? GetEducation()
    {
        var educations = _educationRepository.GetAll();
        if (!educations.Any())
        {
            return null;
        }

        var toDto = educations.Select(education => new EducationDto
        {
            Guid = education.Guid,
            Major = education.Major,
            Degree = education.Degree,
            Gpa = education.Gpa,
            UniversityGuid = education.UniversityGuid
        }).ToList();

        return toDto;
    }

    public EducationDto? GetEducation(Guid guid)
    {
        var education = _educationRepository.GetByGuid(guid);

        if (education is null)
        {
            return null;
        }

        var toDto = new EducationDto
        {
            Guid = education.Guid,
            Major = education.Major,
            Degree = education.Degree,
            Gpa = education.Gpa,
            UniversityGuid = education.UniversityGuid
        };

        return toDto;
    }

    public EducationDto? CreateEducation(EducationDto educationDto)
    {
        var education = new Education
        {
            Guid = educationDto.Guid,
            Major = educationDto.Major,
            Degree = educationDto.Degree,
            Gpa = educationDto.Gpa,
            UniversityGuid = educationDto.UniversityGuid,
            CreatedDate = DateTime.Now,
            ModifiedDate = DateTime.Now
        };

        var createdEducation = _educationRepository.Create(education);

        if (createdEducation == null)
        {
            return null;
        }

        var toDto = new EducationDto
        {
            Guid = createdEducation.Guid,
            Major = createdEducation.Major,
            Degree = createdEducation.Degree,
            Gpa = createdEducation.Gpa,
            UniversityGuid = createdEducation.UniversityGuid
        };

        return toDto;
    }

    public int UpdateEducation(EducationDto educationDto)
    {
        var isExist = _educationRepository.IsExist(educationDto.Guid);

        if (!isExist)
        {
            return -1;
        }

        var getEducation = _educationRepository.GetByGuid(educationDto.Guid);
        var education = new Education
        {
            Guid = educationDto.Guid,
            Major = educationDto.Major,
            Degree = educationDto.Degree,
            Gpa = educationDto.Gpa,
            UniversityGuid = educationDto.UniversityGuid,
            ModifiedDate = DateTime.Now,
            CreatedDate = getEducation!.CreatedDate
        };

        var isUpdate = _educationRepository.Update(education);

        if (!isUpdate)
        {
            return 0;
        }

        return 1;
    }

    public int DeleteEducation(Guid guid)
    {
        var isExist = _educationRepository.IsExist(guid);

        if (!isExist)
        {
            return -1;
        }

        var education = _educationRepository.GetByGuid(guid);
        var isDelete = _educationRepository.Delete(education!);
        if (!isDelete)
        {
            return 0;
        }

        return 1;
    }
}
