using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Dto.Request;
using Service.Dto.Request.Admin;
using Service.Dto.Response;
using Service.Interface;
using Webservice.Helper;

namespace Webservice.Controllers
{
    [Authorize]
    public class SellerController : Controller
    {
        private readonly CookieUserDetailsHandler cookieUserDetailsHandler;
        private readonly ISellerService sellerService;
        public SellerController(CookieUserDetailsHandler cookieUserDetailsHandler, ISellerService sellerService)
        {
            this.cookieUserDetailsHandler = cookieUserDetailsHandler;
            this.sellerService = sellerService;
        }
        public IActionResult SendRequest()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SendRequest(SellerRequestDto dto)
        {
            dto.Requestor = await cookieUserDetailsHandler.GetUserDetail(this.User.Identity as ClaimsIdentity);
            var data = await sellerService.Request(dto);

            TempData["apiResponseMessage"] = data.Message;
            TempData["apiResponseStatusCode"] = data.StatusCodes.ToString();

            if (data.Result is not null)
            {
                return View("ViewRequest", (SellerRequestResponse)data.Result);
            }

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Update(UpdateSellerRequestStatus dto)
        {
            dto.Requestor = await cookieUserDetailsHandler.GetUserDetail(this.User.Identity as ClaimsIdentity);
            await sellerService.UpdateRequestStatus(dto);

            return RedirectToAction("SellerRequests", "Admin");
        }
        public async Task<IActionResult> ViewRequest()
        {
            var requestor = await cookieUserDetailsHandler.GetUserDetail(this.User.Identity as ClaimsIdentity);
            var data = await sellerService.Get(requestor);
            return View((SellerRequestResponse)data.Result);
        }
    }
}