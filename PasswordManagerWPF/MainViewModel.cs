using PasswordManagerWPF;
using PasswordManagerWPF.Dialogs;
using PasswordManagerWPF.Models;
using PasswordManagerWPF.Services;
using PasswordManagerWPF.Utilities;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

public class MainViewModel : INotifyPropertyChanged
{
    private readonly PasswordService passwordService;
    private readonly IDialogService dialogService;

    private string currentPassword;
    private PasswordSettings settings;
    private ObservableCollection<SavedPassword> passwordHistory;
    private SavedPassword selectedPassword;
    private double passwordEntropy;
    private bool isPasswordVisible;
    private ObservableCollection<string> generatedPasswords;
    private int passwordsCount = 5;
    public MainViewModel(PasswordService passwordService, IDialogService dialogService)
    {
        this.passwordService = passwordService;
        this.dialogService = dialogService;
        this.passwordService.ErrorOccurred += OnErrorOccurred;
        settings = new PasswordSettings();
        GenerateFirstPassword();
        LoadPasswordHistory();

        GeneratePasswordCommand = new RelayCommand(GeneratePassword);
        SavePasswordCommand = new RelayCommand(SaveCurrentPassword, CanSavePassword);
        DeletePasswordCommand = new RelayCommand(DeleteSelectedPassword, CanDeletePassword);
        ClearHistoryCommand = new RelayCommand(ClearHistory);
        CopyPasswordCommand = new RelayCommand(CopyToClipboard);
        TogglePasswordVisibilityCommand = new RelayCommand(TogglePasswordVisibility);
        VerifyPasswordCommand = new RelayCommand(VerifyPassword);
        GenerateMultiplePasswordsCommand = new RelayCommand(GenerateMultiplePasswords);
        OpenSettingsCommand = new RelayCommand(OpenSettings);
        GeneratedPasswords = new ObservableCollection<string>();
    }

    public ObservableCollection<string> GeneratedPasswords
    {
        get => generatedPasswords;
        set
        {
            generatedPasswords = value;
            OnPropertyChanged();
        }
    }
    public int PasswordsCount
    {
        get => passwordsCount;
        set
        {
            passwordsCount = value;
            OnPropertyChanged();
        }
    }
    public string CurrentPassword
    {
        get => currentPassword;
        set
        {
            currentPassword = value;
            OnPropertyChanged();
            PasswordEntropy = passwordService.CalculateEntropy(value, Settings);
            CommandManager.InvalidateRequerySuggested();
        }
    }

    public PasswordSettings Settings
    {
        get => settings;
        set
        {
            settings = value;
            OnPropertyChanged();
        }
    }

    public ObservableCollection<SavedPassword> PasswordHistory
    {
        get => passwordHistory;
        set
        {
            passwordHistory = value;
            OnPropertyChanged();
        }
    }

    public SavedPassword SelectedPassword
    {
        get => selectedPassword;
        set
        {
            selectedPassword = value;
            OnPropertyChanged();
            CommandManager.InvalidateRequerySuggested();
        }
    }

    public double PasswordEntropy
    {
        get => passwordEntropy;
        set
        {
            passwordEntropy = value;
            OnPropertyChanged();
        }
    }

    public bool IsPasswordVisible
    {
        get => isPasswordVisible;
        set
        {
            isPasswordVisible = value;
            OnPropertyChanged();
        }
    }

    public ICommand GeneratePasswordCommand { get; }
    public ICommand SavePasswordCommand { get; }
    public ICommand DeletePasswordCommand { get; }
    public ICommand ClearHistoryCommand { get; }
    public ICommand CopyPasswordCommand { get; }
    public ICommand TogglePasswordVisibilityCommand { get; }
    public ICommand VerifyPasswordCommand { get; }
    public ICommand GenerateMultiplePasswordsCommand { get; }
    public ICommand OpenSettingsCommand { get; }
    public ICommand SelectAllPasswordsCommand { get; }
    public ICommand DeselectAllPasswordsCommand { get; }
    public ICommand SaveSelectedPasswordsCommand { get; }
    public ICommand SaveAllPasswordsCommand { get; }

    private void GenerateFirstPassword()
    {
        try
        {
            CurrentPassword = passwordService.GeneratePassword(Settings);
        }
        catch (ArgumentException ex)
        {
            dialogService.ShowError(ex.Message);
        }
    }
    private void GeneratePassword()
    {
        try
        {
            CurrentPassword = passwordService.GeneratePassword(Settings);
            dialogService.ShowMessage("Новый пароль успешно сгенерирован!");
        }
        catch (ArgumentException ex)
        {
            dialogService.ShowError(ex.Message);
        }
    }

    private bool CanSavePassword() => !string.IsNullOrEmpty(CurrentPassword);

    private void SaveCurrentPassword()
    {
        var result = dialogService.ShowInputDialog("Введите имя/описание для пароля:", "Сохранение пароля");
        if (!string.IsNullOrEmpty(result))
        {
            var newPassword = new SavedPassword
            {
                Name = result,
                Password = CurrentPassword,
                Created = DateTime.Now
            };

            PasswordHistory.Add(newPassword);

            if (!passwordService.TrySavePasswordHistory(PasswordHistory.ToList(), out var error))
            {
                dialogService.ShowError(error);
            }
            else
            {
                dialogService.ShowMessage("Пароль успешно сохранен!");
            }
        }
    }

    private bool CanDeletePassword() => SelectedPassword != null;

    private void DeleteSelectedPassword()
    {
        if (dialogService.Confirm($"Удалить пароль '{SelectedPassword.Name}'?"))
        {
            PasswordHistory.Remove(SelectedPassword);
            passwordService.SavePasswordHistory(PasswordHistory.ToList());
            dialogService.ShowMessage("Пароль удален из истории.");
        }
    }

    private void ClearHistory()
    {
        if (dialogService.Confirm("Очистить всю историю паролей? Это действие нельзя отменить."))
        {
            PasswordHistory.Clear();
            passwordService.SavePasswordHistory(PasswordHistory.ToList());
            dialogService.ShowMessage("История паролей очищена.");
        }
    }

    private void CopyToClipboard()
    {
        if (!string.IsNullOrEmpty(CurrentPassword))
        {
            Clipboard.SetText(CurrentPassword);
            dialogService.ShowMessage("Пароль скопирован в буфер обмена!");
        }
    }

    private void TogglePasswordVisibility()
    {
        IsPasswordVisible = !IsPasswordVisible;
    }

    private void VerifyPassword()
    {
        var input = dialogService.ShowPasswordInputDialog("Введите пароль для проверки:", "");
        if (input != null)
        {
            if (input == CurrentPassword)
            {
                dialogService.ShowMessage("Пароль введён верно", "Проверка пароля");
            }
            else
            {
                dialogService.ShowError("Неверный пароль", "Проверка пароля");
            }
        }
    }

    private void GenerateMultiplePasswords()
    {
        var initialPasswords = new List<string>();
        for (int i = 0; i < PasswordsCount; i++)
        {
            initialPasswords.Add(passwordService.GeneratePassword(Settings));
        }

        var dialog = new MultiPassword(initialPasswords, passwordService, Settings);
        if (dialog.ShowDialog() == true && dialog.SelectedPasswords != null)
        {
            int maxNumber = PasswordHistory
                .Where(p => p.Name?.StartsWith("password") ?? false)
                .Select(p =>
                {
                    if (int.TryParse(p.Name?.Substring("password".Length), out int num))
                        return num;
                    return 0;
                })
                .DefaultIfEmpty(0)
                .Max();

            for (int i = 0; i < dialog.SelectedPasswords.Count; i++)
            {
                PasswordHistory.Add(new SavedPassword
                {
                    Name = $"password{maxNumber + i + 1}",
                    Password = dialog.SelectedPasswords[i],
                    Created = DateTime.Now
                });
            }

            if (!passwordService.TrySavePasswordHistory(PasswordHistory.ToList(), out var error))
            {
                dialogService.ShowError(error);
            }
            else
            {
                dialogService.ShowMessage($"Сохранено паролей: {dialog.SelectedPasswords.Count}");
            }
        }
    }

    private void OpenSettings()
    {
        var settingsWindow = new SettingsWindow(Settings.Clone());
        if (settingsWindow.ShowDialog() == true)
        {
            Settings = settingsWindow.Settings;
            dialogService.ShowMessage("Настройки генерации паролей сохранены.");
        }
    }
    private void LoadPasswordHistory()
    {
        try
        {
            var history = passwordService.LoadPasswordHistory();
            PasswordHistory = new ObservableCollection<SavedPassword>(history);
        }
        catch (Exception ex)
        {
            dialogService.ShowError($"Ошибка загрузки истории паролей: {ex.Message}");
            PasswordHistory = new ObservableCollection<SavedPassword>();
        }
    }

    private void OnErrorOccurred(string errorMessage)
    {
        dialogService.ShowError(errorMessage);
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}