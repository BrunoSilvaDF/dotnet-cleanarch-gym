using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AwesomeGym.API.Controllers;
using AwesomeGym.Core.Dtos.StudentsDtos;
using AwesomeGym.Core.Exceptions;
using AwesomeGym.Core.Interfaces.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace AwesomeGym.UnitTests;

public class StudentsControllerTest
{
  private readonly Mock<IStudentsService> _serviceStub = new();

  private static StudentsController MakeSut(Mock<IStudentsService> service) =>
    new(service.Object);

  [Fact]
  public async Task GetStudentsAsync_WithServiceThrowing_Returns400()
  {
    // Arrange
    _serviceStub.Setup(serv => serv.GetStudentsAsync()).ThrowsAsync(new Exception("message"));

    var sut = MakeSut(_serviceStub);

    // Act
    var result = await sut.GetStudentsAsync();

    // Assert
    result.Result.Should().BeOfType<BadRequestObjectResult>("message");
  }

  [Fact]
  public async Task GetStudentsAsync_WithUnExistingStudents_Returns204()
  {
    // Arrange
    _serviceStub.Setup(serv => serv.GetStudentsAsync()).ReturnsAsync(new List<StudentDto>());

    var sut = MakeSut(_serviceStub);

    // Act
    var result = await sut.GetStudentsAsync();

    // Assert
    result.Result.Should().BeOfType<NoContentResult>();
  }

  [Fact]
  public async Task GetStudentsAsync_WithExistingStudents_Returns200()
  {
    // Arrange
    var expectedStudents = new[] { CreateRandomStudentDto(), CreateRandomStudentDto(), CreateRandomStudentDto() };

    _serviceStub.Setup(serv => serv.GetStudentsAsync()).ReturnsAsync(expectedStudents);

    var sut = MakeSut(_serviceStub);

    // Act
    var result = await sut.GetStudentsAsync();

    // Assert
    result.Result.Should().BeOfType<OkObjectResult>();
    (result.Result as OkObjectResult).Value.Should().BeEquivalentTo(expectedStudents);
  }

  [Fact]
  public async Task GetStudentAsync_WithUnExistingStudent_Returns400()
  {
    // Arrange
    var ex = new StudentNotFoundException();

    _serviceStub.Setup(serv => serv.GetStudentByIdAsync(It.IsAny<Guid>()))
      .ThrowsAsync(ex);

    var sut = MakeSut(_serviceStub);

    // Act
    var result = await sut.GetStudentAsync(Guid.NewGuid());

    // Assert
    result.Result.Should().BeOfType<BadRequestObjectResult>(ex.Message);
  }

  [Fact]
  public async Task GetStudentAsync_WithExistingStudent_Returns200()
  {
    // Arrange
    var expectedStudent = CreateRandomStudentDto();

    _serviceStub.Setup(serv => serv.GetStudentByIdAsync(It.IsAny<Guid>()))
      .ReturnsAsync(expectedStudent);

    var sut = MakeSut(_serviceStub);

    // Act
    var result = await sut.GetStudentAsync(expectedStudent.Id);

    // Assert
    result.Result.Should().BeOfType<OkObjectResult>();
    (result.Result as OkObjectResult).Value.Should().BeEquivalentTo(expectedStudent);
  }

  [Fact]
  public async Task CreateStudentAsync_WithServiceThrowing_Returns400()
  {
    // Arrange
    _serviceStub.Setup(serv => serv.AddStudentAsync(It.IsAny<AddStudentDto>()))
      .ThrowsAsync(new Exception("message"));

    var sut = MakeSut(_serviceStub);

    // Act
    var result = await sut.CreateStudentAsync(new("", ""));

    // Assert
    result.Result.Should().BeOfType<BadRequestObjectResult>("message");
  }

  [Fact]
  public async Task CreateStudentAsync_WithValidStudent_Returns201()
  {
    // Arrange
    var addStudentDto = new AddStudentDto(Guid.NewGuid().ToString(), Guid.NewGuid().ToString());

    _serviceStub.Setup(serv => serv.AddStudentAsync(It.IsAny<AddStudentDto>()))
      .ReturnsAsync(new StudentDto(Guid.NewGuid(), addStudentDto.Name, addStudentDto.HealthObservations, "Active"));

    var sut = MakeSut(_serviceStub);

    // Act
    var result = await sut.CreateStudentAsync(addStudentDto);

    // Assert
    result.Result.Should().BeOfType<CreatedAtActionResult>();
    (result.Result as CreatedAtActionResult).Value.Should().BeEquivalentTo(result, options =>
      options.ComparingByMembers<AddStudentDto>().ExcludingMissingMembers());
  }

  [Fact]
  public async Task UpdateStudentStatusAsync_WithUnexistingStudent_Returns400()
  {
    // Arrange
    var ex = new StudentNotFoundException();

    _serviceStub.Setup(serv => serv.UpdateStudentStatusAsync(It.IsAny<UpdateStudentStatusDto>()))
      .ThrowsAsync(ex);

    var sut = MakeSut(_serviceStub);

    // Act
    var result = await sut.UpdateStudentStatusAsync(new(Guid.NewGuid(), Core.Enums.StudentStatusEnum.Active));

    // Assert
    result.Should().BeOfType<BadRequestObjectResult>(ex.Message);
  }

  [Fact]
  public async Task UpdateStudentStatusAsync_WithExistingStudent_Returns204()
  {
    // Arrange
    _serviceStub.Setup(serv => serv.UpdateStudentStatusAsync(It.IsAny<UpdateStudentStatusDto>()));

    var sut = MakeSut(_serviceStub);

    // Act
    var result = await sut.UpdateStudentStatusAsync(new(Guid.NewGuid(), Core.Enums.StudentStatusEnum.Active));

    // Assert
    result.Should().BeOfType<NoContentResult>();
  }

  private static StudentDto CreateRandomStudentDto()
  {
    return new StudentDto(
      Guid.NewGuid(),
      Guid.NewGuid().ToString(),
      Guid.NewGuid().ToString(),
      Guid.NewGuid().ToString());
  }
}
