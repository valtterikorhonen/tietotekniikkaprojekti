using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AdManagerWebApp.Models;
using System.DirectoryServices.AccountManagement;
using AdManagerWebApp.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.Net.Mail;

namespace AdManagerWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly string SessionKey = "UserData";

        private readonly PrincipalContext _context;

        private readonly SmtpClient _smtpClient;

        public HomeController(PrincipalContext DomainContext, SmtpClient smtpClient)
        {
            _context = DomainContext;
            _smtpClient = smtpClient;
        }

        protected override void Dispose(bool disposing)
        {  
            _smtpClient.Dispose();  
            base.Dispose(disposing);  
        }


    private void SetSession(UserViewModel user)
        {
            if(HttpContext.Session.Get<UserViewModel>(SessionKey) == default(UserViewModel))
            {
                HttpContext.Session.Set(SessionKey, user);
            }
        }

        private void ReplaceSession(UserViewModel user)
        {
            HttpContext.Session.Set(SessionKey, user);
        }

        private UserViewModel GetSession()
        {
            UserViewModel user = HttpContext.Session.Get<UserViewModel>(SessionKey);
            if(user == default(UserViewModel))
            {
                return null;
            }
            return user;
        }

        private Alue71UserPrincipal GetPrincipal()
        {
            Alue71UserPrincipal model = new Alue71UserPrincipal(_context);
            string name = this.User.Identity.Name;
            model.SamAccountName = name.Split("\\")[1];

            PrincipalSearcher searcher = new PrincipalSearcher(model);
            return (Alue71UserPrincipal)searcher.FindOne();
        }

        private UserViewModel GetUser()
        {
            UserViewModel user = GetSession();

            if(user == null)
            {
                Alue71UserPrincipal DomainUser = GetPrincipal();
                user = DomainUser.ToViewModel();
                SetSession(user);
                return user;
            }
            return user;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public IActionResult Details()
        {
            return View(GetUser());
        }

        [Authorize]
        public IActionResult Edit()
        {
            return View(GetUser());
        }

        // POST: Home/Edit/5
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(UserViewModel user)
        {
            try
            {
                Alue71UserPrincipal principal = GetPrincipal();
                principal.UpdateFromModel(user);
                principal.Save();
                ReplaceSession(principal.ToViewModel());

                return RedirectToAction(nameof(Details));
            }
            catch
            {
                return View(GetUser());
            }
        }

        [Authorize(Roles = "WebNormaali")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Password(IFormCollection password)
        {
            try
            {
                Alue71UserPrincipal principal = GetPrincipal();
                if(password["New"] == password["Repeat"])
                {
                    principal.ChangePassword(password["Old"], password["New"]);
                    principal.Save();
                }

                return RedirectToAction(nameof(Details));
            }
            catch (Exception ex)
            {
                ViewBag.message = ex.Message;
                return View("Edit", GetUser());
            }
        }

        public IActionResult Reset()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Reset(IFormCollection form)
        {
            string code = Convert.ToBase64String(Guid.NewGuid().ToByteArray()).Substring(0, 8);

            await _smtpClient.SendMailAsync(new MailMessage(
                from: "webapp@alue71.local",
                to: form["email"],
                subject: "Password reset link",
                body: "confirmreset/" + code
            ));

            return RedirectToAction("Index");

        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
