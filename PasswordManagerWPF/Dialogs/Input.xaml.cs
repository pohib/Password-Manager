using System.Windows;

namespace PasswordManagerWPF.Dialogs
{
    public partial class Input : Window
    {
        public string InputText { get; private set; }

        public Input(string prompt, string title)
        {
            InitializeComponent();
            Title = title;
            PromptTextBlock.Text = prompt;
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            InputText = InputTextBox.Text;
            DialogResult = true;
        }
    }
}
