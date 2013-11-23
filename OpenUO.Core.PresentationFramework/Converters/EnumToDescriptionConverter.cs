#region References
using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Data;
#endregion

namespace OpenUO.Core.PresentationFramework.Converters
{
	public sealed class EnumToDescriptionConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value == null || !(value is Enum))
			{
				return null;
			}

			Enum en = (Enum)value;

			return en.GetAttribute<DescriptionAttribute>().Description;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}