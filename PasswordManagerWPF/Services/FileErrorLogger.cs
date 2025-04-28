using System.IO;

namespace PasswordManagerWPF.Services
{
    public class FileErrorLogger : IErrorLogger
    {
        private readonly string _logFilePath;

        public FileErrorLogger(string logFilePath = "password_manager.log")
        {
            _logFilePath = logFilePath;
        }

        public void LogError(string message, Exception ex = null)
        {
            string logMessage = $"[ERROR] {DateTime.Now}: {message}";
            if (ex != null)
            {
                logMessage += $"\nException: {ex.GetType().Name}\nMessage: {ex.Message}\nStack Trace: {ex.StackTrace}";
            }
            WriteToFile(logMessage);
        }

        public void LogWarning(string message)
        {
            WriteToFile($"[WARNING] {DateTime.Now}: {message}");
        }

        public void LogInfo(string message)
        {
            WriteToFile($"[INFO] {DateTime.Now}: {message}");
        }

        private void WriteToFile(string message)
        {
            try
            {
                File.AppendAllText(_logFilePath, message + Environment.NewLine);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to write to log file: {ex.Message}");
                Console.WriteLine($"Original message: {message}");
            }
        }
    }
}