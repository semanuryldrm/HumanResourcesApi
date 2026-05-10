# .NET Core İnsan Kaynakları Yönetim Sistemi API

ASP.NET Core ile geliştirilmiş insan kaynakları yönetim sistemi projesidir. Proje kapsamında kullanıcı işlemleri, departman yönetimi, çalışan yönetimi ve bordro hesaplama işlemleri web tabanlı olarak gerçekleştirilmiştir.

## 🎯 Özet

Bu proje, Web Tabanlı Programlama dersi final projesi kapsamında geliştirilmiştir. İnsan kaynakları süreçlerinde kullanılan temel işlemlerin tek bir sistem üzerinden takip edilebilmesi amaçlanmıştır.

Uygulama sayesinde kullanıcı kaydı ve girişi yapılabilmekte, departmanlar oluşturulabilmekte, çalışan bilgileri yönetilebilmekte ve çalışanların brüt maaşları üzerinden örnek bordro hesaplaması yapılabilmektedir. Proje, ASP.NET Core MVC ve Web API yapısı birlikte kullanılarak hazırlanmıştır.

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

## ✨ Ana Özellikler

- Kullanıcı kayıt ve giriş işlemleri
- JWT tabanlı kimlik doğrulama
- Departman ekleme, listeleme, güncelleme ve silme işlemleri
- Çalışan ekleme, listeleme, güncelleme ve silme işlemleri
- Çalışanların departmanlarla ilişkilendirilmesi
- Brüt maaş üzerinden örnek bordro hesaplama
- ASP.NET Core MVC yapısına uygun Razor View arayüzü
- Web API endpointleri ile veri işlemleri
- Entity Framework Core ile veritabanı yönetimi
- SQL Server / SQL Server Express bağlantısı
- Swagger arayüzü ile API test imkânı

## 🧰 Tech Stack

| Teknoloji | Kullanım Amacı |
|---|---|
| C# | Backend geliştirme dili |
| ASP.NET Core 8 | Web API ve MVC altyapısı |
| ASP.NET Core MVC | Razor View tabanlı kullanıcı arayüzü |
| ASP.NET Core Web API | JSON tabanlı API endpointleri |
| Entity Framework Core | Veritabanı işlemleri |
| SQL Server / SQL Server Express | Veritabanı yönetimi |
| ASP.NET Core Identity | Kullanıcı yönetimi |
| JWT | Kimlik doğrulama |
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
  "DefaultConnection": "Server=SEMA\\SQLEXPRESS;Database=HumanResourcesDb;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true"
}
```

Farklı bir bilgisayarda çalıştırılırken `Server` değeri ilgili SQL Server adına göre güncellenmelidir.

## 🧪 Test

Proje manuel olarak web arayüzü ve Swagger üzerinden test edilmiştir. Temel olarak aşağıdaki işlemler kontrol edilmiştir:

- Kullanıcı kayıt işlemi
- Kullanıcı giriş işlemi
- Departman ekleme ve listeleme
- Çalışan ekleme ve listeleme
- Çalışanın departmanla ilişkilendirilmesi
- Bordro hesaplama
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

### Departman Yönetimi

Departman modülünde departman ekleme, listeleme, güncelleme ve silme işlemleri yapılabilmektedir. Çalışan kayıtları departmanlarla ilişkilendirildiği için bu modül sistemin temel parçalarından biridir.

### Çalışan Yönetimi

Çalışan modülünde personel bilgileri sisteme eklenebilir. Çalışan adı, soyadı, e-posta adresi, telefon numarası, işe giriş tarihi, brüt maaş ve bağlı olduğu departman bilgileri tutulmaktadır.

### Bordro Yönetimi

Bordro modülünde çalışanların brüt maaşları üzerinden örnek bir kesinti uygulanarak net maaş hesaplanmaktadır. Bu projede kesinti oranı sade ve anlaşılır olması için sabit oranlı şekilde ele alınmıştır.

Gerçek bordro sistemlerinde gelir vergisi, SGK primi ve işsizlik sigortası gibi daha detaylı kalemler bulunabilir. Bu projede ise temel bordro mantığını göstermek amacıyla sadeleştirilmiş bir hesaplama yapılmıştır.

## 🛣 Roadmap

- [x] Kullanıcı kayıt ve giriş işlemleri
- [x] Departman yönetimi
- [x] Çalışan yönetimi
- [x] Bordro hesaplama
- [x] MVC View yapısının eklenmesi
- [x] SQL Server bağlantısı
- [x] GitHub teslim klasör yapısının düzenlenmesi
- [ ] İzin yönetimi modülünün eklenmesi
- [ ] Rol bazlı yetkilendirme yapısının genişletilmesi
- [ ] Bordro raporlarının PDF olarak alınması
- [ ] Dashboard istatistiklerinin geliştirilmesi

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

