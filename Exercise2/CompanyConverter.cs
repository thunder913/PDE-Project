using System;
using System.Windows.Data;

namespace Exercise2
{
    public class CompanyConverter : IValueConverter
    {
		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
            if (value == null)
            {
				return null;
            }
            
			return ((Company)value).Name;
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			return new Company()
			{
				Name = value.ToString()
			};
		}        
	}
}
