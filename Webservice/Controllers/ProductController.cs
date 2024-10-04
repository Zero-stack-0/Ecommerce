using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Dto.Request.Product;
using Service.Interface;
using Webservice.Helper;

namespace Webservice.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService productService;
        private readonly CookieUserDetailsHandler cookieUserDetailsHandler;
        private readonly IWebHostEnvironment environment;
        public ProductController(IProductService productService, CookieUserDetailsHandler cookieUserDetailsHandler, IWebHostEnvironment environment)
        {
            this.productService = productService;
            this.cookieUserDetailsHandler = cookieUserDetailsHandler;
            this.environment = environment;
        }

        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddRequest dto)
        {
            if (dto.Image is not null)
            {
                dto.ImageUrl = Path.Combine("products", dto.Image.FileName);
            }
            dto.Requestor = await cookieUserDetailsHandler.GetUserDetail(User.Identity as ClaimsIdentity);
            var data = await productService.Add(dto);

            TempData["apiResponseMessage"] = data.Message;
            TempData["apiResponseStatusCode"] = data.StatusCodes.ToString();

            if (data.StatusCodes == StatusCodes.Status200OK && dto.Image is not null)
            {
                var path = Path.Combine(environment.WebRootPath, "products", dto.Image.FileName);
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await dto.Image.CopyToAsync(stream);
                }
            }

            if (data.StatusCodes == StatusCodes.Status200OK)
            {
                return RedirectToAction("List");
            }

            return View();
        }

        public IActionResult List()
        {
            return View();
        }

        [HttpGet]
        public async Task<JsonResult> GetProductList(string searchTerm, int categoryId)
        {
            var requestor = await cookieUserDetailsHandler.GetUserDetail(User.Identity as ClaimsIdentity);
            if (requestor is null)
            {
                return Json(null);
            }

            var data = await productService.GetListByCreatedById(searchTerm, categoryId, requestor);
            return Json(data.Result);
        }

        public async Task<IActionResult> Detail(string productId)
        {
            var product = await productService.GetById(Convert.ToInt64(productId));
            return View(product.Result);
        }
    }
}