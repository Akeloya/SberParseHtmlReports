using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace ParserApp
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static string GetSettingsPath()
        {
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "settings.json");
        }
    }
}
