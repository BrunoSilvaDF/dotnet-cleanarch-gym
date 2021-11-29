using AwesomeGym.Core.Dtos.StudentsDtos;

namespace AwesomeGym.Core.Interfaces.Usecases;

public interface IUpdateStudentStatusAsync
{
  Task UpdateStudentStatusAsync(UpdateStudentStatusDto updateStudentStatusDto);
}
