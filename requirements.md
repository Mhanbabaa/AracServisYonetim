# Araç Servis Yönetim Sistemi Gereksinimleri

## Genel Bakış
Bu proje, C# .NET MVC teknolojisiyle bir araç servis yönetim sistemi geliştirmeyi amaçlamaktadır. Sistem, servis işlemlerini, müşteri bilgilerini, araç kayıtlarını ve personel yönetimini merkezi bir platformda toplayacaktır.

## Teknolojik Yapı
- **Backend**: C# .NET MVC
- **Frontend**: Razor Views, Bootstrap
- **Veritabanı**: MySQL
- **Kimlik Doğrulama**: ASP.NET Identity

## Modüller ve Özellikler

### 1. Müşteri Yönetimi
- Müşteri ekleme/düzenleme/silme
- Müşteriye ait araç bilgileri görüntüleme
- İletişim bilgileri (ad, soyad, e-posta, telefon)
- Adres bilgileri

### 2. Araç Yönetimi
- Araç ekleme/düzenleme/silme
- Plaka, marka, model, yıl, renk bilgileri
- Kilometre takibi
- Garanti durumu yönetimi
- Araçları müşterilerle ilişkilendirme

### 3. Servis İşlemleri
- Yeni servis kaydı oluşturma
- Servis durum takibi (Devam ediyor/Tamamlandı/İptal edildi)
- İşçilik ve parça ücretleri yönetimi
- Toplam ücret hesaplama
- Teknisyen atama
- Servis açıklaması/detayları
- Başlangıç ve bitiş tarihleri

### 4. Personel Yönetimi
- Personel ekleme/düzenleme/silme
- Rol yönetimi (Admin/Teknisyen)
- Şifre yönetimi
- E-posta ve iletişim bilgileri

### 5. Dashboard ve Raporlama
- Aktif servis işlemleri sayısı
- Toplam araç sayısı
- Müşteri sayısı
- Tamamlanan servisler sayısı
- Son servis işlemleri listesi

## Kullanıcı Arayüzü Gereksinimleri
- Responsive tasarım
- Türkçe dil desteği
- Kolay navigasyon için sol menü
- Arama ve filtreleme özellikleri
- Modal pencereler ile form işlemleri
- Bootstrap tabanlı temiz ve modern arayüz
