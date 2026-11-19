using System;
using System.Collections.Generic;

namespace Sis.Models;

public partial class Course
{
    public int CourseId { get; set; }

    public string Code { get; set; } = null!;

    public string Title { get; set; } = null!;

    public int CreditHours { get; set; }

    public int Capacity { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? CreatedAt { get; set; }
}
