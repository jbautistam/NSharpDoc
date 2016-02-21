using System;
using System.Collections.Generic;

namespace Bau.Libraries.LibNSharpDoc.Models.Structs
{
	/// <summary>
	///		Colección de <see cref="StructDocumentationModel"/>
	/// </summary>
	public class StructDocumentationModelCollection : List<StructDocumentationModel>
	{
		/// <summary>
		///		Añade una estructura a la lista
		/// </summary>
		public StructDocumentationModel Add(StructDocumentationModel objParent, StructDocumentationModel.ScopeType intScope, 
																				string strName, string strType, int intOrder)
		{ StructDocumentationModel objStruct = new StructDocumentationModel(objParent, intScope, strName, strType, intOrder);

				// Añade la estructura
					Add(objStruct);
				// Devuelve la estructura añadida
					return objStruct;
		}

		/// <summary>
		///		Depura la colección
		/// </summary>
		public string Debug(int intIndent = 0)
		{ string strDebug = "";

				// Obtiene la cadena de depuración de las estructuras
					foreach (StructDocumentationModel objStruct in this)
						 strDebug += objStruct.Debug(intIndent);
				// Devuelve la cadena de depuración
					return strDebug;
		}

		/// <summary>
		///		Ordena las estructuras por nombre
		/// </summary>
		public void SortByName()
		{ Sort((objFirst, objSecond) => objFirst.Name.CompareTo(objSecond.Name));
		}
	}
}
