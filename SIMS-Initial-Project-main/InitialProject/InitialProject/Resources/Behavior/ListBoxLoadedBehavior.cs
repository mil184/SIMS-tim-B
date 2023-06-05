using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;

namespace InitialProject.Resources.Behavior
{
    public static class ListBoxLoadedBehavior
    {
        public static readonly DependencyProperty LoadedCommandProperty =
            DependencyProperty.RegisterAttached("LoadedCommand", typeof(ICommand), typeof(ListBoxLoadedBehavior), new PropertyMetadata(null, OnLoadedCommandChanged));

        public static ICommand GetLoadedCommand(ListBox listBox)
        {
            return (ICommand)listBox.GetValue(LoadedCommandProperty);
        }

        public static void SetLoadedCommand(ListBox listBox, ICommand value)
        {
            listBox.SetValue(LoadedCommandProperty, value);
        }

        private static void OnLoadedCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ListBox listBox)
            {
                listBox.Loaded -= ListBox_Loaded;
                listBox.Loaded += ListBox_Loaded;
            }
        }

        private static void ListBox_Loaded(object sender, RoutedEventArgs e)
        {
            var listBox = (ListBox)sender;
            var command = GetLoadedCommand(listBox);
            command?.Execute(listBox);
        }
    }
}
