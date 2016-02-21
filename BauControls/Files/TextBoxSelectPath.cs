using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Bau.Controls.Files
{
	/// <summary>
	/// 	Cuadro de texto para seleccionar un directorio
	/// </summary>
	public partial class TextBoxSelectPath : UserControl
	{ // Delegados
			public delegate void ChangedHandler(object objSender, EventArgs evnArgs);
		// Eventos
			public event ChangedHandler Changed;

		public TextBoxSelectPath()
		{	InitializeComponent();
		}

		/// <summary>
		///		Selecciona un directorio del disco
		/// </summary>		
		private void SelectPath()
		{	// Inicializa las propiedades del cuadro de diálogo
				if (!string.IsNullOrEmpty(PathName))
					dlgPath.SelectedPath = PathName;
			// Muestra el cuadro de diálogo
				if (dlgPath.ShowDialog() == DialogResult.OK)
					{ // Cambia el nombre del directorio
							PathName = dlgPath.SelectedPath;
						// Lanza el evento
							RaiseEvent();
					}
		}

		/// <summary>
		///		Lanza el evento
		/// </summary>
		private void RaiseEvent()
		{ if (Changed != null)
				Changed(this, new EventArgs());
		}
		
		[Description("File name"),Category("File"),Browsable(true)]
		public string PathName
		{	get { return txtFileName.Text.Trim(); }
			set 
				{ if (!string.IsNullOrEmpty(value))
						txtFileName.Text = value.Trim(); 
					else
						txtFileName.Text = "";
				}
		}

		[Description("Can write new path name from keyboard"),Category("File"),Browsable(true),DefaultValue(true)]
		public bool CanUseKeyboard
		{ get { return !txtFileName.ReadOnly; }
			set { txtFileName.ReadOnly = !value; }
		}
	
		private void cmdSearchFile_Click(object sender, System.EventArgs e)
		{ SelectPath();			
		}
	}
}
