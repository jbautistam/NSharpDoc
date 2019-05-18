using System;

using Bau.Libraries.LibNSharpDoc.Models.Structs;
using Bau.Libraries.LibNSharpDoc.Processor.Models.Documents;
using Bau.Libraries.LibNSharpDoc.Processor.Models.Templates;
using Bau.Libraries.LibNSharpDoc.Processor.Models.Projects;

namespace Bau.Libraries.LibNSharpDoc.Processor.Processor
{
	/// <summary>
	///		Generador de documentación
	/// </summary>
	internal class DocumentationGenerator
	{
		internal DocumentationGenerator(NSharpDocManager objGenerator, ProjectDocumentationModel project)
		{
			DocumentationProcessor = objGenerator;
			Project = project;
		}

		/// <summary>
		///		Procesa la generación de documentos
		/// </summary>
		internal void Process(StructDocumentationModelCollection structDocs)
		{
			DocumentFileModelCollection documents = new DocumentFileModelCollection();

				// Carga las plantillas
				Templates = new Repository.Templates.TemplateRepository().Load(Project.TemplatePath);
				// Crea los documentos
				foreach (StructDocumentationModel structDoc in structDocs)
					if (Templates.MustGenerateFile(structDoc, Project.GenerationParameters))
					{
						DocumentFileModel document = new DocumentFileModel(null, structDoc, 0);

							// Procesa la estructura del lenguaje
							Generate(structDoc, document);
							// Añade el documento a la colección
							documents.Add(document);
					}
				// Graba los documentos
				ProcessDocuments(documents);
		}

		/// <summary>
		///		Genera la documentación de una estructura
		/// </summary>
		private void Generate(StructDocumentationModel structDoc, DocumentFileModel parent)
		{
			foreach (StructDocumentationModel item in structDoc.Childs)
				if (Templates.MustGenerateFile(item, Project.GenerationParameters))
				{
					DocumentFileModel document = new DocumentFileModel(parent, item, parent.Childs.SearchOrder(item));

						// Añade el documento a los hijos
						parent.Childs.Add(document);
						// Añade los documentos hijo
						Generate(item, document);
				}
		}

		/// <summary>
		///		Genera la documentación
		/// </summary>
		private void ProcessDocuments(DocumentFileModelCollection documents)
		{ 
			// Ordena los archivos por nombres
			documents.SortByName();
			// Genera los archivos de contenido
			GenerateFilesContent(documents, documents);
			// Genera los archivos de índice
			documents.AddRange(GenerateFilesIndex(documents));
			// Transforma los hipervínculos
			documents.TransformSearchLinks(UrlBaseDocuments);
			// Graba los documentos
			SaveDocuments(documents);
		}

		/// <summary>
		///		Genera los archivos de contenido
		/// </summary>
		private void GenerateFilesContent(DocumentFileModelCollection allDocuments, DocumentFileModelCollection documents)
		{
			foreach (DocumentFileModel document in documents)
			{
				TemplateModel template = Templates.Search(document.StructType);

					// Genera la documentación del archivo
					if (template == null)
						AddError(document, "No se encuentra ninguna plantilla para este tipo de estructura");
					else
						try
						{
							new Generators.TemplateDocumentGenerator(Project, this, template, document, UrlBaseDocuments, allDocuments).Process();
						}
						catch (Exception exception)
						{
							AddError(document, $"Error al generar el documento. {exception.Message}");
						}
					// Genera los documentos hijo
					GenerateFilesContent(allDocuments, document.Childs);
			}
		}

		/// <summary>
		///		Genera los archivos de índice
		/// </summary>
		private DocumentFileModelCollection GenerateFilesIndex(DocumentFileModelCollection documents)
		{
			DocumentFileModelCollection filesIndex = new DocumentFileModelCollection();

				// Recorre las plantillas generando los archivos de índice
				foreach (TemplateModel template in Templates)
					if (template.Type == TemplateModel.TemplateType.Index)
						try
						{
							DocumentFileModel fullDocument = documents.Compact(template.Name);
							
								// Procesa el documento
								new Generators.TemplateDocumentGenerator(Project, this, template, fullDocument, UrlBaseDocuments,
																		 documents).Process();
								// ... y lo añade a la colección de índices
								filesIndex.Add(fullDocument);
						}
						catch (Exception exception)
						{
							AddError($"Error al generar el documento de índice de la plantilla {template.RelativeFileName}. {exception.Message}");
						}
				// Devuelve la colección de índices
				return filesIndex;
		}

		/// <summary>
		///		Graba los documentos
		/// </summary>
		private void SaveDocuments(DocumentFileModelCollection documents)
		{
			foreach (DocumentFileModel document in documents)
			{ 
				// Graba el documento
				DocumentationProcessor.Writer.Save(document, document.MLBuilder,
												   Templates.Search(document.StructType)?.FullFileNameRootTemplate,
												   Project.OutputPath);
				// Graba los documentos hijo
				SaveDocuments(document.Childs);
			}
		}

		/// <summary>
		///		Añade un error
		/// </summary>
		private void AddError(DocumentFileModel document, string strMessage)
		{
			AddError($"Error: {strMessage}. Estructura: {document.Name} ({document.StructType})");
		}

		/// <summary>
		///		Añade un error
		/// </summary>
		private void AddError(string strError)
		{
			DocumentationProcessor.Errors.Add(strError);
		}

		/// <summary>
		///		Directorio base para las URL de los documentos
		/// </summary>
		private string UrlBaseDocuments
		{
			get { return System.IO.Path.GetFileName(Project.OutputPath); }
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
