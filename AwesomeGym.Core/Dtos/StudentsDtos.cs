using System.ComponentModel.DataAnnotations;
using AwesomeGym.Core.Enums;

namespace AwesomeGym.Core.Dtos.StudentsDtos;

public record StudentDto(Guid Id, string Name, string HealthObservations, string Status);
public record AddStudentDto([Required] string Name, string HealthObservations);
public record UpdateStudentStatusDto([Required] Guid Id, [Required] StudentStatusEnum Status);
