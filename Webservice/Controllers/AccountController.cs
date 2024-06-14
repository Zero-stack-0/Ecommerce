using System.Reflection.Metadata;
using System.Security.Claims;
using Entities;
using Entities.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Service.Dto;
using Service.Interface;

namespace Webservice.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService accountService;

        public AccountController(IAccountService accountService)
        {
            this.accountService = accountService;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(Service.Dto.LoginRequest dto)
        {
            var data = await accountService.GetUserByLoginCredential(dto);
            ViewData[Constants.VIEW_DATA.LOGIN_MESSAGE] = data.Message;
            if (data.Result is not null)
            {
                var user = (Users)data.Result;
                var Claims = new[]
                {
                    new Claim(ClaimTypes.Name, user.FirstName + "" + user.LastName),
                    new Claim(ClaimTypes.Email , user.EmailId)
                };

                var Identity = new ClaimsIdentity(Claims, CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(Identity));

                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpRequest dto)
        {
            var response = await accountService.Create(dto);

            ViewData["MessageForSignUp"] = response.Message;
            if (response.StatusCodes == StatusCodes.Status200OK)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }
    }
}