using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using RazorApp.Methods;
using RazorApp.Models;
using RazorApp.MongoDb;

namespace RazorApp.Controllers
{
    public class AccController : Controller
    {
        private readonly IMongoCollection<Account> _accountCollection;
        private readonly IMongoCollection<SessionModel> _sessionCollection;
        private readonly IWalletMethods _wallet;

        public AccController(IDbClient dbClient, IWalletMethods wallet)
        {
            _accountCollection = dbClient.GetAccountCollection();
            _sessionCollection = dbClient.GetSessionCollection();
            _wallet = wallet;

        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                Account account = await _accountCollection.Find(u => u.Email == model.Email && u.Password == model.Password).FirstOrDefaultAsync();
                if (account != null)
                {
                   
                    SessionModel session = new SessionModel();
                    session.Email = model.Email;
                    session.Id = Guid.NewGuid().ToString("N");
                    session.SessionStatus = SessionModel.Status.Active;
                    await _sessionCollection.InsertOneAsync(session);


                    await Authenticate(model.Email); // аутентификация
                    
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("", "Некорректные логин и(или) пароль");
            }
            return View(model);
        }

        [Authorize]
        public async Task<IActionResult> Profile()
        {
            var session = await _wallet.GetSession();
            var account = await _accountCollection.Find(u => u.Email == session.Email).FirstOrDefaultAsync();
            return View(account);
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                Account account = await _accountCollection.Find(u => u.Email == model.Email).FirstOrDefaultAsync();
                if (account == null)
                {
                    // Закрываем старую сессию
                    var sessionNow = await _wallet.GetSession();
                    if (sessionNow.SessionStatus == SessionModel.Status.Active)
                    {
                        sessionNow.SessionStatus = SessionModel.Status.Ended;
                        await ChangeStatus(sessionNow.Id, sessionNow.SessionStatus);
                    }

                    //Создаём новую
                    SessionModel session = new SessionModel();
                    session.Email = model.Email;
                    session.Id = Guid.NewGuid().ToString("N");
                    session.SessionStatus = SessionModel.Status.Active;
                    await _sessionCollection.InsertOneAsync(session);
                    
                    // добавляем пользователя в бд  
                    await _accountCollection.InsertOneAsync(new Account{Id = Guid.NewGuid().ToString("N"), Email = model.Email, Password = model.Password});
                    
                    await Authenticate(model.Email); // аутентификация
 
                    return RedirectToAction("Index", "Home");
                }
                else
                    ModelState.AddModelError("", "Некорректные логин и(или) пароль");
            }
            return View(model);
        }
 
        private async Task Authenticate(string userName)
        {
            // создаем один claim
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, userName)
            };
            // создаем объект ClaimsIdentity
            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            // установка аутентификационных куки
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }
 
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            var session = await _wallet.GetSession();
            session.SessionStatus = SessionModel.Status.Ended;
            await ChangeStatus(session.Id, session.SessionStatus);
            
            return RedirectToAction("Login", "Acc");
        }
        public async Task ChangeStatus(string id, SessionModel.Status status)
        {
            try
            {
                var filter = Builders<SessionModel>.Filter.Eq(u => u.Id, id);
                var update = Builders<SessionModel>.Update
                    .Set(u => u.SessionStatus, status);
                await _sessionCollection.UpdateOneAsync(filter, update);
            }
            catch (Exception ex)
            {
                throw new Exception("Error", ex);
            }
        }
    }
}