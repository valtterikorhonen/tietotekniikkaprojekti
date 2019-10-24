using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace AdManagerWebApp.Controllers
{
    public class HrController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}