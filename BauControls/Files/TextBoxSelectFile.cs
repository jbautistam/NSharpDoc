using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Bau.Controls.Files
{
	/// <summary>
	/// 	Cuadro de texto para seleccionar un archivo
	/// </summary>
	public partial class TextBoxSelectFile : UserControl
	{ // Delegados
			public delegate void ChangedHandler(object objSender, EventArgs evnArgs);
		// Eventos
			public event ChangedHandler Changed;
		// Enumerados
			public enum FileSelectType
				{ Save,
					Load
				}
		// Variables privadas
			private FileSelectType intType = FileSelectType.Load;
			private string strFilter = "Todos los archivos|*.*";
			private Color clrBackColor;

		public TextBoxSelectFile()
		{	// Inicializa el componente
				InitializeComponent();
			// Guarda el color del fondo
				clrBackColor = txtFileName.BackColor;
		}

		/// <summary>
		///		Selecciona un archivo del disco
		/// </summary>		
		private void SelectFile()
		{	switch (intType)
				{ case FileSelectType.Save:
							// Inicializa las propiedades del cuadro de diálogo
								if (!string.IsNullOrEmpty(FileName))
									{ dlgFileSave.InitialDirectory = System.IO.Path.GetDirectoryName(FileName);
										dlgFileSave.FileName = FileName;
									}
								else
									dlgFileSave.FileName = "";
								dlgFileSave.CheckFileExists = false;
								dlgFileSave.CheckPathExists = true;
								dlgFileSave.Filter = strFilter;
							// Muestra el cuadro de diálogo
								if (dlgFileSave.ShowDialog() == DialogResult.OK)
									{ // Cambia el nombre del archivo
											FileName = dlgFileSave.FileName;
										// Lanza el evento
											RaiseEvent();
									}
						break;
					case FileSelectType.Load:
							// Inicializa las propiedades del cuadro de diálogo
								if (!string.IsNullOrEmpty(FileName))
									{ dlgFileLoad.InitialDirectory = System.IO.Path.GetDirectoryName(FileName);
										dlgFileLoad.FileName = FileName;
									}
								else
									dlgFileLoad.FileName = "";
								dlgFileLoad.CheckFileExists = true;
								dlgFileLoad.CheckPathExists = true;
								dlgFileLoad.Multiselect = false;
								dlgFileLoad.ShowReadOnly = false;
								dlgFileLoad.Filter = strFilter;
							// Muestra el cuadro de diálogo
								if (dlgFileLoad.ShowDialog() == DialogResult.OK)
									{ // Cambia el nombre de archivo
											FileName = dlgFileLoad.FileName;
										// Lanza el evento
											RaiseEvent();
									}
						break;
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
		public string FileName
		{	get { return txtFileName.Text.Trim(); }
			set 
				{ if (!string.IsNullOrEmpty(value))
						txtFileName.Text = value.Trim(); 
					else
						txtFileName.Text = "";
				}
		}

		[Description("File filter"),Category("File"),Browsable(true)]
		public string Filter
		{ get { return strFilter; }
			set { strFilter = value; }
		}

		[Description("Selection type"),Category("File"),Browsable(true)]
		public FileSelectType Type
		{ get { return intType; }
			set { intType = value; }
		}

		[Description("Can write new file name from keyboard"),Category("File"),Browsable(true),DefaultValue(true)]
		public bool CanUseKeyboard
		{ get { return !txtFileName.ReadOnly; }
			set { txtFileName.ReadOnly = !value; }
		}
		
		[Browsable(true)]
		public Color BackColorEdit
		{	get {	return clrBackColor; }
			set
				{ // Guarda el color de fondo
						clrBackColor = value;
					// Cambia el color del fondo de los controles
						txtFileName.BackColor = value;
						if (Parent != null)
							base.BackColor = Parent.BackColor;
				}
		}
			
		private void cmdSearchFile_Click(object sender, System.EventArgs e)
		{ SelectFile();			
		}
	}
}
