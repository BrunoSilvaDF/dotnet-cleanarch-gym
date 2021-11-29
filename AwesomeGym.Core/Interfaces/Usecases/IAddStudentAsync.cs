using AwesomeGym.Core.Dtos.StudentsDtos;

namespace AwesomeGym.Core.Interfaces.Usecases;

public interface IAddStudentAsync
{
  Task<StudentDto> AddStudentAsync(AddStudentDto addStudentDto);
}
