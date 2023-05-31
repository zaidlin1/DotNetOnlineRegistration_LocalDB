
using Courses.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Courses.API.Data
{
    public interface ICoursesDBContext
    {
        DbSet<Course> Courses { get; }
    }
}
