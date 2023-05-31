using System;
using System.Collections.Generic;

namespace Courses.API.Models
{
    public partial class Course
    {
        public Course()
        {
            MyLessons = new HashSet<MyLesson>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string TeacherName { get; set; } = null!;
        public bool IsMandatory { get; set; }
        public int LessonsCount { get; set; }

        public virtual ICollection<MyLesson> MyLessons { get; set; }
    }
}
