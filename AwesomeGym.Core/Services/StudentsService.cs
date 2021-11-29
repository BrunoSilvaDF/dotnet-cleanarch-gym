using AwesomeGym.Core.Dtos.StudentsDtos;
using AwesomeGym.Core.Entities;
using AwesomeGym.Core.Exceptions;
using AwesomeGym.Core.Extensions;
using AwesomeGym.Core.Interfaces.Repositories;
using AwesomeGym.Core.Interfaces.Services;

namespace AwesomeGym.Core.Services;

public class StudentsService : IStudentsService
{
  private readonly IStudentsRepository _studentsRepository;

  public StudentsService(IStudentsRepository studentRepository)
  {
    this._studentsRepository = studentRepository;
  }

  public async Task<IEnumerable<StudentDto>> GetStudentsAsync()
  {
    return (await _studentsRepository.GetStudentsAsync()).AsDtos();
  }

  public async Task<StudentDto> GetStudentByIdAsync(Guid id)
  {
    var student = await _studentsRepository.GetStudentByIdAsync(id);

    if (student is null)
    {
      throw new StudentNotFoundException();
    }

    return await Task.FromResult(student.AsDto());
  }

  public async Task<StudentDto> AddStudentAsync(AddStudentDto addStudentDto)
  {
    var student = new Student
    {
      Id = Guid.NewGuid(),
      Name = addStudentDto.Name,
      HealthObservations = addStudentDto.HealthObservations,
    };

    await _studentsRepository.AddStudentAsync(student);

    return student.AsDto();
  }

  public async Task UpdateStudentStatusAsync(UpdateStudentStatusDto updateStudentStatusDto)

  {
    var student = await _studentsRepository.GetStudentByIdAsync(updateStudentStatusDto.Id);

    if (student is null)
    {
      throw new StudentNotFoundException();
    }

    student.Status = updateStudentStatusDto.Status;

    await _studentsRepository.UpdateStudentAsync(student);
  }
}
