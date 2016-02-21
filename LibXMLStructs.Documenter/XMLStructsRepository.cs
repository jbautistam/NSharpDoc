using System;

using Bau.Libraries.LibHelper.Extensors;
using Bau.Libraries.LibMarkupLanguage;
using Bau.Libraries.LibNSharpDoc.Models.Structs;

namespace Bau.Libraries.LibXMLStructs.Documenter
{
	/// <summary>
	///		Repository de <see cref="StructDocumentationModel"/>
	/// </summary>
	public class XMLStructsRepository
	{	// Constantes privadas
			private const string cnstStrTagStruct = "Struct";
			private const string cnstStrTagScope = "Scope";
			private const string cnstStrTagName = "Name";
			private const string cnstStrTagType = "Type";
			private const string cnstStrTagOrder = "Order";
			private const string cnstStrTagParameter = "Parameter";
			private const string cnstStrTagKey = "Key";
			private const string cnstStrTagReference = "Reference";
		
		/// <summary>
		///		Carga las estructuras de documentación de un archivo
		/// </summary>
		public StructDocumentationModel Load(string strFileName)
		{ StructDocumentationModel objStruct = new StructDocumentationModel(null, StructDocumentationModel.ScopeType.Global, "", "", 0);

				// Carga el archivo XML
					if (System.IO.File.Exists(strFileName))
						{ MLFile objMLFile = new LibMarkupLanguage.Services.XML.XMLParser().Load(strFileName);

								foreach (MLNode objMLNode in objMLFile.Nodes)
									if (objMLNode.Name == cnstStrTagStruct)
										{ // Interpreta el elemento
												LoadStruct(objMLNode, objStruct);
											// Interpreta los hijos
												LoadChilds(objMLNode, objStruct);
											// Interpreta los parámetros
												LoadParameters(objMLNode, objStruct);
										}
						}
				// Devuelve la estructura
					return objStruct;
		}

		/// <summary>
		///		Carga los parámetros de una estructura
		/// </summary>
		private void LoadStruct(MLNode objMLNode, StructDocumentationModel objStruct)
		{ objStruct.Scope = (StructDocumentationModel.ScopeType) objMLNode.Nodes[cnstStrTagScope].Value.GetInt(0);
			objStruct.Name = objMLNode.Nodes[cnstStrTagName].Value;
			objStruct.Type = objMLNode.Nodes[cnstStrTagType].Value;
			objStruct.Order = objMLNode.Nodes[cnstStrTagOrder].Value.GetInt(0);
		}

		/// <summary>
		///		Carga los hijos de un nodo
		/// </summary>
		private void LoadChilds(MLNode objMLNode, StructDocumentationModel objParent)
		{ foreach (MLNode objMLChild in objMLNode.Nodes)
				if (objMLChild.Name == cnstStrTagStruct)
					{ StructDocumentationModel objStruct = new StructDocumentationModel(objParent, StructDocumentationModel.ScopeType.Global,
																																							"", "", 0);

							// Asigna los parámetros a la estructura
								LoadStruct(objMLChild, objStruct);
							// Carga las estructuras hija y los parámetros
								LoadChilds(objMLChild, objStruct);
								LoadParameters(objMLChild, objStruct);
							// Añade la estructura a la colección
								objParent.Childs.Add(objStruct);
					}
		}

		/// <summary>
		///		Carga los parámetros de una estructura
		/// </summary>
		private void LoadParameters(MLNode objMLNode, StructDocumentationModel objStruct)
		{ foreach (MLNode objMLChild in objMLNode.Nodes)
				if (objMLChild.Name == cnstStrTagParameter)
					objStruct.Parameters.Add(objMLChild.Attributes[cnstStrTagKey].Value, 
																	 objMLChild.Value,
																	 objMLChild.Attributes[cnstStrTagReference].Value);
		}

		/// <summary>
		///		Graba en un archivo XML una estructura de documentación
		/// </summary>
		public void Save(string strFileName, StructDocumentationModel objStruct)
		{ MLFile objMLFile = new MLFile();

				// Añade el nodo de la structura
					objMLFile.Nodes.Add(GetStructNode(objStruct));
				// Graba el archivo
					new LibMarkupLanguage.Services.XML.XMLWriter().Save(objMLFile, strFileName);
		}

		/// <summary>
		///		Obtiene el nodo con los datos de una estructura
		/// </summary>
		private MLNode GetStructNode(StructDocumentationModel objStruct)
		{ MLNode objMLNode = new MLNode(cnstStrTagStruct);

				// Añade los datos de la estructura
					objMLNode.Nodes.Add(cnstStrTagName, objStruct.Name);
					objMLNode.Nodes.Add(cnstStrTagScope, ((int) objStruct.Scope).ToString());
					objMLNode.Nodes.Add(cnstStrTagType, objStruct.Type);
					objMLNode.Nodes.Add(cnstStrTagOrder, objStruct.Order);
				// Añade las estructuras hija y los parámetros
					foreach (StructDocumentationModel objChild in objStruct.Childs)
						objMLNode.Nodes.Add(GetStructNode(objChild));
					objMLNode.Nodes.AddRange(GetParametersNodes(objStruct));
				// Devuelve el nodo
					return objMLNode;
		}

		/// <summary>
		///		Obtiene los nodos de los parámetros
		/// </summary>
		private MLNodesCollection GetParametersNodes(StructDocumentationModel objStruct)
		{ MLNodesCollection objColMLNodes = new MLNodesCollection();

				// Añade los parámetros
					foreach (System.Collections.Generic.KeyValuePair<string, StructParameterModel> objParameter in objStruct.Parameters.Parameters)
						{ MLNode objMLNode = new MLNode(cnstStrTagParameter, objParameter.Value.Value?.ToString());

								// Añade los atributos
									objMLNode.Attributes.Add(cnstStrTagKey, objParameter.Value.Key);
									objMLNode.Attributes.Add(cnstStrTagReference, objParameter.Value.ReferenceKey);
								// Añade el nodo a la colección
									objColMLNodes.Add(objMLNode);
						}
				// Devuelve la colección de nodos
					return objColMLNodes;
		}
	}
}
