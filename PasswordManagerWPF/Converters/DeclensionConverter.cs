using System.Globalization;
using System.Windows.Data;

namespace PasswordManagerWPF.Converters
{
    public class DeclensionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || parameter == null)
                return string.Empty;

            if (!int.TryParse(value.ToString(), out int num))
                return string.Empty;

            string[] forms = parameter.ToString().Split('|');
            if (forms.Length != 3)
                return string.Empty;
            string wordForm = GetDeclensionForm(num, forms);
            return $"{num} {wordForm}";
        }

        private string GetDeclensionForm(int number, string[] forms)
        {
            int absNumber = Math.Abs(number);
            int lastTwoDigits = absNumber % 100;
            int lastDigit = absNumber % 10;

            if (lastTwoDigits > 10 && lastTwoDigits < 20)
                return forms[2];

            switch (lastDigit)
            {
                case 1:
                    return forms[0];
                case 2:
                case 3:
                case 4:
                    return forms[1];
                default:
                    return forms[2];
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

