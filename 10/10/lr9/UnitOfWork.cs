using lr9.Data;

namespace lr9
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly UniverContext _context;
        public UnitOfWork(UniverContext context)
        {
            _context = context;

            Student = new StudentRepository(_context);
            Course = new CourseRepository(_context);
        }

        public IStudentRepository Student { get; private set; }
        public ICourseRepository Course { get; private set; }

        public int Complete()
        {
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
