using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ParserApp.Controls
{
    public class AppWindows
    {
        public static void OpenSettings()
        {
            var wnd = new Window
            {
                Height = 600,
                Width = 400,
                Title = Properties.Resources.WndTitleEditSettings,
                Content = new EditSettings()
            };

            wnd.ShowDialog();
        }
    }
}
