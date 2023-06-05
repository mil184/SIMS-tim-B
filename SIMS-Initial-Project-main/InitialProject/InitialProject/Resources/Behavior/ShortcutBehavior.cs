using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;

namespace InitialProject.Resources.Behavior
{
    public static class KeyboardShortcutBehavior
    {
        public static readonly DependencyProperty ShortcutCommandProperty =
            DependencyProperty.RegisterAttached("ShortcutCommand", typeof(ICommand), typeof(KeyboardShortcutBehavior), new PropertyMetadata(null));

        public static ICommand GetShortcutCommand(UIElement element)
        {
            return (ICommand)element.GetValue(ShortcutCommandProperty);
        }

        public static void SetShortcutCommand(UIElement element, ICommand value)
        {
            element.SetValue(ShortcutCommandProperty, value);
        }

        public static readonly DependencyProperty ShortcutKeyProperty =
            DependencyProperty.RegisterAttached("ShortcutKey", typeof(Key), typeof(KeyboardShortcutBehavior), new PropertyMetadata(Key.None));

        public static Key GetShortcutKey(UIElement element)
        {
            return (Key)element.GetValue(ShortcutKeyProperty);
        }

        public static void SetShortcutKey(UIElement element, Key value)
        {
            element.SetValue(ShortcutKeyProperty, value);
        }

        public static readonly DependencyProperty ShortcutModifiersProperty =
            DependencyProperty.RegisterAttached("ShortcutModifiers", typeof(ModifierKeys), typeof(KeyboardShortcutBehavior), new PropertyMetadata(ModifierKeys.None));

        public static ModifierKeys GetShortcutModifiers(UIElement element)
        {
            return (ModifierKeys)element.GetValue(ShortcutModifiersProperty);
        }

        public static void SetShortcutModifiers(UIElement element, ModifierKeys value)
        {
            element.SetValue(ShortcutModifiersProperty, value);
        }

        public static readonly DependencyProperty IsEnabledProperty =
            DependencyProperty.RegisterAttached("IsEnabled", typeof(bool), typeof(KeyboardShortcutBehavior), new PropertyMetadata(true, OnIsEnabledChanged));

        public static bool GetIsEnabled(UIElement element)
        {
            return (bool)element.GetValue(IsEnabledProperty);
        }

        public static void SetIsEnabled(UIElement element, bool value)
        {
            element.SetValue(IsEnabledProperty, value);
        }

        private static void OnIsEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var element = (UIElement)d;
            if ((bool)e.NewValue)
            {
                element.PreviewKeyDown += Element_PreviewKeyDown;
            }
            else
            {
                element.PreviewKeyDown -= Element_PreviewKeyDown;
            }
        }

        private static void Element_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            var element = (UIElement)sender;
            var shortcutCommand = GetShortcutCommand(element);
            var shortcutKey = GetShortcutKey(element);
            var shortcutModifiers = GetShortcutModifiers(element);

            if (e.Key == shortcutKey && Keyboard.Modifiers == shortcutModifiers && shortcutCommand != null && shortcutCommand.CanExecute(null))
            {
                shortcutCommand.Execute(null);
                e.Handled = true;
            }
        }
    }
}
