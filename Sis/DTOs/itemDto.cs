namespace Sis.DTOs
{
    public class itemDto
    {
        public int CourseId { get; set; }

        public string Code { get; set; } = null!;

        public string DisplayName { get; set; } = null!;

        public int CreditHours { get; set; }

        public int Capacity { get; set; }

        public bool? IsActive { get; set; }

    }
}
