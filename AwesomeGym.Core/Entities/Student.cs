using AwesomeGym.Core.Enums;

namespace AwesomeGym.Core.Entities;

public class Student
{
  public Guid Id { get; set; }
  public string Name { get; set; } = default!;
  public string HealthObservations { get; set; } = null!;
  public StudentStatusEnum Status { get; set; } = StudentStatusEnum.Active;
  public DateTime CreatedAt { get; set; } = DateTime.Now;
}
