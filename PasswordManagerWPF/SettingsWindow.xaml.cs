using PasswordManagerWPF.Models;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace PasswordManagerWPF
{
    public partial class SettingsWindow : Window
    {
        public PasswordSettings Settings { get; private set; }
        public bool Saved { get; private set; } = false;

        public SettingsWindow(PasswordSettings currentSettings)
        {
            InitializeComponent();
            Settings = currentSettings.Clone();
            DataContext = this;
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            Saved = true;
            DialogResult = true;
            Close();
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            foreach (var ch in e.Text)
            {
                if (!char.IsDigit(ch))
                {
                    e.Handled = true;
                    return;
                }
            }

            var textBox = (TextBox)sender;
            var newText = textBox.Text.Insert(textBox.CaretIndex, e.Text);
            if (int.TryParse(newText, out int value))
            {
                if (value < 8 || value > 64)
                {
                    e.Handled = true;
                }
            }
        }
    }
}