using System;

using Bau.Libraries.LibHelper.Extensors;
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
		public ProjectDocumentationModel LoadProject(string strFileName)
		{ return new Repository.Projects.ProjectRepository().Load(strFileName);
		}

		/// <summary>
		///		Graba los datos de un proyecto
		/// </summary>
		public void SaveProject(ProjectDocumentationModel objProject, string strFileName)
		{ new Repository.Projects.ProjectRepository().Save(strFileName, objProject);
		}

		/// <summary>
		///		Genera la documentación a partir de los parámetros de un archivo XML
		/// </summary>
		public void Generate(ProjectDocumentationModel objProject, IDocumentWriter objWriter = null)
		{ // Obtiene el generador
				if (objWriter == null)
					objWriter = GetDocumentWriter(objProject);
			// Guarda el generador
				Writer = objWriter;
			// Borra el directorio destino
				foreach (ProviderModel objProvider in objProject.Providers)
					LibHelper.Files.HelperFiles.KillPath(objProject.OutputPath);
			// Genera la documentación de los distintos proveedores
				CompileProject(objProject);
		}

		/// <summary>
		///		Genera la documentación de un proyecto
		/// </summary>
		private void CompileProject(ProjectDocumentationModel objProject)
		{ StructDocumentationModelCollection objColStructs = new StructDocumentationModelCollection();

				// Genera las estructuras de documentación de los diferentes proveedores
					foreach (ProviderModel objProvider in objProject.Providers)
						try
							{ switch (objProvider.Type.TrimIgnoreNull().ToUpper())
									{	case "C#":
												objColStructs.Add(new LibRoslynManager.ProgramParser().Parse(objProvider.Parameters));
											break;
										case "SQLSERVER":
												objColStructs.Add(new LibSqlServer.Documenter.SchemaDocumenter().Parse(objProvider.Parameters));
											break;
										case "XMLSTRUCTS":
												objColStructs.Add(new LibXMLStructs.Documenter.XMLStructsDocumenter().Parse(objProvider.Parameters));
											break;
										default:
												Errors.Add($"No se reconoce el proveedor {objProvider.Type}");
											break;
									}
							}
						catch (Exception objException)
							{ Errors.Add($"Error en la generación de {objProvider.Type}. {objException.Message}");
							}
				// Genera la documentación
					Process(objColStructs, objProject);
		}

		/// <summary>
		///		Genera la documentación a partir de estructuras de documentación
		/// </summary>
		private void Process(StructDocumentationModelCollection objColStructDocuments, ProjectDocumentationModel objProject)
		{ // Copia el contenido del directorio de plantillas
				CopyTemplates(objProject);
			// Genera la documentación
				new Processor.DocumentationGenerator(this, objProject).Process(objColStructDocuments);
		}

		/// <summary>
		///		Copia los archivos del directorio donde se encuentran las plantillas
		/// </summary>
		private void CopyTemplates(ProjectDocumentationModel objProject)
		{ string strPathTemplate = objProject.TemplatePath;

				if (!strPathTemplate.IsEmpty() && System.IO.Directory.Exists(strPathTemplate))
					{ string [] arrStrPath = System.IO.Directory.GetDirectories(strPathTemplate);
						string [] arrStrFiles = System.IO.Directory.GetFiles(strPathTemplate);

							// Copia los directorios
								foreach (string strPath in arrStrPath)
									LibHelper.Files.HelperFiles.CopyPath(strPath, System.IO.Path.Combine(objProject.OutputPath, System.IO.Path.GetFileName(strPath)));
							// Copia los archivos (excepto las plantillas)
								foreach (string strFile in arrStrFiles)
									if (!strFile.EndsWith(".tpt", StringComparison.CurrentCultureIgnoreCase) && !strFile.StartsWith("_"))
										LibHelper.Files.HelperFiles.CopyFile(strFile, System.IO.Path.Combine(objProject.OutputPath, System.IO.Path.GetFileName(strFile)));
					}
		}

		/// <summary>
		///		Obtiene el interface para la generación de la documentación final
		/// </summary>
		private IDocumentWriter GetDocumentWriter(ProjectDocumentationModel objProject)
		{ switch (objProject.IDType)
				{	case ProjectDocumentationModel.DocumentationType.Nhtml:
						return new Processor.Writers.NHaml.NHamlWriter();
					case ProjectDocumentationModel.DocumentationType.Html:
						return new Processor.Writers.Html.HtmlWriter();
					case ProjectDocumentationModel.DocumentationType.Xml:
						return new Processor.Writers.Xml.XmlWriter();
					default:
						throw new NotImplementedException("No se reconoce el tipo de documentación: " + objProject.IDType.ToString());
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
