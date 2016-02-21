using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Bau.Controls.ColorSelection
{
	/// <summary>
	/// 	Cuadro de texto para seleccionar un archivo
	/// </summary>
	public partial class ColorSelect : UserControl
	{ // Eventos
			public event EventHandler Changed;

		public ColorSelect()
		{	InitializeComponent();
			Color = Color.White;
		}

		/// <summary>
		///		Selecciona un color
		/// </summary>		
		private void SelectColor()
		{	if (dlgColor.ShowDialog() == DialogResult.OK)
				Color = dlgColor.Color;
		}

		/// <summary>
		///		Lanza el evento
		/// </summary>
		private void RaiseEvent()
		{ if (Changed != null)
				Changed(this, EventArgs.Empty);
		}

		[Description("Color"), Browsable(true)]
		public Color Color
		{ get { return lblColor.BackColor; }
			set { lblColor.BackColor = value; }
		}
		
		private void cmdSearchColor_Click(object sender, System.EventArgs e)
		{ SelectColor();			
		}
	}
}
