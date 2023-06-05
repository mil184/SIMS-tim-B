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
    public static class DatePickerBehavior
    {
        public static readonly DependencyProperty GotFocusCommandProperty =
            DependencyProperty.RegisterAttached("GotFocusCommand", typeof(ICommand), typeof(DatePickerBehavior), new PropertyMetadata(null, GotFocusCommandChanged));

        public static readonly DependencyProperty LostFocusCommandProperty =
            DependencyProperty.RegisterAttached("LostFocusCommand", typeof(ICommand), typeof(DatePickerBehavior), new PropertyMetadata(null, LostFocusCommandChanged));

        public static ICommand GetGotFocusCommand(DatePicker datePicker)
        {
            return (ICommand)datePicker.GetValue(GotFocusCommandProperty);
        }

        public static void SetGotFocusCommand(DatePicker datePicker, ICommand value)
        {
            datePicker.SetValue(GotFocusCommandProperty, value);
        }

        public static ICommand GetLostFocusCommand(DatePicker datePicker)
        {
            return (ICommand)datePicker.GetValue(LostFocusCommandProperty);
        }

        public static void SetLostFocusCommand(DatePicker datePicker, ICommand value)
        {
            datePicker.SetValue(LostFocusCommandProperty, value);
        }

        private static void GotFocusCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var datePicker = (DatePicker)d;
            datePicker.PreviewGotKeyboardFocus -= DatePicker_PreviewGotKeyboardFocus;

            if (e.NewValue is ICommand command)
            {
                datePicker.PreviewGotKeyboardFocus += DatePicker_PreviewGotKeyboardFocus;
            }
        }

        private static void LostFocusCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var datePicker = (DatePicker)d;
            datePicker.PreviewLostKeyboardFocus -= DatePicker_PreviewLostKeyboardFocus;

            if (e.NewValue is ICommand command)
            {
                datePicker.PreviewLostKeyboardFocus += DatePicker_PreviewLostKeyboardFocus;
            }
        }

        private static void DatePicker_PreviewGotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            var datePicker = (DatePicker)sender;
            var command = GetGotFocusCommand(datePicker);
            command?.Execute(datePicker);
        }

        private static void DatePicker_PreviewLostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            var datePicker = (DatePicker)sender;
            var command = GetLostFocusCommand(datePicker);
            command?.Execute(datePicker);
        }
    }
}
