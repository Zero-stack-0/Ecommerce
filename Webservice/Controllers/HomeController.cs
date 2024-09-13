using Microsoft.AspNetCore.Mvc;
using Service.Interface;

namespace Webservice.Controllers;

public class HomeController : Controller
{
    private readonly IProductService productService;
    public HomeController(IProductService productService)
    {
        this.productService = productService;
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
        return Json(await productService.GetOpenList(searchTerm, categoryId));
    }
}
