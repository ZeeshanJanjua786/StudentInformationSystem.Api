using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Sis.DTOs;
using Sis.Exceptions;
using Sis.Models;

namespace Sis.Middlewear
{
    public class GlobalExceptionHandling : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandling> _logger;
        public readonly SisDbContext _context;
        public GlobalExceptionHandling(SisDbContext context)
        {
            _context = context;
        }
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            var errDetail = exception switch
            {
                NotFoundException => new ResponseDTO
                {
                    isSuccess = false,
                    code = 404,
                    data = null,
                    message = "The exception's message  "

                },
                ConflictException => new ResponseDTO
                {
                    isSuccess = false,
                    code = 409,
                    data = null,
                    message = "•The new status is the same as the current one  "

                },
                _=> new ResponseDTO
                {
                    isSuccess = false,
                    code = 400,
                    data = null,
                    message = "•The new status is the same as the current one  "

                },

            };
            _logger.LogError(exception, "error found");
            await httpContext.Response.WriteAsJsonAsync(errDetail);
            return true;
           

        }
    }
}
