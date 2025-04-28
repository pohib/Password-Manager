using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace PasswordManagerWPF.Converters
{
    public class EntropyToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double entropy)
            {
                if (entropy < 50) return Brushes.Red;
                if (entropy < 70) return Brushes.Orange;
                return Brushes.Green;
            }
            return Brushes.Black;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
