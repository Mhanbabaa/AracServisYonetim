# Araç Servis Yönetim Sistemi Proje Yapısı

## Proje Klasör Yapısı

```
AracServisYonetim/
├── AracServisYonetim.sln
├── AracServisYonetim/
│   ├── App_Start/
│   │   ├── BundleConfig.cs
│   │   ├── FilterConfig.cs
│   │   ├── IdentityConfig.cs
│   │   ├── RouteConfig.cs
│   │   └── Startup.Auth.cs
│   ├── Content/
│   │   ├── bootstrap.css
│   │   ├── site.css
│   │   └── images/
│   ├── Controllers/
│   │   ├── AccountController.cs
│   │   ├── AracController.cs
│   │   ├── HomeController.cs
│   │   ├── MusteriController.cs
│   │   ├── PersonelController.cs
│   │   └── ServisController.cs
│   ├── Models/
│   │   ├── AccountViewModels.cs
│   │   ├── Arac.cs
│   │   ├── IdentityModels.cs
│   │   ├── Musteri.cs
│   │   ├── Personel.cs
│   │   └── Servis.cs
│   ├── Scripts/
│   │   ├── bootstrap.js
│   │   ├── jquery.js
│   │   └── site.js
│   ├── Views/
│   │   ├── Account/
│   │   │   ├── Login.cshtml
│   │   │   └── Register.cshtml
│   │   ├── Arac/
│   │   │   ├── Index.cshtml
│   │   │   ├── Create.cshtml
│   │   │   ├── Edit.cshtml
│   │   │   └── Delete.cshtml
│   │   ├── Home/
│   │   │   ├── Index.cshtml
│   │   │   └── About.cshtml
│   │   ├── Musteri/
│   │   │   ├── Index.cshtml
│   │   │   ├── Create.cshtml
│   │   │   ├── Edit.cshtml
│   │   │   └── Delete.cshtml
│   │   ├── Personel/
│   │   │   ├── Index.cshtml
│   │   │   ├── Create.cshtml
│   │   │   ├── Edit.cshtml
│   │   │   └── Delete.cshtml
│   │   ├── Servis/
│   │   │   ├── Index.cshtml
│   │   │   ├── Create.cshtml
│   │   │   ├── Edit.cshtml
│   │   │   └── Delete.cshtml
│   │   ├── Shared/
│   │   │   ├── _Layout.cshtml
│   │   │   ├── _LoginPartial.cshtml
│   │   │   └── Error.cshtml
│   │   └── Web.config
│   ├── Global.asax
│   ├── Web.config
│   └── packages.config
└── AracServisYonetim.Tests/
    ├── Controllers/
    │   └── HomeControllerTest.cs
    ├── App.config
    └── packages.config
```

## Veritabanı Şeması

```
Musteri
- Id (int, PK)
- Ad (string)
- Soyad (string)
- Email (string)
- Telefon (string)
- Adres (string)
- CreatedAt (DateTime)
- UpdatedAt (DateTime)

Arac
- Id (int, PK)
- Plaka (string)
- Marka (string)
- Model (string)
- Yil (int)
- Renk (string)
- Kilometre (int)
- GarantiKapsaminda (bool)
- MusteriId (int, FK)
- CreatedAt (DateTime)
- UpdatedAt (DateTime)

Servis
- Id (int, PK)
- AracId (int, FK)
- Aciklama (string)
- Durum (enum: DevamEdiyor, Tamamlandi, IptalEdildi)
- BaslangicTarihi (DateTime)
- BitisTarihi (DateTime)
- IscilikUcreti (decimal)
- ParcaUcreti (decimal)
- ToplamUcret (decimal)
- TeknisyenId (int, FK)
- CreatedAt (DateTime)
- UpdatedAt (DateTime)

ApplicationUser (ASP.NET Identity)
- Id (string, PK)
- Email (string)
- PasswordHash (string)
- SecurityStamp (string)
- PhoneNumber (string)
- UserName (string)
- Ad (string)
- Soyad (string)
- Rol (string)
- CreatedAt (DateTime)
- UpdatedAt (DateTime)
```

## Kullanılacak NuGet Paketleri

- Microsoft.AspNet.Mvc
- Microsoft.AspNet.Identity.EntityFramework
- Microsoft.AspNet.Identity.Owin
- EntityFramework
- MySql.Data.EntityFramework
- Bootstrap
- jQuery
- Modernizr
- Respond
- Microsoft.jQuery.Unobtrusive.Validation
