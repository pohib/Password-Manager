using PasswordManagerWPF.Models;
using PasswordManagerWPF.Services;
using PasswordManagerWPF.ViewModels;
using System.Windows;

namespace PasswordManagerWPF.Dialogs
{
    public partial class MultiPassword : Window
    {
        private readonly MultiPasswordViewModel viewModel;

        public List<string> SelectedPasswords => viewModel.GetSelectedPasswords();

        public MultiPassword(List<string> passwords, PasswordService passwordService, PasswordSettings settings)
        {
            InitializeComponent();
            viewModel = new MultiPasswordViewModel(passwords, passwordService, settings);
            DataContext = viewModel;
            viewModel.SetWindow(this);
            Owner = Application.Current.MainWindow;
        }
    }
}