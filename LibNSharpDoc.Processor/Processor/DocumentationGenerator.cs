using System;

using Bau.Libraries.LibNSharpDoc.Models.Structs;
using Bau.Libraries.LibNSharpDoc.Processor.Models.Documents;
using Bau.Libraries.LibNSharpDoc.Processor.Models.Projects;
using Bau.Libraries.LibNSharpDoc.Processor.Models.Templates;

namespace Bau.Libraries.LibNSharpDoc.Processor.Processor
{
	/// <summary>
	///		Generador de documentación
	/// </summary>
	internal class DocumentationGenerator
	{ 
		internal DocumentationGenerator(NSharpDocManager objGenerator, ProjectDocumentationModel objProject)
		{ DocumentationProcessor = objGenerator;
			Project = objProject;
		}

		/// <summary>
		///		Procesa la generación de documentos
		/// </summary>
		internal void Process(StructDocumentationModelCollection objColStructDocuments)
		{ DocumentFileModelCollection objColDocuments = new DocumentFileModelCollection();
			
				// Carga las plantillas
					Templates = new Repository.Templates.TemplateRepository().Load(Project.TemplatePath);
				// Crea los documentos
					foreach (StructDocumentationModel objStructDocument in objColStructDocuments)
						if (Templates.MustGenerateFile(objStructDocument, Project.GenerationParameters))
							{	DocumentFileModel objDocument = new DocumentFileModel(null, objStructDocument, 0);

									// Procesa la estructura del lenguaje
										Generate(objStructDocument, objDocument);
									// Añade el documento a la colección
										objColDocuments.Add(objDocument);
							}
				// Graba los documentos
					ProcessDocuments(objColDocuments);
		}

		/// <summary>
		///		Genera la documentación de una estructura
		/// </summary>
		private void Generate(StructDocumentationModel objStruct, DocumentFileModel objParent)
		{ foreach (StructDocumentationModel objItem in objStruct.Childs)
				if (Templates.MustGenerateFile(objItem, Project.GenerationParameters))
					{ DocumentFileModel objDocument = new DocumentFileModel(objParent, objItem, objParent.Childs.SearchOrder(objItem));

							// Añade el documento a los hijos
								objParent.Childs.Add(objDocument);
							// Añade los documentos hijo
								Generate(objItem, objDocument);
					}
		}

		/// <summary>
		///		Genera la documentación
		/// </summary>
		private void ProcessDocuments(DocumentFileModelCollection objColDocuments)
		{ // Ordena los archivos por nombres
				objColDocuments.SortByName();
			// Genera los archivos de contenido
				GenerateFilesContent(objColDocuments, objColDocuments);
			// Genera los archivos de índice
				objColDocuments.AddRange(GenerateFilesIndex(objColDocuments));
			// Transforma los hipervínculos
				objColDocuments.TransformSearchLinks(UrlBaseDocuments);
			// Graba los documentos
				SaveDocuments(objColDocuments);
		}

		/// <summary>
		///		Genera los archivos de contenido
		/// </summary>
		private void GenerateFilesContent(DocumentFileModelCollection objColAllDocuments, DocumentFileModelCollection objColDocuments)
		{	foreach (DocumentFileModel objDocument in objColDocuments)
				{ TemplateModel objTemplate = Templates.Search(objDocument.StructType);
						
						// Genera la documentación del archivo
							if (objTemplate == null)
								AddError(objDocument, "No se encuentra ninguna plantilla para este tipo de estructura");
							else
								try
									{ Generators.TemplateDocumentGenerator objGenerator = new Generators.TemplateDocumentGenerator(Project, this, objTemplate, objDocument, 
																																																								 UrlBaseDocuments, objColAllDocuments);

											objGenerator.Process();
									}
								catch (Exception objException)
									{ AddError(objDocument, $"Error al generar el documento. {objException.Message}");
									}
						// Genera los documentos hijo
							GenerateFilesContent(objColAllDocuments, objDocument.Childs);
				}
		}

		/// <summary>
		///		Genera los archivos de índice
		/// </summary>
		private DocumentFileModelCollection GenerateFilesIndex(DocumentFileModelCollection objColDocuments)
		{ DocumentFileModelCollection objColFilesIndex = new DocumentFileModelCollection();

				// Recorre las plantillas generando los archivos de índice
					foreach (TemplateModel objTemplate in Templates)
						if (objTemplate.IDType == TemplateModel.TemplateType.Index)
							try
								{ DocumentFileModel objFullDocument = objColDocuments.Compact(objTemplate.Name);
									Generators.TemplateDocumentGenerator objGenerator = new Generators.TemplateDocumentGenerator(Project, this, objTemplate,
																																																							 objFullDocument, UrlBaseDocuments,
																																																							 objColDocuments);

										// Procesa el documento
											objGenerator.Process();
										// ... y lo añade a la colección de índices
											objColFilesIndex.Add(objFullDocument);
								}
							catch (Exception objException)
								{ AddError($"Error al generar el documento de índice de la plantilla {objTemplate.RelativeFileName}. {objException.Message}");
								}
				// Devuelve la colección de índices
					return objColFilesIndex;
		}

		/// <summary>
		///		Graba los documentos
		/// </summary>
		private void SaveDocuments(DocumentFileModelCollection objColDocuments)
		{ foreach (DocumentFileModel objDocument in objColDocuments)
				{ // Graba el documento
						DocumentationProcessor.Writer.Save(objDocument, objDocument.MLBuilder, 
																							 Templates.Search(objDocument.StructType)?.FullFileNameRootTemplate, 
																							 Project.OutputPath);
					// Graba los documentos hijo
						SaveDocuments(objDocument.Childs);
				}
		}

		/// <summary>
		///		Añade un error
		/// </summary>
		private void AddError(DocumentFileModel objDocument, string strMessage)
		{ AddError($"Error: {strMessage}. Estructura: {objDocument.Name} ({objDocument.StructType})");
		}

		/// <summary>
		///		Añade un error
		/// </summary>
		private void AddError(string strError)
		{ DocumentationProcessor.Errors.Add(strError);
		}

		/// <summary>
		///		Directorio base para las URL de los documentos
		/// </summary>
		private string UrlBaseDocuments
		{ get { return System.IO.Path.GetFileName(Project.OutputPath); }
		}

		/// <summary>
		///		Procesador de la documentación
		/// </summary>
		internal NSharpDocManager DocumentationProcessor { get; }

		/// <summary>
		///		Proveedor
		/// </summary>
		private ProjectDocumentationModel Project { get; }

		/// <summary>
		///		Plantillas
		/// </summary>
		internal TemplateModelCollection Templates { get; private set; }
	}
}
