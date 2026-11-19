using System;
using System.Collections.Generic;

namespace Sis.Models;

public partial class UserRegister
{
    public string FullName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;
}
