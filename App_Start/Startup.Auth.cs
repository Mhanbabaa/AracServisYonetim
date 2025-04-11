using AracServisYonetim.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Owin;
using System;

namespace AracServisYonetim
{
    public partial class Startup
    {
        // Kimlik doğrulama yapılandırması için bu metot çağrılır
        public void ConfigureAuth(IAppBuilder app)
        {
            // Veritabanı bağlamını, kullanıcı yöneticisini ve giriş yöneticisini istek başına tek bir örnek olarak kullanacak şekilde yapılandırma
            app.CreatePerOwinContext(ApplicationDbContext.Create);
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
            app.CreatePerOwinContext<ApplicationSignInManager>(ApplicationSignInManager.Create);

            // Oturum açan kullanıcı hakkındaki bilgileri depolamak için çerez kullanmayı etkinleştirme
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
                Provider = new CookieAuthenticationProvider
                {
                    // Kullanıcı oturum açtığında güvenlik damgasının uygulama tarafından doğrulanmasını sağlar.
                    OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<ApplicationUserManager, ApplicationUser>(
                        validateInterval: TimeSpan.FromMinutes(30),
                        regenerateIdentity: (manager, user) => user.GenerateUserIdentityAsync(manager))
                }
            });
            
            // Üçüncü taraf oturum açma sağlayıcılarıyla oturum açmayı etkinleştirmek için aşağıdaki satırların açıklamasını kaldırın
            // app.UseMicrosoftAccountAuthentication(
            //    clientId: "",
            //    clientSecret: "");

            // app.UseTwitterAuthentication(
            //   consumerKey: "",
            //   consumerSecret: "");

            // app.UseFacebookAuthentication(
            //   appId: "",
            //   appSecret: "");

            // app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions()
            // {
            //    ClientId = "",
            //    ClientSecret = ""
            // });
        }
    }
}
