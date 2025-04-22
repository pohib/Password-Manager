using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;

class PasswordManager
{
    private static string? currentPassword;
    private static List<SavedPassword> passwordHistory = new List<SavedPassword>();

    public static string GenerateAesKey()
    {
        using (var aes = Aes.Create())
        {
            aes.KeySize = 256;
            aes.GenerateKey();
            return Convert.ToBase64String(aes.Key);
        }
    }

    private static PasswordSettings settings = new PasswordSettings
    {
        Length = 12,
        UseUpper = true,
        UseLower = true,
        UseNumbers = true,
        UseSymbols = true,
        ExcludeSimilar = true,
        ExcludeAmbiguous = false
    };

    public class PasswordSettings
    {
        public int Length { get; set; }
        public bool UseUpper { get; set; }
        public bool UseLower { get; set; }
        public bool UseNumbers { get; set; }
        public bool UseSymbols { get; set; }
        public bool ExcludeSimilar { get; set; }
        public bool ExcludeAmbiguous { get; set; }
    }

    [DataContract]
    public class SavedPassword
    {
        [DataMember]
        public string? Name { get; set; }

        [DataMember]
        public string? Password { get; set; }

        [DataMember]
        public DateTime Created { get; set; }
    }

    static void Main()
    {
        currentPassword = GeneratePassword(settings);
        LoadPasswordHistory();

        while (true)
        {
            Console.Clear();
            ShowMainMenu();

            var input = Console.ReadKey().Key;
            Console.WriteLine();

            switch (input)
            {
                case ConsoleKey.D1:
                    ShowCurrentPassword();
                    break;
                case ConsoleKey.D2:
                    GenerateNewPassword();
                    break;
                case ConsoleKey.D3:
                    VerifyPassword();
                    break;
                case ConsoleKey.D4:
                    PasswordSettingsMenu();
                    break;
                case ConsoleKey.D5:
                    PasswordHistoryMenu();
                    break;
                case ConsoleKey.D6:
                    SaveCurrentPassword();
                    break;
                case ConsoleKey.D7:
                    GenerateMultiplePasswords();
                    break;
                case ConsoleKey.D8:
                    Environment.Exit(0);
                    break;
                default:
                    ShowError("Неизвестная команда");
                    break;
            }
        }
    }

    private static void ShowMainMenu()
    {
        Console.WriteLine("=== МЕНЕДЖЕР ПАРОЛЕЙ ===");
        Console.WriteLine($"Текущая длина пароля: {settings.Length}");
        Console.WriteLine("Используемые символы: " + GetUsedCharsDescription());
        Console.WriteLine("\n1 - Показать текущий пароль");
        Console.WriteLine("2 - Сгенерировать новый пароль");
        Console.WriteLine("3 - Проверить пароль");
        Console.WriteLine("4 - Настройки генерации");
        Console.WriteLine("5 - История паролей");
        Console.WriteLine("6 - Сохранить текущий пароль");
        Console.WriteLine("7 - Сгенерировать несколько паролей");
        Console.WriteLine("8 - Выход");
        Console.Write("\nВыберите действие: ");
    }

    private static string GetUsedCharsDescription()
    {
        var parts = new List<string>();
        if (settings.UseUpper) parts.Add("A-Z");
        if (settings.UseLower) parts.Add("a-z");
        if (settings.UseNumbers) parts.Add("0-9");
        if (settings.UseSymbols) parts.Add("!@#$%...");

        return string.Join(", ", parts);
    }

    private static void ShowCurrentPassword()
    {
        Console.Clear();
        Console.WriteLine($"Текущий пароль: {currentPassword}");
        Console.WriteLine($"Длина: {currentPassword.Length} символов");
        Console.WriteLine($"Энтропия: {CalculateEntropy(currentPassword):F2} бит");
        WaitForEnter();
    }

    private static double CalculateEntropy(string password)
    {
        int poolSize = 0;
        if (settings.UseUpper) poolSize += 26;
        if (settings.UseLower) poolSize += 26;
        if (settings.UseNumbers) poolSize += 10;
        if (settings.UseSymbols) poolSize += 32;

        return Math.Log(Math.Pow(poolSize, password.Length), 2);
    }

    private static void GenerateNewPassword()
    {
        currentPassword = GeneratePassword(settings);
        Console.Clear();
        Console.WriteLine("Новый пароль успешно сгенерирован!");
        Console.WriteLine($"Новый пароль: {currentPassword}");
        WaitForEnter();
    }

    private static void VerifyPassword()
    {
        Console.Clear();
        Console.Write("Введите пароль для проверки: ");
        string input = ReadPassword();

        Console.WriteLine();
        if (input == currentPassword)
        {
            Console.WriteLine("✓ Доступ разрешён!");
        }
        else
        {
            Console.WriteLine("× Неверный пароль!");
        }
        WaitForEnter();
    }

    private static string ReadPassword()
    {
        string password = "";
        ConsoleKeyInfo key;

        do
        {
            key = Console.ReadKey(true);

            if (key.Key != ConsoleKey.Backspace && key.Key != ConsoleKey.Enter)
            {
                password += key.KeyChar;
                Console.Write("*");
            }
            else if (key.Key == ConsoleKey.Backspace && password.Length > 0)
            {
                password = password.Substring(0, (password.Length - 1));
                Console.Write("\b \b");
            }
        } while (key.Key != ConsoleKey.Enter);

        return password;
    }

    private static void PasswordSettingsMenu()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("=== НАСТРОЙКИ ГЕНЕРАЦИИ ПАРОЛЕЙ ===");
            Console.WriteLine($"1. Длина пароля: {settings.Length}");
            Console.WriteLine($"2. Использовать заглавные буквы (A-Z): {(settings.UseUpper ? "Да" : "Нет")}");
            Console.WriteLine($"3. Использовать строчные буквы (a-z): {(settings.UseLower ? "Да" : "Нет")}");
            Console.WriteLine($"4. Использовать цифры (0-9): {(settings.UseNumbers ? "Да" : "Нет")}");
            Console.WriteLine($"5. Использовать спецсимволы (!@#$): {(settings.UseSymbols ? "Да" : "Нет")}");
            Console.WriteLine($"6. Исключать похожие символы (1,l,I,0,O): {(settings.ExcludeSimilar ? "Да" : "Нет")}");
            Console.WriteLine($"7. Исключать неоднозначные символы ([]()/\'\"): {(settings.ExcludeAmbiguous ? "Да" : "Нет")}");
            Console.WriteLine("8. Сбросить настройки");
            Console.WriteLine("9. Вернуться в главное меню");
            Console.Write("\nВыберите параметр для изменения: ");

            var key = Console.ReadKey().Key;
            Console.WriteLine();

            switch (key)
            {
                case ConsoleKey.D1:
                    Console.Write("Введите новую длину пароля (8-64): ");
                    if (int.TryParse(Console.ReadLine(), out int length) && length >= 8 && length <= 64)
                    {
                        settings.Length = length;
                        Console.WriteLine("Длина успешно изменена!");
                    }
                    else
                    {
                        ShowError("Некорректное значение длины");
                    }
                    break;

                case ConsoleKey.D2:
                    settings.UseUpper = !settings.UseUpper;
                    Console.WriteLine($"Использование заглавных букв: {(settings.UseUpper ? "Включено" : "Отключено")}");

                    break;

                case ConsoleKey.D3:
                    settings.UseLower = !settings.UseLower;
                    Console.WriteLine($"Использование строчных букв: {(settings.UseLower ? "Включено" : "Отключено")}");
                    break;
                case ConsoleKey.D4:
                    settings.UseNumbers = !settings.UseNumbers;
                    Console.WriteLine($"Использование цифр: {(settings.UseLower ? "Включено" : "Отключено")}");
                    break;
                case ConsoleKey.D5:
                    settings.UseSymbols = !settings.UseSymbols;
                    Console.WriteLine($"Использование спец. символов: {(settings.UseSymbols ? "Включено" : "Отключено")}");
                    break;
                case ConsoleKey.D6:
                    settings.ExcludeSimilar = !settings.ExcludeSimilar;
                    Console.WriteLine($"Использование похожих символов: {(settings.ExcludeSimilar ? "Включено" : "Отключено")}");
                    break;
                case ConsoleKey.D7:
                    settings.ExcludeAmbiguous = !settings.ExcludeAmbiguous;
                    Console.WriteLine($"Исключение неоднозначных символов: {(settings.ExcludeSimilar ? "Включено" : "Отключено")}");
                    break;
                case ConsoleKey.D8:
                    settings = new PasswordSettings
                    {
                        Length = 12,
                        UseUpper = true,
                        UseLower = true,
                        UseNumbers = true,
                        UseSymbols = true,
                        ExcludeSimilar = true,
                        ExcludeAmbiguous = false
                    };
                    Console.WriteLine("Настройки сброшены к значениям по умолчанию");
                    break;

                case ConsoleKey.D9:
                    return;

                default:
                    ShowError("Неизвестная команда");
                    break;
            }
            WaitForEnter();
        }
    }

    private static void PasswordHistoryMenu()
    {
        Console.Clear();
        Console.WriteLine("=== ИСТОРИЯ СОХРАНЕННЫХ ПАРОЛЕЙ ===");

        if (passwordHistory.Count == 0)
        {
            Console.WriteLine("История паролей пуста");
        }
        else
        {
            for (int i = 0; i < passwordHistory.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {passwordHistory[i].Name} - {passwordHistory[i].Password} ({passwordHistory[i].Created:g})");
            }
        }

        Console.WriteLine("\n1. Удалить пароль");
        Console.WriteLine("2. Очистить историю");
        Console.WriteLine("3. Вернуться");
        Console.WriteLine();

        for (int i = 0; i < 1; i++)
        {
            Console.Write("\nВыберите действие: ");
            switch (Console.ReadKey().Key)
            {
                case ConsoleKey.D1:
                    if (passwordHistory.Count == 0)
                    {
                        Console.WriteLine();
                        ShowError("Нет паролей для удаления");
                        break;
                    }
                    Console.Write("\nВведите номер пароля для удаления: ");
                    if (int.TryParse(Console.ReadLine(), out int index) && index > 0 && index <= passwordHistory.Count)
                    {
                        ShowSuccess($"Пароль {passwordHistory[index - 1].Name} успешно удалён.");
                        passwordHistory.RemoveAt(index - 1);
                        SavePasswordHistory();
                    }
                    else
                    {
                        Console.WriteLine();
                        ShowError("Некорректный номер пароля");
                        i--;
                        continue;
                    }
                    break;
                case ConsoleKey.D2:
                    passwordHistory.Clear();
                    Console.WriteLine("История очищена");
                    break;
                case ConsoleKey.D3:
                    return;
                default:
                    ShowError("Неизвестная команда");
                    i--;
                    continue;
            }
        }
        WaitForEnter();
    }

    private static void SaveCurrentPassword()
    {
        Console.Clear();
        Console.Write("Введите имя/описание для пароля: ");
        string name = Console.ReadLine();

        passwordHistory.Add(new SavedPassword
        {
            Name = name,
            Password = currentPassword,
            Created = DateTime.Now
        });

        SavePasswordHistory();
        ShowSuccess("Пароль успешно сохранен!");
        WaitForEnter();
    }

    private static void GenerateMultiplePasswords()
    {
        Console.Clear();
        Console.Write("Сколько паролей сгенерировать? (1-20): ");

        if (int.TryParse(Console.ReadLine(), out int count) && count > 0 && count <= 20)
        {
            Console.WriteLine("\nСгенерированные пароли:");
            for (int i = 0; i < count; i++)
            {
                Console.WriteLine($"{i + 1}. {GeneratePassword(settings)}");
            }
        }
        else
        {
            ShowError("Некорректное количество");
        }

        WaitForEnter();
    }

    private static string GeneratePassword(PasswordSettings settings)
    {
        string upper = "ABCDEFGHJKLMNPQRSTUVWXYZ";
        string lower = "abcdefghijkmnpqrstuvwxyz";
        string numbers = "23456789";
        string symbols = "!@#$%^&*()_+-={}[]|:;<>,.?/";

        if (settings.ExcludeSimilar)
        {
            upper = upper.Replace("S", "").Replace("I", "").Replace("O", "");
            lower = lower.Replace("s", "").Replace("l", "").Replace("o", "");
            numbers = numbers.Replace("5", "").Replace("3", "");
        }

        if (settings.ExcludeAmbiguous)
        {
            symbols = symbols.Replace("{", "").Replace("}", "")
                            .Replace("[", "").Replace("]", "")
                            .Replace("(", "").Replace(")", "")
                            .Replace("/", "").Replace("\\", "")
                            .Replace("'", "").Replace("\"", "");
        }

        var charPool = new StringBuilder();
        if (settings.UseUpper && upper.Length > 0) charPool.Append(upper);
        if (settings.UseLower && lower.Length > 0) charPool.Append(lower);
        if (settings.UseNumbers && numbers.Length > 0) charPool.Append(numbers);
        if (settings.UseSymbols && symbols.Length > 0) charPool.Append(symbols);

        string pool = charPool.ToString();

        if (pool.Length == 0)
        {
            throw new ArgumentException("Невозможно сгенерировать пароль - пул символов пуст. Проверьте настройки.");
        }

        byte[] randomBytes = new byte[settings.Length * 4];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomBytes);
        }

        var password = new char[settings.Length];

        for (int i = 0; i < settings.Length; i++)
        {
            uint randomValue = BitConverter.ToUInt32(randomBytes, i * 4);
            int index = (int)(randomValue % (uint)pool.Length);
            password[i] = pool[index];
        }

        return new string(password);
    }



    private static void LoadPasswordHistory()
    {
        try
        {
            if (File.Exists("passwords.secure"))
            {
                byte[] encryptedData = File.ReadAllBytes("passwords.secure");
                byte[] decryptedData = ProtectedData.Unprotect(encryptedData, Encoding.UTF8.GetBytes(GetUserSpecificKey()), DataProtectionScope.CurrentUser);
                using (var ms = new MemoryStream(decryptedData))
                {
                    var serializer = new DataContractJsonSerializer(typeof(List<SavedPassword>));
                    passwordHistory = (List<SavedPassword>)serializer.ReadObject(ms);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка загрузки: {ex.Message}");
            passwordHistory = new List<SavedPassword>();
        }
    }

    private static string GetUserSpecificKey()
    {
        string userSid = WindowsIdentity.GetCurrent().User.Value;
        using (var sha = SHA256.Create())
        {
            byte[] hash = sha.ComputeHash(Encoding.UTF8.GetBytes(userSid + "my-secret-salt"));
            return Convert.ToBase64String(hash);
        }
    }

    private static void SavePasswordHistory()
    {
        try
        {
            using (var ms = new MemoryStream())
            {
                var serializer = new DataContractJsonSerializer(typeof(List<SavedPassword>));
                serializer.WriteObject(ms, passwordHistory);
                byte[] encryptedData = ProtectedData.Protect(
                    ms.ToArray(),
                    Encoding.UTF8.GetBytes(GetUserSpecificKey()),
                    DataProtectionScope.CurrentUser);
                File.WriteAllBytes("passwords.secure", encryptedData);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка сохранения: {ex.Message}");
        }
    }

    private static void WaitForEnter()
    {
        Console.WriteLine("\nНажмите Enter для продолжения...");
        while (Console.ReadKey().Key != ConsoleKey.Enter) { }
    }


    private static void ShowSuccess(string message)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine(message);
        Console.ResetColor();
        Thread.Sleep(1500);
    }
    private static void ShowError(string message)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(message);
        Console.ResetColor();
        Thread.Sleep(1500);
    }
}


