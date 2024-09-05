using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace lr9.Models
{
    public class Student
    {
        public int Id { get; set; }
        public string FirstName { get; set; }       
        public string LastName { get; set; }
        public int Age { get; set; }
        public virtual ICollection<Course> Courses { get; set; }
    }
}
