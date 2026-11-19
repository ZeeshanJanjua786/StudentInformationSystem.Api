using System.ComponentModel.DataAnnotations;

namespace Sis.DTOs
{
    public class AddStudentDto
    {
        [Length(2,50, ErrorMessage = "must be withing 2 to 50")]
        public string FirstName { get; set; } = null!;
        [Length(2, 50, ErrorMessage = "must be withing 2 to 50")]
        public string LastName { get; set; } = null!;


      
        public string Email { get; set; } = null!;

        public DateOnly DateOfBirth { get; set; }
    }
}
