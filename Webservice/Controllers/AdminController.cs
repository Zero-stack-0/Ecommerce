using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Dto.Request.Admin;
using Service.Interface;
using Webservice.Helper;

namespace Webservice.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private readonly IAccountService accountService;
        private readonly CookieUserDetailsHandler cookieUserDetailsHandler;
        private readonly ISellerService sellerService;
        public AdminController(IAccountService accountService, CookieUserDetailsHandler cookieUserDetailsHandler, ISellerService sellerService)
        {
            this.accountService = accountService;
            this.cookieUserDetailsHandler = cookieUserDetailsHandler;
            this.sellerService = sellerService;
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

        public IActionResult Users()
        {
            return View();
        }

        public async Task<JsonResult> GetSellerRequestList(GetSellerRequestList dto)
        {
            dto.Requestor = await cookieUserDetailsHandler.GetUserDetail(User.Identity as ClaimsIdentity);
            if (dto.Requestor is null)
            {
                return Json(null);
            }

            var result = await sellerService.GetRequestList(dto);

            var json = Json(result);
            return Json(json);
        }

        public IActionResult SellerRequests()
        {
            return View();
        }
    }
}