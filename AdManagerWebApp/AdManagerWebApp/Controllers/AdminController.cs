using AdManagerWebApp.Helpers;
using AdManagerWebApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
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
            foreach(Principal p in group.GetMembers())
            {
                //Users.Add(p.ToViewModel());
                Users.Add(new UserViewModel { Name = p.Name, SamAccountName = p.SamAccountName });
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
                model.SamAccountName = model.GivenName.ToLower() + model.Surname.ToLower();
                newUser.UpdateFromModel(model);
                newUser.SetPassword("admin");
                newUser.ExpirePasswordNow();
                newUser.Enabled = true;
                newUser.Save();

                GroupPrincipal grp = GroupPrincipal.FindByIdentity(_context, "WebNormaali");

                if (grp != null)
                {
                    grp.Members.Add(newUser);
                    grp.Save();
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.message = ex.Message;
                return View(model);
            }
        }

        // GET: Admin/Edit/5
        public ActionResult Edit(string id)
        {
            Alue71UserPrincipal model = new Alue71UserPrincipal(_context);
            model.SamAccountName = id;

            PrincipalSearcher searcher = new PrincipalSearcher(model);
            Alue71UserPrincipal edituser = (Alue71UserPrincipal)searcher.FindOne();

            return View(edituser.ToViewModel());
        }

        // POST: Admin/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(UserViewModel user)
        {
            try
            {
                Alue71UserPrincipal model = new Alue71UserPrincipal(_context);
                model.SamAccountName = user.SamAccountName;

                PrincipalSearcher searcher = new PrincipalSearcher(model);
                Alue71UserPrincipal edituser = (Alue71UserPrincipal)searcher.FindOne();
                edituser.UpdateFromModel(user);
                edituser.Save();

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.message = ex.Message;
                return View(user);
            }
        }
    }
}