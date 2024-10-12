using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Service.Dto.Request.Order;
using Service.Interface;
using Webservice.Helper;

namespace Webservice.Controllers
{
    public class OrderController : Controller
    {
        private readonly CookieUserDetailsHandler cookieUserDetailsHandler;
        private readonly IOrderService orderService;

        public OrderController(CookieUserDetailsHandler cookieUserDetailsHandler, IOrderService orderService)
        {
            this.cookieUserDetailsHandler = cookieUserDetailsHandler;
            this.orderService = orderService;
        }

        public async Task<IActionResult> Add(AddRequest dto)
        {
            dto.Requestor = await cookieUserDetailsHandler.GetUserDetail(User.Identity as ClaimsIdentity);
            var data = await orderService.Add(dto);

            if (data.StatusCodes == StatusCodes.Status200OK)
            {
                return View("Checkout", data.Result);
            }

            return RedirectToAction("List", "Cart");
        }
    }
}