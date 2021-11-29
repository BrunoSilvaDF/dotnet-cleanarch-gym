using AwesomeGym.Core.Entities;
using AwesomeGym.Core.Interfaces.Repositories;

namespace AwesomeGym.Infra.Persistence.Repositories;

public class InMemStudentsRepository : IStudentsRepository
{
  private List<Student> Students { get; set; } = new();

  public async Task AddStudentAsync(Student student)
  {
    Students.Add(student);
    await Task.CompletedTask;
  }

  public async Task DeleteStudentAsync(Guid id)
  {
    var student = await GetStudentByIdAsync(id);

    Students.Remove(student);
  }

  public Task<Student> GetStudentByIdAsync(Guid id)
  {
    var student = Students.Find(x => x.Id == id);

    if (student is null)
    {
      throw new ArgumentException("student not found", nameof(id));
    }

    return Task.FromResult(student);
  }

  public async Task<IEnumerable<Student>> GetStudentsAsync()
  {
    return await Task.FromResult(Students);
  }

  public async Task UpdateStudentAsync(Student studentToUpdate)
  {
    var student = await GetStudentByIdAsync(studentToUpdate.Id);

    student.Name = studentToUpdate.Name;
    student.HealthObservations = studentToUpdate.HealthObservations;
    student.Status = studentToUpdate.Status;
  }
}
