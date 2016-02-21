using System;
using System.Collections.Generic;

using Bau.Libraries.LibHelper.Extensors;
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
		public string Debug(int intIndent)
		{ string strDebug = "";

				// Crea la cadena de depuración
					foreach (DocumentFileModel objDocument in this)
						strDebug += objDocument.Debug(intIndent);
				// Devuelve la cadena de depuración
					return strDebug;
		}

		/// <summary>
		///		Comprueba si existen elementos de un tipo determinado
		/// </summary>
		internal bool ExistsItems(string strStructType)
		{ // Recorre los elementos
				foreach (DocumentFileModel objDocument in this)
					if (objDocument.StructType.EqualsIgnoreCase(strStructType))
						return true;
			// Si ha llegado hasta aquí es porque no existen elementos
				return false;
		}

		/// <summary>
		///		Ordena los documentos por nombres
		/// </summary>
		internal void SortByName()
		{ // Ordena los elementos hijo
				foreach (DocumentFileModel objDocument in this)
					objDocument.Childs.SortByNameInner();
			// Ordena los elementos
				SortByNameInner();
		}

		/// <summary>
		///		Obtiene el orden de un elemento
		/// </summary>
		internal int SearchOrder(StructDocumentationModel objItem)
		{ int intOrder = 0;

				// Cuenta los elementos que tengan el mismo nombre
					foreach (DocumentFileModel objDocument in this)
						if (objItem.Name.EqualsIgnoreCase(objDocument.Name))
							intOrder++;
				// Devuelve el orden
					return intOrder;
		}

		/// <summary>
		///		Ordena los documentos por nombres (rutina interna)
		/// </summary>
		private void SortByNameInner()
		{ Sort((objFirst, objSecond) => objFirst.Name.CompareTo(objSecond.Name));
		}

		/// <summary>
		///		Transforma los vínculos de búsqueda
		/// </summary>
		internal void TransformSearchLinks(string strPathBase)
		{ Dictionary<string, DocumentFileModel> dctLinks = new Dictionary<string, DocumentFileModel>();
		
				// Crea el diccionario con los vínculos 
					SearchDocumentLinks(dctLinks, this);
				// Transforma los vínculos
					TransformSearchLinks(dctLinks, this, strPathBase);
		}

		/// <summary>
		///		Obtiene las estructuras generadas y los documentos en que se han generado
		/// </summary>
		private void SearchDocumentLinks(Dictionary<string, DocumentFileModel> dctLinks, DocumentFileModelCollection objColDocuments)
		{ foreach (DocumentFileModel objDocument in objColDocuments)
				{ // Añade los nombres de las estructuras generadas en el documento
						foreach (StructDocumentationModel objStruct in objDocument.StructsReferenced)
							if (!dctLinks.ContainsKey(objStruct.Name))
								dctLinks.Add(objStruct.Name, objDocument);
					// Añade los nombres de las estructuras de los documentos hijo
						SearchDocumentLinks(dctLinks, objDocument.Childs);
				}
		}

		/// <summary>
		///		Transforma los vínculos de búsqueda utilizando el diccionario
		/// </summary>
		private void TransformSearchLinks(Dictionary<String, DocumentFileModel> dctLinks, DocumentFileModelCollection objColDocuments, string strPathBase)
		{ foreach (DocumentFileModel objDocument in objColDocuments)
				{ // Transforma los vínculos de búsqueda
						objDocument.TransformSearchLinks(dctLinks, strPathBase);
					// Transforma los documentos hijo
						TransformSearchLinks(dctLinks, objDocument.Childs, strPathBase);
				}
		}

		/// <summary>
		///		Busca el documento que se asocia a una estructura
		/// </summary>
		internal DocumentFileModel Search(StructDocumentationModel objStruct)
		{ DocumentFileModel objSearch = null;

				// Busca el elemento en la colección
					foreach (DocumentFileModel objDocument in this)
						if (objSearch == null)
							if (objDocument.LanguageStruct != null &&
									objDocument.LanguageStruct.Type.EqualsIgnoreCase(objStruct.Type) &&
									objDocument.LanguageStruct.Name.EqualsIgnoreCase(objStruct.Name) &&
									objDocument.Order == objStruct.Order)
								objSearch = objDocument;
							else
								objSearch = objDocument.Childs.Search(objStruct);
				// Devuelve el elemento localizado
					return objSearch;
		}

		/// <summary>
		///		Genera un documento a partir de una serie de documentos
		/// </summary>
		internal DocumentFileModel Compact(string strName)
		{ DocumentFileModel objDocument = new DocumentFileModel(null, 
																													  new StructDocumentationModel(null, StructDocumentationModel.ScopeType.Global, 
																																												 strName, "Index", 0), 
																														0);

				// Añade las estructuras de todos los documentos hijo
					foreach (DocumentFileModel objChild in this)
						objDocument.LanguageStruct.Childs.Add(objChild.LanguageStruct);
				// Devuelve el documento
					return objDocument;
		}
	}
}