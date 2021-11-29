using AwesomeGym.Core.Dtos.StudentsDtos;
using AwesomeGym.Core.Entities;
using AwesomeGym.Core.Enums;
using AwesomeGym.Core.Exceptions;
using AwesomeGym.Core.Extensions;
using AwesomeGym.Core.Interfaces.Repositories;
using AwesomeGym.Core.Services;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace AwesomeGym.UnitTests;

public class StudentsServiceTest
{
  private readonly Mock<IStudentsRepository> _repositoryStub = new();

  private static StudentsService MakeSut(Mock<IStudentsRepository> repository) =>
    new(repository.Object);

  [Fact]
  public async Task GetStudentsAsync_WithExistingStudents_ReturnsAllStudents()
  {
    // Arrange
    var expectedStudents = new List<Student>() { CreateRandomStudent(), CreateRandomStudent(), CreateRandomStudent() };

    _repositoryStub.Setup(repo => repo.GetStudentsAsync()).ReturnsAsync(expectedStudents);

    var sut = MakeSut(_repositoryStub);

    // Act
    var actualStudents = await sut.GetStudentsAsync();

    // Assert
    actualStudents.Should().BeEquivalentTo(expectedStudents.AsEnumerable().AsDtos());
  }

  [Fact]
  public async Task GetStudentByIdAsync_WithUnexistingStudent_ThrowsStudentNotFoundException()
  {
    // Arr
    _repositoryStub.Setup(repo => repo.GetStudentByIdAsync(It.IsAny<Guid>())).ReturnsAsync(null as Student);

    var sut = MakeSut(_repositoryStub);

    // Act
    Func<Task> act = () => sut.GetStudentByIdAsync(Guid.NewGuid());

    // Assr
    await act.Should().ThrowAsync<StudentNotFoundException>();
  }

  [Fact]
  public async Task GetStudentByIdAsync_WithExistingStudent_ReturnsStudent()
  {
    // Arr
    var expectedStudent = CreateRandomStudent();
    _repositoryStub.Setup(repo => repo.GetStudentByIdAsync(expectedStudent.Id)).ReturnsAsync(expectedStudent);

    var sut = MakeSut(_repositoryStub);

    // Act
    var student = await sut.GetStudentByIdAsync(expectedStudent.Id);

    // Assr
    student.Should().BeEquivalentTo(expectedStudent.AsDto());
  }

  [Fact]
  public async Task AddStudentAsync_WithStudentToAdd_ReturnStudentAsync()
  {
    // Arrange
    var studentToCreate = new AddStudentDto(Guid.NewGuid().ToString(), Guid.NewGuid().ToString());

    var sut = MakeSut(_repositoryStub);

    // Act
    var createdStudent = await sut.AddStudentAsync(studentToCreate);

    // Assert
    createdStudent.Should()
      .BeEquivalentTo(studentToCreate, opt => opt.ComparingByMembers<Student>().ExcludingMissingMembers());
    createdStudent.Id.Should().NotBeEmpty();
    createdStudent.Status.Should().Be(StudentStatusEnum.Active.ToString());
  }

  [Fact]
  public async Task UpdateStudentStatusAsync_WithUnexistingStudent_ThrowsStudentNotFoundException()
  {
    // Arr
    _repositoryStub.Setup(repo => repo.GetStudentByIdAsync(It.IsAny<Guid>())).ReturnsAsync(null as Student);

    var sut = MakeSut(_repositoryStub);

    // Act
    Func<Task> act = () => sut.UpdateStudentStatusAsync(new(Guid.NewGuid(), StudentStatusEnum.Active));

    // Assr
    await act.Should().ThrowAsync<StudentNotFoundException>();
  }

  [Fact]
  public async Task UpdateStudentStatusAsync_WithExistingStudent_Execute()
  {
    // Arr
    var existingStudent = CreateRandomStudent();

    _repositoryStub.Setup(repo => repo.GetStudentByIdAsync(existingStudent.Id)).ReturnsAsync(existingStudent);

    var sut = MakeSut(_repositoryStub);

    // Act
    Func<Task> act = () => sut.UpdateStudentStatusAsync(new(existingStudent.Id, StudentStatusEnum.Suspended));

    // Assr
    await act.Should().NotThrowAsync<StudentNotFoundException>();
  }

  private static Student CreateRandomStudent()
  {
    return new()
    {
      Id = Guid.NewGuid(),
      Name = Guid.NewGuid().ToString(),
      HealthObservations = Guid.NewGuid().ToString(),
      CreatedAt = DateTime.Now,
      Status = StudentStatusEnum.Active,
    };
  }
}
