using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Service.Dto.Request.Admin;
using Service.Interface;
using Webservice.Helper;

namespace Webservice.Controllers
{
    public class AdminController : Controller
    {
        private readonly IAccountService accountService;
        private readonly CookieUserDetailsHandler cookieUserDetailsHandler;
        public AdminController(IAccountService accountService, CookieUserDetailsHandler cookieUserDetailsHandler)
        {
            this.accountService = accountService;
            this.cookieUserDetailsHandler = cookieUserDetailsHandler;
        }

        public IActionResult UserList()
        {
            return View();
        }

        public async Task<JsonResult> GetUsers(GetUserListRequest dto)
        {
            dto.Requestor = await cookieUserDetailsHandler.GetUserDetail(User.Identity as ClaimsIdentity);
            if (dto.Requestor is null)
            {
                return Json(null);
            }

            var result = await accountService.GetUserList(dto);

            var json = Json(result);
            return Json(json);
        }
    }
}