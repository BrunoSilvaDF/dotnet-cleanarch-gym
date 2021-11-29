using AwesomeGym.Core.Dtos.StudentsDtos;

namespace AwesomeGym.Core.Interfaces.Usecases;

public interface IGetStudentByIdAsync
{
  Task<StudentDto> GetStudentByIdAsync(Guid id);
}
