using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Interactivity;
using System.Windows;
using System.Windows.Data;

namespace InitialProject.Resources.Behavior
{
    public class WatermarkBehavior : Behavior<TextBox>
    {
        public static readonly DependencyProperty WatermarkTextProperty =
            DependencyProperty.Register("WatermarkText", typeof(string), typeof(WatermarkBehavior), new PropertyMetadata(null));

        public string WatermarkText
        {
            get { return (string)GetValue(WatermarkTextProperty); }
            set { SetValue(WatermarkTextProperty, value); }
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.GotKeyboardFocus += OnGotKeyboardFocus;
            AssociatedObject.LostKeyboardFocus += OnLostKeyboardFocus;
            SetWatermarkText();
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.GotKeyboardFocus -= OnGotKeyboardFocus;
            AssociatedObject.LostKeyboardFocus -= OnLostKeyboardFocus;
        }

        private void OnGotKeyboardFocus(object sender, RoutedEventArgs e)
        {
            if (AssociatedObject.Text == WatermarkText)
            {
                AssociatedObject.Text = string.Empty;
                AssociatedObject.SetBinding(TextBox.ForegroundProperty, new Binding("Foreground") { Source = AssociatedObject });
            }
        }

        private void OnLostKeyboardFocus(object sender, RoutedEventArgs e)
        {
            SetWatermarkText();
        }

        private void SetWatermarkText()
        {
            if (string.IsNullOrEmpty(AssociatedObject.Text))
            {
                AssociatedObject.Text = WatermarkText;
                AssociatedObject.ClearValue(TextBox.ForegroundProperty);
            }
        }
    }

}
