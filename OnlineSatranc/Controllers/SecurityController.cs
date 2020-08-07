using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OnlineSatranc.Models.EntityFramework;
using System.Web.Security;

namespace OnlineSatranc.Controllers
{
    public class SecurityController : Controller
    {
        OnlineSatrancEntities db = new OnlineSatrancEntities();
        // GET: Security
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost, AllowAnonymous]
        public ActionResult Login(kullanicilar kullanici, string beniHatirla)
        {
            var kullaniciInDb = db.kullanicilar.FirstOrDefault(x => x.kAdi == kullanici.kAdi && x.sifre == kullanici.sifre);
            bool hatirla = false;

            if (kullaniciInDb != null)
            {
                if (beniHatirla == "true") hatirla = true;

                FormsAuthentication.SetAuthCookie(kullaniciInDb.ID.ToString(), hatirla);
                return RedirectPermanent("/");
            }
            else
            {
                ViewBag.Mesaj = "Geçersiz kullanıcı adı veya şifre";
                return View();
            }
        }

        [Route("cikis")]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }
    }
}