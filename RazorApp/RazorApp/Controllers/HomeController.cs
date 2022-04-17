using System.Diagnostics;
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
    public async Task<IActionResult> Test(string cardNumber, string password, string phoneNumber)
    {
        Wallet wallet = await _wallet.CreateWallet(cardNumber, password, phoneNumber);
        return RedirectToAction("TestResult", "Home", wallet);
    }
    [HttpGet]
    public IActionResult TestResult(Wallet wallet)
    {
        return View(wallet);
    }
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
    }
}