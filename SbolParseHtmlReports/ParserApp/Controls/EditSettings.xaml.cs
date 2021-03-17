using ParserCore;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ParserApp.Controls
{
    /// <summary>
    /// Логика взаимодействия для EditSettings.xaml
    /// </summary>
    public partial class EditSettings : UserControl
    {
        private readonly string _settingsPath;
        public EditSettings()
        {
            _settingsPath = App.GetSettingsPath();
            InitializeComponent();
            try
            {
                DataContext = DataSet.LoadSettings(_settingsPath);
            }
            catch (FileNotFoundException)
            {
                DataContext = DataSet.LoadDefault();
            }
        }

        private void Close_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Window.GetWindow(this).Close();
        }

        private void Save_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var obj = (IDataSet)DataContext;
            obj.Save(_settingsPath);
        }
    }
}
