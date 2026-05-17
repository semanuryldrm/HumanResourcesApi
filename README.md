[README (6).md](https://github.com/user-attachments/files/27905718/README.6.md)# .NET Core İnsan Kaynakları Yönetim Sistemi API

ASP.NET Core ile geliştirilmiş insan kaynakları yönetim sistemi projesidir. Proje kapsamında kullanıcı işlemleri, rol bazlı yetkilendirme, departman yönetimi, çalışan yönetimi, izin yönetimi, bordro hesaplama, bordro PDF raporu alma ve dashboard istatistikleri web tabanlı olarak gerçekleştirilmiştir.

## 🎯 Özet

Bu proje, BMU1208 Web Tabanlı Programlama dersi final projesi kapsamında geliştirilmiştir. İnsan kaynakları süreçlerinde kullanılan temel işlemlerin tek bir sistem üzerinden takip edilebilmesi amaçlanmıştır.

Uygulama sayesinde kullanıcı kaydı ve girişi yapılabilmekte, Admin / IK / Personel rollerine göre farklı yetkiler uygulanabilmekte, departmanlar oluşturulabilmekte, çalışan bilgileri yönetilebilmekte, çalışanlara ait izin talepleri takip edilebilmekte ve çalışanların brüt maaşları üzerinden örnek bordro hesaplaması yapılabilmektedir. Ayrıca oluşturulan bordro kayıtları PDF olarak indirilebilmektedir. Proje, ASP.NET Core MVC ve Web API yapısı birlikte kullanılarak hazırlanmıştır.

## 🙋‍♀️ Hazırlayan

| Bilgi | Açıklama |
|---|---|
| Öğrenci | Semanur Yıldırım |
| Öğrenci No | 23080410007 |
| Proje Kodu | P30 |
| Ders | BMU1208 Web Tabanlı Programlama |
| Kurum | Bitlis Eren Üniversitesi |
| Fakülte | Mühendislik-Mimarlık Fakültesi |
| Bölüm | Bilgisayar Mühendisliği |
| Ders Yürütücüsü | Dr. Öğr. Üyesi Davut ARI |
| E-posta | semanuryildirim.03@gmail.com |

## 🎥 Demo

Bu proje yerel geliştirme ortamında çalıştırılacak şekilde hazırlanmıştır. Canlı demo bağlantısı bulunmamaktadır.

Uygulama çalıştırıldıktan sonra terminalde görünen localhost adresi tarayıcı üzerinden açılarak sistem kullanılabilir.

Örnek:

```text
http://localhost:5207
```

Swagger arayüzü için:

```text
http://localhost:5207/swagger
```

> Not: Port numarası bilgisayardaki çalışma ayarlarına göre değişebilir. Kesin adres için `dotnet run` çıktısındaki `Now listening on:` satırı kontrol edilmelidir.

## 👤 Demo Hesaplar

Proje çalıştırıldığında demo kullanıcılar otomatik olarak oluşturulacak şekilde hazırlanmıştır.

| Rol | E-posta | Şifre |
|---|---|---|
| Admin | admin@example.com | 123456 |
| IK | ik@example.com | 123456 |
| Personel | personel@example.com | 123456 |

Demo kullanıcıların yetkileri farklıdır:

- **Admin**: Tüm yönetim işlemlerini görebilir ve kullanabilir.
- **IK**: Departman, çalışan, izin ve bordro süreçlerini yönetebilir.
- **Personel**: Sadece kendisine uygun arayüzü görür, izin talebi oluşturabilir ve yetkisi olmayan yönetim butonlarını görmez.

## ✨ Ana Özellikler

- Kullanıcı kayıt ve giriş işlemleri
- JWT tabanlı kimlik doğrulama
- Admin / IK / Personel rollerine göre rol bazlı yetkilendirme
- Role göre değişen frontend görünümü
- Demo kullanıcıların otomatik oluşturulması
- Departman ekleme, listeleme, güncelleme ve silme işlemleri
- Çalışan ekleme, listeleme, güncelleme ve silme işlemleri
- Çalışanların departmanlarla ilişkilendirilmesi
- İzin talebi oluşturma, listeleme, onaylama, reddetme ve silme işlemleri
- Brüt maaş üzerinden örnek bordro hesaplama
- Bordro kayıtlarını PDF olarak indirme
- Dashboard üzerinde departman, çalışan, bordro ve izin istatistikleri
- Modern ve responsive web arayüzü
- ASP.NET Core MVC yapısına uygun Razor View arayüzü
- Web API endpointleri ile veri işlemleri
- Entity Framework Core ile veritabanı yönetimi
- SQL Server / SQL Server Express bağlantısı
- Swagger arayüzü ile API test imkânı
- GitHub teslim klasör yapısına uygun proje düzeni

## 🧰 Tech Stack

| Teknoloji | Kullanım Amacı |
|---|---|
| C# | Backend geliştirme dili |
| ASP.NET Core 8 | Web API ve MVC altyapısı |
| ASP.NET Core MVC | Razor View tabanlı kullanıcı arayüzü |
| ASP.NET Core Web API | JSON tabanlı API endpointleri |
| Entity Framework Core | Veritabanı işlemleri |
| SQL Server / SQL Server Express | Veritabanı yönetimi |
| ASP.NET Core Identity | Kullanıcı ve rol yönetimi |
| JWT | Kimlik doğrulama |
| QuestPDF | Bordro raporlarını PDF olarak oluşturma |
| Swagger / OpenAPI | API test ve dokümantasyonu |
| HTML, CSS, JavaScript | Arayüz geliştirme |
| Git & GitHub | Versiyon kontrolü ve proje teslimi |

Teknoloji seçimleri ve proje detayları `PROJE-RAPORU.md` ve `PROJE-RAPORU-SABLON.docx` dosyalarında açıklanmıştır.

## 🏗 Mimari

Proje, ASP.NET Core MVC ve Web API yapısının birlikte kullanıldığı hibrit bir mimariye sahiptir.

Genel yapı şu şekildedir:

```text
Kullanıcı Arayüzü
        |
        v
Views / wwwroot
        |
        v
Controllers
        |
        v
Services
        |
        v
Data / ApplicationDbContext
        |
        v
SQL Server Veritabanı
```

Projede sayfa yapıları `Views/` klasörü altında, CSS ve JavaScript dosyaları ise `wwwroot/` klasörü altında tutulmuştur. API isteklerini karşılayan yapılar `Controllers/` klasöründe, iş mantığı ise `Services/` klasöründe yer almaktadır. Veritabanı bağlantısı `Data/ApplicationDbContext.cs` dosyası üzerinden yönetilmektedir.

## 🚀 Kurulum

### Gereksinimler

- .NET 8 SDK
- SQL Server veya SQL Server Express
- Visual Studio 2022 veya Visual Studio Code
- Git

### Adım Adım Kurulum

Öncelikle repository bilgisayara klonlanır:

```powershell
git clone https://github.com/semanuryldrm/HumanResourcesApi.git
```

Proje klasörüne girilir:

```powershell
cd HumanResourcesApi
```

Kaynak kodların bulunduğu `repo/` klasörüne geçilir:

```powershell
cd repo
```

Bağımlılıklar yüklenir:

```powershell
dotnet restore
```

Veritabanı migration işlemi uygulanır:

```powershell
dotnet ef database update
```

Proje çalıştırılır:

```powershell
dotnet run
```

Uygulama çalıştıktan sonra terminalde görünen localhost adresi tarayıcıda açılır.

## 🗄️ Veritabanı Bilgisi

Proje SQL Server / SQL Server Express üzerinde çalışacak şekilde hazırlanmıştır. Veritabanı bağlantısı `repo/appsettings.json` dosyasında bulunan `DefaultConnection` alanından düzenlenebilir.

Örnek bağlantı yapısı:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=SEMA\\\\SQLEXPRESS;Database=HumanResourcesDb;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true"
}
```

Farklı bir bilgisayarda çalıştırılırken `Server` değeri ilgili SQL Server adına göre güncellenmelidir.

## 🧪 Test

Proje manuel olarak web arayüzü ve Swagger üzerinden test edilmiştir. Temel olarak aşağıdaki işlemler kontrol edilmiştir:

- Kullanıcı kayıt işlemi
- Kullanıcı giriş işlemi
- Demo kullanıcılarla giriş testi
- Admin / IK / Personel rollerine göre arayüz kontrolü
- Rol bazlı yetkilendirme kontrolü
- Departman ekleme, listeleme, güncelleme ve silme
- Çalışan ekleme, listeleme, güncelleme ve silme
- Çalışanın departmanla ilişkilendirilmesi
- İzin talebi oluşturma
- İzin talebini onaylama ve reddetme
- Bordro hesaplama
- Bordro PDF raporu indirme
- Dashboard istatistiklerinin güncellenmesi
- Verilerin SQL Server veritabanına kaydedilmesi

## 📁 Klasör Yapısı

Bu repository, ders kapsamında verilen final proje teslim klasör yapısına uygun olarak düzenlenmiştir.

```text
.
├── README.md
├── PROJE-RAPORU.md
├── PROJE-RAPORU-SABLON.docx
├── LICENSE
├── .env.example
├── .gitignore
└── repo/
    ├── Controllers/
    ├── Data/
    ├── DTOs/
    ├── Migrations/
    ├── Models/
    ├── Properties/
    ├── Services/
    ├── Views/
    ├── wwwroot/
    ├── Program.cs
    ├── appsettings.json
    ├── appsettings.Development.json
    ├── HumanResourcesApi.csproj
    ├── HumanResourcesApi.sln
    └── README.md
```

## 📌 Proje Modülleri

### Kullanıcı İşlemleri

Sistemde kullanıcı kayıt ve giriş işlemleri bulunmaktadır. Kullanıcı giriş yaptığında JWT token üretilir ve korumalı API işlemlerinde bu token kullanılır.

### Rol Bazlı Yetkilendirme

Projede Admin, IK ve Personel rolleri bulunmaktadır. Admin ve IK rolleri yönetim işlemlerine erişebilirken, Personel rolü daha sınırlı bir arayüz üzerinden sistemi kullanmaktadır. Bu sayede kullanıcılar sadece yetkili oldukları işlemleri görebilmekte ve gerçekleştirebilmektedir.

### Departman Yönetimi

Departman modülünde departman ekleme, listeleme, güncelleme ve silme işlemleri yapılabilmektedir. Çalışan kayıtları departmanlarla ilişkilendirildiği için bu modül sistemin temel parçalarından biridir.

### Çalışan Yönetimi

Çalışan modülünde personel bilgileri sisteme eklenebilir. Çalışan adı, soyadı, e-posta adresi, telefon numarası, işe giriş tarihi, brüt maaş ve bağlı olduğu departman bilgileri tutulmaktadır.

### İzin Yönetimi

İzin yönetimi modülünde çalışanlara ait izin talepleri oluşturulabilir ve listelenebilir. Admin ve IK rolleri izin taleplerini onaylayabilir, reddedebilir veya silebilir. Personel rolü ise sadece izin talebi oluşturabilir ve izin kayıtlarını görüntüleyebilir.

### Bordro Yönetimi

Bordro modülünde çalışanların brüt maaşları üzerinden örnek bir kesinti uygulanarak net maaş hesaplanmaktadır. Bu projede kesinti oranı sade ve anlaşılır olması için sabit oranlı şekilde ele alınmıştır.

Gerçek bordro sistemlerinde gelir vergisi, SGK primi ve işsizlik sigortası gibi daha detaylı kalemler bulunabilir. Bu projede ise temel bordro mantığını göstermek amacıyla sadeleştirilmiş bir hesaplama yapılmıştır.

### Bordro PDF Raporu

Oluşturulan bordro kayıtları PDF olarak indirilebilir. PDF raporunda çalışan bilgileri, bordro dönemi, brüt maaş, kesinti tutarı ve net maaş bilgileri yer almaktadır.

### Dashboard İstatistikleri

Ana sayfada toplam departman sayısı, toplam çalışan sayısı, bordro kayıt sayısı ve izin taleplerinin durumlarına göre istatistikleri gösterilmektedir. Bu sayede sistemdeki temel insan kaynakları verileri hızlıca takip edilebilmektedir.

## 🛣 Roadmap

- [x] Kullanıcı kayıt ve giriş işlemleri
- [x] Departman yönetimi
- [x] Çalışan yönetimi
- [x] Bordro hesaplama
- [x] MVC View yapısının eklenmesi
- [x] SQL Server bağlantısı
- [x] GitHub teslim klasör yapısının düzenlenmesi
- [x] İzin yönetimi modülünün eklenmesi
- [x] Rol bazlı yetkilendirme yapısının genişletilmesi
- [x] Bordro raporlarının PDF olarak alınması
- [x] Dashboard istatistiklerinin geliştirilmesi
- [x] Demo kullanıcıların otomatik oluşturulması
- [x] Role göre değişen frontend görünümünün hazırlanması
- [x] Modern responsive arayüz tasarımının yapılması

## 🤝 Katkı

Bu proje BMU1208 Web Tabanlı Programlama dersi kapsamında Bitlis Eren Üniversitesi Bilgisayar Mühendisliği bölümünde final ödevi olarak geliştirilmiştir.

Kod katkısı beklenmemektedir. Proje eğitim amacıyla hazırlanmıştır.

## 📜 Lisans

MIT © 2026 Semanur Yıldırım

Tam metin için `LICENSE` dosyası incelenebilir.

## 🙋‍♀️ İletişim

| Bilgi | Açıklama |
|---|---|
| Öğrenci | Semanur Yıldırım |
| Öğrenci No | 23080410007 |
| E-posta | semanuryildirim.03@gmail.com |
| Ders | BMU1208 Web Tabanlı Programlama |
| Kurum | Bitlis Eren Üniversitesi |
| Bölüm | Bilgisayar Mühendisliği |

## 🤖 Not

Bu projede geliştirme sürecini hızlandırmak, kod yapısını düzenlemek ve dokümantasyon hazırlamak amacıyla yapay zekâ destekli araçlardan yararlanılmıştır. Projenin mimari kararları, uygulama tercihleri ve son kontrolleri öğrenci tarafından yapılmıştır.
