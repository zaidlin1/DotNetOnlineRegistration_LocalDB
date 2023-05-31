using Courses.API.Models;
using Courses.API.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Courses.API.Controllers
{
    [ApiController]
    [Route("api/v1/[Controller]")]
    public class CoursesController : ControllerBase
    {
        private readonly ICourseRepository _repository;
        private readonly ILogger<CoursesController> _logger;

        public CoursesController(ICourseRepository repository, ILogger<CoursesController> logger
            )
        {
            _repository = repository;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Course>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<Course>>> GetCourses()
        {
            var courses = await _repository.GetCourses();
            return Ok(courses);
        }

        [HttpGet("{id}", Name = "GetCourse")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(Course), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Course>> GetCourseById(int id)
        {
            var course = await _repository.GetCourse(id);
            if (course == null)
            {
                _logger.LogError($"Course with id: {id}, not found.");
                return NotFound();
            }
            return Ok(course);
        }

        [HttpGet("GetCourseByName/{name}", Name = "GetCourseByName")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(Course), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Course>> GetCourseByName(string name)
        {
            var course = await _repository.GetCourseByName(name);
            if (course == null)
            {
                _logger.LogError($"Course with name: {name}, not found.");
                return NotFound();
            }
            return Ok(course);
        }


        [HttpPost("CheckCourses")]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<bool>> CheckCourses([FromBody] IEnumerable<Course> courses)
        {
            var isCompleted = await _repository.CheckCourses(courses);

            return Ok(isCompleted);
        }

        [HttpPost("CheckIntersectCourses")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<string>> CheckIntersectCourses([FromBody] IEnumerable<Course> courses)
        {
            var getMessage = await _repository.CheckIntersectCourses(courses);

            return Ok(getMessage);
        }

        [HttpPost]
        [ProducesResponseType(typeof(Course), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Course>> CreateCourse([FromBody] Course course)
        {
            await _repository.CreateCourse(course);

            return CreatedAtRoute("GetCourse", new { id = course.Id }, course);
        }

        [HttpPut]
        [ProducesResponseType(typeof(Course), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateCourse([FromBody] Course course)
        {
            return Ok(await _repository.UpdateCourse(course));
        }

        [HttpDelete("{id:length(24)}", Name = "DeleteCourse")]
        [ProducesResponseType(typeof(Course), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteCourseById(int id)
        {
            return Ok(await _repository.DeleteCourse(id));
        }
    }
}
