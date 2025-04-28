using Microsoft.Extensions.DependencyInjection;
using PasswordManagerWPF.Services;
using System.Windows;

namespace PasswordManagerWPF
{
    public partial class App : Application
    {
        private readonly ServiceProvider serviceProvider;

        public App()
        {
            var services = new ServiceCollection();
            ConfigureServices(services);
            serviceProvider = services.BuildServiceProvider();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IErrorLogger, FileErrorLogger>();
            services.AddSingleton<IDialogService, DialogService>();
            services.AddSingleton<PasswordService>();
            services.AddSingleton<MainViewModel>();
            services.AddSingleton<MainWindow>();
        }

        private void OnStartup(object sender, StartupEventArgs e)
        {
            var mainWindow = serviceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }
    }
}