using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RazorApp.Methods;
using RazorApp.Models;

namespace RazorApp.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ITestMethods _test;
    private readonly IWalletMethods _wallet;

    public HomeController(ILogger<HomeController> logger, ITestMethods test, IWalletMethods wallet)
    {
        _logger = logger;
        _test = test;
        _wallet = wallet;
    }

    [Authorize]
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }
    
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
    }
}