using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using AracServisYonetim.Models;

namespace AracServisYonetim.Controllers
{
    [Authorize]
    public class AracController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Arac
        public ActionResult Index()
        {
            var araclar = db.Araclar.Include(a => a.Musteri);
            return View(araclar.ToList());
        }

        // GET: Arac/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Arac arac = db.Araclar.Include(a => a.Musteri).FirstOrDefault(a => a.Id == id);
            if (arac == null)
            {
                return HttpNotFound();
            }
            return View(arac);
        }

        // GET: Arac/Create
        public ActionResult Create()
        {
            ViewBag.MusteriId = new SelectList(db.Musteriler, "Id", "TamAd");
            return View();
        }

        // POST: Arac/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Plaka,Marka,Model,ModelYili,VIN,Kilometre,MusteriId")] Arac arac)
        {
            if (ModelState.IsValid)
            {
                arac.CreatedAt = DateTime.Now;
                arac.UpdatedAt = DateTime.Now;
                db.Araclar.Add(arac);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MusteriId = new SelectList(db.Musteriler, "Id", "TamAd", arac.MusteriId);
            return View(arac);
        }

        // GET: Arac/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Arac arac = db.Araclar.Find(id);
            if (arac == null)
            {
                return HttpNotFound();
            }
            ViewBag.MusteriId = new SelectList(db.Musteriler, "Id", "TamAd", arac.MusteriId);
            return View(arac);
        }

        // POST: Arac/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Plaka,Marka,Model,ModelYili,VIN,Kilometre,MusteriId")] Arac arac)
        {
            if (ModelState.IsValid)
            {
                var existingArac = db.Araclar.Find(arac.Id);
                if (existingArac == null)
                {
                    return HttpNotFound();
                }

                existingArac.Plaka = arac.Plaka;
                existingArac.Marka = arac.Marka;
                existingArac.Model = arac.Model;
                existingArac.ModelYili = arac.ModelYili;
                existingArac.VIN = arac.VIN;
                existingArac.Kilometre = arac.Kilometre;
                existingArac.MusteriId = arac.MusteriId;
                existingArac.UpdatedAt = DateTime.Now;

                db.Entry(existingArac).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MusteriId = new SelectList(db.Musteriler, "Id", "TamAd", arac.MusteriId);
            return View(arac);
        }

        // GET: Arac/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Arac arac = db.Araclar.Include(a => a.Musteri).FirstOrDefault(a => a.Id == id);
            if (arac == null)
            {
                return HttpNotFound();
            }
            
            // Araca ait servis kayıtlarını kontrol et
            var servisler = db.Servisler.Where(s => s.AracId == id).ToList();
            ViewBag.ServisKayitlari = servisler;
            
            return View(arac);
        }

        // POST: Arac/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            // Araca ait servis kayıtlarını kontrol et
            var servisler = db.Servisler.Where(s => s.AracId == id).ToList();
            if (servisler.Any())
            {
                ModelState.AddModelError("", "Bu araca ait servis kayıtları bulunmaktadır. Önce servis kayıtlarını silmelisiniz.");
                Arac aracc = db.Araclar.Include(a => a.Musteri).FirstOrDefault(a => a.Id == id);
                ViewBag.ServisKayitlari = servisler;
                return View(aracc);
            }
            
            Arac arac = db.Araclar.Find(id);
            db.Araclar.Remove(arac);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // AJAX: Araç arama
        [HttpGet]
        public JsonResult Search(string term)
        {
            var araclar = db.Araclar
                .Where(a => a.Plaka.Contains(term) || a.Marka.Contains(term) || a.Model.Contains(term))
                .Select(a => new { id = a.Id, text = a.Plaka + " - " + a.Marka + " " + a.Model })
                .ToList();

            return Json(araclar, JsonRequestBehavior.AllowGet);
        }

        // AJAX: Müşteriye göre araç listesi
        [HttpGet]
        public JsonResult GetAraclarByMusteri(int musteriId)
        {
            var araclar = db.Araclar
                .Where(a => a.MusteriId == musteriId)
                .Select(a => new { id = a.Id, text = a.Plaka + " - " + a.Marka + " " + a.Model })
                .ToList();

            return Json(araclar, JsonRequestBehavior.AllowGet);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
