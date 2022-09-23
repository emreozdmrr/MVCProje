using SuperMarketMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace SuperMarketMVC.Controllers
{
    public class LoginController : Controller
    {

        DbSuperMarketEntities db = new DbSuperMarketEntities();
        // GET: Login

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public ActionResult GirisYap(TBLKULLANICILAR kullanici)
        {
            var kullaniciBilgileri = db.TBLKULLANICILAR.FirstOrDefault(m => m.KULLANICIAD == kullanici.KULLANICIAD && m.KULLANICISIFRE == kullanici.KULLANICISIFRE);
            if (kullaniciBilgileri != null)
            {
                FormsAuthentication.SetAuthCookie(kullaniciBilgileri.KULLANICIAD, false);
                Session["KULLANICIAD"] = kullaniciBilgileri.KULLANICIAD;
                return RedirectToAction("Index", "Kategori");
            }
            return View("Index");
        }

        [HttpGet]
        [Authorize]
        public ActionResult CikisYap()
        {
            Session.Abandon();
            return RedirectToAction("Index", "Home");
        }
    }
}