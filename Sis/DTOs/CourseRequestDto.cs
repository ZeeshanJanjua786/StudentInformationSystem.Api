namespace Sis.DTOs
{
    public class CourseRequestDto
    {
        public string Code { get; set; } = null!;
        public string Title { get; set; } = null!;
        public int CreditHours { get; set; }

        public int Capacity { get; set; }
    }
}
