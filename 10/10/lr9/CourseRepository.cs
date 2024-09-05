using lr9.Data;
using lr9.Models;

namespace lr9
{
    class CourseRepository : Repository<Course>, ICourseRepository
    {
        public CourseRepository(UniverContext context) : base(context)
        {
        }
    }
}
