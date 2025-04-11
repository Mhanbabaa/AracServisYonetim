# Araç Servis Yönetim Sistemi - Proje Özeti

Bu proje, C# .NET MVC teknolojisiyle geliştirilmiş kapsamlı bir araç servis yönetim sistemidir. Sistem, servis işlemlerini, müşteri bilgilerini, araç kayıtlarını ve personel yönetimini merkezi bir platformda toplamaktadır.

## Proje Yapısı

Proje aşağıdaki ana modüllerden oluşmaktadır:

1. **Müşteri Yönetimi**
   - Müşteri ekleme/düzenleme/silme
   - Müşteriye ait araç bilgileri
   - İletişim ve adres bilgileri

2. **Araç Yönetimi**
   - Araç ekleme/düzenleme/silme
   - Plaka, marka, model, yıl bilgileri
   - Kilometre takibi
   - Garanti durumu yönetimi

3. **Servis İşlemleri**
   - Yeni servis kaydı oluşturma
   - Servis durum takibi (Devam ediyor/Tamamlandı/İptal edildi)
   - İşçilik ve parça ücretleri yönetimi
   - Teknisyen atama

4. **Personel Yönetimi**
   - Personel ekleme/düzenleme/silme
   - Rol yönetimi (Admin/Teknisyen)
   - Şifre yönetimi

5. **Dashboard ve Raporlama**
   - Ana panel istatistikleri
   - Servis durumu özeti
   - Araç ve müşteri sayıları

## Teknolojik Yapı

- **Backend**: C# .NET MVC
- **Frontend**: Razor Views, Bootstrap
- **Veritabanı**: MySQL
- **Authentication**: ASP.NET Identity

## Kurulum Talimatları

Detaylı kurulum talimatları için uygulamadaki `/Setup` sayfasını ziyaret ediniz.

## Test Verileri

Sistem test verileri oluşturmak için `/Test/SeedData` sayfasını kullanabilirsiniz. Bu işlem, veritabanında örnek müşteriler, araçlar, servis kayıtları ve kullanıcılar oluşturacaktır.

## Giriş Bilgileri

- **E-posta**: admin@example.com
- **Şifre**: Admin123!

## Geliştirici Bilgileri

Bu proje, müşteri talebine göre özel olarak geliştirilmiştir. Tüm hakları saklıdır.

Tarih: Nisan 2025
