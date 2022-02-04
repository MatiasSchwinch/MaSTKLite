using System;
using System.Windows;
using MaSTK_Lite.Service;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
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
            Services = ConfigureServices();

            if (!System.IO.File.Exists(DbName)) Services.GetService<DBConnector>().Database.Migrate();

            HandyControl.Tools.ConfigHelper.Instance.SetLang("es");
        }

        private static IServiceProvider ConfigureServices()
        {
            ServiceCollection services = new();

            _ = services.AddDbContext<DBConnector>(options =>
                  options.UseSqlite(connectionString: $"Data Source={DbName}"));
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
