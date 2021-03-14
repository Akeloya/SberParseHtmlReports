using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ParserApp
{
    static class AppCommands
    {
        public static readonly RoutedCommand About = new RoutedCommand(
            nameof(About), 
            typeof(MainWindow), 
            new InputGestureCollection(
                new List<InputGesture>() { new KeyGesture(Key.F1, ModifierKeys.Control) })
            );

        public static readonly RoutedCommand OpenSettings = new RoutedCommand(
            nameof(OpenSettings),
            typeof(MainWindow),
            new InputGestureCollection(
                new List<InputGesture>() { new KeyGesture(Key.F6) })
            );
    }
}
