using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using RazorApp.Methods;
using RazorApp.Models;

namespace RazorApp.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ITestMethods _test;

    public HomeController(ILogger<HomeController> logger, ITestMethods test)
    {
        _logger = logger;
        _test = test;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    
    [HttpGet]
    public IActionResult Test()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Test(string log, string pass)
    {
        Account account = await _test.Do(log, pass);
        return RedirectToAction("TestResult", "Home", account);
    }
    [HttpGet]
    public IActionResult TestResult(Account account)
    {
        return View(account);
    }
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
    }
}