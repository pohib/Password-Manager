using PasswordManagerWPF.Models;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;

namespace PasswordManagerWPF.Services
{
    public class PasswordService
    {
        private readonly IErrorLogger logger;
        public event Action<string> ErrorOccurred;

        public PasswordService(IErrorLogger logger)
        {
            this.logger = logger;
        }

        public bool TrySavePasswordHistory(List<SavedPassword> passwords, out string errorMessage)
        {
            try
            {
                SavePasswordHistory(passwords);
                errorMessage = null;
                return true;
            }
            catch (Exception ex)
            {
                errorMessage = GetUserFriendlyErrorMessage(ex);
                logger.LogError("Ошибка сохранения истории паролей", ex);
                ErrorOccurred?.Invoke(errorMessage);
                return false;
            }
        }

        public bool TryLoadPasswordHistory(out List<SavedPassword> passwords, out string errorMessage)
        {
            try
            {
                passwords = LoadPasswordHistory();
                errorMessage = null;
                return true;
            }
            catch (Exception ex)
            {
                passwords = new List<SavedPassword>();
                errorMessage = GetUserFriendlyErrorMessage(ex);
                logger.LogError("Ошибка загрузки истории паролей", ex);
                ErrorOccurred?.Invoke(errorMessage);
                return false;
            }
        }

        public void SavePasswordHistory(List<SavedPassword> passwords)
        {
            using (var ms = new MemoryStream())
            {
                var serializer = new DataContractJsonSerializer(typeof(List<SavedPassword>));
                serializer.WriteObject(ms, passwords);
                byte[] encryptedData = ProtectedData.Protect(
                    ms.ToArray(),
                    Encoding.UTF8.GetBytes(GetUserSpecificKey()),
                    DataProtectionScope.CurrentUser);
                File.WriteAllBytes("passwords.secure", encryptedData);
            }
        }

        public List<SavedPassword> LoadPasswordHistory()
        {
            if (!File.Exists("passwords.secure"))
                return new List<SavedPassword>();

            byte[] encryptedData = File.ReadAllBytes("passwords.secure");
            byte[] decryptedData = ProtectedData.Unprotect(
                encryptedData,
                Encoding.UTF8.GetBytes(GetUserSpecificKey()),
                DataProtectionScope.CurrentUser);

            using (var ms = new MemoryStream(decryptedData))
            {
                var serializer = new DataContractJsonSerializer(typeof(List<SavedPassword>));
                return (List<SavedPassword>)serializer.ReadObject(ms);
            }
        }

        private string GetUserFriendlyErrorMessage(Exception ex)
        {
            return ex switch
            {
                UnauthorizedAccessException => "Отказано в доступе к файлу",
                FileNotFoundException => "Файл \"passwords.secure\" не найден",
                CryptographicException => "Ошибка дешифрования истории паролей",
                SerializationException => "Невозможно прочитать сохраненные пароли",
                IOException => "Ошибка при работе с файлом",
                _ => "Произошла непредвиденная ошибка"
            };
        }
        public string GeneratePassword(PasswordSettings settings)
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

        public double CalculateEntropy(string password, PasswordSettings settings)
        {
            int poolSize = 0;
            if (settings.UseUpper) poolSize += 26;
            if (settings.UseLower) poolSize += 26;
            if (settings.UseNumbers) poolSize += 10;
            if (settings.UseSymbols) poolSize += 32;

            return Math.Log(Math.Pow(poolSize, password.Length), 2);
        }
        private string GetUserSpecificKey()
        {
            string userSid = WindowsIdentity.GetCurrent().User.Value;
            using (var sha = SHA256.Create())
            {
                byte[] hash = sha.ComputeHash(Encoding.UTF8.GetBytes(userSid + "my-secret-salt"));
                return Convert.ToBase64String(hash);
            }
        }
    }
}
