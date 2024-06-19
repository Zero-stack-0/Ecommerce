using System.Net.Sockets;
using System.Security.Claims;
using Entities;
using Entities.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Dto;
using Service.Interface;
using Webservice.Helper;

namespace Webservice.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService accountService;
        private readonly CookieUserDetailsHandler cookieUserDetailsHandler;

        public AccountController(IAccountService accountService, CookieUserDetailsHandler cookieUserDetailsHandler)
        {
            this.accountService = accountService;
            this.cookieUserDetailsHandler = cookieUserDetailsHandler;
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
                    new Claim(ClaimTypes.Email , user.EmailId),
                    new Claim(ClaimTypes.Role, user.Role.Id.ToString())
                };

                var Identity = new ClaimsIdentity(Claims, CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(Identity));

                return RedirectToAction("Index", "Home", user);
            }
            return View();
        }

        public async Task<IActionResult> SignUp()
        {
            var country = await GetStatesAndCity(1);
            ViewData["Country"] = country;
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

        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        public async Task<IActionResult> Profile()
        {
            var user = await cookieUserDetailsHandler.GetUserDetail(this.User.Identity as ClaimsIdentity);
            if (user is null)
            {
                return RedirectToAction("Login");
            }
            return View(user);
        }

        [HttpGet]
        public async Task<Country?> GetStatesAndCity(long id)
        {
            return await accountService.GetCountry(id);
        }

        [HttpGet]
        public async Task<JsonResult> GetStates(long countryId)
        {
            var country = await accountService.GetCountry(countryId);
            return Json(country.State);
        }

        [HttpGet]
        public async Task<JsonResult> GetCities(long stateId, long countryId)
        {
            var country = await accountService.GetCountry(countryId);
            var cities = country.State.FirstOrDefault(s => s.Id == stateId).City;
            return Json(cities);
        }
    }
}