using AdManagerWebApp.Helpers;
using AdManagerWebApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;

namespace AdManagerWebApp.Controllers
{
    [Authorize(Roles = "WebAdmin")]
    public class AdminController : Controller
    {
        private readonly PrincipalContext _context;

        public AdminController(PrincipalContext DomainContext)
        {
            _context = DomainContext;
        }

        private List<UserViewModel> GetUsers()
        {
            var group = GroupPrincipal.FindByIdentity(_context, "WebNormaali");
            List<UserViewModel> Users = new List<UserViewModel>();
            foreach(Alue71UserPrincipal p in group.Members)
            {
                Users.Add(p.ToViewModel());
            }
            return Users;
        }

        // GET: Admin
        public ActionResult Index()
        {
            return View(GetUsers());
        }

        // GET: Admin/Details/5
        public ActionResult Details(int id)
        {
            return View(new UserViewModel());
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(UserViewModel model)
        {
            if (false == ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                Alue71UserPrincipal newUser = new Alue71UserPrincipal(_context);
                model.DisplayName = model.GivenName;
                model.Name = model.GivenName + " " + model.Surname;
                model.Email = model.GivenName + "." + model.Surname + "@alue71.local";
                newUser.UpdateFromModel(model);
                newUser.SetPassword("admin");
                newUser.ExpirePasswordNow();
                newUser.UnlockAccount();
                newUser.Save();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(model);
            }
        }

        // GET: Admin/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Admin/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, UserViewModel user)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Admin/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Admin/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}