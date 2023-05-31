using Courses.API.Models;

namespace Courses.API.Repositories
{
    public interface ICourseRepository
    {
        Task<IEnumerable<Course>> GetCourses();
        Task<Course> GetCourse(int id);
        Task<IEnumerable<Course>> GetCourseByName(string name);

        Task CreateCourse(Course course);
        Task<bool> UpdateCourse(Course course);
        Task<bool> DeleteCourse(int id);

        Task<bool> CheckCourses(IEnumerable<Course> courses);

        Task<string> CheckIntersectCourses(IEnumerable<Course> courses);
    }
}
