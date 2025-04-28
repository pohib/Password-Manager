using PasswordManagerWPF.Models;
using PasswordManagerWPF.Services;
using PasswordManagerWPF.Utilities;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace PasswordManagerWPF.ViewModels
{
    public class MultiPasswordViewModel
    {
        private Window window;
        private readonly PasswordService passwordService;
        private readonly PasswordSettings settings;

        public ObservableCollection<PasswordItem> PasswordItems { get; }
        public ICommand GenerateCommand { get; }
        public ICommand SelectAllCommand { get; }
        public ICommand DeselectAllCommand { get; }
        public ICommand SaveCommand { get; }

        private int passwordsCount = 5;
        public int PasswordsCount
        {
            get => passwordsCount;
            set => passwordsCount = value > 0 ? value : 1;
        }

        public MultiPasswordViewModel(List<string> initialPasswords, PasswordService passwordService, PasswordSettings settings)
        {
            this.passwordService = passwordService;
            this.settings = settings;

            PasswordItems = new ObservableCollection<PasswordItem>(
                initialPasswords.Select(p => new PasswordItem { Password = p }));

            GenerateCommand = new RelayCommand(GeneratePasswords);
            SelectAllCommand = new RelayCommand(SelectAll);
            DeselectAllCommand = new RelayCommand(DeselectAll);
            SaveCommand = new RelayCommand(() =>
            {
                if (window != null)
                {
                    window.DialogResult = true;
                    window.Close();
                }
            });
        }

        public void SetWindow(Window window)
        {
            this.window = window;
        }

        public List<string> GetSelectedPasswords()
        {
            return PasswordItems.Where(p => p.IsSelected).Select(p => p.Password).ToList();
        }

        private void GeneratePasswords()
        {
            PasswordItems.Clear();
            for (int i = 0; i < PasswordsCount; i++)
            {
                var password = passwordService.GeneratePassword(settings);
                PasswordItems.Add(new PasswordItem { Password = password });
            }
        }

        private void SelectAll()
        {
            foreach (var item in PasswordItems)
                item.IsSelected = true;
        }

        private void DeselectAll()
        {
            foreach (var item in PasswordItems)
                item.IsSelected = false;
        }
    }
}