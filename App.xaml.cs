using System;
using System.Windows;
using MaSTK_Lite.Service;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using MaSTK_Lite.ViewModel;
using MaSTK_Lite.View;

namespace MaSTK_Lite
{
    public partial class App : Application
    {
        public new static App Current => (App)Application.Current;
        public IServiceProvider Services { get; }
        private const string DbName = "MaSTKLite.db";

        public App()
        {
            HandyControl.Tools.ConfigHelper.Instance.SetLang("es");
            Services = ConfigureServices();
        }

        private static IServiceProvider ConfigureServices()
        {
            ServiceCollection services = new();

            _ = services.AddDbContext<DBConnector>(options =>
                  options.UseSqlite(connectionString: $"Filename={DbName}",
                  sqliteOptionsAction: op =>
                  {
                      _ = op.MigrationsAssembly(Assembly.GetExecutingAssembly().FullName);
                  }));
            _ = services.AddSingleton<MainViewModel>();

            return services.BuildServiceProvider();
        }

        private void OnStartup(object sender, StartupEventArgs e)
        {
            MainWindow window = new();
            window.DataContext = Services.GetService<MainViewModel>();
            window.Show();
        }
    }
}
