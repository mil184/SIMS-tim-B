using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;

namespace InitialProject.Resources.Behavior
{
    public static class KeyBindingHelper
    {
        public static readonly DependencyProperty KeyEventArgsProperty =
            DependencyProperty.RegisterAttached(
                "KeyEventArgs",
                typeof(KeyEventArgs),
                typeof(KeyBindingHelper),
                new PropertyMetadata(null));

        public static KeyEventArgs GetKeyEventArgs(DependencyObject obj)
        {
            return (KeyEventArgs)obj.GetValue(KeyEventArgsProperty);
        }

        public static void SetKeyEventArgs(DependencyObject obj, KeyEventArgs value)
        {
            obj.SetValue(KeyEventArgsProperty, value);
        }
    }
}
