using System;

using Bau.Libraries.LibCommonHelper.Extensors;
using Bau.Libraries.LibMarkupLanguage;
using Bau.Libraries.LibNSharpDoc.Models.Structs;

namespace Bau.Libraries.LibXMLStructs.Documenter
{
	/// <summary>
	///		Repository de <see cref="StructDocumentationModel"/>
	/// </summary>
	public class XMLStructsRepository
	{   
		// Constantes privadas
		private const string TagStruct = "Struct";
		private const string TagScope = "Scope";
		private const string TagName = "Name";
		private const string TagType = "Type";
		private const string TagOrder = "Order";
		private const string TagParameter = "Parameter";
		private const string TagKey = "Key";
		private const string TagReference = "Reference";

		/// <summary>
		///		Carga las estructuras de documentación de un archivo
		/// </summary>
		public StructDocumentationModel Load(string fileName)
		{
			StructDocumentationModel structDoc = new StructDocumentationModel(null, StructDocumentationModel.ScopeType.Global, "", "", 0);

				// Carga el archivo XML
				if (System.IO.File.Exists(fileName))
				{
					MLFile fileML = new LibMarkupLanguage.Services.XML.XMLParser().Load(fileName);

						foreach (MLNode nodeML in fileML.Nodes)
							if (nodeML.Name == TagStruct)
							{ 
								// Interpreta el elemento
								LoadStruct(nodeML, structDoc);
								// Interpreta los hijos
								LoadChilds(nodeML, structDoc);
								// Interpreta los parámetros
								LoadParameters(nodeML, structDoc);
							}
				}
				// Devuelve la estructura
				return structDoc;
		}

		/// <summary>
		///		Carga los parámetros de una estructura
		/// </summary>
		private void LoadStruct(MLNode nodeML, StructDocumentationModel structDoc)
		{
			structDoc.Scope = (StructDocumentationModel.ScopeType)nodeML.Nodes[TagScope].Value.GetInt(0);
			structDoc.Name = nodeML.Nodes[TagName].Value;
			structDoc.Type = nodeML.Nodes[TagType].Value;
			structDoc.Order = nodeML.Nodes[TagOrder].Value.GetInt(0);
		}

		/// <summary>
		///		Carga los hijos de un nodo
		/// </summary>
		private void LoadChilds(MLNode nodeML, StructDocumentationModel parent)
		{
			foreach (MLNode childML in nodeML.Nodes)
				if (childML.Name == TagStruct)
				{
					StructDocumentationModel structDoc = new StructDocumentationModel(parent, StructDocumentationModel.ScopeType.Global, "", "", 0);

						// Asigna los parámetros a la estructura
						LoadStruct(childML, structDoc);
						// Carga las estructuras hija y los parámetros
						LoadChilds(childML, structDoc);
						LoadParameters(childML, structDoc);
						// Añade la estructura a la colección
						parent.Childs.Add(structDoc);
				}
		}

		/// <summary>
		///		Carga los parámetros de una estructura
		/// </summary>
		private void LoadParameters(MLNode nodeML, StructDocumentationModel structDoc)
		{
			foreach (MLNode childML in nodeML.Nodes)
				if (childML.Name == TagParameter)
					structDoc.Parameters.Add(childML.Attributes[TagKey].Value, childML.Value, childML.Attributes[TagReference].Value);
		}

		/// <summary>
		///		Graba en un archivo XML una estructura de documentación
		/// </summary>
		public void Save(string fileName, StructDocumentationModel structDoc)
		{
			MLFile fileML = new MLFile();

				// Añade el nodo de la structura
				fileML.Nodes.Add(GetStructNode(structDoc));
				// Graba el archivo
				new LibMarkupLanguage.Services.XML.XMLWriter().Save(fileName, fileML);
		}

		/// <summary>
		///		Obtiene el nodo con los datos de una estructura
		/// </summary>
		private MLNode GetStructNode(StructDocumentationModel structDoc)
		{
			MLNode nodeML = new MLNode(TagStruct);

				// Añade los datos de la estructura
				nodeML.Nodes.Add(TagName, structDoc.Name);
				nodeML.Nodes.Add(TagScope, ((int)structDoc.Scope).ToString());
				nodeML.Nodes.Add(TagType, structDoc.Type);
				nodeML.Nodes.Add(TagOrder, structDoc.Order);
				// Añade las estructuras hija y los parámetros
				foreach (StructDocumentationModel child in structDoc.Childs)
					nodeML.Nodes.Add(GetStructNode(child));
				nodeML.Nodes.AddRange(GetParametersNodes(structDoc));
				// Devuelve el nodo
				return nodeML;
		}

		/// <summary>
		///		Obtiene los nodos de los parámetros
		/// </summary>
		private MLNodesCollection GetParametersNodes(StructDocumentationModel structDoc)
		{
			MLNodesCollection nodesML = new MLNodesCollection();

				// Añade los parámetros
				foreach (System.Collections.Generic.KeyValuePair<string, StructParameterModel> parameter in structDoc.Parameters.Parameters)
				{
					MLNode nodeML = new MLNode(TagParameter, parameter.Value.Value?.ToString());

						// Añade los atributos
						nodeML.Attributes.Add(TagKey, parameter.Value.Key);
						nodeML.Attributes.Add(TagReference, parameter.Value.ReferenceKey);
						// Añade el nodo a la colección
						nodesML.Add(nodeML);
				}
				// Devuelve la colección de nodos
				return nodesML;
		}
	}
}
