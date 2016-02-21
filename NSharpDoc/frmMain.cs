using System;
using System.Windows.Forms;

using Bau.Libraries.LibHelper.Extensors;
using Bau.Libraries.LibNSharpDoc.Processor;
using Bau.Libraries.LibNSharpDoc.Processor.Models.Projects;

namespace Bau.Applications.NSharpDoc
{
	/// <summary>
	///		Formulario principal
	/// </summary>
	public partial class frmMain : Form
	{ // Constantes privadas
			private const string cnstStrMaskFiles = "Archivos de proyecto (*.nsharpdoc)|*.nsharpdoc|Todos los archivos (*.*)|*.*";
		// Variables privadas
			private ProjectDocumentationModel objProject = new ProjectDocumentationModel();

		public frmMain()
		{	InitializeComponent();
		}

		/// <summary>
		///		Inicializa los controles del formulario
		/// </summary>
		private void InitForm()
		{ // Inicializa el combo de tipos
				cboDocumentType.Items.Clear();
				cboDocumentType.AddItem((int) ProjectDocumentationModel.DocumentationType.Html, "Html");
				cboDocumentType.AddItem((int) ProjectDocumentationModel.DocumentationType.Xml, "Xml");
				cboDocumentType.AddItem((int) ProjectDocumentationModel.DocumentationType.Nhtml, "Nhtml");
				cboDocumentType.SelectedID = (int) ProjectDocumentationModel.DocumentationType.Html;
			// Limpia los datos
				Clear(true);
		}

		/// <summary>
		///		Limpia los datos
		/// </summary>
		private void Clear(bool blnLoadLast)
		{ // Crea un nuevo objeto de proyecto
				objProject = new ProjectDocumentationModel();
			// Inicializa la lista de proveedores
				InitListProviders();
			// Carga el objeto de proyecto si es necesario
				if (blnLoadLast && !Configuration.LastProject.IsEmpty() && System.IO.File.Exists(Configuration.LastProject))
					LoadProject(Configuration.LastProject);
		}
		
		/// <summary>
		///		Abre un archivo de proyecto
		/// </summary>
		private void OpenFileProject()
		{ string strFileName = Bau.Controls.Forms.Helper.GetFileNameOpen(cnstStrMaskFiles);

				if (!strFileName.IsEmpty())
					LoadProject(strFileName);
		}

		/// <summary>
		///		Carga los datos de un proyecto
		/// </summary>
		private void LoadProject(string strFileName)
		{ // Carga los datos del archivo
				objProject = new NSharpDocManager().LoadProject(strFileName);
			// Muestra los datos del proyecto
				cboDocumentType.SelectedID = (int) objProject.IDType;
				pthTarget.PathName = objProject.OutputPath;
				pthTemplates.PathName = objProject.TemplatePath;
				if (pthTemplates.PathName.IsEmpty())
					pthTemplates.PathName = System.IO.Path.Combine(Application.StartupPath, "Data\\Templates");
			// Carga los parámetros de generación
				chkShowPublic.Checked = objProject.GenerationParameters.ShowPublic;
				chkShowInternal.Checked = objProject.GenerationParameters.ShowInternal;
				chkShowProtected.Checked = objProject.GenerationParameters.ShowProtected;
				chkShowPrivate.Checked = objProject.GenerationParameters.ShowPrivate;
			// Carga los provedores
				LoadListProviders();
			// Cambia la configuración
				SaveConfiguration();
		}

		/// <summary>
		///		Inicializa la lista de proveedores
		/// </summary>
		private void InitListProviders()
		{ // Limpia la lista
				lswProviders.Clear();
			// Añade las columnas
				lswProviders.AddColumn(200, "Tipo");
				lswProviders.AddColumn(300, "Directorio adicional");
				lswProviders.AddColumn(200, "Parámetros");
		}

		/// <summary>
		///		Carga la lista de instrucciones
		/// </summary>
		private void LoadListProviders()
		{	// Inicializa la lista
				InitListProviders();
			// Carga la lista
				if (objProject != null)
					foreach (ProviderModel objProvider in objProject.Providers)
						{ ListViewItem lsiItem = lswProviders.AddRecord(objProvider.ID, objProvider.Type);

								// Añade los elementos
									lsiItem.SubItems.Add(objProvider.AdditionalDocumentsPath);
									lsiItem.SubItems.Add(objProvider.Parameters.GetFullParameters());
						}
		}

		/// <summary>
		///		Abre el formulario de modificación de proveedores
		/// </summary>
		private void OpenFormUpdateProvider(string strKey)
		{ frmProvider frmNewInstruction = new frmProvider();

				// Asigna las propiedades
					frmNewInstruction.Provider = objProject.Providers[strKey];
				// Muestra el formulario
					frmNewInstruction.ShowDialog();
				// Guarda los datos
					if (frmNewInstruction.DialogResult == DialogResult.OK)
						{ // Asigna la instrucción
								if (strKey.IsEmpty())
									objProject.Providers.Add(frmNewInstruction.Provider);
								else
									objProject.Providers[strKey] = frmNewInstruction.Provider;
							// Actualiza la lista
								LoadListProviders();
						}
		}

		/// <summary>
		///		Borra una instrucción
		/// </summary>
		private void DeleteInstruction(string strKey)
		{ if (!strKey.IsEmpty() && Bau.Controls.Forms.Helper.ShowQuestion(this, "¿Realmente desea eliminar esta instrucción?"))
				{ // Borra la instrucción
						objProject.Providers.RemoveByID(strKey);
					// Carga la lista
						LoadListProviders();
				}
		}

		/// <summary>
		///		Comprueba los datos introducidos de la solución
		/// </summary>
		private bool ValidateData()
		{ bool blnValidate = false;

				// Comprueba los datos
					if (objProject == null)
						Bau.Controls.Forms.Helper.ShowMessage(this, "No hay ningún proyecto");
					else if (pthTarget.PathName.IsEmpty())
						Bau.Controls.Forms.Helper.ShowMessage(this, "Seleccione el directorio de salida de la documentación");
					else if (pthTemplates.PathName.IsEmpty() || !System.IO.Directory.Exists(pthTemplates.PathName))
						Bau.Controls.Forms.Helper.ShowMessage(this, "Seleccione el directorio donde se encuentran las plantillas");
					else if (objProject.Providers.Count == 0)
						Bau.Controls.Forms.Helper.ShowMessage(this, "El proyecto no tiene parámetros de generación");
					else
						blnValidate = true;
				// Devuelve el valor que indica si los datos son correctos
					return blnValidate;
		}

		/// <summary>
		///		Graba un archivo de proyecto
		/// </summary>
		private void Save()
		{ string strFileName = Bau.Controls.Forms.Helper.GetFileNameSave(objProject.FileName, cnstStrMaskFiles);

				if (!strFileName.IsEmpty())
					{ // Crea un nuevo proyecto
							objProject.FileName = strFileName;
						// Asigna los datos de contexto
							AssignParameters(objProject);
						// Graba el archivo 
							new NSharpDocManager().SaveProject(objProject, strFileName);
						// Graba la configuración
							SaveConfiguration();
					}
		}

		/// <summary>
		///		Asigna los parámetros del formulario al proyecto
		/// </summary>
		private void AssignParameters(ProjectDocumentationModel objProject)
		{	// Recupera el tipo y los directorios
				objProject.IDType = (ProjectDocumentationModel.DocumentationType) (cboDocumentType.SelectedID ?? 0);
				objProject.OutputPath = pthTarget.PathName;
				objProject.TemplatePath = pthTemplates.PathName;
			// Recupera los parámetros de generación
				objProject.GenerationParameters.ShowInternal = chkShowInternal.Checked;
				objProject.GenerationParameters.ShowPrivate = chkShowPrivate.Checked;
				objProject.GenerationParameters.ShowProtected = chkShowProtected.Checked;
				objProject.GenerationParameters.ShowPublic = chkShowPublic.Checked;
		}

		/// <summary>
		///		Gemera la documentación
		/// </summary>
		private void GenerateDocuments()
		{ if (ValidateData())
				{ NSharpDocManager objManager = new NSharpDocManager();

						// Asigna los parámetros del formulario al proyecto
							AssignParameters(objProject);
						// Cabecera de log
							AddLog(0, "Comienzo de la generación de documentación de " + objProject.FileName);
						// Genera la documentación
							objManager.Generate(objProject);
						// Muestra la documentación
							if (objManager.Errors.Count == 0)
								AddLog(0, "Documentación generada sin errores");
							else
								{ AddLog(0, "Errores en la generación de la documentación");
									foreach (string strError in objManager.Errors)
										AddLog(1, strError);
								}
						// Cierre de log
							AddLogSeparator();
				}
		}

		/// <summary>
		///		Añade un separador al log
		/// </summary>
		private void AddLogSeparator()
		{ AddLog(0, "");
			AddLog(0, new string('-', 80));
			AddLog(0, "");
		}

		/// <summary>
		///		Añade una línea al log
		/// </summary>
		private void AddLog(int intIndex, string strMessage)
		{ txtLog.AppendText(new string('\t', intIndex) + strMessage + Environment.NewLine);
		}

		/// <summary>
		///		Obtiene los parámetros de generación del documento
		/// </summary>
		private DocumentationParameters GetParametersDocuments()
		{ DocumentationParameters objParameters = new DocumentationParameters();

				// Asigna los valores del formulario
					objProject.IDType = (ProjectDocumentationModel.DocumentationType) (cboDocumentType.SelectedID ?? 0);
				// Graba la configuración de la documentación
					SaveConfiguration();
				// Devuelve los parámetros
					return objParameters;
		}

		/// <summary>
		///		Graba la configuración
		/// </summary>
		private void SaveConfiguration()
		{ if (objProject != null && !objProject.FileName.IsEmpty())
				Configuration.Save(objProject.FileName);
		}

		private void cmdGenerateDocuments_Click(Object sender, EventArgs e)
		{ GenerateDocuments();
		}

		private void frmMain_Load(Object sender, EventArgs e)
		{ InitForm();
		}

		private void cmdOpen_Click(object sender, EventArgs e)
		{ OpenFileProject();
		}

		private void cmdSave_Click(Object sender, EventArgs e)
		{ Save();
		}

		private void cmdClear_Click(object sender, EventArgs e)
		{ if (tabControl1.TabIndex == 0)
				Clear(false);
			else
				txtLog.Text = "";
		}

		private void lswProviders_OnUpdateRecord(object objSender, string strKey)
		{ OpenFormUpdateProvider(strKey);
		}

		private void lswProviders_OnDeleteRecord(object objSender, string strKey)
		{ DeleteInstruction(strKey);
		}

		private void cmdSend_Click(Object sender, EventArgs e)
		{ GenerateDocuments();
		}
	}
}
