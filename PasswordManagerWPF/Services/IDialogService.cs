using PasswordManagerWPF.Dialogs;
using PasswordManagerWPF.Models;
using System.Windows;

namespace PasswordManagerWPF.Services
{
    public interface IDialogService
    {
        void ShowMessage(string message, string title = "");
        void ShowError(string message, string title = "Ошибка");
        bool Confirm(string message, string title = "");
        string ShowInputDialog(string prompt, string title);
        string ShowPasswordInputDialog(string prompt, string title);
        int ShowNumericInputDialog(string prompt, string title, int defaultValue, int min, int max);
        List<string> ShowMultiPasswordDialog(List<string> passwords, PasswordService passwordService, PasswordSettings settings);

    }

    public class DialogService : IDialogService
    {
        public void ShowMessage(string message, string title = "")
        {
            MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public void ShowError(string message, string title = "Ошибка")
        {
            MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public bool Confirm(string message, string title = "")
        {
            return MessageBox.Show(message, title, MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes;
        }

        public string ShowInputDialog(string prompt, string title)
        {
            var dialog = new Input(prompt, title);
            return dialog.ShowDialog() == true ? dialog.InputText : null;
        }

        public string ShowPasswordInputDialog(string prompt, string title)
        {
            var dialog = new PasswordInput(prompt, title);
            return dialog.ShowDialog() == true ? dialog.Password : null;
        }

        public int ShowNumericInputDialog(string prompt, string title, int defaultValue, int min, int max)
        {
            var dialog = new NumericInput(prompt, title, defaultValue, min, max);
            return dialog.ShowDialog() == true ? dialog.SelectedValue : defaultValue;
        }

        public List<string> ShowMultiPasswordDialog(List<string> passwords, PasswordService passwordService, PasswordSettings settings)
        {
            var dialog = new MultiPassword(passwords, passwordService, settings);
            return dialog.ShowDialog() == true ? dialog.SelectedPasswords : null;
        }
    }
}