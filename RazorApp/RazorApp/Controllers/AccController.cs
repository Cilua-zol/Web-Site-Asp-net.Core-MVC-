using Microsoft.AspNetCore.Mvc;
using RazorApp.Methods;
using RazorApp.Models;

namespace RazorApp.Controllers
{
    public class AccController : Controller
    {
        private readonly ITestMethods _test;
        public AccController(ITestMethods test)
        {
            _test = test;
        }

        [HttpGet]
        public IActionResult AccReq()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AccReq(string login)
        {
            Account acc = await _test.GetAccByLogin(login);
            return RedirectToAction("AccView", "Acc", acc);
        }

        [HttpGet]
        public IActionResult AccView(Account account)
        {
            return View(account);
        }
    }
}