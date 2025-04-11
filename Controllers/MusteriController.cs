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
    public class MusteriController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Musteri
        public ActionResult Index()
        {
            return View(db.Musteriler.ToList());
        }

        // GET: Musteri/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Musteri musteri = db.Musteriler.Find(id);
            if (musteri == null)
            {
                return HttpNotFound();
            }
            return View(musteri);
        }

        // GET: Musteri/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Musteri/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Ad,Soyad,Email,Telefon,Adres")] Musteri musteri)
        {
            if (ModelState.IsValid)
            {
                musteri.CreatedAt = DateTime.Now;
                musteri.UpdatedAt = DateTime.Now;
                db.Musteriler.Add(musteri);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(musteri);
        }

        // GET: Musteri/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Musteri musteri = db.Musteriler.Find(id);
            if (musteri == null)
            {
                return HttpNotFound();
            }
            return View(musteri);
        }

        // POST: Musteri/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Ad,Soyad,Email,Telefon,Adres")] Musteri musteri)
        {
            if (ModelState.IsValid)
            {
                var existingMusteri = db.Musteriler.Find(musteri.Id);
                if (existingMusteri == null)
                {
                    return HttpNotFound();
                }

                existingMusteri.Ad = musteri.Ad;
                existingMusteri.Soyad = musteri.Soyad;
                existingMusteri.Email = musteri.Email;
                existingMusteri.Telefon = musteri.Telefon;
                existingMusteri.Adres = musteri.Adres;
                existingMusteri.UpdatedAt = DateTime.Now;

                db.Entry(existingMusteri).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(musteri);
        }

        // GET: Musteri/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Musteri musteri = db.Musteriler.Find(id);
            if (musteri == null)
            {
                return HttpNotFound();
            }
            return View(musteri);
        }

        // POST: Musteri/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Musteri musteri = db.Musteriler.Find(id);
            
            // Müşteriye ait araçları kontrol et
            var araclar = db.Araclar.Where(a => a.MusteriId == id).ToList();
            if (araclar.Any())
            {
                ModelState.AddModelError("", "Bu müşteriye ait araçlar bulunmaktadır. Önce araçları silmelisiniz.");
                return View(musteri);
            }
            
            db.Musteriler.Remove(musteri);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // AJAX: Müşteri arama
        [HttpGet]
        public JsonResult Search(string term)
        {
            var musteriler = db.Musteriler
                .Where(m => m.Ad.Contains(term) || m.Soyad.Contains(term) || m.Email.Contains(term))
                .Select(m => new { id = m.Id, text = m.TamAd })
                .ToList();

            return Json(musteriler, JsonRequestBehavior.AllowGet);
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
