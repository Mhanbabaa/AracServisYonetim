using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using AracServisYonetim.Models;
using Microsoft.AspNet.Identity;

namespace AracServisYonetim.Controllers
{
    [Authorize]
    public class ServisController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Servis
        public ActionResult Index()
        {
            var servisler = db.Servisler.Include(s => s.Arac).Include(s => s.Arac.Musteri);
            return View(servisler.ToList());
        }

        // GET: Servis/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Servis servis = db.Servisler.Include(s => s.Arac).Include(s => s.Arac.Musteri).FirstOrDefault(s => s.Id == id);
            if (servis == null)
            {
                return HttpNotFound();
            }
            return View(servis);
        }

        // GET: Servis/Create
        public ActionResult Create()
        {
            ViewBag.AracId = new SelectList(db.Araclar, "Id", "TamBilgi");
            ViewBag.Durumlar = new SelectList(Enum.GetValues(typeof(ServisDurumu))
                .Cast<ServisDurumu>()
                .Select(v => new SelectListItem
                {
                    Text = v.ToString(),
                    Value = ((int)v).ToString()
                }), "Value", "Text");
            return View();
        }

        // POST: Servis/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,AracId,Açıklama,Durumu,ServisTarihi,TeslimTarihi,ServisBedeli")] Servis servis)
        {
            if (ModelState.IsValid)
            {
                servis.CreatedAt = DateTime.Now;
                servis.UpdatedAt = DateTime.Now;
                db.Servisler.Add(servis);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AracId = new SelectList(db.Araclar, "Id", "TamBilgi", servis.AracId);
            ViewBag.Durumlar = new SelectList(Enum.GetValues(typeof(ServisDurumu))
                .Cast<ServisDurumu>()
                .Select(v => new SelectListItem
                {
                    Text = v.ToString(),
                    Value = ((int)v).ToString()
                }), "Value", "Text", (int)servis.Durumu);
            return View(servis);
        }

        // GET: Servis/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Servis servis = db.Servisler.Find(id);
            if (servis == null)
            {
                return HttpNotFound();
            }
            ViewBag.AracId = new SelectList(db.Araclar, "Id", "TamBilgi", servis.AracId);
            ViewBag.Durumlar = new SelectList(Enum.GetValues(typeof(ServisDurumu))
                .Cast<ServisDurumu>()
                .Select(v => new SelectListItem
                {
                    Text = v.ToString(),
                    Value = ((int)v).ToString()
                }), "Value", "Text", (int)servis.Durumu);
            return View(servis);
        }

        // POST: Servis/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,AracId,Açıklama,Durumu,ServisTarihi,TeslimTarihi,ServisBedeli")] Servis servis)
        {
            if (ModelState.IsValid)
            {
                var existingServis = db.Servisler.Find(servis.Id);
                if (existingServis == null)
                {
                    return HttpNotFound();
                }

                existingServis.AracId = servis.AracId;
                existingServis.Açıklama = servis.Açıklama;
                existingServis.Durumu = servis.Durumu;
                existingServis.ServisTarihi = servis.ServisTarihi;
                existingServis.TeslimTarihi = servis.TeslimTarihi;
                existingServis.ServisBedeli = servis.ServisBedeli;
                existingServis.UpdatedAt = DateTime.Now;

                db.Entry(existingServis).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AracId = new SelectList(db.Araclar, "Id", "TamBilgi", servis.AracId);
            ViewBag.Durumlar = new SelectList(Enum.GetValues(typeof(ServisDurumu))
                .Cast<ServisDurumu>()
                .Select(v => new SelectListItem
                {
                    Text = v.ToString(),
                    Value = ((int)v).ToString()
                }), "Value", "Text", (int)servis.Durumu);
            return View(servis);
        }

        // GET: Servis/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Servis servis = db.Servisler.Include(s => s.Arac).Include(s => s.Arac.Musteri).FirstOrDefault(s => s.Id == id);
            if (servis == null)
            {
                return HttpNotFound();
            }
            return View(servis);
        }

        // POST: Servis/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Servis servis = db.Servisler.Find(id);
            db.Servisler.Remove(servis);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // AJAX: Servis durumunu güncelleme
        [HttpPost]
        public JsonResult UpdateStatus(int id, int durum)
        {
            try
            {
                var servis = db.Servisler.Find(id);
                if (servis == null)
                {
                    return Json(new { success = false, message = "Servis kaydı bulunamadı." });
                }

                servis.Durumu = (ServisDurumu)durum;
                servis.UpdatedAt = DateTime.Now;
                
                // Eğer durum "Tamamlandı" ise teslim tarihini şu anki tarih yap
                if (servis.Durumu == ServisDurumu.Tamamlandı && !servis.TeslimTarihi.HasValue)
                {
                    servis.TeslimTarihi = DateTime.Now;
                }

                db.Entry(servis).State = EntityState.Modified;
                db.SaveChanges();

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
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
