using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AracServisYonetim.Controllers
{
    public class SetupController : Controller
    {
        // GET: Setup
        [AllowAnonymous]
        public ActionResult Index()
        {
            ViewBag.Title = "Kurulum Talimatları";
            return View();
        }
    }
}
