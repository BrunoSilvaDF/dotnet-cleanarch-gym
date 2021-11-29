using AwesomeGym.Core.Interfaces.Usecases;

namespace AwesomeGym.Core.Interfaces.Services;

public interface IStudentsService : IAddStudentAsync, IUpdateStudentStatusAsync, IGetStudentsAsync, IGetStudentByIdAsync
{
}
