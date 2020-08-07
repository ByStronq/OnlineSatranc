using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OnlineSatranc.Models.EntityFramework;
using OnlineSatranc.Models;

namespace OnlineSatranc.Controllers
{
    public class HomeController : Controller
    {

        OnlineSatrancEntities db = new OnlineSatrancEntities();

        [Route("")]
        public ActionResult Index()
        {
            var model = new cookieDbUser().getDb();
            return View(model);
        }

        [Route("kayit"), AllowAnonymous]
        public ActionResult KayitOl()
        {
            return View();
        }

        [Route("kayit"), HttpPost, AllowAnonymous]
        public ActionResult KayitOl(kullanicilar kullanici)
        {
            var model = db.kullanicilar.Add(kullanici);
            db.SaveChanges();
            return RedirectPermanent("/");
        }

        [Route("yasak"), AllowAnonymous]
        public ActionResult Yasak()
        {
            return View();
        }
    }
}