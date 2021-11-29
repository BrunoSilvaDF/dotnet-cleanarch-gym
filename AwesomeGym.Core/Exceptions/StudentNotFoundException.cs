namespace AwesomeGym.Core.Exceptions
{
    public class StudentNotFoundException : Exception
    {
        private const string StudentNotFoundMessage = "student not found";
        public StudentNotFoundException() : base(StudentNotFoundMessage)
        {
        }
    }
}
