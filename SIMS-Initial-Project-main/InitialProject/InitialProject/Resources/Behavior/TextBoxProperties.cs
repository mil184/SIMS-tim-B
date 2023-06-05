using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Interactivity;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace InitialProject.Resources.Behavior
{
    public static class TextBoxProperties
    {
        public static readonly DependencyProperty WatermarkTextProperty =
            DependencyProperty.RegisterAttached("WatermarkText", typeof(string), typeof(TextBoxProperties),
                new PropertyMetadata(string.Empty, OnWatermarkTextChanged));

        public static string GetWatermarkText(DependencyObject obj)
        {
            return (string)obj.GetValue(WatermarkTextProperty);
        }

        public static void SetWatermarkText(DependencyObject obj, string value)
        {
            obj.SetValue(WatermarkTextProperty, value);
        }

        private static void OnWatermarkTextChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            var textBox = obj as TextBox;
            if (textBox == null)
                return;

            textBox.Loaded -= TextBox_Loaded;
            textBox.TextChanged -= TextBox_TextChanged;
            textBox.GotFocus -= TextBox_GotFocus;
            textBox.LostFocus -= TextBox_LostFocus;

            if (string.IsNullOrEmpty(textBox.Text))
                ShowWatermark(textBox);

            textBox.Loaded += TextBox_Loaded;
            textBox.TextChanged += TextBox_TextChanged;
            textBox.GotFocus += TextBox_GotFocus;
            textBox.LostFocus += TextBox_LostFocus;
        }

        private static void TextBox_Loaded(object sender, RoutedEventArgs e)
        {
            var textBox = (TextBox)sender;
            ShowWatermark(textBox);
        }

        private static void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = (TextBox)sender;
            if (string.IsNullOrEmpty(textBox.Text))
                ShowWatermark(textBox);
            else
                HideWatermark(textBox);
        }

        private static void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            var textBox = (TextBox)sender;
            if (textBox.Foreground == Brushes.Gray)
            {
                textBox.Text = string.Empty;
                textBox.Foreground = SystemColors.WindowTextBrush;
            }
        }

        private static void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            var textBox = (TextBox)sender;
            if (string.IsNullOrEmpty(textBox.Text))
                ShowWatermark(textBox);
        }

        private static void ShowWatermark(TextBox textBox)
        {
            var watermarkText = GetWatermarkText(textBox);
            if (!string.IsNullOrEmpty(watermarkText) && string.IsNullOrEmpty(textBox.Text))
            {
                var visualBrush = new VisualBrush
                {
                    Stretch = Stretch.None,
                    AlignmentX = AlignmentX.Left,
                    AlignmentY = AlignmentY.Center,
                    Visual = new TextBlock
                    {
                        Text = watermarkText,
                        Foreground = Brushes.Gray,
                        FontStyle = FontStyles.Italic,
                        Opacity = 0.5,
                        IsHitTestVisible = false
                    }
                };

                textBox.Background = visualBrush;
            }
        }

        private static void HideWatermark(TextBox textBox)
        {
            textBox.Background = Brushes.Transparent;
        }
    }
}
