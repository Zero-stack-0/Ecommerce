using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Service.Interface;
using Webservice.Helper;

namespace Webservice.Controllers;

public class HomeController : Controller
{
    private readonly IProductService productService;
    private readonly CookieUserDetailsHandler cookieUserDetailsHandler;
    public HomeController(IProductService productService, CookieUserDetailsHandler cookieUserDetailsHandler)
    {
        this.productService = productService;
        this.cookieUserDetailsHandler = cookieUserDetailsHandler;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Explore()
    {
        return View();
    }

    public async Task<JsonResult> Products(string searchTerm, int categoryId)
    {
        var loggedInUser = await cookieUserDetailsHandler.GetUserDetail(User.Identity as ClaimsIdentity);
        if (loggedInUser is null)
        {
            return Json(await productService.GetOpenList(searchTerm, categoryId, null));
        }
        return Json(await productService.GetOpenList(searchTerm, categoryId, loggedInUser.Id));
    }
}
