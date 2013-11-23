#region References
using System.Windows;
#endregion

namespace OpenUO.Core.PresentationFramework.TypeEditors
{
	public class NumberTypeEditor<T> : IntegerTypeEditor
	{
		public string Typ { get; set; }

		static NumberTypeEditor()
		{
			DefaultStyleKeyProperty.OverrideMetadata(
				typeof(NumberTypeEditor<T>), new FrameworkPropertyMetadata(typeof(IntegerTypeEditor)));
		}
	}
}