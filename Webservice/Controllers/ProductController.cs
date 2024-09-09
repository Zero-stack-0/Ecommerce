using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Dto.Request.Product;
using Service.Interface;
using Webservice.Helper;

namespace Webservice.Controllers
{
    [Authorize]
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

            return View();
        }
    }
}