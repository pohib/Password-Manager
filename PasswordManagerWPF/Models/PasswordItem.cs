using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace PasswordManagerWPF.Models
{
    public class PasswordItem : INotifyPropertyChanged
    {
        private string password;
        private bool isSelected;

        public string Password
        {
            get => password;
            set { password = value; OnPropertyChanged(); }
        }

        public bool IsSelected
        {
            get => isSelected;
            set { isSelected = value; OnPropertyChanged(); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
