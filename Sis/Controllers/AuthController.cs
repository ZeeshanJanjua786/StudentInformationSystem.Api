using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sis.DTOs;
using Sis.Models;

namespace Sis.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly SisDbContext _context;
        private readonly IPasswordHasher<User> _hasher;
        public AuthController(SisDbContext context, IPasswordHasher<User> hasher)
        {
            _context = context;
            _hasher = hasher;

        }
        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser(UserRegisterDto dto)
        {
            if(string.IsNullOrWhiteSpace(dto.Email) || string.IsNullOrWhiteSpace(dto.FullName)|| string.IsNullOrWhiteSpace(dto.Password))
            {
                return BadRequest("All Fields Are Required!");
            }
            if(dto.Password.Length < 8)
            {
                return BadRequest("Password length must be greater then 8 char");
            }

            var normalizeEmail = dto.Email.Trim().ToUpperInvariant();
            var checkDuplicateEmail = await _context.Users.AnyAsync(e => e.Email == normalizeEmail);
            if (checkDuplicateEmail)
            {
                return BadRequest("Email Already Exist");
            }
            var defaultRole = await _context.Roles.FirstOrDefaultAsync(r => r.RoleName == "member");
            if (defaultRole == null)
            {
                return BadRequest("Defualt role configuartion is missing");
            }


            var user = new User
            {
                Email = dto.Email.Trim(),
                NormalizedEmail = normalizeEmail,
                FullName = dto.FullName,
                RoleId = defaultRole.RoleId,
                IsActive = true,
                CreatedAt = DateTime.UtcNow



            };
            user.PasswordHash = _hasher.HashPassword(user, dto.Password);
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            var response = new AuthResponseDto {
                    Email = normalizeEmail,
                    FullName = dto.FullName,
                    Role = defaultRole.RoleName,
                    UserId = user.UserId,
                    Message = "Registration successful."

            };
            return StatusCode(201, response);



        }


        [HttpPost("login")]
        public async Task<IActionResult> LoginUser(LoginRequestDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Email) || string.IsNullOrWhiteSpace(dto.Password))
            {
                return BadRequest("All Fields Are Required!");
            }
            var normalizeEmail = dto.Email.Trim().ToUpperInvariant();
            var user = await _context.Users.Include(u => u.Role).
                FirstOrDefaultAsync(e => e.NormalizedEmail == normalizeEmail);
            if (user == null)
            {
                return BadRequest("Email or Password Wrong");
            }

            if (!user.IsActive)
            {
                return BadRequest("User is not active");
            }

            var verifyPassword = _hasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password);
            if(verifyPassword == PasswordVerificationResult.Failed)
            {
                return BadRequest("Email or Password Wrong");
            }


            HttpContext.Session.SetInt32("UserId", user.UserId);
            HttpContext.Session.SetString("Email", user.Email);
            HttpContext.Session.SetString("Role", user.Role.RoleName);


            return Ok(new AuthResponseDto
            {
                UserId = user.UserId,
                FullName = user.FullName,
                Email = user.Email,
                Role = user.Role.RoleName,
                Message = "Login successful."
            });
        }


        [HttpPut("change-password")]
        public async Task<IActionResult> ChangePassword(UpdatePasswordRequestDto dto)
        {
            if(string.IsNullOrWhiteSpace(dto.NewPassword) || string.IsNullOrWhiteSpace(dto.CurrentPassword))
            {
                return BadRequest("new and curr password are required");

            }
            var user = await _context.Users.FirstAsync(u => u.UserId == dto.UserId);
            if (user == null)
            {
                return NotFound("user not found");
            }

            var verifyPassword = _hasher.VerifyHashedPassword(user, user.PasswordHash, dto.CurrentPassword);
            if(verifyPassword == PasswordVerificationResult.Failed)
            {
                return NotFound("Current password is incorrect");
            }

            if(dto.NewPassword.Length < 8)
            {
                return BadRequest("pasword length would be greater then 8");
            }
            user.PasswordHash = _hasher.HashPassword(user, dto.NewPassword);

            await _context.SaveChangesAsync();
            return Ok("Password updated successfully.");



        }

        [HttpGet("session-profile")]

        public async Task<IActionResult> GetSessionProfile()
        {
            var userid = HttpContext.Session.GetInt32("UserId");
            if (userid == null)
            {
                return NotFound("Please login first.");
            }
            var user = _context.Users.Include(u=>u.Role)
                .FirstOrDefault(u => u.UserId == userid.Value);
            if (user.Role == null)
            {
                return NotFound("User role not found.");
            }
            var response = new AuthResponseDto
            {
                FullName = user.FullName,
                Email = user.Email,
                Role = user.Role.RoleName,
                UserId = user.UserId,
                Message = "Date get Succefully."
            };

            return Ok(response);
        }

    }
}
