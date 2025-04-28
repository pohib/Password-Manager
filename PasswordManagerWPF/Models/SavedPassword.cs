using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

namespace PasswordManagerWPF.Models
{
    [DataContract]
    public class SavedPassword : INotifyPropertyChanged
    {
        private string? name;
        private string? password;
        private DateTime created;

        [DataMember]
        public string? Name
        {
            get => name;
            set { name = value; OnPropertyChanged(); }
        }

        [DataMember]
        public string? Password
        {
            get => password;
            set { password = value; OnPropertyChanged(); }
        }

        [DataMember]
        public DateTime Created
        {
            get => created;
            set { created = value; OnPropertyChanged(); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
