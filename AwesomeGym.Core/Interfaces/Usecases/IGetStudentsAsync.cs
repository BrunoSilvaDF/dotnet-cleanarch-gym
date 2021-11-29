
using AwesomeGym.Core.Dtos.StudentsDtos;

namespace AwesomeGym.Core.Interfaces.Usecases;

public interface IGetStudentsAsync
{
  Task<IEnumerable<StudentDto>> GetStudentsAsync();
}
