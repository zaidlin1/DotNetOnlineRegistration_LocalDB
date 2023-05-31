using Courses.API.Data;
using Courses.API.Models;
using Microsoft.EntityFrameworkCore;
using SharpCompress.Common;
using System.Text;

namespace Courses.API.Repositories
{
    public class CourseRepository : ICourseRepository
    {
        const int MIN_NOT_MANDATORY_COURSES = 75;

        private readonly CoursesDBContext _context;
        public CourseRepository(CoursesDBContext context)
        {
            _context = context;
        }
        public async Task CreateCourse(Course course)
        {
            await _context.Courses.AddAsync(course);
            _context.SaveChanges();

        }

        public async Task<bool> DeleteCourse(int id)
        {
            var course = await (from c in _context.Courses.Include("MyLessons")
                                select c).Where(p => p.Id == id).FirstOrDefaultAsync();

            if (course == null)
            {
                return false;
            }
            else
            {
                try
                {
                    _context.Courses.Remove(course);
                    _context.SaveChanges();

                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

        public async Task<Course> GetCourse(int id)
        {
            var course = await (from c in _context.Courses.Include("MyLessons")
                                select c).Where(p => p.Id == id).FirstOrDefaultAsync();

            return course;
        }



        public async Task<IEnumerable<Course>> GetCourseByName(string name)
        {

            var courses = from c in _context.Courses.Include("MyLessons")
                          select c;

            if (!String.IsNullOrEmpty(name))
            {
                courses = courses.Where(s => s.Name!.Contains(name));
            }

            return await courses.ToListAsync();

        }
        /// <summary>
        /// Get list of all courses
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Course>> GetCourses()
        {
            var courses = from c in _context.Courses.Include("MyLessons")
                          select c;

            return await courses.ToListAsync();
        }

        public async Task<bool> UpdateCourse(Course course)
        {
            var _course = await (from c in _context.Courses.Include("MyLessons")
                                 select c).Where(p => p.Id == course.Id).FirstOrDefaultAsync();

            if (_course == null)
            {
                return false;
            }

            _context.Entry(_course).CurrentValues.SetValues(course);

            try
            {
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }

        /// <summary>
        /// check if there is intersection in time in taken courses
        /// </summary>
        /// <param name="courses"></param>
        /// <returns></returns>
        public async Task<string> CheckIntersectCourses(IEnumerable<Course> courses)
        {
            var exepCourses = new HashSet<Course>();
            var sb = new StringBuilder();
            foreach (var course in courses)
            {
                exepCourses.Add(course); // take courses except of current
                var courseDays = new HashSet<string>(course.MyLessons.Select(d => d.Day)); // days of current course
                foreach (var ec in courses.Except(exepCourses)) 
                {
                    var anotherCourseDays = new HashSet<string>(ec.MyLessons.Select(d => d.Day));
                    var commonDays = new HashSet<string>(courseDays);
                    commonDays.IntersectWith(anotherCourseDays); // get common days

                    foreach (var day in commonDays) // check time intersection
                    {
                        var courseTime = course.MyLessons.Select(d => d.Time);
                        var anotherCourseTime = ec.MyLessons.Select(d => d.Time);
                        var commonTimes = new HashSet<string>(courseTime.Intersect(anotherCourseTime));
                        if (commonTimes.Count > 0)
                        {
                            sb.Append($"{ec.Name} and {course.Name} have intersection on {day} on ");
                            sb.Append(string.Join(",", commonTimes));
                            sb.Append("\n");

                            return sb.ToString();
                        }
                    }
                }
            }

            return "";


        }

        /// <summary>
        /// check if all madnatory courses are taken and there is quota on non-mandatory courses
        /// </summary>
        /// <param name="courses"></param>
        /// <returns></returns>
        public async Task<bool> CheckCourses(IEnumerable<Course> courses)
        {
            var allCoursesAsync = await GetCourses();
            var allCourses = allCoursesAsync.Where(x => x.IsMandatory).Select(x => x.Id).ToHashSet();
            var checkCourses = courses.Where(x => x.IsMandatory).Select(x => x.Id).ToHashSet();
            var result = allCourses.Except(checkCourses);

            if (result.Count<int>() == 0) // includes all mandatory courses
            {
                // check non mandatory courses quota
                var checkNotMandatoryCourses = courses.Where(x => !x.IsMandatory).Select(x => x.LessonsCount);
                int sum = checkNotMandatoryCourses.Sum();

                return sum >= MIN_NOT_MANDATORY_COURSES;
            }

            return false;
        }
    }
}
