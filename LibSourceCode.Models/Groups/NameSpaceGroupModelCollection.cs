using System;
using System.Linq;
using System.Collections.Generic;

using Bau.Libraries.LibHelper.Extensors;
using Bau.Libraries.LibSourceCode.Models.CompilerSymbols.Structs;

namespace Bau.Libraries.LibSourceCode.Models.Groups
{
	/// <summary>
	///		Colección de espacios de <see cref="NameSpaceGroupModel"/>
	/// </summary>
	public class NameSpaceGroupModelCollection : List<NameSpaceGroupModel>
	{
		/// <summary>
		///		Obtiene la cadena de depuración
		/// </summary>
		public string Debug(int intIndent)
		{ string strDebug = "";

				// Añade los nombres de los espacios de nombres
					foreach (NameSpaceGroupModel objNameSpace in this)
						{ // Añade el nombre del elemento
								strDebug += new string('\t', intIndent) + objNameSpace.Name;
							// Añade el nombre del espacio de nombres real (si existe)
								if (objNameSpace.NameSpace != null)
									strDebug += " --> " + objNameSpace.NameSpace.Name;
							// Añade los elementos
								if (objNameSpace.NameSpace == null)
									strDebug += " (NameSpace == null)";
								else if (objNameSpace.NameSpace.Items.Count == 0)
									strDebug += " (Sin elementos semánticos)";
								else
									{ strDebug += " (Elementos ";
										foreach (CompilerSymbols.Base.LanguageStructModel objStruct in objNameSpace.NameSpace.Items)
											strDebug += objStruct.Name + "; ";
										strDebug += ")";
									}
							// Añade el salto de línea
								strDebug += Environment.NewLine;
						}
				// Devuelve la cadena
					return strDebug;
		}

		/// <summary>
		///		Añade un espacio de nombres a la colección
		/// </summary>
		public void Add(NameSpaceModel objNameSpace)
		{	NameSpaceGroupModel objGroup = Search(objNameSpace.Name);

				if (objGroup == null)
					Add(new NameSpaceGroupModel(objNameSpace, objNameSpace.Name));
		}

		/// <summary>
		///		Busca un espacio de nombres
		/// </summary>
		public NameSpaceGroupModel Search(string strNameSpace)
		{ return this.FirstOrDefault(objGroup => objGroup.Name.EqualsIgnoreNull(strNameSpace));
		}
	}
}
