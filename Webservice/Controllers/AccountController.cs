using System.Security.Claims;
using Entities;
using Entities.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service;
using Service.Dto;
using Service.Dto.Response;
using Service.Interface;
using Webservice.Helper;

namespace Webservice.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService accountService;
        private readonly CookieUserDetailsHandler cookieUserDetailsHandler;
        private readonly IWebHostEnvironment environment;

        public AccountController(IAccountService accountService, CookieUserDetailsHandler cookieUserDetailsHandler, IWebHostEnvironment environment)
        {
            this.accountService = accountService;
            this.cookieUserDetailsHandler = cookieUserDetailsHandler;
            this.environment = environment;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginRequest dto)
        {
            var data = await accountService.GetUserByLoginCredential(dto);
            ViewData[Constants.VIEW_DATA.LOGIN_MESSAGE] = data.Message;
            if (data.Result is not null)
            {
                var user = (Users)data.Result;
                await CreateClaimsAndSigIn(user);

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
            if (dto.ProfilePic is not null)
            {
                dto.ProfilePicUrl = Path.Combine("uploads", dto.ProfilePic.FileName);
            }

            var response = await accountService.Create(dto);
            ViewData["MessageForSignUp"] = response.Message;
            if (response.StatusCodes == StatusCodes.Status200OK && response.Result is not null)
            {
                if (dto.ProfilePic is not null)
                {
                    var path = Path.Combine(environment.WebRootPath, "uploads", dto.ProfilePic.FileName);
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await dto.ProfilePic.CopyToAsync(stream);
                    }
                }

                await CreateClaimsAndSigIn((Users)response.Result);
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

        public async Task<IActionResult> UserProfile(string emailId)
        {
            var requestor = await cookieUserDetailsHandler.GetUserDetail(this.User.Identity as ClaimsIdentity);
            var apiResponse = await accountService.GetUserProfile(requestor, emailId);

            if (apiResponse.Result is null)
            {
                return RedirectToAction("UserList", "Admin");
            }

            return View((UserResponse2)apiResponse.Result);
        }

        public async Task<IActionResult> VerifyAccount(string accountVerificationCode)
        {
            var accountVerify = await accountService.VerifyAccount(accountVerificationCode);
            ViewData["AccountVerifyResponseMessage"] = accountVerify.Message;

            if (accountVerify.StatusCodes == StatusCodes.Status200OK)
            {
                return RedirectToAction("Profile", "Account");
            }

            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// API to share data to AJAX in razor pages
        /// </summary>
        [HttpGet]
        public async Task<JsonResult> GetStates(long countryId)
        {
            var country = await accountService.GetCountry(countryId);
            if (country is not null)
            {
                return Json(country.State);
            }
            return Json(null);
        }

        [HttpGet]
        public async Task<JsonResult> GetCities(long stateId, long countryId)
        {
            var country = await accountService.GetCountry(countryId);
            if (country is not null && country.State.Any())
            {
                var cities = country.State.FirstOrDefault(s => s.Id == stateId).City;
                return Json(cities);
            }
            return Json(null);
        }


        private async Task CreateClaimsAndSigIn(Users user)
        {
            if (user is not null)
            {
                var Claims = new[]
                {
                    new Claim(ClaimTypes.Name, user.FirstName + "" + user.LastName),
                    new Claim(ClaimTypes.Email , user.EmailId),
                    new Claim(ClaimTypes.Role, user.RoleId.ToString()),
                    new Claim(ClaimTypes.UserData, string.IsNullOrEmpty(user.ProfilePicUrl) ? "" : user.ProfilePicUrl)
                };

                var Identity = new ClaimsIdentity(Claims, CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(Identity));
            }
        }
    }
}