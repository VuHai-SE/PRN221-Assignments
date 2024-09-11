using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SalesRepositories;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SalesRazorPageApp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IConfiguration _configuration;
        private readonly IMemberRepository _memberRepository;

        public IndexModel(ILogger<IndexModel> logger, IConfiguration configuration, IMemberRepository memberRepository)
        {
            _logger = logger;
            _configuration = configuration;
            _memberRepository = memberRepository;
        }

        [BindProperty]
        public string Email { get; set; }

        [BindProperty]
        public string Password { get; set; }

        public string ErrorMessage { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost()
        {
            var defaultEmail = _configuration["DefaultAccount:Email"];
            var defaultPassword = _configuration["DefaultAccount:Password"];

            if (Email == defaultEmail && Password == defaultPassword)
            {
                await SignInUser("Admin Company", "Admin", 0); // Admin has MemberId = 0
                return RedirectToPage("/Products/Index");
            }
            else if (IsValidUser(Email, Password))
            {
                var member = _memberRepository.GetMemberByEmail(Email);
                await SignInUser(member.CompanyName, "NormalUser", member.MemberId);
                return RedirectToPage("/Products/Index");
            }
            else
            {
                ErrorMessage = "Invalid login attempt.";
                return Page();
            }
        }

        private bool IsValidUser(string email, string password)
        {
            var member = _memberRepository.GetMemberByEmail(email);
            if (member == null)
                return false;

            var hashedPassword = HashPassword(password);
            return member.Password == hashedPassword;
        }

        private async Task SignInUser(string companyName, string role, int memberId)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, companyName),
                new Claim(ClaimTypes.Role, role),
                new Claim("CompanyName", companyName), // Add CompanyName as a claim
                new Claim("MemberId", memberId.ToString()) // Add MemberId as a claim
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true,
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                var builder = new StringBuilder();
                foreach (var b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}
