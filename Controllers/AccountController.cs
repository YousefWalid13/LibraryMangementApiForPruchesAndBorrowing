using LibraryManagementAPI.Dto.Account.ChangePassword;
using LibraryManagementAPI.Dto.Account.ForgetPassword;
using LibraryManagementAPI.Dto.Account.Login;
using LibraryManagementAPI.Dto.Account.Register;
using LibraryManagementAPI.Dto.Account.Reset_Password;
using LibraryManagementAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace LibraryManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;
        public AccountController(IConfiguration configuration,UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }

        // ✅ Register
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var user = new ApplicationUser
            {
                UserName = model.UserName,
                Email = model.Email,
                FullName = model.FullName
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
                return BadRequest(new RegisterResponseDto
                {
                    Success = false,
                    Errors = result.Errors.Select(e => e.Description).ToList()
                });

            return Ok(new RegisterResponseDto
            {
                Success = true,
                Message = "User registered successfully"
            });
        }

        [Authorize]
        // ✅ Login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // ✅ البحث عن المستخدم بالايميل
            var userFromDb = await _userManager.FindByEmailAsync(model.Email);
            if (userFromDb == null)
            {
                ModelState.AddModelError("Email", "Invalid email or password");
                return Unauthorized(new LoginResponseDto
                {
                    Success = false,
                    Message = "Invalid email or password"
                });
            }

            // ✅ التحقق من كلمة السر
            var found = await _userManager.CheckPasswordAsync(userFromDb, model.Password);
            if (!found)
            {
                ModelState.AddModelError("Password", "Invalid email or password");
                return Unauthorized(new LoginResponseDto
                {
                    Success = false,
                    Message = "Invalid email or password"
                });
            }

            // ✅ Claims
            var userClaims = new List<Claim>
    {
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        new Claim(ClaimTypes.NameIdentifier, userFromDb.Id),
        new Claim(ClaimTypes.Name, userFromDb.UserName ?? ""),
        new Claim(ClaimTypes.Email, userFromDb.Email ?? "")
    };

            var userRoles = await _userManager.GetRolesAsync(userFromDb);
            foreach (var role in userRoles)
            {
                userClaims.Add(new Claim(ClaimTypes.Role, role));
            }

            // ✅ مفتاح التوقيع
            var signInKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _configuration["Jwt:Key"]));

            var signingCred = new SigningCredentials(signInKey, SecurityAlgorithms.HmacSha256);

            // ✅ بناء الـ Token
            var myToken = new JwtSecurityToken(
                audience: _configuration["Jwt:Audience"],
                issuer: _configuration["Jwt:Issuer"],
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(_configuration["Jwt:ExpireMinutes"])),
                claims: userClaims,
                signingCredentials: signingCred
            );

            // ✅ إخراج التوكن
            return Ok(new LoginResponseDto
            {
                Success = true,
                Message = "Login successful",
                Token = new JwtSecurityTokenHandler().WriteToken(myToken),
                Expiration = myToken.ValidTo
            });
        }

        [Authorize]
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequestDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // ✅ Get Current Logged-in User
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return Unauthorized(new { Message = "User not found" });

            if (model.NewPassword != model.ConfirmNewPassword)
                return BadRequest(new { Message = "Passwords do not match" });

            // ✅ Change password
            var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);

            if (!result.Succeeded)
            {
                return BadRequest(new
                {
                    Success = false,
                    Errors = result.Errors.Select(e => e.Description)
                });
            }

            return Ok(new
            {
                Success = true,
                Message = "Password changed successfully"
            });
        }
        // ✅ Forgot Password (Generate Reset Token)
        [Authorize]

        [HttpPost("forgot-password")]

        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequestDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
                return NotFound(new ForgotPasswordResponseDto
                {
                    Success = false,
                    Message = "User not found."
                });

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            // ⚠️ للتجربة فقط - في الواقع لازم تبعته عبر Email Service
            return Ok(new ForgotPasswordResponseDto
            {
                Success = true,
                ResetToken = token
            });
        }
        [Authorize]

        // ✅ Reset Password
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequestDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
                return NotFound(new ResetPasswordResponseDto
                {
                    Success = false,
                    Message = "User not found."
                });

            var result = await _userManager.ResetPasswordAsync(user, model.Token, model.NewPassword);

            if (!result.Succeeded)
                return BadRequest(new ResetPasswordResponseDto
                {
                    Success = false,
                    Errors = result.Errors.Select(e => e.Description).ToList()
                });

            return Ok(new ResetPasswordResponseDto
            {
                Success = true,
                Message = "Password reset successful"
            });
        }
    }
}
