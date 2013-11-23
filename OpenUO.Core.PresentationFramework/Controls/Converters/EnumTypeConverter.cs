#region References
using System;
using System.Globalization;
using System.Windows.Data;
#endregion

namespace OpenUO.Core.PresentationFramework.Converters
{
	public class EnumTypeConverter : IValueConverter
	{
		#region IValueConverter Members
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return Enum.GetValues(value.GetType());
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotSupportedException();
		}
		#endregion
	}
}