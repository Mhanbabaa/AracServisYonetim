using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Data.Entity;
using MySql.Data.EntityFramework;
using MySql.Data.MySqlClient;

namespace AracServisYonetim.Models
{
    public enum UserRole
    {
        [Display(Name = "Admin")]
        Admin = 0,
        
        [Display(Name = "Teknisyen")]
        Teknisyen = 1
    }

    // ApplicationUser sınıfı, ASP.NET Identity'nin IdentityUser sınıfını genişletir
    public class ApplicationUser : IdentityUser
    {
        [Required(ErrorMessage = "Ad alanı zorunludur.")]
        [Display(Name = "Ad")]
        [StringLength(50)]
        public string Ad { get; set; }

        [Required(ErrorMessage = "Soyad alanı zorunludur.")]
        [Display(Name = "Soyad")]
        [StringLength(50)]
        public string Soyad { get; set; }

        [Required(ErrorMessage = "Rol seçimi zorunludur.")]
        [Display(Name = "Rol")]
        public UserRole Rol { get; set; }

        [Display(Name = "Oluşturulma Tarihi")]
        public DateTime CreatedAt { get; set; }

        [Display(Name = "Güncellenme Tarihi")]
        public DateTime UpdatedAt { get; set; }

        public ApplicationUser()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        // Kullanıcının tam adını döndüren yardımcı metot
        public string TamAd
        {
            get { return $"{Ad} {Soyad}"; }
        }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // authenticationType, CookieAuthenticationOptions.AuthenticationType ile aynı olmalıdır
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            
            // Özel kullanıcı bilgilerini buraya ekleyin
            userIdentity.AddClaim(new Claim("Ad", Ad));
            userIdentity.AddClaim(new Claim("Soyad", Soyad));
            userIdentity.AddClaim(new Claim("Rol", Rol.ToString()));
            
            return userIdentity;
        }
    }

    // Custom MySQL database initializer
    public class CustomMySqlInitializer : IDatabaseInitializer<ApplicationDbContext>
    {
        public void InitializeDatabase(ApplicationDbContext context)
        {
            // Force recreate the database to apply correct types
            try
            {
                if (context.Database.Exists())
                {
                    context.Database.Delete();
                }
                context.Database.Create();
                
                // Additional MySQL-specific configuration
                using (var connection = new MySqlConnection(context.Database.Connection.ConnectionString))
                {
                    connection.Open();
                    
                    // Set MySQL to properly handle DECIMAL types
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = "SET GLOBAL sql_mode = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION';";
                        command.ExecuteNonQuery();
                    }
                }
                
                // Create default admin user
                CreateDefaultAdminUser(context);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Database initialization error: " + ex.Message);
            }
        }
        
        private void CreateDefaultAdminUser(ApplicationDbContext context)
        {
            var userManager = new Microsoft.AspNet.Identity.UserManager<ApplicationUser>(
                new Microsoft.AspNet.Identity.EntityFramework.UserStore<ApplicationUser>(context));
                
            var roleManager = new Microsoft.AspNet.Identity.RoleManager<Microsoft.AspNet.Identity.EntityFramework.IdentityRole>(
                new Microsoft.AspNet.Identity.EntityFramework.RoleStore<Microsoft.AspNet.Identity.EntityFramework.IdentityRole>(context));

            // Create Admin role if it doesn't exist
            if (!roleManager.RoleExists("Admin"))
            {
                roleManager.Create(new Microsoft.AspNet.Identity.EntityFramework.IdentityRole("Admin"));
            }

            // Create Admin user if it doesn't exist
            var adminUser = userManager.FindByName("admin@example.com");
            if (adminUser == null)
            {
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
                    userManager.AddToRole(user.Id, "Admin");
                }
            }
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        static ApplicationDbContext()
        {
            // Set custom initializer for MySQL
            Database.SetInitializer(new CustomMySqlInitializer());
        }
        
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
            // İstek başına proxy oluşturmayı devre dışı bırakma (performans için)
            this.Configuration.ProxyCreationEnabled = false;
            this.Configuration.LazyLoadingEnabled = false;
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public System.Data.Entity.DbSet<Musteri> Musteriler { get; set; }
        public System.Data.Entity.DbSet<Arac> Araclar { get; set; }
        public System.Data.Entity.DbSet<Servis> Servisler { get; set; }
        
        // ASP.NET Identity tabloları - bunlar zaten IdentityDbContext tarafından sağlanıyor
        // public System.Data.Entity.DbSet<ApplicationUser> Users { get; set; } 
        // public System.Data.Entity.DbSet<IdentityRole> Roles { get; set; }
        public System.Data.Entity.DbSet<IdentityUserRole> UserRoles { get; set; }
        public System.Data.Entity.DbSet<IdentityUserClaim> UserClaims { get; set; }
        public System.Data.Entity.DbSet<IdentityUserLogin> UserLogins { get; set; }

        protected override void OnModelCreating(System.Data.Entity.DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Tabloların adlarını özelleştirme
            modelBuilder.Entity<ApplicationUser>().ToTable("Kullanicilar");
            modelBuilder.Entity<IdentityRole>().ToTable("Roller");
            modelBuilder.Entity<IdentityUserRole>().ToTable("KullaniciRoller");
            modelBuilder.Entity<IdentityUserClaim>().ToTable("KullaniciTalepler");
            modelBuilder.Entity<IdentityUserLogin>().ToTable("KullaniciGirisler");

            // İlişkileri tanımlama
            modelBuilder.Entity<Musteri>()
                .HasMany(m => m.Araclar)
                .WithRequired(a => a.Musteri)
                .HasForeignKey(a => a.MusteriId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Arac>()
                .HasRequired(a => a.Musteri)
                .WithMany(m => m.Araclar)
                .HasForeignKey(a => a.MusteriId);
                
            // Define decimal properties properly for MySQL compatibility
            modelBuilder.Entity<Servis>()
                .Property(s => s.ServisBedeli)
                .HasColumnType("decimal")
                .HasPrecision(10, 2);
        }
    }
}
