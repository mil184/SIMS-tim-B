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
    public static class TextBoxBehavior
    {
        public static readonly DependencyProperty GotFocusCommandProperty =
            DependencyProperty.RegisterAttached("GotFocusCommand", typeof(ICommand), typeof(TextBoxBehavior), new PropertyMetadata(null, GotFocusCommandChanged));

        public static readonly DependencyProperty LostFocusCommandProperty =
            DependencyProperty.RegisterAttached("LostFocusCommand", typeof(ICommand), typeof(TextBoxBehavior), new PropertyMetadata(null, LostFocusCommandChanged));

        public static ICommand GetGotFocusCommand(TextBox textBox)
        {
            return (ICommand)textBox.GetValue(GotFocusCommandProperty);
        }

        public static void SetGotFocusCommand(TextBox textBox, ICommand value)
        {
            textBox.SetValue(GotFocusCommandProperty, value);
        }

        public static ICommand GetLostFocusCommand(TextBox textBox)
        {
            return (ICommand)textBox.GetValue(LostFocusCommandProperty);
        }

        public static void SetLostFocusCommand(TextBox textBox, ICommand value)
        {
            textBox.SetValue(LostFocusCommandProperty, value);
        }

        private static void GotFocusCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var textBox = (TextBox)d;
            textBox.GotFocus -= TextBox_GotFocus;

            if (e.NewValue is ICommand command)
            {
                textBox.GotFocus += TextBox_GotFocus;
            }
        }

        private static void LostFocusCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var textBox = (TextBox)d;
            textBox.LostFocus -= TextBox_LostFocus;

            if (e.NewValue is ICommand command)
            {
                textBox.LostFocus += TextBox_LostFocus;
            }
        }

        private static void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            var textBox = (TextBox)sender;
            var command = GetGotFocusCommand(textBox);
            command?.Execute(textBox);
        }

        private static void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            var textBox = (TextBox)sender;
            var command = GetLostFocusCommand(textBox);
            command?.Execute(textBox);
        }
    }

}