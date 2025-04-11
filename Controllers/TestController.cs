using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AracServisYonetim.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;

namespace AracServisYonetim.Controllers
{
    public class TestController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Test
        [AllowAnonymous]
        public ActionResult Index()
        {
            try
            {
                // Veritabanı bağlantısını test et
                var musteriSayisi = db.Musteriler.Count();
                var aracSayisi = db.Araclar.Count();
                var servisSayisi = db.Servisler.Count();
                var kullaniciSayisi = db.Users.Count();

                ViewBag.DatabaseStatus = "Başarılı";
                ViewBag.MusteriSayisi = musteriSayisi;
                ViewBag.AracSayisi = aracSayisi;
                ViewBag.ServisSayisi = servisSayisi;
                ViewBag.KullaniciSayisi = kullaniciSayisi;
            }
            catch (Exception ex)
            {
                ViewBag.DatabaseStatus = "Hata: " + ex.Message;
            }

            return View();
        }

        [AllowAnonymous]
        public ActionResult SeedData()
        {
            try
            {
                // Veritabanını oluştur ve tabloları yarat
                using (var context = new ApplicationDbContext())
                {
                    if (!context.Database.Exists())
                    {
                        // Veritabanını oluştur
                        context.Database.Create();
                    }
                    else
                    {
                        // Tabloların oluşturulup oluşturulmadığını kontrol et
                        try
                        {
                            // Entity modellerini kullanarak tabloları oluştur
                            if (!TableExists(context, "AspNetRoles"))
                            {
                                context.Database.ExecuteSqlCommand(
                                    @"CREATE TABLE `AspNetRoles` (
                                    `Id` varchar(128) NOT NULL,
                                    `Name` varchar(256) NOT NULL,
                                    PRIMARY KEY (`Id`)
                                  ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;");
                                
                                context.Database.ExecuteSqlCommand(
                                    @"CREATE TABLE `AspNetUsers` (
                                    `Id` varchar(128) NOT NULL,
                                    `Email` varchar(256) DEFAULT NULL,
                                    `EmailConfirmed` tinyint(1) NOT NULL,
                                    `PasswordHash` longtext,
                                    `SecurityStamp` longtext,
                                    `PhoneNumber` longtext,
                                    `PhoneNumberConfirmed` tinyint(1) NOT NULL,
                                    `TwoFactorEnabled` tinyint(1) NOT NULL,
                                    `LockoutEndDateUtc` datetime DEFAULT NULL,
                                    `LockoutEnabled` tinyint(1) NOT NULL,
                                    `AccessFailedCount` int(11) NOT NULL,
                                    `UserName` varchar(256) NOT NULL,
                                    `Ad` varchar(50) NOT NULL,
                                    `Soyad` varchar(50) NOT NULL,
                                    `Rol` int(11) NOT NULL,
                                    `CreatedAt` datetime NOT NULL,
                                    `UpdatedAt` datetime NOT NULL,
                                    PRIMARY KEY (`Id`)
                                  ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;");
                                
                                context.Database.ExecuteSqlCommand(
                                    @"CREATE TABLE `AspNetUserRoles` (
                                    `UserId` varchar(128) NOT NULL,
                                    `RoleId` varchar(128) NOT NULL,
                                    PRIMARY KEY (`UserId`,`RoleId`),
                                    KEY `IX_RoleId` (`RoleId`),
                                    CONSTRAINT `FK_AspNetUserRoles_AspNetRoles_RoleId` FOREIGN KEY (`RoleId`) REFERENCES `AspNetRoles` (`Id`) ON DELETE CASCADE,
                                    CONSTRAINT `FK_AspNetUserRoles_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `AspNetUsers` (`Id`) ON DELETE CASCADE
                                  ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;");
                                
                                context.Database.ExecuteSqlCommand(
                                    @"CREATE TABLE `AspNetUserClaims` (
                                    `Id` int(11) NOT NULL AUTO_INCREMENT,
                                    `UserId` varchar(128) NOT NULL,
                                    `ClaimType` longtext,
                                    `ClaimValue` longtext,
                                    PRIMARY KEY (`Id`),
                                    KEY `IX_UserId` (`UserId`),
                                    CONSTRAINT `FK_AspNetUserClaims_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `AspNetUsers` (`Id`) ON DELETE CASCADE
                                  ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;");
                                
                                context.Database.ExecuteSqlCommand(
                                    @"CREATE TABLE `AspNetUserLogins` (
                                    `LoginProvider` varchar(128) NOT NULL,
                                    `ProviderKey` varchar(128) NOT NULL,
                                    `UserId` varchar(128) NOT NULL,
                                    PRIMARY KEY (`LoginProvider`,`ProviderKey`,`UserId`),
                                    KEY `IX_UserId` (`UserId`),
                                    CONSTRAINT `FK_AspNetUserLogins_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `AspNetUsers` (`Id`) ON DELETE CASCADE
                                  ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;");
                            }
                            
                            // Müşteri tablosu oluştur
                            if (!TableExists(context, "Musteriler"))
                            {
                                context.Database.ExecuteSqlCommand(
                                    @"CREATE TABLE `Musteriler` (
                                    `Id` int(11) NOT NULL AUTO_INCREMENT,
                                    `Ad` varchar(50) NOT NULL,
                                    `Soyad` varchar(50) NOT NULL,
                                    `Email` varchar(100) DEFAULT NULL,
                                    `Telefon` varchar(20) DEFAULT NULL,
                                    `Adres` varchar(250) DEFAULT NULL,
                                    `CreatedAt` datetime NOT NULL,
                                    `UpdatedAt` datetime NOT NULL,
                                    PRIMARY KEY (`Id`)
                                  ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;");
                            }
                            
                            // Araç tablosu oluştur
                            if (!TableExists(context, "Araclar"))
                            {
                                context.Database.ExecuteSqlCommand(
                                    @"CREATE TABLE `Araclar` (
                                    `Id` int(11) NOT NULL AUTO_INCREMENT,
                                    `MusteriId` int(11) NOT NULL,
                                    `Marka` varchar(50) NOT NULL,
                                    `Model` varchar(50) NOT NULL,
                                    `ModelYili` int(11) NOT NULL,
                                    `Plaka` varchar(20) DEFAULT NULL,
                                    `VIN` varchar(50) DEFAULT NULL,
                                    `Kilometre` int(11) DEFAULT 0,
                                    `CreatedAt` datetime NOT NULL,
                                    `UpdatedAt` datetime NOT NULL,
                                    PRIMARY KEY (`Id`),
                                    KEY `IX_MusteriId` (`MusteriId`),
                                    CONSTRAINT `FK_Araclar_Musteriler_MusteriId` FOREIGN KEY (`MusteriId`) REFERENCES `Musteriler` (`Id`) ON DELETE CASCADE
                                  ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;");
                            }
                            
                            // Servis tablosu oluştur
                            if (!TableExists(context, "Servisler"))
                            {
                                context.Database.ExecuteSqlCommand(
                                    @"CREATE TABLE `Servisler` (
                                    `Id` int(11) NOT NULL AUTO_INCREMENT,
                                    `AracId` int(11) NOT NULL,
                                    `ServisTarihi` datetime NOT NULL,
                                    `TeslimTarihi` datetime DEFAULT NULL,
                                    `Açıklama` varchar(500) DEFAULT NULL,
                                    `ServisBedeli` decimal(10,2) NOT NULL DEFAULT '0.00',
                                    `Durumu` int(11) NOT NULL,
                                    `CreatedAt` datetime NOT NULL,
                                    `UpdatedAt` datetime NOT NULL,
                                    PRIMARY KEY (`Id`),
                                    KEY `IX_AracId` (`AracId`),
                                    CONSTRAINT `FK_Servisler_Araclar_AracId` FOREIGN KEY (`AracId`) REFERENCES `Araclar` (`Id`) ON DELETE CASCADE
                                  ) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;");
                            }
                            
                            context.SaveChanges();
                        }
                        catch (Exception ex)
                        {
                            ViewBag.TableCreationError = "Tablolar oluşturulurken hata: " + ex.Message;
                        }
                    }

                    // Veritabanı bağlantısını test et
                    context.Database.Connection.Open();
                    ViewBag.ConnectionStatus = "Veritabanı bağlantısı başarılı.";
                    context.Database.Connection.Close();
                }

                // Admin kullanıcısını oluştur
                CreateAdminUser();

                // Örnek müşteriler ekle
                AddSampleCustomers();

                // Başarı mesajı
                ViewBag.Message = "Veri oluşturma işlemi başarıyla tamamlandı.";
                return View();
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Veri oluşturma sırasında bir hata oluştu: " + ex.Message;
                if (ex.InnerException != null)
                {
                    ViewBag.InnerErrorMessage = ex.InnerException.Message;
                }
                return View();
            }
        }

        private bool TableExists(ApplicationDbContext context, string tableName)
        {
            try
            {
                // MySQL'de tablo varlığını kontrol etme
                var result = context.Database.SqlQuery<int>($"SELECT COUNT(*) FROM information_schema.tables WHERE table_schema = 'ornekoto' AND table_name = '{tableName}'").FirstOrDefault();
                return result > 0;
            }
            catch
            {
                // Hata durumunda tabloyu oluşturacağız
                return false;
            }
        }

        private void CreateAdminUser()
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext()));

            // Admin rolünü oluştur
            if (!roleManager.RoleExists("Admin"))
            {
                var roleResult = roleManager.Create(new IdentityRole("Admin"));
            }

            // Admin kullanıcısını oluştur
            var user = userManager.FindByName("admin@example.com");
            if (user == null)
            {
                var adminUser = new ApplicationUser
                {
                    UserName = "admin@example.com",
                    Email = "admin@example.com",
                    Ad = "Admin",
                    Soyad = "User",
                    Rol = UserRole.Admin,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };

                var userResult = userManager.Create(adminUser, "Admin123!");
                if (userResult.Succeeded)
                {
                    var result = userManager.AddToRole(adminUser.Id, "Admin");
                }
            }
        }

        private void AddSampleCustomers()
        {
            using (var context = new ApplicationDbContext())
            {
                // Örnek müşterileri ekle
                if (!context.Musteriler.Any())
                {
                    // Örnek Müşteri 1
                    var musteri1 = new Musteri
                    {
                        Ad = "Ahmet",
                        Soyad = "Yılmaz",
                        Email = "ahmet.yilmaz@example.com",
                        Telefon = "5551234567",
                        Adres = "Atatürk Cad. No:123 İstanbul",
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now
                    };
                    context.Musteriler.Add(musteri1);
                    context.SaveChanges();

                    // Müşteri 1'e ait Araç
                    var arac1 = new Arac
                    {
                        MusteriId = musteri1.Id,
                        Marka = "Toyota",
                        Model = "Corolla",
                        ModelYili = 2020,
                        Plaka = "34AB1234",
                        VIN = "JT2BF22K1X0123456",
                        Kilometre = 25000,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now
                    };
                    context.Araclar.Add(arac1);
                    context.SaveChanges();

                    // Örnek Müşteri 2
                    var musteri2 = new Musteri
                    {
                        Ad = "Ayşe",
                        Soyad = "Demir",
                        Email = "ayse.demir@example.com",
                        Telefon = "5559876543",
                        Adres = "Cumhuriyet Mah. Sokak No:45 Ankara",
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now
                    };
                    context.Musteriler.Add(musteri2);
                    context.SaveChanges();

                    // Müşteri 2'ye ait Araç
                    var arac2 = new Arac
                    {
                        MusteriId = musteri2.Id,
                        Marka = "Honda",
                        Model = "Civic",
                        ModelYili = 2019,
                        Plaka = "06CD5678",
                        VIN = "1HGCM82633A123456",
                        Kilometre = 35000,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now
                    };
                    context.Araclar.Add(arac2);
                    context.SaveChanges();

                    // Örnek Servis Kaydı
                    var servis1 = new Servis
                    {
                        AracId = arac1.Id,
                        ServisTarihi = DateTime.Now.AddDays(-30),
                        TeslimTarihi = DateTime.Now.AddDays(-29),
                        Açıklama = "Yağ değişimi ve genel bakım yapıldı.",
                        ServisBedeli = 750.00m,
                        Durumu = ServisDurumu.Tamamlandı,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now
                    };
                    context.Servisler.Add(servis1);
                    context.SaveChanges();
                }
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
