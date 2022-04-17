using Microsoft.AspNetCore.Mvc;
using RazorApp.Methods;
using RazorApp.Models;

namespace RazorApp.Controllers
{

    public class BankController : Controller
    {
        private readonly IWalletMethods _wallet;
        private readonly ITestMethods _test;
        public BankController(ITestMethods test, IWalletMethods wallet)
        {
            _test = test;
            _wallet = wallet;
            
            
        }
        [HttpGet]
        public IActionResult CreateWallet()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateWallet(string cardNumber, string password, string phoneNumber)
        {
            Wallet wallet = await _wallet.CreateWallet(cardNumber, password, phoneNumber);
            return RedirectToAction("WalletResult", "Bank", wallet);
        }
        [HttpGet]
        public IActionResult WalletResult(Wallet wallet)
        {
            return View(wallet);
        }
        
    }
}