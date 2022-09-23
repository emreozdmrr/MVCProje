using SuperMarketMVC.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;

namespace SuperMarketMVC.Controllers
{
    public class UrunlerController : Controller
    {
        DbSuperMarketEntities db = new DbSuperMarketEntities();

        // GET: Urunler
        [Authorize]
        public ActionResult Index()
        {
            var urunler = db.TBLÜRÜNLER.ToList();
            return View(urunler);
        }

        [HttpGet]
        public ActionResult UrunleriListele(int sayfa = 1)
        {

            var urunler = db.TBLÜRÜNLER.ToList().ToPagedList(sayfa, 6);
            ViewBag.baslik = "Tüm Ürünler";
            return View("Urunler", urunler);
        }

        [HttpGet]
        public ActionResult KategoriDetay(int id, int sayfa = 1)
        {
            if (id == 0)
            {
                var urunler = db.TBLÜRÜNLER.Where(m => m.URUNINDIRIMORANI > 0 && m.URUNINDIRIMORANI != null).ToList().ToPagedList(sayfa, 6);
                ViewBag.baslik = "İndirimli Ürünler";
                return View("Urunler", urunler);
            }
            else
            {
                var urunler = db.TBLÜRÜNLER.Where(m => m.KATEGORIID == id).ToList().ToPagedList(sayfa, 6);
                ViewBag.baslik = db.TBLKATEGORİLER.Find(id).KATEGORIAD;
                return View("Urunler", urunler);
            }
        }

        [HttpGet]
        [Authorize]
        public ActionResult Ekle()
        {
            var kategoriler = db.TBLKATEGORİLER.ToList();
            ViewBag.kategoriler = kategoriler;
            return View("UrunlerForm", new TBLÜRÜNLER());
        }

        [HttpPost]
        [Authorize]
        public ActionResult Kaydet(TBLÜRÜNLER urun)
        {
            if (!ModelState.IsValid)
            {
                var kategoriler = db.TBLKATEGORİLER.ToList();
                ViewBag.kategoriler = kategoriler;
                return View("UrunlerForm", urun);
            }
            if (urun.URUNID == 0)
            {
                if (Request.Files.Count > 0 && Request.Files[0].FileName != "")
                {
                    string fileName = Path.GetFileName(Request.Files[0].FileName);
                    string extension = Path.GetExtension(Request.Files[0].FileName);
                    string path = "~/Images/" + fileName + extension;
                    Request.Files[0].SaveAs(Server.MapPath(path));
                    urun.URUNRESIM = "/Images/" + fileName + extension;
                }
                db.TBLÜRÜNLER.Add(urun);
            }
            else
            {
                if (Request.Files.Count > 0 && Request.Files[0].FileName != "")
                {
                    string fileName = Path.GetFileName(Request.Files[0].FileName);
                    string extension = Path.GetExtension(Request.Files[0].FileName);
                    string path = "~/Images/" + fileName + extension;
                    Request.Files[0].SaveAs(Server.MapPath(path));
                    urun.URUNRESIM = "/Images/" + fileName + extension;
                }
                var guncellenecekUrun = db.TBLÜRÜNLER.Find(urun.URUNID);
                guncellenecekUrun.URUNAD = urun.URUNAD;
                guncellenecekUrun.KATEGORIID = urun.KATEGORIID;
                guncellenecekUrun.URUNBIRIM = urun.URUNBIRIM;
                guncellenecekUrun.URUNFIYAT = urun.URUNFIYAT;
                guncellenecekUrun.URUNINDIRIMORANI = urun.URUNINDIRIMORANI;
                if (urun.URUNRESIM != null)
                    guncellenecekUrun.URUNRESIM = urun.URUNRESIM;
            }
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [Authorize]
        public ActionResult Sil(int id)
        {
            var silinecekUrun = db.TBLÜRÜNLER.Find(id);
            if (silinecekUrun == null)
                return HttpNotFound();
            db.TBLÜRÜNLER.Remove(silinecekUrun);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [Authorize]
        public ActionResult Guncelle(int id)
        {
            var guncellenecekUrun = db.TBLÜRÜNLER.Find(id);
            ViewBag.kategoriler = db.TBLKATEGORİLER.ToList();
            return View("UrunlerForm", guncellenecekUrun);
        }

        public ActionResult KategorileriListele()
        {
            var kategoriler = db.TBLKATEGORİLER.ToList();
            ViewBag.baslik = "Kategoriler";
            return View(kategoriler);
        }

    }
}