using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AracServisYonetim.Models;

namespace AracServisYonetim.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            // Dashboard istatistikleri
            ViewBag.AktifServisSayisi = db.Servisler.Count(s => s.Durumu == ServisDurumu.Islemde);
            ViewBag.ToplamAracSayisi = db.Araclar.Count();
            ViewBag.MusteriSayisi = db.Musteriler.Count();
            ViewBag.TamamlananServisSayisi = db.Servisler.Count(s => s.Durumu == ServisDurumu.Tamamlandı);

            // Son servis işlemleri
            var sonServisler = db.Servisler
                .Include("Arac")
                .Include("Arac.Musteri")
                .OrderByDescending(s => s.ServisTarihi)
                .Take(5)
                .ToList();

            return View(sonServisler);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Araç Servis Yönetim Sistemi Hakkında";

            return View();
        }

        [HttpGet]
        public JsonResult GetServisDurumIstatistikleri()
        {
            var bekleyen = db.Servisler.Count(s => s.Durumu == ServisDurumu.Bekliyor);
            var islemde = db.Servisler.Count(s => s.Durumu == ServisDurumu.Islemde);
            var tamamlanan = db.Servisler.Count(s => s.Durumu == ServisDurumu.Tamamlandı);
            var iptalEdilen = db.Servisler.Count(s => s.Durumu == ServisDurumu.IptalEdildi);

            return Json(new { 
                bekleyen = bekleyen,
                islemde = islemde, 
                tamamlanan = tamamlanan, 
                iptalEdilen = iptalEdilen 
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetAylikServisIstatistikleri()
        {
            var bugun = DateTime.Today;
            var sonAltiAy = Enumerable.Range(0, 6)
                .Select(i => bugun.AddMonths(-i))
                .Select(date => new { 
                    Ay = date.ToString("MMMM"), 
                    Yil = date.Year,
                    BaslangicTarihi = new DateTime(date.Year, date.Month, 1),
                    BitisTarihi = new DateTime(date.Year, date.Month, DateTime.DaysInMonth(date.Year, date.Month))
                })
                .ToList();

            var sonuc = sonAltiAy.Select(ay => new {
                ay = ay.Ay,
                servisSayisi = db.Servisler.Count(s => 
                    s.ServisTarihi >= ay.BaslangicTarihi && 
                    s.ServisTarihi <= ay.BitisTarihi)
            }).ToList();

            return Json(sonuc, JsonRequestBehavior.AllowGet);
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
