using System;

namespace lr9
{
    public interface IUnitOfWork : IDisposable
    {
        IStudentRepository Student { get; }
        ICourseRepository Course { get; }

        int Complete();
    }
}
