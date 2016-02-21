using System;
using System.Collections.Generic;

namespace Bau.Controls.TabControls
{
	/// <summary>
	///		Control que extiende <see cref="TabControl"/> para permitir ocultar fichas
	/// </summary>
	public class TabControlExtended : System.Windows.Forms.TabControl
	{ // Variables con las páginas
			private List<System.Windows.Forms.TabPage> objColPages = null;
			private bool[] arrBoolPagesVisible;
		
		/// <summary>
		///		Inicializa las variables antes de procesar
		/// </summary>	
		private void InitControl()
		{ if (objColPages == null)
				{ // Inicializa la colección de páginas y elementos visibles
						objColPages = new List<System.Windows.Forms.TabPage>();
						arrBoolPagesVisible = new bool[TabPages.Count];
					// Añade las páginas de la ficha a la colección e indica que son visibles
						for (int intIndex = 0; intIndex < TabPages.Count; intIndex++)
							{ // Añade la página
									objColPages.Add(TabPages[intIndex]);
								// Indica que es visible
									arrBoolPagesVisible[intIndex] = true;
							}
				}
		}	
		
		/// <summary>
		///		Muestra una ficha
		/// </summary>
		public void ShowTab(int intTab)
		{ ShowHideTab(intTab, true);
		}
				
		/// <summary>
		///		Oculta una ficha
		/// </summary>
		public void HideTab(int intTab)
		{ ShowHideTab(intTab, false);
		}
		
		/// <summary>
		///		Muestra / oculta una ficha
		/// </summary>
		public void ShowHideTab(int intTab, bool blnVisible)
		{ // Inicializa el control
				InitControl();
			// Oculta la página
				arrBoolPagesVisible[intTab] = blnVisible;
			// Elimina todas las fichas
				TabPages.Clear();
			// Añade únicamente las fichas visibles
				for (int intIndex = 0; intIndex < objColPages.Count; intIndex++)
					if (arrBoolPagesVisible[intIndex])
						TabPages.Add(objColPages[intIndex]);
		}

		/// <summary>
		///		Cuenta el número de fichas visibles
		/// </summary>
		public int CountTabsVisible
		{ get
				{ int intNumber = 0;
				
						// Cuenta el número de páginas visibles
							if (objColPages != null)
								for (int intIndex = 0; intIndex < arrBoolPagesVisible.Length; intIndex++)
									if (arrBoolPagesVisible[intIndex])
										intNumber++;
						// Devuelve el número de páginas visibles
							return intNumber;
				}
		}
	}
}
