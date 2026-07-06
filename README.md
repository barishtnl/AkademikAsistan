<div align="center">

# 🎓 Akademik Asistan

### Üniversite öğrencileri için akıllı masaüstü takip uygulaması
### Smart desktop tracking application for university students

![.NET](https://img.shields.io/badge/.NET_8-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![WPF](https://img.shields.io/badge/WPF-0078D4?style=for-the-badge&logo=windows&logoColor=white)
![SQLite](https://img.shields.io/badge/SQLite-003B57?style=for-the-badge&logo=sqlite&logoColor=white)
![AI Assisted](https://img.shields.io/badge/AI_Destekli-Claude_by_Anthropic-E07856?style=for-the-badge)

</div>

---

## 🤖 AI Desteği Hakkında / About AI Assistance

> **TR:** Bu proje, [Anthropic](https://www.anthropic.com) tarafından geliştirilen **Claude** yapay zeka asistanı ile birlikte geliştirilmiştir. Proje mimarisi, MVVM katman tasarımı, veritabanı şeması ve kod iskeletleri Claude'un yardımıyla oluşturulmuş; kod gözden geçirme, hata ayıklama ve geliştirme sürecinde AI destekten aktif olarak yararlanılmıştır.

> **EN:** This project was developed in collaboration with **Claude**, an AI assistant by [Anthropic](https://www.anthropic.com). The project architecture, MVVM layer design, database schema, and code scaffolding were created with Claude's assistance. AI support was actively used throughout code review, debugging, and the development process.

---

## 📖 Proje Hakkında / About

**TR:** Akademik Asistan, üniversite öğrencilerinin ders programlarını yönetmelerine, not ortalamalarını hesaplamalarına ve dönemlik akademik gelişimlerini takip etmelerine yardımcı olan bir WPF masaüstü uygulamasıdır. Temiz kod (Clean Code) prensiplerine, SOLID kurallarına ve katı bir MVVM mimarisine göre inşa edilmiştir.

**EN:** Akademik Asistan is a WPF desktop application that helps university students manage their course schedules, calculate grade averages, and track their academic progress over semesters. It is built following Clean Code principles, SOLID rules, and a strict MVVM architecture.

---

## ✨ Özellikler / Features

| Modül / Module | TR | EN |
|---|---|---|
| 📅 **Ders Planı** | Haftalık ders programını kanban panosu görünümünde ekle, görüntüle ve sil | Add, view, and delete your weekly course schedule in a kanban board layout |
| 🧮 **Not Hesaplama** | Vize/final ağırlıklarına göre ağırlıklı ortalama ve harf notu hesapla | Calculate weighted average and letter grade based on midterm/final weights |
| 📈 **Gelişim Takibi** | Dönemlik GNO ve DNO değişimini interaktif çizgi grafiğiyle izle | Track semester-by-semester GPA and term GPA changes with an interactive line chart |

---

## 🛠️ Teknoloji Yığını / Tech Stack

| Katman / Layer | Teknoloji / Technology | Açıklama / Description |
|---|---|---|
| **UI** | WPF (.NET 8) | Masaüstü arayüzü / Desktop UI |
| **Mimari / Architecture** | MVVM | Model-View-ViewModel |
| **Veritabanı / Database** | SQLite | Yerel dosya tabanlı DB / Local file-based DB |
| **ORM** | Dapper | Hafif micro-ORM / Lightweight micro-ORM |
| **DI** | Microsoft.Extensions.DI | Bağımlılık enjeksiyonu / Dependency injection |
| **Grafik / Chart** | LiveCharts2 | GNO/DNO trend grafiği / GPA trend chart |
| **Test** | xUnit | Birim testler / Unit tests |
| **AI Asistan / AI Assistant** | Claude (Anthropic) | Geliştirme desteği / Development support |

---

## 🏗️ Mimari Yapı / Architecture

```
AkademikAsistan/
├── AkademikAsistan.Core/        # Domain modelleri ve interface'ler (bağımlılıksız)
│                                # Domain models and interfaces (dependency-free)
├── AkademikAsistan.Data/        # SQLite + Dapper repository implementasyonları
│                                # SQLite + Dapper repository implementations
├── AkademikAsistan.Services/    # İş mantığı katmanı (not hesaplama algoritması vb.)
│                                # Business logic layer (grade calculation algorithm etc.)
├── AkademikAsistan.App/         # WPF UI — View ve ViewModel (code-behind'da mantık yok)
│   ├── Theme/                   #   Renk paleti ve kontrol stilleri / Color palette & styles
│   ├── Converters/              #   IValueConverter implementasyonları
│   ├── Common/                  #   ViewModelBase, RelayCommand, AsyncRelayCommand
│   ├── ViewModels/              #   Ana VM + 3 modül ViewModel'i
│   └── Views/                   #   Shell (MainWindow) + 3 modül View'i
└── AkademikAsistan.Tests/       # xUnit birim testleri / xUnit unit tests
```

**Tasarım Prensipleri / Design Principles:**
- ✅ SOLID prensipleri / SOLID principles
- ✅ Clean Code
- ✅ Dependency Injection (Constructor Injection)
- ✅ Repository Pattern
- ✅ ViewModel-first navigasyon / ViewModel-first navigation
- ✅ Code-behind'da sıfır iş mantığı / Zero business logic in code-behind

---

## 📥 Kurulum Gerektirmez — Direkt İndir / No Install Needed — Just Download

> **TR:** Uygulamayı kullanmak için Visual Studio veya .NET kurmanıza **gerek yoktur**.
>
> 1. Sağ taraftaki **Releases** bölümüne gidin (veya [buraya tıklayın](../../releases/latest))
> 2. En son sürümdeki `AkademikAsistan-vX.X.X-win-x64.zip` dosyasını indirin
> 3. ZIP'i çıkartın → `AkademikAsistan.App.exe` dosyasına çift tıklayın
> 4. ✅ Uygulama açılır — kurulum yok, kayıt yok, bağımlılık yok

> **EN:** You do **not** need Visual Studio or .NET installed.
>
> 1. Go to **Releases** on the right (or [click here](../../releases/latest))
> 2. Download `AkademikAsistan-vX.X.X-win-x64.zip` from the latest release
> 3. Extract the ZIP → double-click `AkademikAsistan.App.exe`
> 4. ✅ Done — no installer, no registration, no dependencies

---

## 🔖 Yeni Sürüm Yayınlama (Geliştirici) / Publishing a Release (Developers)

GitHub Actions her `v*` etiketi push'unda otomatik `.exe` üretir ve Release'e yükler:

```bash
git tag v1.0.0
git push origin v1.0.0
# GitHub Actions çalışır → Release sayfasına .zip otomatik eklenir
```

---

## 🚀 Geliştirici Kurulumu / Developer Setup

### Gereksinimler / Requirements
- **Visual Studio 2022** (17.8+) — ".NET Desktop Development" iş yükü / workload
- **.NET 8 SDK**

### Adımlar / Steps

**TR:**
1. Repoyu klonla veya ZIP olarak indir
2. `AkademikAsistan.sln` dosyasını Visual Studio'da aç
3. Solution Explorer'da **AkademikAsistan.App** → sağ tık → **Set as Startup Project**
4. **F5** ile çalıştır — NuGet paketleri otomatik yüklenir

**EN:**
1. Clone the repo or download as ZIP
2. Open `AkademikAsistan.sln` in Visual Studio
3. In Solution Explorer, right-click **AkademikAsistan.App** → **Set as Startup Project**
4. Press **F5** — NuGet packages are restored automatically

> İlk çalıştırmada `akademik_asistan.db` SQLite veritabanı otomatik oluşturulur.
> On first run, the `akademik_asistan.db` SQLite database is created automatically.

---

## ⚙️ Yapılandırma / Configuration

**TR:** Harf notu aralıklarını kendi üniversitenizin kriterlerine göre düzenlemek için:
`AkademikAsistan.Services/DefaultLetterGradeRanges.cs` dosyasını güncelleyin.

**EN:** To adjust letter grade ranges to your university's criteria, update:
`AkademikAsistan.Services/DefaultLetterGradeRanges.cs`

---

## 📦 Kullanılan Paketler / NuGet Packages

| Paket / Package | Lisans / License |
|---|---|
| Dapper | Apache 2.0 |
| Microsoft.Data.Sqlite | MIT |
| Microsoft.Extensions.DependencyInjection | MIT |
| LiveChartsCore.SkiaSharpView.WPF | MIT |
| xUnit | Apache 2.0 |

---

## 📄 Lisans / License

Bu proje **MIT Lisansı** ile lisanslanmıştır. Detaylar için `LICENSE` dosyasına bakın.

This project is licensed under the **MIT License**. See the `LICENSE` file for details.

---

<div align="center">

**🤖 Bu proje Claude (Anthropic) AI asistanı ile birlikte geliştirilmiştir.**
**🤖 This project was developed in collaboration with Claude (Anthropic) AI assistant.**


</div>
