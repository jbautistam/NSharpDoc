using System;

namespace Bau.Controls.Split
{
	/// <summary>
	///		Clase de diseño simple para eliminar propiedades de <see cref="CollapsibleSplitter"/>
	///	en tiempo de diseño
	/// </summary>
	public class CollapsibleSplitterDesigner : System.Windows.Forms.Design.ControlDesigner
	{
		/// <summary>
		///		Elimina las propiedades no deseadas
		/// </summary>
		protected override void PreFilterProperties(System.Collections.IDictionary dctProperties)
		{	dctProperties.Remove("IsCollapsed");
			dctProperties.Remove("BorderStyle");
			dctProperties.Remove("Size");
		}
	}
}
