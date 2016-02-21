using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace Bau.Controls.TextBox
{
	/// <summary>
	///		Extensión del control de Windows NumericUpDown para permitir utilizar el punto como coma decimal
	/// y seleccionar todo el texto cuando se entre en el control
	/// </summary>
	public class NumericUpDowExtended : System.Windows.Forms.NumericUpDown
	{
		public NumericUpDowExtended()
		{ TextAlign = HorizontalAlignment.Right;
			Maximum = 9999999;
		}

		/// <summary>
		///		Sobrescribe el evento para seleccionar todo el texto
		/// </summary>
		protected override void OnGotFocus(EventArgs e)
		{ // Realiza el evento base
				base.OnGotFocus(e);
			// Selecciona todo el texto
				Select(0, Text.Length);
		}

		protected override void OnClick(EventArgs e)
		{ // Realize el evento base
				base.OnClick(e);
			// Selecciona todo el texto
				Select(0, Text.Length);
		}
		
		/// <summary>
		///		Sobrescribe el evento para tratar el punto como una coma decimal
		/// </summary>
		protected override void OnKeyPress(KeyPressEventArgs e)
		{ // Sustituye el punto por una coma
				if (e.KeyChar == '.' || e.KeyChar == ',')
					{ if (Text.IndexOf(',') >= 0)
							e.Handled = true;
						else
							e.KeyChar = ',';
					}
			// Realiza el evento base
				base.OnKeyPress(e);
		}
	}
}
