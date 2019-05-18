using System;
using System.Collections.Generic;

using Bau.Libraries.LibCommonHelper.Extensors;
using Bau.Libraries.LibNSharpDoc.Models.Structs;

namespace Bau.Libraries.LibNSharpDoc.Processor.Models.Documents
{
	/// <summary>
	///		Colección de <see cref="DocumentFileModel"/>
	/// </summary>
	public class DocumentFileModelCollection : List<DocumentFileModel>
	{
		/// <summary>
		///		Obtiene la cadena de depuración
		/// </summary>
		public string Debug(int indent)
		{
			string debug = "";

				// Crea la cadena de depuración
				foreach (DocumentFileModel document in this)
					debug += document.Debug(indent);
				// Devuelve la cadena de depuración
				return debug;
		}

		/// <summary>
		///		Comprueba si existen elementos de un tipo determinado
		/// </summary>
		internal bool ExistsItems(string structType)
		{
			// Recorre los elementos
			foreach (DocumentFileModel document in this)
				if (document.StructType.EqualsIgnoreCase(structType))
					return true;
			// Si ha llegado hasta aquí es porque no existen elementos
			return false;
		}

		/// <summary>
		///		Ordena los documentos por nombres
		/// </summary>
		internal void SortByName()
		{
			// Ordena los elementos hijo
			foreach (DocumentFileModel document in this)
				document.Childs.SortByNameInner();
			// Ordena los elementos
			SortByNameInner();
		}

		/// <summary>
		///		Obtiene el orden de un elemento
		/// </summary>
		internal int SearchOrder(StructDocumentationModel item)
		{
			int order = 0;

				// Cuenta los elementos que tengan el mismo nombre
				foreach (DocumentFileModel document in this)
					if (item.Name.EqualsIgnoreCase(document.Name))
						order++;
				// Devuelve el orden
				return order;
		}

		/// <summary>
		///		Ordena los documentos por nombres (rutina interna)
		/// </summary>
		private void SortByNameInner()
		{
			Sort((first, second) => first.Name.CompareIgnoreNullTo(second.Name));
		}

		/// <summary>
		///		Transforma los vínculos de búsqueda
		/// </summary>
		internal void TransformSearchLinks(string pathBase)
		{
			Dictionary<string, DocumentFileModel> links = new Dictionary<string, DocumentFileModel>();

				// Crea el diccionario con los vínculos 
				SearchDocumentLinks(links, this);
				// Transforma los vínculos
				TransformSearchLinks(links, this, pathBase);
		}

		/// <summary>
		///		Obtiene las estructuras generadas y los documentos en que se han generado
		/// </summary>
		private void SearchDocumentLinks(Dictionary<string, DocumentFileModel> links, DocumentFileModelCollection documents)
		{
			foreach (DocumentFileModel document in documents)
			{
				// Añade los nombres de las estructuras generadas en el documento
				foreach (StructDocumentationModel structDoc in document.StructsReferenced)
					if (!links.ContainsKey(structDoc.Name))
						links.Add(structDoc.Name, document);
				// Añade los nombres de las estructuras de los documentos hijo
				SearchDocumentLinks(links, document.Childs);
			}
		}

		/// <summary>
		///		Transforma los vínculos de búsqueda utilizando el diccionario
		/// </summary>
		private void TransformSearchLinks(Dictionary<string, DocumentFileModel> links, DocumentFileModelCollection documents, string pathBase)
		{
			foreach (DocumentFileModel document in documents)
			{
				// Transforma los vínculos de búsqueda
				document.TransformSearchLinks(links, pathBase);
				// Transforma los documentos hijo
				TransformSearchLinks(links, document.Childs, pathBase);
			}
		}

		/// <summary>
		///		Busca el documento que se asocia a una estructura
		/// </summary>
		internal DocumentFileModel Search(StructDocumentationModel structDoc)
		{
			DocumentFileModel search = null;

				// Busca el elemento en la colección
				foreach (DocumentFileModel document in this)
					if (search == null)
						if (document.LanguageStruct != null &&
								document.LanguageStruct.Type.EqualsIgnoreCase(structDoc.Type) &&
								document.LanguageStruct.Name.EqualsIgnoreCase(structDoc.Name) &&
								document.Order == structDoc.Order)
							search = document;
						else
							search = document.Childs.Search(structDoc);
				// Devuelve el elemento localizado
				return search;
		}

		/// <summary>
		///		Genera un documento a partir de una serie de documentos
		/// </summary>
		internal DocumentFileModel Compact(string name)
		{
			DocumentFileModel document = new DocumentFileModel(null,
															   new StructDocumentationModel(null, StructDocumentationModel.ScopeType.Global,
																							name, "Index", 0),
															   0);

				// Añade las estructuras de todos los documentos hijo
				foreach (DocumentFileModel child in this)
					document.LanguageStruct.Childs.Add(child.LanguageStruct);
				// Devuelve el documento
				return document;
		}
	}
}