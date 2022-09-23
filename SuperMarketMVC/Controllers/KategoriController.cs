using SuperMarketMVC.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SuperMarketMVC.Controllers
{
    public class KategoriController : Controller
    {

        DbSuperMarketEntities db = new DbSuperMarketEntities();
        // GET: Kategori

        [Authorize]
        public ActionResult Index()
        {
            var model = db.TBLKATEGORİLER.ToList();
            return View(model);
        }


        [HttpGet]
        [Authorize]
        public ActionResult Ekle()
        {
            return View("KategoriForm", new TBLKATEGORİLER());
        }

        [HttpPost]
        [Authorize]
        public ActionResult Kaydet(TBLKATEGORİLER kategori)
        {
            if (!ModelState.IsValid)
            {
                return View("KategoriForm", new TBLKATEGORİLER());
            }
            if (kategori.KATEGORIID == 0)
            {
                if (Request.Files.Count > 0 && Request.Files[0].FileName != "")
                {
                    string fileName = Path.GetFileName(Request.Files[0].FileName);
                    string extension = Path.GetExtension(Request.Files[0].FileName);
                    string path = "~/Images/" + fileName + extension;
                    Request.Files[0].SaveAs(Server.MapPath(path));
                    kategori.KATEGORIRESIM = "/Images/" + fileName + extension;
                }
                db.TBLKATEGORİLER.Add(kategori);
            }
            else
            {
                var guncellenecekKategori = db.TBLKATEGORİLER.Find(kategori.KATEGORIID);
                if (guncellenecekKategori == null)
                    return HttpNotFound();
                if (Request.Files.Count > 0 && Request.Files[0].FileName != "")
                {
                    string fileName = Path.GetFileName(Request.Files[0].FileName);
                    string extension = Path.GetExtension(Request.Files[0].FileName);
                    string path = "~/Images/" + fileName + extension;
                    Request.Files[0].SaveAs(Server.MapPath(path));
                    kategori.KATEGORIRESIM = "/Images/" + fileName + extension;
                }
                guncellenecekKategori.KATEGORIAD = kategori.KATEGORIAD;
                if (kategori.KATEGORIRESIM != null)
                    guncellenecekKategori.KATEGORIRESIM = kategori.KATEGORIRESIM;
            }
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [Authorize]
        public ActionResult Sil(int id)
        {
            var silinecekKategori = db.TBLKATEGORİLER.Find(id);
            if (silinecekKategori == null)
                return HttpNotFound();
            var iliskiliUrun = db.TBLÜRÜNLER.Where(m => m.KATEGORIID == id).FirstOrDefault();
            if (iliskiliUrun != null)
            {
                return RedirectToAction("Index");
            }
            db.TBLKATEGORİLER.Remove(silinecekKategori);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [Authorize]
        public ActionResult Guncelle(int id)
        {
            var guncellenecekKategori = db.TBLKATEGORİLER.Find(id);
            return View("KategoriForm", guncellenecekKategori);
        }
    }
}