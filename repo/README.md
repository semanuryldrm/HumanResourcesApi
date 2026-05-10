# C# .NET İnsan Kaynakları Yönetim Sistemi

Bu proje, Web Tabanlı Programlama dersi final projesi kapsamında geliştirilmiş bir **İnsan Kaynakları Yönetim Sistemi** uygulamasıdır. Projede C# ve .NET 8 kullanılarak bir Web API geliştirilmiş, bu API üzerinde departman, çalışan ve maaş bordrosu işlemleri gerçekleştirilmiştir. Ayrıca kullanıcı kayıt/giriş işlemleri için ASP.NET Identity ve JWT tabanlı kimlik doğrulama yapısı kullanılmıştır.

Proje yalnızca API olarak bırakılmamış, kullanıcıların işlemleri daha kolay yapabilmesi için basit ve düzenli bir web arayüzü de eklenmiştir.

---

## Projenin Amacı

Bu projenin amacı, bir kurumda yer alan departmanların ve çalışanların dijital ortamda yönetilebildiği temel bir insan kaynakları sistemi geliştirmektir.

Sistem üzerinden kullanıcılar:

- Sisteme kayıt olabilir.
- Giriş yapabilir.
- Departman ekleyebilir, listeleyebilir, güncelleyebilir ve silebilir.
- Çalışan ekleyebilir, listeleyebilir, güncelleyebilir ve silebilir.
- Çalışanları departmanlarla ilişkilendirebilir.
- Çalışanların brüt maaşı üzerinden örnek bordro hesaplayabilir.
- Hesaplanan bordro kayıtlarını listeleyebilir ve silebilir.

---

## Kullanılan Teknolojiler

Projede kullanılan temel teknolojiler şunlardır:

- C#
- .NET 8
- ASP.NET Core Web API
- Entity Framework Core
- SQL Server LocalDB
- ASP.NET Identity
- JWT Authentication
- Swagger / OpenAPI
- HTML
- CSS
- JavaScript

---

## Proje Yapısı

Proje temel olarak backend API ve basit web arayüzünden oluşmaktadır.

```text
HumanResourcesApi/
│
├── Controllers/
│   ├── AuthController.cs
│   ├── DepartmentsController.cs
│   ├── EmployeesController.cs
│   └── PayrollsController.cs
│
├── Data/
│   └── ApplicationDbContext.cs
│
├── DTOs/
│   ├── AuthDtos.cs
│   ├── DepartmentDtos.cs
│   ├── EmployeeDtos.cs
│   └── PayrollDtos.cs
│
├── Models/
│   ├── ApplicationUser.cs
│   ├── Department.cs
│   ├── Employee.cs
│   └── Payroll.cs
│
├── Services/
│   ├── PayrollService.cs
│   └── TokenService.cs
│
├── wwwroot/
│   ├── index.html
│   ├── style.css
│   └── app.js
│
├── appsettings.json
├── Program.cs
└── README.md
```
## Veritabanı Yapısı

Projede **SQL Server LocalDB** kullanılmıştır. LocalDB, SQL Server’ın geliştirme ortamlarında kullanılan yerel sürümüdür. Bu nedenle proje, kullanıcının bilgisayarındaki yerel SQL Server altyapısı üzerinden çalışmaktadır.

Veritabanı işlemleri **Entity Framework Core** ile yapılmaktadır. Entity Framework Core sayesinde C# sınıfları veritabanı tablolarına dönüştürülmüş, migration işlemleriyle veritabanı oluşturulmuştur.

Temel tablolar şunlardır:

---

### Department

`Department` tablosu departman bilgilerini tutar.

| Alan | Açıklama |
|---|---|
| Id | Departmanın benzersiz kimliği |
| Name | Departman adı |
| Description | Departman açıklaması |
| CreatedAt | Oluşturulma tarihi |

---

### Employee

`Employee` tablosu çalışan bilgilerini tutar.

| Alan | Açıklama |
|---|---|
| Id | Çalışanın benzersiz kimliği |
| FirstName | Çalışanın adı |
| LastName | Çalışanın soyadı |
| Email | E-posta adresi |
| PhoneNumber | Telefon numarası |
| HireDate | İşe giriş tarihi |
| GrossSalary | Brüt maaş |
| DepartmentId | Bağlı olduğu departman |

---

### Payroll

`Payroll` tablosu maaş bordrosu kayıtlarını tutar.

| Alan | Açıklama |
|---|---|
| Id | Bordro kaydının benzersiz kimliği |
| EmployeeId | Bordronun ait olduğu çalışan |
| Period | Bordro dönemi |
| GrossSalary | Brüt maaş |
| DeductionAmount | Kesinti tutarı |
| NetSalary | Net maaş |
| CreatedAt | Bordro oluşturulma tarihi |

---

### ApplicationUser

`ApplicationUser` tablosu, ASP.NET Identity tarafından kullanılan kullanıcı bilgilerini tutar. Kullanıcı kayıt ve giriş işlemleri bu yapı üzerinden gerçekleştirilmiştir.

---

## İlişkiler

Projede departman ve çalışan arasında **bire çok (1:N)** ilişki kurulmuştur.

- Bir departmanda birden fazla çalışan bulunabilir.
- Bir çalışan yalnızca bir departmana bağlıdır.
- `Employee` tablosundaki `DepartmentId` alanı, çalışanın hangi departmana bağlı olduğunu belirtir.

Çalışan ve bordro arasında da ilişki vardır.

- Bir çalışanın birden fazla bordro kaydı olabilir.
- Her bordro kaydı yalnızca bir çalışana aittir.

---

## Temel Özellikler

### Kullanıcı İşlemleri

- Kullanıcı kayıt olabilir.
- Kullanıcı giriş yapabilir.
- Giriş başarılı olduğunda JWT token oluşturulur.
- Korunan işlemler yalnızca giriş yapan kullanıcı tarafından yapılabilir.

---

### Departman İşlemleri

- Departman ekleme
- Departman listeleme
- Departman güncelleme
- Departman silme

Departmana bağlı çalışan varsa departman silme işlemi engellenir. Böylece çalışanı bulunan bir departmanın yanlışlıkla silinmesi önlenir.

---

### Çalışan İşlemleri

- Çalışan ekleme
- Çalışan listeleme
- Çalışan güncelleme
- Çalışan silme

Çalışan eklenirken geçerli bir departman seçilmelidir. Bu sayede çalışan-departman ilişkisi korunur.

---

### Bordro İşlemleri

- Çalışan seçilerek bordro hesaplanabilir.
- Brüt maaş üzerinden örnek kesinti yapılır.
- Net maaş hesaplanır.
- Bordro kayıtları listelenebilir.
- Bordro kayıtları silinebilir.

Bordro hesaplama işleminde örnek olarak **%15 kesinti oranı** kullanılmıştır.

Örnek hesaplama:

```text
Brüt Maaş: 30000 TL
Kesinti: 4500 TL
Net Maaş: 25500 TL
```

Ayrıca çalışanın işe giriş tarihinden önceki dönemler için bordro oluşturulması engellenmiştir.

Örneğin bir çalışan Nisan ayında işe girdiyse, Şubat ayı için bordro oluşturulamaz.

---

## API Endpointleri

### Auth

| Metot | Endpoint | Açıklama |
|---|---|---|
| POST | `/api/Auth/register` | Kullanıcı kaydı oluşturur |
| POST | `/api/Auth/login` | Kullanıcı girişi yapar |

---

### Departments

| Metot | Endpoint | Açıklama |
|---|---|---|
| GET | `/api/Departments` | Departmanları listeler |
| GET | `/api/Departments/{id}` | Seçilen departmanı getirir |
| POST | `/api/Departments` | Yeni departman ekler |
| PUT | `/api/Departments/{id}` | Departman günceller |
| DELETE | `/api/Departments/{id}` | Departman siler |

---

### Employees

| Metot | Endpoint | Açıklama |
|---|---|---|
| GET | `/api/Employees` | Çalışanları listeler |
| GET | `/api/Employees/{id}` | Seçilen çalışanı getirir |
| POST | `/api/Employees` | Yeni çalışan ekler |
| PUT | `/api/Employees/{id}` | Çalışan günceller |
| DELETE | `/api/Employees/{id}` | Çalışan siler |

---

### Payrolls

| Metot | Endpoint | Açıklama |
|---|---|---|
| GET | `/api/Payrolls` | Bordro kayıtlarını listeler |
| GET | `/api/Payrolls/employee/{employeeId}` | Bir çalışanın bordrolarını listeler |
| POST | `/api/Payrolls/calculate/{employeeId}` | Çalışan için bordro hesaplar |
| DELETE | `/api/Payrolls/{id}` | Bordro kaydını siler |

---

### Minimal API Örneği

| Metot | Endpoint | Açıklama |
|---|---|---|
| GET | `/api/minimal/status` | Minimal API örneği olarak sistem durumunu döndürür |

---

## Web Arayüzü

Projede kullanıcıların API endpointlerini doğrudan kullanmak zorunda kalmaması için web arayüzü hazırlanmıştır.

Web arayüzünde şu sayfalar bulunmaktadır:

- Giriş / Kayıt Sayfası
- Ana Sayfa
- Departmanlar Sayfası
- Çalışanlar Sayfası
- Bordro Sayfası

Web arayüzünde teknik token, JSON response veya Swagger bağlantısı gösterilmemiştir. Kullanıcı tarafı daha sade ve anlaşılır olacak şekilde hazırlanmıştır.

---

## Projeyi Çalıştırma

Öncelikle proje klasörüne girilir.

```powershell
cd HumanResourcesApi
```

Proje derlenir.

```powershell
dotnet build
```

Proje çalıştırılır.

```powershell
dotnet run
```

Uygulama çalıştıktan sonra web arayüzü aşağıdaki adresten açılır:

```text
http://localhost:5207/
```

Swagger/OpenAPI ekranı ise geliştirme ve test amacıyla aşağıdaki adresten açılabilir:

```text
http://localhost:5207/swagger
```

---

## Migration ve Veritabanı

Projede Entity Framework Core migration kullanılmıştır.

Migration oluşturmak için:

```powershell
dotnet ef migrations add InitialCreate
```

Veritabanını güncellemek için:

```powershell
dotnet ef database update
```

Projede kullanılan bağlantı cümlesi `appsettings.json` dosyasında yer almaktadır.

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=HumanResourcesDb;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true"
}
```

Bu bağlantı cümlesi, projenin kullanıcının bilgisayarındaki yerel SQL Server LocalDB instance’ına bağlanmasını sağlar.

Bağlantı yapılan veritabanı:

```text
Server: (localdb)\MSSQLLocalDB
Database: HumanResourcesDb
```

---

## Örnek Kullanıcı

Test amacıyla aşağıdaki kullanıcı bilgileri kullanılabilir:

```text
E-posta: semanur@example.com
Şifre: 123456
```

Yeni kullanıcı kaydı da web arayüzündeki kayıt formu üzerinden yapılabilir.

---

## Örnek Demo Akışı

Sunum sırasında proje şu şekilde gösterilebilir:

1. Kullanıcı sisteme giriş yapar.
2. Ana sayfada toplam departman, çalışan ve bordro sayıları gösterilir.
3. Departmanlar sayfasında yeni bir departman eklenir.
4. Eklenen departman düzenlenir.
5. Çalışanlar sayfasında yeni bir çalışan eklenir.
6. Çalışan bir departmana bağlanır.
7. Çalışan bilgileri güncellenir.
8. Bordro sayfasında çalışan için bordro hesaplanır.
9. İşe giriş tarihinden önceki dönem için bordro oluşturulamadığı gösterilir.
10. Hesaplanan bordro kaydı silinir.

---

## Projede Dikkat Edilen Noktalar

- Kod yapısı klasörlere ayrılarak düzenli tutulmuştur.
- API endpointleri Controller yapısı ile geliştirilmiştir.
- Minimal API yaklaşımı ayrıca örnek endpoint ile gösterilmiştir.
- Veritabanı işlemlerinde Entity Framework Core kullanılmıştır.
- Kullanıcı yönetimi için ASP.NET Identity tercih edilmiştir.
- JWT token ile kimlik doğrulama yapılmıştır.
- Departman ve çalışan arasında 1:N ilişki kurulmuştur.
- Bordro hesaplaması servis sınıfı üzerinden yapılmıştır.
- İşe giriş tarihinden önce bordro oluşturulması engellenmiştir.
- Aynı çalışan için aynı döneme tekrar bordro oluşturulması engellenmiştir.
- Kullanıcı arayüzü teknik detaylardan arındırılarak sade tutulmuştur.

---

## Sonuç

Bu proje kapsamında C# .NET 8 ile çalışan bir İnsan Kaynakları Yönetim Sistemi geliştirilmiştir. Proje içerisinde Web API geliştirme, SQL Server ile veritabanı yönetimi, Entity Framework Core migration kullanımı, kullanıcı kimlik doğrulama, JWT token üretimi, CRUD işlemleri, ilişkisel veri modeli ve web arayüzü geliştirme konuları uygulanmıştır.

Proje, Web Tabanlı Programlama dersi kapsamında temel backend ve frontend kavramlarını bir arada gösterecek şekilde hazırlanmıştır.

---

## Hazırlayan

Bu proje, **Web Tabanlı Programlama** dersi final projesi kapsamında **Semanur YILDIRIM** tarafından hazırlanmıştır.


---