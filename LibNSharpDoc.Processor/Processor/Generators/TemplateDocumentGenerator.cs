using System;

using Bau.Libraries.LibHelper.Extensors;
using Bau.Libraries.LibMarkupLanguage;
using Bau.Libraries.LibNSharpDoc.Processor.Models.Documents;
using Bau.Libraries.LibNSharpDoc.Processor.Models.Templates;
using Bau.Libraries.LibNSharpDoc.Models.Structs;

namespace Bau.Libraries.LibNSharpDoc.Processor.Processor.Generators
{
	/// <summary>
	///		Genera la documentación intermedia a partir de una plantilla
	/// </summary>
	internal class TemplateDocumentGenerator
	{ // Constantes privadas
			private const string cnstStrTagIfExists = "IfExists"; // ... comprueba si existen estructuras de un tipo dentro de la estructura que interpretamos
			private const string cnstStrTagIfValue = "IfValue"; // ... comprueba si existe un valor en la estructura que interpretamos
			private const string cnstStrTagSwitch = "Switch"; // ... comprueba si existe una serie de valores en la estructura que interpretamos
			private const string cnstStrTagForEach = "ForEach"; // ... por cada uno de los elementos de un tipo de estructura
			private const string cnstStrTagSelect = "Select"; // ... por el primer elemento de la estructura
			private const string cnstStrTagAttributeParameter = "Parameter";
			private const string cnstStrTagAttributeAddToReferences = "AddToReferences";
			private const string cnstStrTagAttributeStructType = "StructType";
			private const string cnstStrTagAttributeValueType = "ValueType";
			private const string cnstStrTagAttributeValue = "Value";

		internal TemplateDocumentGenerator(Models.Projects.ProjectDocumentationModel objProject,
																			 DocumentationGenerator objDocumentationGenerator, 
																			 TemplateModel objTemplate, DocumentFileModel objDocument,
																			 string strUrlBase,
																			 DocumentFileModelCollection objColAllDocuments)
		{ Project = objProject;
			Generator = objDocumentationGenerator;
			Template = objTemplate;
			Document = objDocument;
			UrlBase = strUrlBase;
			DocumentsGenerated = objColAllDocuments;
		}

		/// <summary>
		///		Procesa el documento
		/// </summary>
		internal void Process()
		{ MLFile objMLFile = new LibMarkupLanguage.Services.XML.XMLParser().Load(Template.FullFileName);

				// Limpia el generador
					Document.MLBuilder.Clear();
				// Añade la estructura a la colección de estructuras generadas
					Document.StructsReferenced.Add(Document.LanguageStruct);
				// Recorre los datos del archivo generando la salida
					foreach (MLNode objMLSource in objMLFile.Nodes)
						if (objMLSource.Name == Document.MLBuilder.Root.Name)
							TreatChilds(objMLSource.Nodes, Document.MLBuilder.Root, Document.LanguageStruct);
		}

		/// <summary>
		///		Trata una serie de nodos
		/// </summary>
		private void TreatChilds(MLNodesCollection objColMLSource, MLNode objMLParentTarget, StructDocumentationModel objStruct)
		{ foreach (MLNode objMLSource in objColMLSource)
				TreatNode(objMLSource, objMLParentTarget, objStruct);
		}

		/// <summary>
		///		Trata el nodo
		/// </summary>
		private void TreatNode(MLNode objMLSource, MLNode objMLParentTarget, StructDocumentationModel objStruct)
		{ switch (objMLSource.Name)
				{	case cnstStrTagIfExists:
							if (CheckIfExistsChild(objStruct, objMLSource.Attributes[cnstStrTagAttributeStructType].Value))
								TreatChilds(objMLSource.Nodes, objMLParentTarget, objStruct);
						break;
					case cnstStrTagIfValue:
							if (CheckIfExistsValue(objStruct, objMLSource.Attributes[cnstStrTagAttributeValueType].Value))
								TreatChilds(objMLSource.Nodes, objMLParentTarget, objStruct);
						break;
					case cnstStrTagForEach:
					case cnstStrTagSelect:
							TreatLoop(objMLSource, objMLParentTarget, objStruct, objMLSource.Attributes[cnstStrTagAttributeStructType].Value);
						break;
					case cnstStrTagSwitch:
							string strValue = GetStructParameterContent(objMLSource.Attributes[cnstStrTagAttributeValueType].Value, objStruct);
							bool blnTreated = false;

								foreach (MLNode objMLChild in objMLSource.Nodes)
									if (!blnTreated &&
												((objMLChild.Name == "Case" && 
													objMLChild.Attributes[cnstStrTagAttributeValue].Value.EqualsIgnoreCase(strValue)) ||
													objMLChild.Name == "Default"))
										{ // Indica que se ha tratado el nodo
												blnTreated = true;
											// Trata los hijos
												TreatChilds(objMLChild.Nodes, objMLParentTarget, objStruct);
										}
						break;
					default:
							MLNode objMLTarget = CloneNode(objMLSource, objStruct);

								// Añade el nodo
									objMLParentTarget.Nodes.Add(objMLTarget);
								// Trata los hijos
									TreatChilds(objMLSource.Nodes, objMLTarget, objStruct);
						break;
				}
		}

		/// <summary>
		///		Comprueba si existe un tipo de estructura hija
		/// </summary>
		private bool CheckIfExistsChild(StructDocumentationModel objStruct, string strStructType)
		{	StructDocumentationModelCollection objColStructs = SelectItemsForGeneration(objStruct, strStructType);

				// Devuelve el valor que indica si existe
					return objColStructs != null && objColStructs.Count > 0;
		}

		/// <summary>
		///		Comprueba si existe algún valor en una estructura
		/// </summary>
		private bool CheckIfExistsValue(StructDocumentationModel objStruct, string strName)
		{ return !GetStructParameterContent(strName, objStruct).IsEmpty();
			//MLNode objMLNode = GetStructParameterValue(strName, objStruct);

			//	// Comprueba si existe algún valor
			//		return objMLNode != null && !objMLNode.Value.IsEmpty();
		}

		/// <summary>
		///		Trata un bucle
		/// </summary>
		private void TreatLoop(MLNode objMLSource, MLNode objMLParentTarget, StructDocumentationModel objStruct, string strStructType)
		{ StructDocumentationModelCollection objColStructs = SelectItemsForGeneration(objStruct, strStructType);

				// Crea los nodos hijo
					foreach (StructDocumentationModel objChild in objColStructs)
						{ // Si hay que añadirla a la colección de estructuras por referencia, se añade
								if (objMLSource.Attributes[cnstStrTagAttributeAddToReferences].Value.GetBool())
									Document.StructsReferenced.Add(objChild);
							// Trata los nodos hijo
								TreatChilds(objMLSource.Nodes, objMLParentTarget, objChild);
						}
		}

		/// <summary>
		///		Clona un nodo con los datos de una estructura
		/// </summary>
		private MLNode CloneNode(MLNode objMLSource, StructDocumentationModel objStruct)
		{ MLNode objMLTarget = new MLNode(objMLSource.Name);
		
				// Interpreta el contenido
					if (objMLSource.Nodes.Count == 0)
						objMLTarget.Nodes.AddRange(Parse(objMLSource.Value, objStruct));
				// Clona los atributos
					foreach (MLAttribute objMLAttribute in objMLSource.Attributes)
						objMLTarget.Attributes.Add(objMLAttribute.Name, objMLAttribute.Value);
				// Devuelve el nodo
					return objMLTarget;
		}

		/// <summary>
		///		Interpreta una cadena sustituyendo los parámetros {{xxxx}} por el valor de la estructura o argumento
		/// </summary>
		private MLNodesCollection Parse(string strValue, StructDocumentationModel objStruct)
		{ MLNodesCollection objColMLNodes = new MLNodesCollection();

				// Interpreta la cadena
					while (!strValue.IsEmpty())
						{ string strFirst = strValue.Cut("{{", out strValue);

								// Añade el nodo con la parte inicial de la cadena
									if (!strFirst.IsEmpty())
										objColMLNodes.Add(Document.MLBuilder.GetSpan(strFirst));
								// Si queda algo, recoge hasta el siguiente }}
									if (!strValue.IsEmpty())
										{ string strName = strValue.Cut("}}", out strValue).TrimIgnoreNull();

												// Procesa el valor
													objColMLNodes.Add(GetStructParameterValue(strName, objStruct));
										}
						}
				// Devuelve el valor
					return objColMLNodes;
		}

		/// <summary>
		///		Interpreta un parámetro
		/// </summary>
		private MLNode GetStructParameterValue(string strName, StructDocumentationModel objStruct)
		{	MLNode objMLNode = null;
			bool blnIsLink = false;

				// Comprueba si es un vínculo
					if (!strName.IsEmpty() && strName.EndsWith("|Link", StringComparison.CurrentCultureIgnoreCase))
						{ // Indica que es un vínculo
								blnIsLink = true;
							// Quita el vínculo (|Link) del nombre
								strName = strName.Left(strName.Length - "|Link".Length);
						}
				// Obtiene un nodo con el valor
					objMLNode = GetStructParameterValue(strName, blnIsLink, objStruct);
				// Devuelve el nodo
					return objMLNode;
		}

		/// <summary>
		///		Obtiene el valor de una estructura
		/// </summary>
		private MLNode GetStructParameterValue(string strName, bool blnIncludeLink, StructDocumentationModel objStruct)
		{ MLNode objMLNode = null;

				// Obtiene el nodo de un nombre fijo
					switch (strName.TrimIgnoreNull().ToUpper())
						{	case "MODIFIER":
									objMLNode = Document.MLBuilder.GetSpan(objStruct.Scope.ToString());
								break;
							case "NAME":
									if (!blnIncludeLink)
										objMLNode = Document.MLBuilder.GetSpan(objStruct.Name);
									else
										objMLNode = GetLinkToDocument(objStruct);
								break;
							case "STRUCTTYPE":
									objMLNode = Document.MLBuilder.GetSpan(objStruct.Type);
								break;
							default:
									StructParameterModel objParameter = objStruct.Parameters.Search(strName);

										if (objParameter != null)
											{	if (!blnIncludeLink || objParameter.Reference == null)
													objMLNode = Document.MLBuilder.GetSpan(objParameter.Value?.ToString());
												else
													objMLNode = GetLinkToDocument(objParameter.Reference);
											}
								break;
						}
				// Devuelve el nodo
					return objMLNode;
		}

		/// <summary>
		///		Obtiene el valor de un parámetro en una estructura
		/// </summary>
		private string GetStructParameterContent(string strName, StructDocumentationModel objStruct)
		{ return objStruct.Parameters.Search(strName)?.Value?.ToString();
		}

		/// <summary>
		///		Obtiene un vínculo a un documento
		/// </summary>
		private MLNode GetLinkToDocument(StructDocumentationModel objStruct)
		{	DocumentFileModel objDocument = DocumentsGenerated.Search(objStruct);

				// Devuelve el vínculo
					if (objDocument == null)
						return Document.MLBuilder.GetSpan(objStruct.Name);
					else
						return Document.MLBuilder.GetLink(objDocument.Name, objDocument.GetUrl(UrlBase));
		}

		/// <summary>
		///		Obtiene los elementos de determinada estructura que se deben documentar
		/// </summary>
		private StructDocumentationModelCollection SelectItemsForGeneration(StructDocumentationModel objStruct, string strIDType)
		{ StructDocumentationModelCollection objColStructs = new StructDocumentationModelCollection();

				// Obtiene las estructuras
					foreach (StructDocumentationModel objChild in objStruct.Childs)
						if (objChild.Type.EqualsIgnoreCase(strIDType) && 
								Generator.Templates.MustGenerateDocumentation(objChild, Project.GenerationParameters))
							objColStructs.Add(objChild);
				// Ordena las estructuras por nombre
					objColStructs.SortByName();
				// Devuelve la colección
					return objColStructs;
		}

		/// <summary>
		///		Proyecto
		/// </summary>
		internal Models.Projects.ProjectDocumentationModel Project { get; }

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
