using System;

using Bau.Libraries.LibCommonHelper.Extensors;
using Bau.Libraries.LibNSharpDoc.Models.Structs;
using Bau.Libraries.LibNSharpDoc.Processor.Models.Projects;
using Bau.Libraries.LibNSharpDoc.Processor.Processor.Writers;

namespace Bau.Libraries.LibNSharpDoc.Processor
{
	/// <summary>
	///		Manager para la generación de documentación
	/// </summary>
	public class NSharpDocManager
	{
		/// <summary>
		///		Carga los datos de un proyecto
		/// </summary>
		public ProjectDocumentationModel LoadProject(string fileName)
		{
			return new Repository.Projects.ProjectRepository().Load(fileName);
		}

		/// <summary>
		///		Graba los datos de un proyecto
		/// </summary>
		public void SaveProject(ProjectDocumentationModel project, string fileName)
		{
			new Repository.Projects.ProjectRepository().Save(fileName, project);
		}

		/// <summary>
		///		Genera la documentación de un proyecto
		/// </summary>
		public void Generate(string fileName)
		{
			Generate(LoadProject(fileName));
		}

		/// <summary>
		///		Genera la documentación a partir de los parámetros de un archivo XML
		/// </summary>
		public void Generate(ProjectDocumentationModel project, IDocumentWriter writer = null)
		{ 
			// Obtiene el generador
			if (writer == null)
				writer = GetDocumentWriter(project);
			// Guarda el generador
			Writer = writer;
			// Borra el directorio destino
			LibCommonHelper.Files.HelperFiles.KillPath(project.OutputPath);
			// Genera la documentación de los distintos proveedores
			CompileProject(project);
		}

		/// <summary>
		///		Genera la documentación de un proyecto
		/// </summary>
		private void CompileProject(ProjectDocumentationModel project)
		{
			StructDocumentationModelCollection structsDoc = new StructDocumentationModelCollection();

				// Genera las estructuras de documentación de los diferentes proveedores
				foreach (ProviderModel provider in project.Providers)
					try
					{
						switch (provider.Type.TrimIgnoreNull().ToUpper())
						{
							case "C#":
									structsDoc.Add(new LibNetParser.ProgramParser().Parse(provider.Parameters));
								break;
							case "VISUALBASIC":
									structsDoc.Add(new LibNetParser.ProgramParser().Parse(provider.Parameters));
								break;
							case "SQLSERVER":
									structsDoc.Add(CompileDatabase(LibDataBaseDocumenter.StructDataBaseConversor.DataBaseType.SqlServer,
																   provider.Parameters));
								break;
							case "OLEDB":
									structsDoc.Add(CompileDatabase(LibDataBaseDocumenter.StructDataBaseConversor.DataBaseType.SqlServer,
																   provider.Parameters));
								break;
							case "XMLSTRUCTS":
									structsDoc.Add(new LibXMLStructs.Documenter.XMLStructsDocumenter().Parse(provider.Parameters));
								break;
							case "HELPPAGES":
									structsDoc.Add(new LibHelpPages.Documenter.HelpPages().Parse(provider.Parameters));
								break;
							default:
									Errors.Add($"No se reconoce el proveedor {provider.Type}");
								break;
						}
					}
					catch (Exception exception)
					{
						Errors.Add($"Error en la generación de {provider.Type}. {exception.Message}");
					}
				// Genera la documentación
				Process(structsDoc, project);
		}

		/// <summary>
		///		Compila las estructuras de una base de datos
		/// </summary>
		private StructDocumentationModel CompileDatabase(LibDataBaseDocumenter.StructDataBaseConversor.DataBaseType type, StructParameterModelDictionary structParameters)
		{
			return new LibDataBaseDocumenter.StructDataBaseConversor().Convert(type, structParameters);
		}

		/// <summary>
		///		Genera la documentación a partir de estructuras de documentación
		/// </summary>
		private void Process(StructDocumentationModelCollection structDocs, ProjectDocumentationModel project)
		{ 
			// Copia el contenido del directorio de plantillas
			CopyTemplates(project);
			// Genera la documentación
			new Processor.DocumentationGenerator(this, project).Process(structDocs);
		}

		/// <summary>
		///		Copia los archivos del directorio donde se encuentran las plantillas
		/// </summary>
		private void CopyTemplates(ProjectDocumentationModel project)
		{
			string pathTemplate = project.TemplatePath;

				if (!pathTemplate.IsEmpty() && System.IO.Directory.Exists(pathTemplate))
				{
					// Copia los directorios
					foreach (string path in System.IO.Directory.GetDirectories(pathTemplate))
						LibCommonHelper.Files.HelperFiles.CopyPath(path, System.IO.Path.Combine(project.OutputPath, System.IO.Path.GetFileName(path)));
					// Copia los archivos (excepto las plantillas)
					foreach (string file in System.IO.Directory.GetFiles(pathTemplate))
						if (!file.EndsWith(".tpt", StringComparison.CurrentCultureIgnoreCase) && !file.StartsWith("_"))
							LibCommonHelper.Files.HelperFiles.CopyFile(file, System.IO.Path.Combine(project.OutputPath, System.IO.Path.GetFileName(file)));
				}
		}

		/// <summary>
		///		Obtiene el interface para la generación de la documentación final
		/// </summary>
		private IDocumentWriter GetDocumentWriter(ProjectDocumentationModel project)
		{
			switch (project.IDType)
			{
				case ProjectDocumentationModel.DocumentationType.Nhtml:
					return new Processor.Writers.NHaml.NHamlWriter();
				case ProjectDocumentationModel.DocumentationType.Html:
					return new Processor.Writers.Html.HtmlWriter();
				case ProjectDocumentationModel.DocumentationType.Xml:
					return new Processor.Writers.Xml.XmlWriter();
				default:
					throw new NotImplementedException("No se reconoce el tipo de documentación: " + project.IDType.ToString());
			}
		}

		/// <summary>
		///		Errores de la documentación
		/// </summary>
		public System.Collections.Generic.List<string> Errors { get; } = new System.Collections.Generic.List<string>();

		/// <summary>
		///		Generador de la documentación
		/// </summary>
		internal IDocumentWriter Writer { get; private set; }
	}
}
