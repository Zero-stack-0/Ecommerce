using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Webservice.Helper;
using Webservice.Models;

namespace Webservice.Controllers;

public class HomeController : Controller
{
    private readonly CookieUserDetailsHandler cookieUserDetailsHandler;

    public HomeController(CookieUserDetailsHandler cookieUserDetailsHandler)
    {
        this.cookieUserDetailsHandler = cookieUserDetailsHandler;
    }

    public async Task<IActionResult> Index()
    {
        var claimsIdentity = User.Identity as ClaimsIdentity;
        var requestor = await cookieUserDetailsHandler.GetUserDetail(claimsIdentity);
        return View(requestor);
    }
}
