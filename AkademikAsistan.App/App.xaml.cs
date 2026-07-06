using System.Windows;
using AkademikAsistan.App.ViewModels;
using AkademikAsistan.App.ViewModels.DersPlani;
using AkademikAsistan.App.ViewModels.GelisimTakibi;
using AkademikAsistan.App.ViewModels.NotHesaplama;
using AkademikAsistan.App.Views.Shell;
using AkademikAsistan.Core.Interfaces;
using AkademikAsistan.Data;
using AkademikAsistan.Data.Repositories;
using AkademikAsistan.Services;
using Microsoft.Extensions.DependencyInjection;

namespace AkademikAsistan.App
{
    /// <summary>
    /// Composition root: tüm servis / repository / viewmodel kayıtları burada
    /// yapılır. Uygulama genelinde başka hiçbir yerde "new SomeConcrete()" çağrısı
    /// yoktur; her şey constructor injection ile gelir.
    /// </summary>
    public partial class App : Application
    {
        public static IServiceProvider Services { get; private set; } = null!;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var sc = new ServiceCollection();
            ConfigureServices(sc);
            Services = sc.BuildServiceProvider();

            // Uygulama ilk açılışında yoksa schema.sql'den tabloları oluştur.
            Services.GetRequiredService<DatabaseInitializer>().EnsureCreated();

            Services.GetRequiredService<MainWindow>().Show();
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            const string connectionString = "Data Source=akademik_asistan.db";

            // ── Data katmanı ──────────────────────────────────────────────
            services.AddSingleton<ISqliteConnectionFactory>(
                _ => new SqliteConnectionFactory(connectionString));
            services.AddSingleton<DatabaseInitializer>();
            services.AddScoped<ICourseRepository, CourseRepository>();
            services.AddScoped<ISemesterRepository, SemesterRepository>();

            // ── Services katmanı ──────────────────────────────────────────
            services.AddSingleton<IGradeCalculationService, GradeCalculationService>();

            // ── ViewModel'ler ─────────────────────────────────────────────
            // Transient: her seferinde taze bir örnek.
            // Scoped da kullanılabilir — masaüstü uygulamasında genelde Transient yeterli.
            services.AddTransient<GradeCalculatorViewModel>();
            services.AddTransient<DersPlaniViewModel>();
            services.AddTransient<StudentProgressViewModel>();
            services.AddTransient<MainViewModel>();

            // ── Pencere ───────────────────────────────────────────────────
            services.AddTransient<MainWindow>();
        }
    }
}
