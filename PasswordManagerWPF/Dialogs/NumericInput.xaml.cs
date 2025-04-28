using System.Windows;

namespace PasswordManagerWPF.Dialogs
{
    public partial class NumericInput : Window
    {
        public string Prompt { get; }
        public int MinValue { get; }
        public int MaxValue { get; }
        public int SelectedValue { get; set; }

        public NumericInput(string prompt, string title, int defaultValue, int min, int max)
        {
            InitializeComponent();
            Title = title;
            Prompt = prompt;
            MinValue = min;
            MaxValue = max;
            SelectedValue = defaultValue;
            DataContext = this;
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}