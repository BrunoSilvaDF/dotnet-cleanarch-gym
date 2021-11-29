using AwesomeGym.Core.Dtos.StudentsDtos;
using AwesomeGym.Core.Extensions;
using AwesomeGym.Core.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace AwesomeGym.API.Controllers;

[ApiController]
[Route("[controller]")]
public class StudentsController : ControllerBase
{
  private readonly IStudentsService _studentsService;

  public StudentsController(IStudentsService studentService)
  {
    this._studentsService = studentService;
  }

  [HttpGet]
  public async Task<ActionResult<IEnumerable<StudentDto>>> GetStudentsAsync()
  {
    try
    {
      var students = await _studentsService.GetStudentsAsync();

      if (students != null && students.IsEmpty())
        return NoContent();

      return Ok(students);
    }
    catch (Exception ex)
    {
      return BadRequest(ex.Message);
    }
  }

  [HttpGet("{id}")]
  public async Task<ActionResult<StudentDto>> GetStudentAsync(Guid id)
  {
    try
    {
      var student = await _studentsService.GetStudentByIdAsync(id);
      return Ok(student);
    }
    catch (Exception ex)
    {
      return BadRequest(ex.Message);
    }
  }

  [HttpPost]
  public async Task<ActionResult<StudentDto>> CreateStudentAsync(AddStudentDto studentDto)
  {
    try
    {
      var student = await _studentsService.AddStudentAsync(studentDto);

      return CreatedAtAction(nameof(GetStudentAsync), new { id = student.Id }, student);
    }
    catch (Exception ex)
    {
      return BadRequest(ex.Message);
    }
  }

  [HttpPut]
  public async Task<ActionResult> UpdateStudentStatusAsync(UpdateStudentStatusDto studentStatusDto)
  {
    try
    {
      await _studentsService.UpdateStudentStatusAsync(studentStatusDto);
      return NoContent();
    }
    catch (Exception ex)
    {
      return BadRequest(ex.Message);
    }
  }
}
