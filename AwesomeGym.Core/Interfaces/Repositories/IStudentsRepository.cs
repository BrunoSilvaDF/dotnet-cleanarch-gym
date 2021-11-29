using AwesomeGym.Core.Entities;

namespace AwesomeGym.Core.Interfaces.Repositories;

public interface IStudentsRepository
{
  Task AddStudentAsync(Student student);
  Task UpdateStudentAsync(Student student);
  Task DeleteStudentAsync(Guid id);
  Task<Student> GetStudentByIdAsync(Guid id);
  Task<IEnumerable<Student>> GetStudentsAsync();
}
