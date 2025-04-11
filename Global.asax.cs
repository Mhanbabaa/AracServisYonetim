using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using AracServisYonetim.Models;
using System.Data.Entity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace AracServisYonetim
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            // BundleConfig.RegisterBundles(BundleTable.Bundles); // Yorum satırına alındı

            try
            {
                // EntityFramework başlatma ayarı
                Database.SetInitializer(new CreateDatabaseIfNotExists<ApplicationDbContext>());
                
                // Veritabanını başlat ve Admin kullanıcısı oluştur
                using (var context = new ApplicationDbContext())
                {
                    context.Database.Initialize(true);

                    // Admin kullanıcısı oluşturma
                    CreateAdminUser(context);
                }
            }
            catch (Exception ex)
            {
                // Hata durumunda loglama yapılabilir
                System.Diagnostics.Debug.WriteLine("Veritabanı başlatılırken hata oluştu: " + ex.Message);
            }
        }

        private void CreateAdminUser(ApplicationDbContext context)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            // Admin rolünü oluştur
            if (!roleManager.RoleExists("Admin"))
            {
                roleManager.Create(new IdentityRole("Admin"));
            }

            // Admin kullanıcısı var mı kontrol et
            var adminUser = userManager.FindByName("admin@example.com");
            if (adminUser == null)
            {
                // Admin kullanıcısı oluştur
                var user = new ApplicationUser
                {
                    UserName = "admin@example.com",
                    Email = "admin@example.com",
                    Ad = "Admin",
                    Soyad = "User",
                    Rol = UserRole.Admin,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };

                var result = userManager.Create(user, "Admin123!");
                if (result.Succeeded)
                {
                    // Kullanıcıya Admin rolü atama
                    userManager.AddToRole(user.Id, "Admin");
                }
            }
        }
    }
}
