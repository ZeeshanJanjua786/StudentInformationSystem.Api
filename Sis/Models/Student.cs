using System;
using System.Collections.Generic;

namespace Sis.Models;

public partial class Student
{
    public int StudentId { get; set; }

    public string RegNo { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public DateOnly DateOfBirth { get; set; }

    public string Status { get; set; } = null!;

    public DateTime CreatedAt { get; set; }
}
