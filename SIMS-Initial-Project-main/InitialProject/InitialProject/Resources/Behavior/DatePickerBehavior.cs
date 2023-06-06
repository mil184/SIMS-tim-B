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
    public static class DatePickerFocusBehavior
    {
        public static ICommand GetGotFocusCommand(DependencyObject obj)
        {
            return (ICommand)obj.GetValue(GotFocusCommandProperty);
        }

        public static void SetGotFocusCommand(DependencyObject obj, ICommand value)
        {
            obj.SetValue(GotFocusCommandProperty, value);
        }

        public static ICommand GetLostFocusCommand(DependencyObject obj)
        {
            return (ICommand)obj.GetValue(LostFocusCommandProperty);
        }

        public static void SetLostFocusCommand(DependencyObject obj, ICommand value)
        {
            obj.SetValue(LostFocusCommandProperty, value);
        }

        public static readonly DependencyProperty GotFocusCommandProperty =
            DependencyProperty.RegisterAttached("GotFocusCommand", typeof(ICommand), typeof(DatePickerFocusBehavior), new PropertyMetadata(null, OnGotFocusCommandChanged));

        public static readonly DependencyProperty LostFocusCommandProperty =
            DependencyProperty.RegisterAttached("LostFocusCommand", typeof(ICommand), typeof(DatePickerFocusBehavior), new PropertyMetadata(null, OnLostFocusCommandChanged));

        private static void OnGotFocusCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is DatePicker datePicker)
            {
                if (e.NewValue is ICommand command)
                {
                    AttachGotFocusHandler(datePicker, command);
                }
                else
                {
                    DetachGotFocusHandler(datePicker);
                }
            }
        }

        private static void OnLostFocusCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is DatePicker datePicker)
            {
                if (e.NewValue is ICommand command)
                {
                    AttachLostFocusHandler(datePicker, command);
                }
                else
                {
                    DetachLostFocusHandler(datePicker);
                }
            }
        }

        private static void AttachGotFocusHandler(DatePicker datePicker, ICommand command)
        {
            var textBox = GetDatePickerTextBox(datePicker);
            if (textBox != null)
            {
                textBox.GotFocus += (sender, e) =>
                {
                    if (command.CanExecute(datePicker))
                    {
                        command.Execute(datePicker);
                    }
                };
            }
        }

        private static void DetachGotFocusHandler(DatePicker datePicker)
        {
            var textBox = GetDatePickerTextBox(datePicker);
            if (textBox != null)
            {
                textBox.GotFocus -= (sender, e) => { };
            }
        }

        private static void AttachLostFocusHandler(DatePicker datePicker, ICommand command)
        {
            var textBox = GetDatePickerTextBox(datePicker);
            if (textBox != null)
            {
                textBox.LostFocus += (sender, e) =>
                {
                    if (command.CanExecute(datePicker))
                    {
                        command.Execute(datePicker);
                    }
                };
            }
        }

        private static void DetachLostFocusHandler(DatePicker datePicker)
        {
            var textBox = GetDatePickerTextBox(datePicker);
            if (textBox != null)
            {
                textBox.LostFocus -= (sender, e) => { };
            }
        }

        private static TextBox GetDatePickerTextBox(DatePicker datePicker)
        {
            var template = datePicker.Template;
            if (template != null)
            {
                var textBox = (TextBox)template.FindName("PART_TextBox", datePicker);
                return textBox;
            }
            return null;
        }
    }

}
