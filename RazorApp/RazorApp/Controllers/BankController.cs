using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using RazorApp.Methods;
using RazorApp.Models;
using RazorApp.MongoDb;

namespace RazorApp.Controllers
{

    public class BankController : Controller
    {
        private readonly IWalletMethods _wallet;
        private readonly ITestMethods _test;
        private readonly ITransactionMethods _transactionMethods;
        private readonly IMongoCollection<Wallet> _walletCollection;
        
        public BankController(IDbClient client,ITestMethods test, IWalletMethods wallet, ITransactionMethods transactionMethods)
        {
            _walletCollection = client.GetWalletCollection();
            _test = test;
            _wallet = wallet;
            _transactionMethods = transactionMethods;

        }

        [Authorize]
        public async Task<IActionResult> Transactions()
        {
            return View();
        }

        [Authorize]
        [HttpGet]
        public IActionResult Operation()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Operation(string sumCard)
        {
            var sum = Convert.ToDouble(sumCard);
            var session = await _wallet.GetSession();
            await _transactionMethods.Replenishment(session.Email, sum);
            return RedirectToAction("GetWallet", "Bank");

        }

        public async Task<IActionResult> WalletResult()
        {
            var session = await _wallet.GetSession();
            var operations = await _transactionMethods.GetAllOperations(session.Email);
            return View(operations);
        }

        [Authorize]
        public async Task<IActionResult> GetWallet()
        {
            var session = await _wallet.GetSession();
            var wallet = await _wallet.GetWalletByEmail(session.Email);
            if (wallet is not null)
            {
                return View(wallet);
            }

            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> CreateWallet()
        {
            SessionModel sessionModel = await _wallet.GetSession();
            var walletSes = await _walletCollection.Find(u => u.Email == sessionModel.Email).FirstOrDefaultAsync();
            if (walletSes == null)
            {
                return View(); 
            }

            return RedirectToAction("GetWallet", "Bank");
        }

        [HttpPost]
        public async Task<IActionResult> CreateWallet(string cardNumber, string password, string phoneNumber)
        {
            SessionModel sessionModel = await _wallet.GetSession();
            Wallet walletSes = await _wallet.GetWalletByEmail(sessionModel.Email);
            await _wallet.CreateWallet(cardNumber, password, phoneNumber);
                return RedirectToAction("Index", "Home");
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Transaction()
        {
            return View();
        }

        [HttpPost]
        /*string email, 
        string emailGet, 
        string phone, 
        double summ, 
        string description*/
        public async Task<IActionResult> Transaction(string Card, string phone, string sum, string description)
        {
            double sumd = Convert.ToDouble(sum);
            var session = await _wallet.GetSession();
            var transaction = await _transactionMethods.Transaction(session.Email, Card, phone, sumd, description);
            return RedirectToAction("Index", "Home");
        }
        
        
    }
}