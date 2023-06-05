using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;

namespace InitialProject.Resources.Behavior
{
    public static class KeyPressBehavior
    {
        public static readonly DependencyProperty KeyPressCommandProperty =
            DependencyProperty.RegisterAttached(
                "KeyPressCommand",
                typeof(ICommand),
                typeof(KeyPressBehavior),
                new PropertyMetadata(null, OnKeyPressCommandChanged));

        public static ICommand GetKeyPressCommand(DependencyObject obj)
        {
            return (ICommand)obj.GetValue(KeyPressCommandProperty);
        }

        public static void SetKeyPressCommand(DependencyObject obj, ICommand value)
        {
            obj.SetValue(KeyPressCommandProperty, value);
        }

        private static void OnKeyPressCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is UIElement element)
            {
                if (e.OldValue != null)
                {
                    element.PreviewKeyDown -= Element_PreviewKeyDown;
                }

                if (e.NewValue != null)
                {
                    element.PreviewKeyDown += Element_PreviewKeyDown;
                }
            }
        }

        private static void Element_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (sender is UIElement element && GetKeyPressCommand(element) is ICommand command)
            {
                command.Execute(e);
                e.Handled = true;
            }
        }
    }
}
