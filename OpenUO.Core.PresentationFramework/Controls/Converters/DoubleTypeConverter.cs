#region References
using System;
using System.Globalization;
using System.Threading;
using System.Windows.Data;
#endregion

namespace OpenUO.Core.PresentationFramework.Converters
{
	public class DoubleTypeConverter : IValueConverter
	{
		#region IValueConverter Members
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return value == null ? null : ((double)value).ToString(Thread.CurrentThread.CurrentCulture);
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return value == null ? 0.0 : double.Parse((string)value, Thread.CurrentThread.CurrentCulture);
		}
		#endregion
	}
}