using System;
using System.Collections.Generic;
using System.Text;

namespace Bau.Controls.ListRepeat
{
	/// <summary>
	///		Colecci�n de controles
	/// </summary>
	internal class colListControls : List<clsListControl>
	{	// Variables privadas
			private ListRepeatControls udtParent;
			
		public colListControls(ListRepeatControls udtParent)
		{ Parent = udtParent;
		}
		
		/// <summary>
		///		A�ade un control a la colecci�n
		/// </summary>
		public void Add(int intPaddingLeft, System.Windows.Forms.Control ctlControl)
		{ // A�ade un control
				Add(new clsListControl(intPaddingLeft, ctlControl));
			// Calcula las posiciones
				ComputeTops();
		}
		
		/// <summary>
		///		Elimina un control
		/// </summary>
		public void Remove(System.Windows.Forms.Control ctlControl)
		{ // Elimina el control
				for (int intIndex = Count; intIndex >= 0; intIndex--)
					if (ctlControl == this[intIndex].Control)
						RemoveAt(intIndex);
			// Calcula la posiciones
				ComputeTops();
		}
		
		/// <summary>
		///		Calcula las posiciones de la esquina superior
		/// </summary>
		private void ComputeTops()
		{ int intTop = 0;
		
				// Recorre los controles modificando su esquina superior
					foreach (clsListControl objControl in this)
						{ // Cambia la coordenada superior
								objControl.Top = intTop;
							// Incrementa el punto superior
								intTop += objControl.Control.Height + Parent.Spacing;
						}
		}
		
		/// <summary>
		///		Devuelve la suma de altura de todos los controles
		/// </summary>
		public int TotalHeight()
		{ int intHeight = 0;
		
				// Calcula la altura
					foreach (clsListControl objControl in this)
						intHeight += objControl.Control.Height + Parent.Spacing;
				// Devuelve la altura
					return intHeight;
		}
		
		public ListRepeatControls Parent
		{ get { return udtParent; }
			set { udtParent = value; }
		}
	}
}
