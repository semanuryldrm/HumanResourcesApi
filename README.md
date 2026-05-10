# C# .NET Core İK API

> **.NET Core ile İK (HR) yönetim API — personel, izin, bordro**

![Zorluk](https://img.shields.io/badge/Zorluk-Zor-red)
![Puan](https://img.shields.io/badge/Puan-55-blue)
![Hafta](https://img.shields.io/badge/Hafta-3-gray)
![Lisans](https://img.shields.io/badge/License-MIT-green)
![Durum](https://img.shields.io/badge/Durum-Development-yellow)

<!-- Kodunuzu yazdıktan sonra aşağıdaki bölümleri doldurun ve ekleyin:
![CI](https://github.com/KULLANICI_ADI/final-p30-c-net-core-ik-a/actions/workflows/ci.yml/badge.svg)
![Deploy](https://img.shields.io/website?url=https://c-net-core-ik-api.vercel.app)
-->

## 🎯 Özet

[*1-2 paragraf: Ürün ne yapıyor, kime, hangi problemi çözüyor? Jargon yok.*]

## 🎥 Demo

🔗 **Canlı Demo:** https://c-net-core-ik-api.vercel.app  
👤 **Demo Hesap:** `demo@example.com` · `demo123`

![Demo GIF](repo/docs/demo.gif)

> _Not: Ekran görüntülerini ve demo GIF'ini kendi `repo/` içinde istediğiniz klasör yapısında tutabilirsiniz. Aşağıdaki görsel yolları örnektir; gerçek dosya konumlarınıza göre güncelleyin._

### Ekran Görüntüleri (Örnek yerleşim)

| Landing | Dashboard | Mobile |
|---------|-----------|--------|
| ![landing](repo/docs/screenshots/01-landing.png) | ![dashboard](repo/docs/screenshots/02-dashboard.png) | ![mobile](repo/docs/screenshots/03-mobile.png) |

## ✨ Ana Özellikler

- ✅ Personel CRUD (TC kimlik, eğitim, tecrübe)
- ✅ İzin takibi (yıllık, hastalık, raporlu)
- ✅ Bordro hesaplama (brüt → net, SGK, gelir vergisi)
- ✅ Organizasyon şeması (hiyerarşi)
- ✅ Performans değerlendirme (OKR / KPI)
- ✅ Belge yönetimi (sözleşme, sertifika — PDF)
- ✅ Rapor: maaş dağılımı, izin istatistiği
- ✅ Role-based access (Admin, İK, Yönetici, Personel)

## 🧰 Tech Stack

**Framework:** `.NET 9`, `ASP.NET Core Web API`, `Minimal API`  
**Database:** `PostgreSQL 16 + Entity Framework Core 9`  
**Auth:** `Identity + JWT`  
**Validation:** `FluentValidation`  
**PDF:** `QuestPDF (bordro)`  
**Test:** `xUnit + Testcontainers`  
**Deployment:** `Docker + Azure App Service / Railway`  

> Teknoloji seçimlerinin detaylı gerekçesi: [PROJE-RAPORU.md · Bölüm 7](PROJE-RAPORU.md#7-teknoloji-yığını-tech-stack)

## 🏗 Mimari

[*Mimari diyagramınızı buraya ekleyiniz — örn. `repo/docs/diagrams/container.png`*]

[Detaylı mimari ve ADR'lar →](PROJE-RAPORU.md#8-sistem-mimarisi)

## 🚀 Kurulum

### Gereksinimler

- .NET ≥ 9 SDK
- PostgreSQL ≥ 15

### Adım Adım

```bash
# 1) Repo'yu klonla
git clone https://github.com/KULLANICI_ADI/final-p30-c-net-core-ik-a.git
cd final-p30-c-net-core-ik-a

# 2) Environment dosyası
cp .env.example .env
# .env içindekileri doldurun (DATABASE_URL, JWT_SECRET, ...)

# 3) Bağımlılıkları yükle
dotnet restore

# 4) Veritabanını hazırla (varsa)
dotnet ef database update

# 5) Çalıştır
dotnet run --project src/Api
```

Proje: http://localhost:5000

## 🧪 Test

```bash
dotnet test
```

## 📁 Klasör Yapısı (bu teslimde)

```
.
├── README.md                   (bu dosya — özet, kurulum, demo)
├── PROJE-RAPORU.md             (uzun form final raporu — markdown)
├── PROJE-RAPORU-SABLON.docx    (uzun form final raporu — Word)
├── LICENSE
├── .env.example
└── repo/                       (projenizin kaynak kodu — kendi yapınız)
    └── README.md               (repo'nuzun kendi README'si)
```

> Ekran görüntüleri, diyagramlar, API dokümantasyonu gibi ek dosyaları `repo/` içinde **kendi tercih ettiğiniz alt klasör yapısında** tutabilirsiniz. Rapor belgesinde bu dosyalara referans verirsiniz.

## 🛣 Roadmap

- [x] V1 — MVP (bu teslim)
- [ ] V2 — SGK API entegrasyonu, e-imza ile sözleşme imzalama
- [ ] V3 — AI ile işe alım filtreleme (CV analizi), chatbot İK asistanı

## 🤝 Katkı

Bu proje **BMU1208 Web Tabanlı Programlama** dersi kapsamında **Bitlis Eren Üniversitesi** — **Bilgisayar Mühendisliği** bölümünde bir final ödevi olarak geliştirilmiştir.

Ders yürütücüsü: **Dr. Öğr. Üyesi Davut ARI**

Kod katkısı beklenmez, ancak fikir / feedback için issue açabilirsiniz.

## 📜 Lisans

MIT © 2026 **SEMANUR YILDIRIM** — Tam metin için [LICENSE](LICENSE).

## 🙋‍♂️ İletişim

- **Öğrenci:** SEMANUR YILDIRIM
- **Öğrenci No:** 23080410007
- **E-posta:** semanuryildirim.03@gmail.com
- **Ders:** BMU1208 · Web Tabanlı Programlama
- **Kurum:** Bitlis Eren Üniversitesi — Mühendislik-Mimarlık Fakültesi

---

<sub>🤖 Bu projede [Claude Code](https://claude.com/claude-code) ve [Cursor](https://cursor.sh) gibi AI asistanları kullanılmıştır. Tüm mimari kararlar ve kullanım tercihleri öğrenci tarafından yapılmıştır.</sub>
