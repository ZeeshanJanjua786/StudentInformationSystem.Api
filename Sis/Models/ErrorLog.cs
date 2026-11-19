using System;
using System.Collections.Generic;

namespace Sis.Models;

public partial class ErrorLog
{
    public int ErrorLogId { get; set; }

    public int ErrorCode { get; set; }

    public string ErrorMessage { get; set; } = null!;

    public string ExceptionType { get; set; } = null!;

    public string HttpMethod { get; set; } = null!;

    public string? RequestPath { get; set; }

    public DateTime CreatedAt { get; set; }
}
