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
    public static class ComboBoxBehavior
    {
        public static readonly DependencyProperty GotFocusCommandProperty =
            DependencyProperty.RegisterAttached("GotFocusCommand", typeof(ICommand), typeof(ComboBoxBehavior), new PropertyMetadata(null, GotFocusCommandChanged));

        public static readonly DependencyProperty LostFocusCommandProperty =
            DependencyProperty.RegisterAttached("LostFocusCommand", typeof(ICommand), typeof(ComboBoxBehavior), new PropertyMetadata(null, LostFocusCommandChanged));

        public static ICommand GetGotFocusCommand(ComboBox comboBox)
        {
            return (ICommand)comboBox.GetValue(GotFocusCommandProperty);
        }

        public static void SetGotFocusCommand(ComboBox comboBox, ICommand value)
        {
            comboBox.SetValue(GotFocusCommandProperty, value);
        }

        public static ICommand GetLostFocusCommand(ComboBox comboBox)
        {
            return (ICommand)comboBox.GetValue(LostFocusCommandProperty);
        }

        public static void SetLostFocusCommand(ComboBox comboBox, ICommand value)
        {
            comboBox.SetValue(LostFocusCommandProperty, value);
        }

        private static void GotFocusCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var comboBox = (ComboBox)d;
            comboBox.GotFocus -= ComboBox_GotFocus;

            if (e.NewValue is ICommand command)
            {
                comboBox.GotFocus += ComboBox_GotFocus;
            }
        }

        private static void LostFocusCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var comboBox = (ComboBox)d;
            comboBox.LostFocus -= ComboBox_LostFocus;

            if (e.NewValue is ICommand command)
            {
                comboBox.LostFocus += ComboBox_LostFocus;
            }
        }

        private static void ComboBox_GotFocus(object sender, RoutedEventArgs e)
        {
            var comboBox = (ComboBox)sender;
            var command = GetGotFocusCommand(comboBox);
            command?.Execute(comboBox);
        }

        private static void ComboBox_LostFocus(object sender, RoutedEventArgs e)
        {
            var comboBox = (ComboBox)sender;
            var command = GetLostFocusCommand(comboBox);
            command?.Execute(comboBox);
        }
    }
}
