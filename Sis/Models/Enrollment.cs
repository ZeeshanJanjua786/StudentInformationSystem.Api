using System;
using System.Collections.Generic;

namespace Sis.Models;

public partial class Enrollment
{
    public int EnrollmentId { get; set; }

    public int StudentId { get; set; }

    public int CourseId { get; set; }

    public string Semester { get; set; } = null!;

    public string Status { get; set; } = null!;

    public string? Grade { get; set; }

    public DateTime EnrolledAt { get; set; }
}
