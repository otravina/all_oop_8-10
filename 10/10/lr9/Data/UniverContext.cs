using lr9.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace lr9.Data
{
    public class UniverContext : DbContext
    {
        public UniverContext(DbContextOptions<UniverContext> options) : base(options)
        {
        }

        public DbSet<Student> Students { get; set; }
        public DbSet<Course> Courses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Course>()
               .HasOne(c => c.Student)
               .WithMany(s => s.Courses)
               .HasForeignKey(c => c.StudentId);
        }
    }
}
