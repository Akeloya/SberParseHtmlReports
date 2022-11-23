using System;
using System.IO;
using System.Windows;

namespace ParserApp
{
    public partial class App : Application
    {
        private readonly ShellBootstrapper<MainWindowViewModel> _bootstrapper = new ShellBootstrapper<MainWindowViewModel>();

        public App()
        {
            _bootstrapper.Initialize();
        }

        public static string GetSettingsPath()
        {
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "settings.json");
        }
    }
}
