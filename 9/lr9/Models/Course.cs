using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lr9.Models
{
    public class Course
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int StudentId { get; set; }
        public virtual Student Student { get; set; }
    }
}
