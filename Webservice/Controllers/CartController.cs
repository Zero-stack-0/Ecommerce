using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Service.Dto.Request.Cart;
using Service.Helper;
using Service.Interface;
using Webservice.Helper;

namespace Webservice.Controllers
{
    public class CartController : Controller
    {
        private readonly CookieUserDetailsHandler cookieUserDetailsHandler;
        private readonly ICartService cartService;
        private readonly IProductService productService;
        public CartController(CookieUserDetailsHandler cookieUserDetailsHandler, ICartService cartService, IProductService productService)
        {
            this.cookieUserDetailsHandler = cookieUserDetailsHandler;
            this.cartService = cartService;
            this.productService = productService;
        }


        public async Task<IActionResult> Add(long productId)
        {
            var requestor = await cookieUserDetailsHandler.GetUserDetail(User.Identity as ClaimsIdentity);
            if (requestor is null)
            {
                return RedirectToAction("Login", "Account");
            }
            var product = await productService.GetById(productId - 10000);
            return View(product.Result);
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddRequest dto)
        {
            dto.Requestor = await cookieUserDetailsHandler.GetUserDetail(User.Identity as ClaimsIdentity);
            if (dto.Requestor is null)
            {
                return RedirectToAction("Login", "Account");
            }
            var data = await cartService.Add(dto);

            TempData["apiResponseMessage"] = data.Message;
            TempData["apiResponseStatusCode"] = data.StatusCodes.ToString();

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<ApiResponse> GetList(GetListRequest dto)
        {
            dto.Requestor = await cookieUserDetailsHandler.GetUserDetail(User.Identity as ClaimsIdentity);

            var data = await cartService.GetList(dto);

            return data;
        }
    }
}