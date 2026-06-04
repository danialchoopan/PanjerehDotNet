using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PanjerehDotNet.Domain.Interfaces;
using System.Security.Claims;

namespace PanjerehDotNet.Web.Pages
{
    public class LoginModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;

        public LoginModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string phoneNumber)
        {
            var user = await _unitOfWork.Users.GetByPhoneNumberAsync(phoneNumber);
            if (user == null)
            {
                // Create user for demo if not exists
                user = new PanjerehDotNet.Domain.Entities.User { PhoneNumber = phoneNumber, FullName = "کاربر مهمان" };
                await _unitOfWork.Users.AddAsync(user);
                await _unitOfWork.CompleteAsync();
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.FullName ?? user.PhoneNumber),
                new Claim(ClaimTypes.Role, user.IsAdmin ? "Admin" : "User")
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

            return RedirectToPage("/Index");
        }
    }
}
