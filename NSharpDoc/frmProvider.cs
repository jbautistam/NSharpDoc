using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

using Bau.Libraries.LibHelper.Extensors;
using Bau.Libraries.LibNSharpDoc.Processor.Models.Projects;

namespace Bau.Applications.NSharpDoc
{
	/// <summary>
	///		Formulario para el mantenimiento de datos de un proveedor de documentación
	/// </summary>
	public partial class frmProvider : Form
	{
		public frmProvider()
		{	InitializeComponent();
		}

		/// <summary>
		///		Inicializa el formulario
		/// </summary>
		private void Initform()
		{ // Inicializa los datos
				if (Provider == null)
					Provider = new ProviderModel();
			// Carga los datos
				LoadData(Provider);
		}

		/// <summary>
		///		Carga los datos de un proveedor
		/// </summary>
		private void LoadData(ProviderModel objProvider)
		{ // Carga los datos específicos del proveedor
				switch (objProvider.Type.TrimIgnoreNull().ToUpper())
					{	case "C#":
								optSourceCode.Checked = true;
								fnSolution.FileName = Provider.Parameters.GetValue("FileName");
							break;
						case "SQLSERVER":
								optDataBase.Checked = true;
								txtServer.Text = Provider.Parameters.GetValue("Server");
								txtDataBase.Text = Provider.Parameters.GetValue("DataBase");
								txtUser.Text = Provider.Parameters.GetValue("User");
								txtPassword.Text = Provider.Parameters.GetValue("Password");
								txtRepeatPassword.Text = txtPassword.Text;
							break;
						case "XMLSTRUCTS":
								optXMLStructs.Checked = true;
								fnXMLStructs.FileName = Provider.Parameters.GetValue("FileName");
							break;
					}
			// Carga los directorios
				pthAdditional.PathName = Provider.AdditionalDocumentsPath;
		}

		/// <summary>
		///		Habilita / inhabilita los controles
		/// </summary>
		private void EnableControls()
		{ // Habilita los controles de C#
				fnSolution.Enabled = optSourceCode.Checked;
			// Habilita los controles de base de datos
				txtServer.Enabled = optDataBase.Checked;
				txtDataBase.Enabled = optDataBase.Checked;
				txtUser.Enabled = optDataBase.Checked;
				txtPassword.Enabled = optDataBase.Checked;
				txtRepeatPassword.Enabled = optDataBase.Checked;
			// Habilita los controles de estructuras XML
				fnXMLStructs.Enabled = optXMLStructs.Checked;
		}

		/// <summary>
		///		Comprueba los datos introducidos
		/// </summary>
		private bool ValidateData()
		{ bool blnValidate = false;

				// Comprueba los datos introducidos
					if (optSourceCode.Checked)
						{ if (fnSolution.FileName.IsEmpty() || !System.IO.File.Exists(fnSolution.FileName))
								Bau.Controls.Forms.Helper.ShowMessage(this, "Seleccione un nombre de archivo");
							else
								blnValidate = true;
						}
					else if (optDataBase.Checked)
						{ if (txtServer.Text.IsEmpty()) 
								Bau.Controls.Forms.Helper.ShowMessage(this, "Introduzca el nombre de servidor");
							else if (txtDataBase.Text.IsEmpty()) 
								Bau.Controls.Forms.Helper.ShowMessage(this, "Introduzca el nombre de base de datos");
							else if (txtUser.Text.IsEmpty()) 
								Bau.Controls.Forms.Helper.ShowMessage(this, "Introduzca el nombre de usuario");
							else if (txtPassword.Text.IsEmpty()) 
								Bau.Controls.Forms.Helper.ShowMessage(this, "Introduzca la contraseña");
							else if (txtPassword.Text != txtRepeatPassword.Text)	
								Bau.Controls.Forms.Helper.ShowMessage(this, "Ambas contraseñas deben ser iguales");
							else
								blnValidate = true;
						}
					else if (optXMLStructs.Checked)
						{ if (fnXMLStructs.FileName.IsEmpty() || !System.IO.File.Exists(fnXMLStructs.FileName))
								Bau.Controls.Forms.Helper.ShowMessage(this, "Seleccione el nombre del archivo de estructuras de documentación");
							else
								blnValidate = true;
						}
					else
						Bau.Controls.Forms.Helper.ShowMessage(this, "Seleccione el tipo de documentación");
				// Devuelve el valor que indica si los datos son correctos
					return blnValidate;
		}

		/// <summary>
		///		Graba los datos
		/// </summary>
		private void Save()
		{ if (ValidateData())
				{ // Recupera los datos específicos del proveedor
						if (optSourceCode.Checked)
							{ Provider.Type = "C#";
								Provider.Parameters.Parameters.Clear();
								Provider.Parameters.Add("FileName", fnSolution.FileName);
							}
						else if (optDataBase.Checked)
							{ Provider.Type = "SqlServer";
								Provider.Parameters.Parameters.Clear();
								Provider.Parameters.Add("Server", txtServer.Text);
								Provider.Parameters.Add("DataBase", txtDataBase.Text);
								Provider.Parameters.Add("User", txtUser.Text);
								Provider.Parameters.Add("Password", txtPassword.Text);
							}
						else if (optXMLStructs.Checked)
							{ Provider.Type = "XmlStructs";
								Provider.Parameters.Parameters.Clear();
								Provider.Parameters.Add("FileName", fnXMLStructs.FileName);
							}
					// Recupera los datos adicionales del proveedor
						Provider.AdditionalDocumentsPath = pthAdditional.PathName;
					// Indica que se ha grabado correctamente y cierra
						DialogResult = DialogResult.OK;
						Close();
				}
		}

		/// <summary>
		///		Proveedor de datos
		/// </summary>
		public ProviderModel Provider { get; internal set; }

		private void frmProvider_Load(Object sender, EventArgs e)
		{ Initform();
		}

		private void cmdAccept_Click(Object sender, EventArgs e)
		{ Save();
		}

		private void optSourceCode_CheckedChanged(Object sender, EventArgs e)
		{ EnableControls();
		}
	}
}
