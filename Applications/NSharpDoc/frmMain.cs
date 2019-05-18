using System;
using System.Windows.Forms;

using Bau.Libraries.LibCommonHelper.Extensors;
using Bau.Libraries.LibNSharpDoc.Processor;
using Bau.Libraries.LibNSharpDoc.Processor.Models.Projects;

namespace Bau.Applications.NSharpDoc
{
	/// <summary>
	///		Formulario principal
	/// </summary>
	public partial class frmMain : Form
	{ 
		// Constantes privadas
		private const string MaskFiles = "Archivos de proyecto (*.nsharpdoc)|*.nsharpdoc|Todos los archivos (*.*)|*.*";
		// Variables privadas
		private ProjectDocumentationModel project = new ProjectDocumentationModel();

		public frmMain()
		{
			InitializeComponent();
		}

		/// <summary>
		///		Inicializa los controles del formulario
		/// </summary>
		private void InitForm()
		{ 
			// Inicializa el combo de tipos
			cboDocumentType.Items.Clear();
			cboDocumentType.AddItem((int) ProjectDocumentationModel.DocumentationType.Html, "Html");
			cboDocumentType.AddItem((int) ProjectDocumentationModel.DocumentationType.Xml, "Xml");
			cboDocumentType.AddItem((int) ProjectDocumentationModel.DocumentationType.Nhtml, "Nhtml");
			// Inicializa los valores de pantalla con la configuración
			cboDocumentType.SelectedID = Configuration.OutputType;
			pthTemplates.PathName = Configuration.TemplatesPath;
			pthTarget.PathName = Configuration.OutputPath;
			// Limpia los datos
			Clear(true);
		}

		/// <summary>
		///		Limpia los datos
		/// </summary>
		private void Clear(bool loadLast)
		{ 
			// Crea un nuevo objeto de proyecto
			project = new ProjectDocumentationModel();
			// Inicializa la lista de proveedores
			InitListProviders();
			// Carga el objeto de proyecto si es necesario
			if (loadLast && !Configuration.LastProject.IsEmpty() && System.IO.File.Exists(Configuration.LastProject))
				LoadProject(Configuration.LastProject);
		}

		/// <summary>
		///		Abre un archivo de proyecto
		/// </summary>
		private void OpenFileProject()
		{
			string fileName = Bau.Controls.Forms.Helper.GetFileNameOpen(MaskFiles);

				if (!fileName.IsEmpty())
					LoadProject(fileName);
		}

		/// <summary>
		///		Carga los datos de un proyecto
		/// </summary>
		private void LoadProject(string fileName)
		{
			try
			{ 
				// Carga los datos del archivo
				project = new NSharpDocManager().LoadProject(fileName);
				// Muestra los datos del proyecto
				cboDocumentType.SelectedID = (int)project.IDType;
				pthTarget.PathName = project.OutputPath;
				pthTemplates.PathName = project.TemplatePath;
				if (pthTemplates.PathName.IsEmpty())
					pthTemplates.PathName = System.IO.Path.Combine(Application.StartupPath, "Data\\Templates");
				// Carga los parámetros de generación
				chkShowPublic.Checked = project.GenerationParameters.ShowPublic;
				chkShowInternal.Checked = project.GenerationParameters.ShowInternal;
				chkShowProtected.Checked = project.GenerationParameters.ShowProtected;
				chkShowPrivate.Checked = project.GenerationParameters.ShowPrivate;
				// Carga los provedores
				LoadListProviders();
				// Cambia la configuración
				SaveConfiguration();
			}
			catch (Exception exception)
			{
				Bau.Controls.Forms.Helper.ShowMessage(this, $"Error al cargar {fileName}. {Environment.NewLine} {exception.Message}");
			}
		}

		/// <summary>
		///		Inicializa la lista de proveedores
		/// </summary>
		private void InitListProviders()
		{ 
			// Limpia la lista
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
		{   
			// Inicializa la lista
			InitListProviders();
			// Carga la lista
			if (project != null)
				foreach (ProviderModel provider in project.Providers)
				{
					ListViewItem lsiItem = lswProviders.AddRecord(provider.ID, provider.Type);

						// Añade los elementos
						lsiItem.SubItems.Add(provider.Parameters.GetFullParameters());
				}
		}

		/// <summary>
		///		Abre el formulario de modificación de proveedores
		/// </summary>
		private void OpenFormUpdateProvider(string key)
		{
			frmProvider frmNewInstruction = new frmProvider();

				// Asigna las propiedades
				frmNewInstruction.Provider = project.Providers[key];
				// Muestra el formulario
				frmNewInstruction.ShowDialog();
				// Guarda los datos
				if (frmNewInstruction.DialogResult == DialogResult.OK)
				{ // Asigna la instrucción
					if (key.IsEmpty())
						project.Providers.Add(frmNewInstruction.Provider);
					else
						project.Providers[key] = frmNewInstruction.Provider;
					// Actualiza la lista
					LoadListProviders();
				}
		}

		/// <summary>
		///		Borra una instrucción
		/// </summary>
		private void DeleteInstruction(string key)
		{
			if (!key.IsEmpty() && Bau.Controls.Forms.Helper.ShowQuestion(this, "¿Realmente desea eliminar esta instrucción?"))
			{ 
				// Borra la instrucción
				project.Providers.RemoveByID(key);
				// Carga la lista
				LoadListProviders();
			}
		}

		/// <summary>
		///		Comprueba los datos introducidos de la solución
		/// </summary>
		private bool ValidateData()
		{
			bool validate = false;

				// Comprueba los datos
				if (project == null)
					Bau.Controls.Forms.Helper.ShowMessage(this, "No hay ningún proyecto");
				else if (pthTarget.PathName.IsEmpty())
					Bau.Controls.Forms.Helper.ShowMessage(this, "Seleccione el directorio de salida de la documentación");
				else if (pthTemplates.PathName.IsEmpty() || !System.IO.Directory.Exists(pthTemplates.PathName))
					Bau.Controls.Forms.Helper.ShowMessage(this, "Seleccione el directorio donde se encuentran las plantillas");
				else if (project.Providers.Count == 0)
					Bau.Controls.Forms.Helper.ShowMessage(this, "El proyecto no tiene parámetros de generación");
				else
					validate = true;
				// Devuelve el valor que indica si los datos son correctos
				return validate;
		}

		/// <summary>
		///		Graba un archivo de proyecto
		/// </summary>
		private void Save()
		{
			string fileName = Bau.Controls.Forms.Helper.GetFileNameSave(project.FileName, MaskFiles);

				if (!fileName.IsEmpty())
				{	
					// Crea un nuevo proyecto
					project.FileName = fileName;
					// Asigna los datos de contexto
					AssignParameters(project);
					// Graba el archivo 
					new NSharpDocManager().SaveProject(project, fileName);
					// Graba la configuración
					SaveConfiguration();
				}
		}

		/// <summary>
		///		Asigna los parámetros del formulario al proyecto
		/// </summary>
		private void AssignParameters(ProjectDocumentationModel project)
		{   
			// Recupera el tipo y los directorios
			project.IDType = (ProjectDocumentationModel.DocumentationType) (cboDocumentType.SelectedID ?? 0);
			project.OutputPath = pthTarget.PathName;
			project.TemplatePath = pthTemplates.PathName;
			// Recupera los parámetros de generación
			project.GenerationParameters.ShowInternal = chkShowInternal.Checked;
			project.GenerationParameters.ShowPrivate = chkShowPrivate.Checked;
			project.GenerationParameters.ShowProtected = chkShowProtected.Checked;
			project.GenerationParameters.ShowPublic = chkShowPublic.Checked;
		}

		/// <summary>
		///		Gemera la documentación
		/// </summary>
		private void GenerateDocuments()
		{
			if (ValidateData())
			{  
				// Asigna los parámetros del formulario al proyecto
				AssignParameters(project);
				// Graba el proyecto si aún no estaba grabado
				if (project.FileName.IsEmpty())
					Save();
				// Si no está grabado no se genera
				if (project.FileName.IsEmpty() || !System.IO.File.Exists(project.FileName))
					Bau.Controls.Forms.Helper.ShowMessage(this, "Graba el proyecto antes de generar la documentación");
				else
				{ 
					// Cabecera de log
					AddLog(0, "Comienzo de la generación de documentación de " + project.FileName);
					// Genera la documentación
					GenerateWithLibrary();
					// Cierre de log
					AddLogSeparator();
				}
			}
		}

		/// <summary>
		///		Genera la documentación utilizando la librería
		/// </summary>
		private void GenerateWithLibrary()
		{
			NSharpDocManager manager = new NSharpDocManager();

				// Genera la documentación
				manager.Generate(project);
				// Muestra la documentación
				if (manager.Errors.Count == 0)
					AddLog(0, "Documentación generada sin errores");
				else
				{
					AddLog(0, "Errores en la generación de la documentación");
					foreach (string strError in manager.Errors)
						AddLog(1, strError);
				}
		}

		/// <summary>
		///		Añade un separador al log
		/// </summary>
		private void AddLogSeparator()
		{
			AddLog(0, "");
			AddLog(0, new string('-', 80));
			AddLog(0, "");
		}

		/// <summary>
		///		Añade una línea al log
		/// </summary>
		private void AddLog(int index, string message)
		{
			txtLog.AppendText(new string('\t', index) + message + Environment.NewLine);
		}

		/// <summary>
		///		Obtiene los parámetros de generación del documento
		/// </summary>
		private DocumentationParameters GetParametersDocuments()
		{
			DocumentationParameters parameters = new DocumentationParameters();

				// Asigna los valores del formulario
				project.IDType = (ProjectDocumentationModel.DocumentationType)(cboDocumentType.SelectedID ?? 0);
				// Graba la configuración de la documentación
				SaveConfiguration();
				// Devuelve los parámetros
				return parameters;
		}

		/// <summary>
		///		Graba la configuración
		/// </summary>
		private void SaveConfiguration()
		{
			string lastProject = "";

				// Obtiene el nombre del proyecto
				if (project != null && !project.FileName.IsEmpty())
					lastProject = project.FileName;
				// Asigna las propiedades
				Configuration.OutputType = cboDocumentType.SelectedID ?? 0;
				Configuration.TemplatesPath = pthTemplates.PathName;
				Configuration.OutputPath = pthTarget.PathName;
				Configuration.LastProject = lastProject;
				// Graba la configuración
				Configuration.Save();
		}

		private void cmdGenerateDocuments_Click(object sender, EventArgs e)
		{
			GenerateDocuments();
		}

		private void frmMain_Load(object sender, EventArgs e)
		{
			InitForm();
		}

		private void cmdOpen_Click(object sender, EventArgs e)
		{
			OpenFileProject();
		}

		private void cmdNew_Click(object sender, EventArgs e)
		{
			Clear(false);
		}

		private void cmdSave_Click(object sender, EventArgs e)
		{
			Save();
		}

		private void cmdClear_Click(object sender, EventArgs e)
		{
			txtLog.Text = "";
		}

		private void lswProviders_OnUpdateRecord(object sender, string key)
		{
			OpenFormUpdateProvider(key);
		}

		private void lswProviders_OnDeleteRecord(object sender, string key)
		{
			DeleteInstruction(key);
		}

		private void cmdSend_Click(object sender, EventArgs e)
		{
			GenerateDocuments();
		}

		private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
		{
			SaveConfiguration();
		}
	}
}
