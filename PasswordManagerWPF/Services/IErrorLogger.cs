namespace PasswordManagerWPF.Services
{
    public interface IErrorLogger
    {
        void LogError(string message, Exception ex = null);
        void LogWarning(string message);
        void LogInfo(string message);
    }
}
