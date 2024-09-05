using lr9.Data;
using lr9.Models;

namespace lr9
{
    public class StudentRepository : Repository<Student>, IStudentRepository
    {
        public StudentRepository(UniverContext context) : base(context)
        {
        }
    }
}
