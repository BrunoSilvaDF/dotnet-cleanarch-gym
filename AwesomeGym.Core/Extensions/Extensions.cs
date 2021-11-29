using AwesomeGym.Core.Dtos.StudentsDtos;
using AwesomeGym.Core.Entities;

namespace AwesomeGym.Core.Extensions;

public static class Extensions
{
  public static StudentDto AsDto(this Student s)
  {
    return new(s.Id, s.Name, s.HealthObservations, s.Status.ToString());
  }

  public static IEnumerable<StudentDto> AsDtos(this IEnumerable<Student> ss)
  {
    return ss.Select(s => s.AsDto());
  }

  public static bool IsEmpty<T>(this IEnumerable<T> data)
  {
    return data != null && !data.Any();
  }
}
