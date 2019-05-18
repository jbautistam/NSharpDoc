using System;

using Bau.Libraries.LibCommonHelper.Extensors;
using Bau.Libraries.LibMarkupLanguage;
using Bau.Libraries.LibNSharpDoc.Processor.Models.Documents;
using Bau.Libraries.LibNSharpDoc.Processor.Models.Projects;
using Bau.Libraries.LibNSharpDoc.Processor.Models.Templates;
using Bau.Libraries.LibNSharpDoc.Models.Structs;

namespace Bau.Libraries.LibNSharpDoc.Processor.Processor.Generators
{
	/// <summary>
	///		Genera la documentación intermedia a partir de una plantilla
	/// </summary>
	internal class TemplateDocumentGenerator
	{
		// Constantes privadas
		private const string TagIfExists = "IfExists"; // ... comprueba si existen estructuras de un tipo dentro de la estructura que interpretamos
		private const string TagIfValue = "IfValue"; // ... comprueba si existe un valor en la estructura que interpretamos
		private const string TagSwitch = "Switch"; // ... comprueba si existe una serie de valores en la estructura que interpretamos
		private const string TagForEach = "ForEach"; // ... por cada uno de los elementos de un tipo de estructura
		private const string TagSelect = "Select"; // ... por el primer elemento de la estructura
		private const string TagAttributeAddToReferences = "AddToReferences";
		private const string TagAttributeStructType = "StructType";
		private const string TagAttributeValueType = "ValueType";
		private const string TagAttributeValue = "Value";

		internal TemplateDocumentGenerator(ProjectDocumentationModel project,
										   DocumentationGenerator documentationGenerator,
										   TemplateModel template, DocumentFileModel document,
										   string urlBase, DocumentFileModelCollection allDocuments)
		{
			Project = project;
			Generator = documentationGenerator;
			Template = template;
			Document = document;
			UrlBase = urlBase;
			DocumentsGenerated = allDocuments;
		}

		/// <summary>
		///		Procesa el documento
		/// </summary>
		internal void Process()
		{
			MLFile fileML = new LibMarkupLanguage.Services.XML.XMLParser().Load(Template.FullFileName);

				// Limpia el generador
				Document.MLBuilder.Clear();
				// Añade la estructura a la colección de estructuras generadas
				Document.StructsReferenced.Add(Document.LanguageStruct);
				// Recorre los datos del archivo generando la salida
				foreach (MLNode sourceML in fileML.Nodes)
					if (sourceML.Name == Document.MLBuilder.Root.Name)
						TreatChilds(sourceML.Nodes, Document.MLBuilder.Root, Document.LanguageStruct);
		}

		/// <summary>
		///		Trata una serie de nodos
		/// </summary>
		private void TreatChilds(MLNodesCollection sourcesML, MLNode parentTargetML, StructDocumentationModel structDoc)
		{
			foreach (MLNode sourceML in sourcesML)
				TreatNode(sourceML, parentTargetML, structDoc);
		}

		/// <summary>
		///		Trata el nodo
		/// </summary>
		private void TreatNode(MLNode sourceML, MLNode parentTargetML, StructDocumentationModel structDoc)
		{
			switch (sourceML.Name)
			{
				case TagIfExists:
						if (CheckIfExistsChild(structDoc, sourceML.Attributes[TagAttributeStructType].Value))
							TreatChilds(sourceML.Nodes, parentTargetML, structDoc);
					break;
				case TagIfValue:
						if (CheckIfExistsValue(structDoc, sourceML.Attributes[TagAttributeValueType].Value))
							TreatChilds(sourceML.Nodes, parentTargetML, structDoc);
					break;
				case TagForEach:
				case TagSelect:
						TreatLoop(sourceML, parentTargetML, structDoc, sourceML.Attributes[TagAttributeStructType].Value);
					break;
				case TagSwitch:
						string value = GetStructParameterContent(sourceML.Attributes[TagAttributeValueType].Value, structDoc);
						bool treated = false;

						foreach (MLNode childML in sourceML.Nodes)
							if (!treated &&
								((childML.Name == "Case" && childML.Attributes[TagAttributeValue].Value.EqualsIgnoreCase(value)) ||
								  childML.Name == "Default"))
							{ 
								// Indica que se ha tratado el nodo
								treated = true;
								// Trata los hijos
								TreatChilds(childML.Nodes, parentTargetML, structDoc);
							}
					break;
				default:
						MLNode targetML = CloneNode(sourceML, structDoc);

							// Añade el nodo
							parentTargetML.Nodes.Add(targetML);
							// Trata los hijos
							TreatChilds(sourceML.Nodes, targetML, structDoc);
					break;
			}
		}

		/// <summary>
		///		Comprueba si existe un tipo de estructura hija
		/// </summary>
		private bool CheckIfExistsChild(StructDocumentationModel structDoc, string structType)
		{   
			// Obtiene las estructuras
			if (!structType.IsEmpty())
			{
				string[] structTypes = structType.Split(';');

					foreach (string strStruct in structTypes)
						if (!strStruct.IsEmpty())
						{
							StructDocumentationModelCollection structsDoc = SelectItemsForGeneration(structDoc, strStruct.TrimIgnoreNull());

								if (structsDoc != null && structsDoc.Count > 0)
									return true;
						}
			}
			// Si ha llegado hasta aquí es porque no existía ninguna
			return false;
		}

		/// <summary>
		///		Comprueba si existe algún valor en una estructura
		/// </summary>
		private bool CheckIfExistsValue(StructDocumentationModel structDoc, string name)
		{
			return !GetStructParameterContent(name, structDoc).IsEmpty();
		}

		/// <summary>
		///		Trata un bucle
		/// </summary>
		private void TreatLoop(MLNode sourceML, MLNode parentTargetML, StructDocumentationModel structDoc, string structType)
		{
			StructDocumentationModelCollection structsDoc = SelectItemsForGeneration(structDoc, structType);

				// Crea los nodos hijo
				foreach (StructDocumentationModel child in structsDoc)
				{ 
					// Si hay que añadirla a la colección de estructuras por referencia, se añade
					if (sourceML.Attributes[TagAttributeAddToReferences].Value.GetBool())
						Document.StructsReferenced.Add(child);
					// Trata los nodos hijo
					TreatChilds(sourceML.Nodes, parentTargetML, child);
				}
		}

		/// <summary>
		///		Clona un nodo con los datos de una estructura
		/// </summary>
		private MLNode CloneNode(MLNode sourceML, StructDocumentationModel structDoc)
		{
			MLNode targetML = new MLNode(sourceML.Name);

				// Interpreta el contenido
				if (sourceML.Nodes.Count == 0)
					targetML.Nodes.AddRange(Parse(sourceML.Value, structDoc));
				// Clona los atributos
				foreach (MLAttribute attributeML in sourceML.Attributes)
					targetML.Attributes.Add(attributeML.Name, attributeML.Value);
				// Devuelve el nodo
				return targetML;
		}

		/// <summary>
		///		Interpreta una cadena sustituyendo los parámetros {{xxxx}} por el valor de la estructura o argumento
		/// </summary>
		private MLNodesCollection Parse(string value, StructDocumentationModel structDoc)
		{
			MLNodesCollection nodesML = new MLNodesCollection();

				// Interpreta la cadena
				while (!value.IsEmpty())
				{
					string first = value.Cut("{{", out value);

						// Añade el nodo con la parte inicial de la cadena
						if (!first.IsEmpty())
							nodesML.Add(Document.MLBuilder.GetSpan(first));
						// Si queda algo, recoge hasta el siguiente }}
						if (!value.IsEmpty())
						{
							string name = value.Cut("}}", out value).TrimIgnoreNull();

								// Procesa el valor
								nodesML.Add(GetStructParameterValue(name, structDoc));
						}
				}
				// Devuelve el valor
				return nodesML;
		}

		/// <summary>
		///		Interpreta un parámetro
		/// </summary>
		private MLNode GetStructParameterValue(string name, StructDocumentationModel structDoc)
		{
			bool isLink = false;
			string format = string.Empty;

				// Comprueba si es un vínculo
				if (!name.IsEmpty())
				{ 
					string[] parts = name.Split('|');

						// Asigna el nombre
						name = parts[0];
						// Asigna los parámetros
						for (int index = 1; index < parts.Length; index++)
							if (parts[index].EqualsIgnoreCase("Link"))
								isLink = true;
							else
								format = format.AddWithSeparator(parts[index], "|", false);
				}
				// Devuelve el nodo
				return GetStructParameterValue(name, format, isLink, structDoc);
		}

		/// <summary>
		///		Obtiene el valor de una estructura
		/// </summary>
		private MLNode GetStructParameterValue(string name, string format, bool includeLink, StructDocumentationModel structDoc)
		{
			MLNode nodeML = null;

				// Obtiene el nodo de un nombre fijo
				switch (name.TrimIgnoreNull().ToUpper())
				{
					case "MODIFIER":
							nodeML = Document.MLBuilder.GetSpan(structDoc.Scope.ToString());
						break;
					case "NAME":
							if (!includeLink)
								nodeML = Document.MLBuilder.GetSpan(structDoc.Name);
							else
								nodeML = GetLinkToDocument(structDoc);
						break;
					case "STRUCTTYPE":
							nodeML = Document.MLBuilder.GetSpan(structDoc.Type);
						break;
					default:
							StructParameterModel parameter = structDoc.Parameters.Search(name);

								if (parameter != null)
								{
									if (!includeLink || parameter.Reference == null)
										nodeML = Document.MLBuilder.GetSpan(parameter.Value?.ToString(), format);
									else
										nodeML = GetLinkToDocument(parameter.Reference);
								}
						break;
				}
				// Devuelve el nodo
				return nodeML;
		}

		/// <summary>
		///		Obtiene el valor de un parámetro en una estructura
		/// </summary>
		private string GetStructParameterContent(string name, StructDocumentationModel structDoc)
		{
			return structDoc.Parameters.Search(name)?.Value?.ToString();
		}

		/// <summary>
		///		Obtiene un vínculo a un documento
		/// </summary>
		private MLNode GetLinkToDocument(StructDocumentationModel structDoc)
		{
			DocumentFileModel document = DocumentsGenerated.Search(structDoc);

				// Devuelve el vínculo
				if (document == null)
					return Document.MLBuilder.GetSpan(structDoc.Name);
				else
					return Document.MLBuilder.GetLink(document.Name, document.GetUrl(UrlBase));
		}

		/// <summary>
		///		Obtiene los elementos de determinada estructura que se deben documentar
		/// </summary>
		private StructDocumentationModelCollection SelectItemsForGeneration(StructDocumentationModel structDoc, string typeId)
		{
			StructDocumentationModelCollection structsDoc = new StructDocumentationModelCollection();

				// Obtiene las estructuras
				foreach (StructDocumentationModel child in structDoc.Childs)
					if (child.Type.EqualsIgnoreCase(typeId) &&
							Generator.Templates.MustGenerateDocumentation(child, Project.GenerationParameters))
						structsDoc.Add(child);
				// Ordena las estructuras por nombre
				structsDoc.SortByName();
				// Devuelve la colección
				return structsDoc;
		}

		/// <summary>
		///		Proyecto
		/// </summary>
		internal ProjectDocumentationModel Project { get; }

		/// <summary>
		///		Generador
		/// </summary>
		internal DocumentationGenerator Generator { get; }

		/// <summary>
		///		Plantilla
		/// </summary>
		internal TemplateModel Template { get; }

		/// <summary>
		///		Documento
		/// </summary>
		internal DocumentFileModel Document { get; }

		/// <summary>
		///		Url base
		/// </summary>
		internal string UrlBase { get; }

		/// <summary>
		///		Colección de documentos generados en este proceso (para las búsquedas por vínculos)
		/// </summary>
		internal DocumentFileModelCollection DocumentsGenerated { get; }
	}
}
