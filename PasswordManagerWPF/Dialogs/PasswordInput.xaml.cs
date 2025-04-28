using System.Windows;
using System.Windows.Controls;

namespace PasswordManagerWPF.Dialogs
{
    public partial class PasswordInput : Window
    {
        public string Password { get; private set; }
        public string Prompt { get; }

        public PasswordInput(string prompt, string title)
        {
            InitializeComponent();
            Title = title;
            Prompt = prompt;
            DataContext = this;
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            Password = PasswordBox.Password;
            DialogResult = true;
        }
    }
}
