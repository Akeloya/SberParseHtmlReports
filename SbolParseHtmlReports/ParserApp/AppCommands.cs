using System.Collections.Generic;
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

        public static readonly RoutedCommand OpenHtmlFile = new RoutedCommand(
            nameof(OpenHtmlFile),
            typeof(MainWindow),
            new InputGestureCollection(
                new List<InputGesture>() { new KeyGesture(Key.O, ModifierKeys.Control) })
            );

        public static readonly RoutedCommand OpenCsvFile = new RoutedCommand(
            nameof(OpenCsvFile),
            typeof(MainWindow),
            new InputGestureCollection(
                new List<InputGesture>() { new KeyGesture(Key.O, ModifierKeys.Shift | ModifierKeys.Control) })
            );

    }
}
