using System;
using System.Windows.Forms;

using Bau.Libraries.LibCommonHelper.Extensors;
using Bau.Libraries.LibNSharpDoc.Processor.Models.Projects;

namespace Bau.Applications.NSharpDoc
{
	/// <summary>
	///		Formulario para el mantenimiento de datos de un proveedor de documentación
	/// </summary>
	public partial class frmProvider : Form
	{
		public frmProvider()
		{
			InitializeComponent();
		}

		/// <summary>
		///		Inicializa el formulario
		/// </summary>
		private void Initform()
		{ 
			// Inicializa los datos
			if (Provider == null)
				Provider = new ProviderModel();
			// Carga los datos
			LoadData(Provider);
		}

		/// <summary>
		///		Carga los datos de un proveedor
		/// </summary>
		private void LoadData(ProviderModel provider)
		{ 
			switch (provider.Type.TrimIgnoreNull().ToUpper())
			{
				case "C#":
						optSourceCode.Checked = true;
						fnSolutionCSharp.FileName = Provider.Parameters.GetValue("FileName");
					break;
				case "VISUALBASIC":
						optSourceVisualBasic.Checked = true;
						fnSolutionVisualBasic.FileName = Provider.Parameters.GetValue("FileName");
					break;
				case "SQLSERVER":
						optDataBase.Checked = true;
						txtServer.Text = Provider.Parameters.GetValue("Server");
						txtDataBase.Text = Provider.Parameters.GetValue("DataBase");
						txtUser.Text = Provider.Parameters.GetValue("User");
						txtPassword.Text = Provider.Parameters.GetValue("Password");
						txtRepeatPassword.Text = txtPassword.Text;
						chkUseIntegratedSecurity.Checked = Provider.Parameters.GetValue("UseIntegratedSecurity").GetBool();
					break;
				case "OLEDB":
						optOleDB.Checked = true;
						txtConnectionString.Text = Provider.Parameters.GetValue("ConnectionString");
					break;
				case "XMLSTRUCTS":
						optXMLStructs.Checked = true;
						fnXMLStructs.FileName = Provider.Parameters.GetValue("FileName");
					break;
				case "HELPPAGES":
						optHelpPages.Checked = true;
						pthHelpPages.PathName = Provider.Parameters.GetValue("Path");
					break;
			}
		}

		/// <summary>
		///		Habilita / inhabilita los controles
		/// </summary>
		private void EnableControls()
		{ 
			// Habilita los controles de C#
			fnSolutionCSharp.Enabled = optSourceCode.Checked;
			// Habilita los controles de Visual Basic
			fnSolutionVisualBasic.Enabled = optSourceVisualBasic.Checked;
			// Habilita los controles de SqlServer
			txtServer.Enabled = optDataBase.Checked;
			txtDataBase.Enabled = optDataBase.Checked;
			txtUser.Enabled = optDataBase.Checked;
			txtPassword.Enabled = optDataBase.Checked && !chkUseIntegratedSecurity.Checked;
			txtRepeatPassword.Enabled = optDataBase.Checked && !chkUseIntegratedSecurity.Checked;
			// Habilita los controles de OleDB
			txtConnectionString.Enabled = optOleDB.Checked;
			// Habilita los controles de estructuras XML
			fnXMLStructs.Enabled = optXMLStructs.Checked;
			// Habilita los controles para páginas adicionales
			pthHelpPages.Enabled = optHelpPages.Checked;
		}

		/// <summary>
		///		Comprueba los datos introducidos
		/// </summary>
		private bool ValidateData()
		{
			bool validate = false;

				// Comprueba los datos introducidos
				if (optSourceCode.Checked)
				{
					if (fnSolutionCSharp.FileName.IsEmpty() || !System.IO.File.Exists(fnSolutionCSharp.FileName))
						Bau.Controls.Forms.Helper.ShowMessage(this, "Seleccione un nombre de archivo");
					else
						validate = true;
				}
				else if (optSourceVisualBasic.Checked)
				{
					if (fnSolutionVisualBasic.FileName.IsEmpty() || !System.IO.File.Exists(fnSolutionVisualBasic.FileName))
						Bau.Controls.Forms.Helper.ShowMessage(this, "Seleccione un nombre de archivo");
					else
						validate = true;
				}
				else if (optDataBase.Checked)
				{
					if (txtServer.Text.IsEmpty())
						Bau.Controls.Forms.Helper.ShowMessage(this, "Introduzca el nombre de servidor");
					else if (txtDataBase.Text.IsEmpty())
						Bau.Controls.Forms.Helper.ShowMessage(this, "Introduzca el nombre de base de datos");
					else if (txtUser.Text.IsEmpty())
						Bau.Controls.Forms.Helper.ShowMessage(this, "Introduzca el nombre de usuario");
					else if (txtPassword.Text.IsEmpty() && !chkUseIntegratedSecurity.Checked)
						Bau.Controls.Forms.Helper.ShowMessage(this, "Introduzca la contraseña");
					else if (txtPassword.Text != txtRepeatPassword.Text && !chkUseIntegratedSecurity.Checked)
						Bau.Controls.Forms.Helper.ShowMessage(this, "Ambas contraseñas deben ser iguales");
					else
						validate = true;
				}
				else if (optOleDB.Checked)
				{
					if (txtConnectionString.Text.IsEmpty())
						Bau.Controls.Forms.Helper.ShowMessage(this, "Introduzca la cadena de conexión");
					else
						validate = true;
				}
				else if (optXMLStructs.Checked)
				{
					if (fnXMLStructs.FileName.IsEmpty() || !System.IO.File.Exists(fnXMLStructs.FileName))
						Bau.Controls.Forms.Helper.ShowMessage(this, "Seleccione el nombre del archivo de estructuras de documentación");
					else
						validate = true;
				}
				else if (optHelpPages.Checked)
				{
					if (pthHelpPages.PathName.IsEmpty() || !System.IO.Directory.Exists(pthHelpPages.PathName))
						Bau.Controls.Forms.Helper.ShowMessage(this, "seleccione el directorio donde se encuentran las páginas adicionales");
					else
						validate = true;
				}
				else
					Bau.Controls.Forms.Helper.ShowMessage(this, "Seleccione el tipo de documentación");
				// Devuelve el valor que indica si los datos son correctos
				return validate;
		}

		/// <summary>
		///		Graba los datos
		/// </summary>
		private void Save()
		{
			if (ValidateData())
			{ 
				// Recupera los datos específicos del proveedor
				if (optSourceCode.Checked)
				{
					Provider.Type = "C#";
					Provider.Parameters.Parameters.Clear();
					Provider.Parameters.Add("FileName", fnSolutionCSharp.FileName);
				}
				else if (optSourceVisualBasic.Checked)
				{
					Provider.Type = "VisualBasic";
					Provider.Parameters.Parameters.Clear();
					Provider.Parameters.Add("FileName", fnSolutionVisualBasic.FileName);
				}
				else if (optDataBase.Checked)
				{
					Provider.Type = "SqlServer";
					Provider.Parameters.Parameters.Clear();
					Provider.Parameters.Add("Server", txtServer.Text);
					Provider.Parameters.Add("DataBase", txtDataBase.Text);
					Provider.Parameters.Add("User", txtUser.Text);
					Provider.Parameters.Add("Password", txtPassword.Text);
					Provider.Parameters.Add("UseIntegratedSecurity", chkUseIntegratedSecurity.Checked);
				}
				else if (optOleDB.Checked)
				{
					Provider.Type = "OleDB";
					Provider.Parameters.Parameters.Clear();
					Provider.Parameters.Add("ConnectionString", txtConnectionString.Text);
				}
				else if (optXMLStructs.Checked)
				{
					Provider.Type = "XmlStructs";
					Provider.Parameters.Parameters.Clear();
					Provider.Parameters.Add("FileName", fnXMLStructs.FileName);
				}
				else if (optHelpPages.Checked)
				{
					Provider.Type = "HelpPages";
					Provider.Parameters.Parameters.Clear();
					Provider.Parameters.Add("Path", pthHelpPages.PathName);
				}
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
		{
			Initform();
		}

		private void cmdAccept_Click(Object sender, EventArgs e)
		{
			Save();
		}

		private void optSourceCode_CheckedChanged(Object sender, EventArgs e)
		{
			EnableControls();
		}
	}
}
