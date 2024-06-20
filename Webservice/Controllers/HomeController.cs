using Microsoft.AspNetCore.Mvc;

namespace Webservice.Controllers;

public class HomeController : Controller
{
    public HomeController()
    { }

    public IActionResult Index()
    {
        return View();
    }
}
