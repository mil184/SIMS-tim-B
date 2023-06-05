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
    public class ComboBoxSelectionChangedBehavior
    {
        public static ICommand GetSelectionChangedCommand(DependencyObject obj)
        {
            return (ICommand)obj.GetValue(SelectionChangedCommandProperty);
        }

        public static void SetSelectionChangedCommand(DependencyObject obj, ICommand value)
        {
            obj.SetValue(SelectionChangedCommandProperty, value);
        }

        public static readonly DependencyProperty SelectionChangedCommandProperty =
            DependencyProperty.RegisterAttached("SelectionChangedCommand", typeof(ICommand), typeof(ComboBoxSelectionChangedBehavior), new PropertyMetadata(null, OnSelectionChangedCommandChanged));

        private static void OnSelectionChangedCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ComboBox comboBox)
            {
                if (e.NewValue is ICommand command)
                {
                    comboBox.SelectionChanged += ComboBox_SelectionChanged;
                }
                else
                {
                    comboBox.SelectionChanged -= ComboBox_SelectionChanged;
                }
            }
        }

        private static void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ComboBox comboBox)
            {
                ICommand command = GetSelectionChangedCommand(comboBox);
                if (command != null && command.CanExecute(comboBox.SelectedItem))
                {
                    command.Execute(comboBox.SelectedItem);
                }
            }
        }
    }
}
