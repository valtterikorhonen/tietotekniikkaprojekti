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

namespace AdManagerWebApp.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {

        private readonly PrincipalContext _context;

        public HomeController(PrincipalContext DomainContext)
        {
            _context = DomainContext;
        }

        public IActionResult Index()
        {
            Alue71UserPrincipal model = new Alue71UserPrincipal(_context);
            string name = this.User.Identity.Name;
            model.SamAccountName = name.Split("\\")[1];

            PrincipalSearcher searcher = new PrincipalSearcher(model);
            Alue71UserPrincipal DomainUser = (Alue71UserPrincipal)searcher.FindOne();
            return View(DomainUser.ToViewModel());
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
