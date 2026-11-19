using FluentValidation;
using FluentValidation.Validators;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using Sis.DTOs;
using Sis.Models;
using System.ComponentModel.DataAnnotations;
using System.Net.NetworkInformation;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Sis.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CoursesController : Controller
    {
        private readonly IValidator<CourseRequestDto> _validator;
        private readonly IValidator<updateRequestDto> _validatorUpdate;
        public readonly SisDbContext _context;
        public CoursesController(IValidator<CourseRequestDto> validator, SisDbContext context, IValidator<updateRequestDto> validatorUpdate)
        {
            _validator = validator;
            _context = context;
            _validatorUpdate = validatorUpdate;

        }
        [HttpPost("AddCourse")]
        public async Task<ActionResult> AddCourse(CourseRequestDto dto)
        {
            var validator = _validator.Validate(dto);
            var course = _context.Courses.Select(c => c.Code == dto.Code).ToList();
            if (!validator.IsValid)
            {
                var response = new ResponseDTO
                {
                    isSuccess = false,
                    code = 400,
                    data = validator.Errors,
                    message = null

                };
                return BadRequest(response);
            }
            if (!(course == null))
            {
                var response = new ResponseDTO
                {
                    isSuccess = false,
                    code = StatusCodes.Status409Conflict,
                    data = validator.Errors,
                    message = null

                };
                return Conflict(response);
            }
            var courseAdd = new Course
            {
                Code = dto.Code,
                Title = dto.Title,
                CreditHours = dto.CreditHours,
                Capacity = dto.Capacity
            };

            _context.Courses.Add(courseAdd);
            await _context.SaveChangesAsync();

            return Created();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            var course = await _context.Courses.FindAsync(id);//wrong linq method
            // 
            if (course == null)
            {
                var responsee = new ResponseDTO
                {
                    isSuccess = false,
                    code = 404,
                    data = null,
                    message = "NotFouund"

                };
                return NotFound(responsee);
            }
            var response = new ResponseDTO
            {
                isSuccess = true,
                code = 200,
                data = course,
                message = "Data Found"

            };
            return Ok(response);


        }


        [HttpGet("get-course-by-page")]
        public async Task<IActionResult> GetCourses(string search = "", int page = 1, int pageSize = 5, bool isactive = true)
        {
           
            var courses = _context.Courses.ToList(); ;
            if (isactive == false)
            {

                courses = _context.Courses.Where(c => c.IsActive == isactive).ToList();
            }
            var searchRequest = search.Trim().ToLower();
            if (!string.IsNullOrWhiteSpace(search)) {
                
                courses = _context.Courses.Where(c => c.Code.Contains(searchRequest)).ToList();
               
            }


                //_context.Courses.
                //Select(p => p.Code.Contains(searchRequest)).ToList();
            

            if (pageSize < 1 || pageSize > 20)
            {
                var response = new ResponseDTO
                {
                    isSuccess = false,
                    code = 400,
                    data = null,
                    message = "page size must be withing 1 to 20"

                };
                return BadRequest(response);
            }
            var totalRecords = courses.Count();
            var totalPages = (totalRecords / pageSize);
            var result = courses
                .Skip((page - 1) * pageSize)
                .Take(pageSize).ToList();


            var data = new SearchResponseDto
            {
                page = page,
                pageSize = pageSize,
                totalCount = totalRecords,
                totalPages = totalPages,
                items = result

            };
            return Ok(data);
        }

        [HttpPut("courseput/{id}")]

        public async Task<IActionResult> UpdateCourse(int id, updateRequestDto udto)
        {
            var validator = _validatorUpdate.Validate(udto);
            if (!validator.IsValid)
            {
                var response = new ResponseDTO
                {
                    isSuccess = false,
                    code = 400,
                    data = validator.Errors,
                    message = null

                };
                return BadRequest(response);
            }
            var course = _context.Courses.FirstOrDefault(c=>c.CourseId == id);
            if (course == null)
            {
                var response = new ResponseDTO
                {
                    isSuccess = false,
                    code = 404,
                    data = null,
                    message = "NOtFound"

                };
                return NotFound(response);
            }
            if(course.IsActive == false)
            {
                var response = new ResponseDTO
                {
                    isSuccess = false,
                    code = 404,
                    data = null,
                    message = "course is not active"

                };
                return NotFound(response);

            }

            course.Title = udto.Title;
            course.CreditHours = udto.CreditHours;
            course.Capacity = udto.Capacity;
            await _context.SaveChangesAsync();
            return Ok(course.Title);

        }

        [HttpPost("student")]

        public async Task<IActionResult> AddStudent(AddStudentDto dto)
        {

            var emailValidation = dto.Email.EndsWith("@student.sis.edu");
            if (!emailValidation)
            {
                return Conflict();
            }
            var emailExist = _context.Students.Any(e => e.Email == dto.Email);
            if (emailExist) { return Conflict(); }/////////////
            int curYear = DateTime.UtcNow.Year;
            var regNO = $"SIS-{curYear}";


            var StudentAdd = new Student
            {
                RegNo = regNO,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                DateOfBirth = dto.DateOfBirth


            };

            _context.Students.Add(StudentAdd);
            await _context.SaveChangesAsync();

            return Created();
        }
        [HttpGet("student/{id}")]
        public async Task<IActionResult> GetStudentById(int id)
        {
            var Student = _context.Students.FirstOrDefault(c=>c.StudentId == id);

            if (Student == null)
            {
                var responsee = new ResponseDTO
                {
                    isSuccess = false,
                    code = 404,
                    data = null,
                    message = "NotFouund"

                };
                return NotFound(responsee);
            }
            var response = new ResponseDTO
            {
                isSuccess = true,
                code = 200,
                data = Student,
                message = "Data Found"

            };
            return Ok(response);


        }


        [HttpPatch("patchStudent/{id}")]

        public async Task<IActionResult> updateStudentStatus(int id, string Status)
        {
            var Student = await _context.Students.FindAsync(id);

            if (Student == null)
            {
                var responsee = new ResponseDTO
                {
                    isSuccess = false,
                    code = 404,
                    data = null,
                    message = "NotFouund"

                };
                return NotFound(responsee);
            }
            var statusList = new List<string> { "active", "suspended", "graduated" };

            if (!(statusList.Contains(Status.ToLower())))
            {
                var responsee = new ResponseDTO
                {
                    isSuccess = false,
                    code = 400,
                    data = null,
                    message = "Status value not one of the three "

                };
                return NotFound(responsee);
            }
            if (Status.ToLower() == "graduated")
            {
                var responsee = new ResponseDTO
                {
                    isSuccess = false,
                    code = 400,
                    data = null,
                    message = "•A Graduated student's status can never change "

                };
                return NotFound(responsee);

            }


            if (Student.Status.ToLower() == Status.ToLower())
            {
                var responsee = new ResponseDTO
                {
                    isSuccess = false,
                    code = 400,
                    data = null,
                    message = "•The new status is the same as the current one  "

                };
                return NotFound(responsee);
            }
            Student.Status = Status;
            await _context.SaveChangesAsync();

            return Ok(Student);
        }


        [HttpPost("enrollment")]

        public async Task<IActionResult> updateStudentStatus(EnrollmentRequestDto dto)
        {
            var checkStudent = _context.Students.Find(dto.StudentId);
            if (checkStudent == null)
            {
                var responsee = new ResponseDTO
                {
                    isSuccess = false,
                    code = 404,
                    data = null,
                    message = "•Student  not found  "

                };
                return NotFound(responsee);
            }
            var checkCourse = _context.Courses.Find(dto.CourseId);
            if (checkCourse == null)
            {
                var responsee = new ResponseDTO
                {
                    isSuccess = false,
                    code = 404,
                    data = null,
                    message = "•Course not found  "

                };
                return NotFound(responsee);
            }
            var semester = new List<string>() { "fall", "spring", "summer " };

            if (!(semester.Contains(dto.Semester.ToLower())))
            {
                var responsee = new ResponseDTO
                {
                    isSuccess = false,
                    code = 400,
                    data = null,
                    message = "Status value not one of the three "

                };
                return NotFound(responsee);
            }


            var enrollmentsAdd = new EnrollmentRequestDto
            {
                CourseId = dto.CourseId,
                Semester = dto.Semester,
                StudentId = dto.StudentId

            };


            await _context.SaveChangesAsync();

            return Created();

        }
    }
}
