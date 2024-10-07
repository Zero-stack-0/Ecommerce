using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Service.Dto.Request.Cart;
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

            return View("List");
        }

        public IActionResult List()
        {
            return View();
        }

        [HttpGet]
        public async Task<JsonResult> GetList(GetListRequest dto)
        {
            dto.Requestor = await cookieUserDetailsHandler.GetUserDetail(User.Identity as ClaimsIdentity);

            var data = await cartService.GetList(dto);

            return Json(data);
        }

        public async Task<IActionResult> Detail(long cartId)
        {
            var requestor = await cookieUserDetailsHandler.GetUserDetail(User.Identity as ClaimsIdentity);
            var data = await cartService.GetDetail(cartId, requestor);
            return View(data.Result);
        }

        public async Task<IActionResult> Update(UpdateRequest dto)
        {
            dto.Requestor = await cookieUserDetailsHandler.GetUserDetail(User.Identity as ClaimsIdentity);
            var data = await cartService.Update(dto);

            TempData["apiResponseMessage"] = data.Message;
            TempData["apiResponseStatusCode"] = data.StatusCodes.ToString();

            return View("List");
        }

        public async Task<IActionResult> Delete(long cartId)
        {
            var requestor = await cookieUserDetailsHandler.GetUserDetail(User.Identity as ClaimsIdentity);
            var data = await cartService.Delete(cartId, requestor);

            TempData["apiResponseMessage"] = data.Message;
            TempData["apiResponseStatusCode"] = data.StatusCodes.ToString();

            return View("List");
        }
    }
}