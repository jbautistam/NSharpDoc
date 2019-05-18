using System;
using System.Linq;
using System.Collections.Generic;

using Bau.Libraries.LibCommonHelper.Extensors;
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
		public string Debug(int indent)
		{
			string debug = "";

				// Añade los nombres de los espacios de nombres
				foreach (NameSpaceGroupModel nameSpace in this)
				{ 
					// Añade el nombre del elemento
					debug += new string('\t', indent) + nameSpace.Name;
					// Añade el nombre del espacio de nombres real (si existe)
					if (nameSpace.NameSpace != null)
						debug += " --> " + nameSpace.NameSpace.Name;
					// Añade los elementos
					if (nameSpace.NameSpace == null)
						debug += " (NameSpace == null)";
					else if (nameSpace.NameSpace.Items.Count == 0)
						debug += " (Sin elementos semánticos)";
					else
					{
						debug += " (Elementos ";
						foreach (CompilerSymbols.Base.LanguageStructModel structDoc in nameSpace.NameSpace.Items)
							debug += structDoc.Name + "; ";
						debug += ")";
					}
					// Añade el salto de línea
					debug += Environment.NewLine;
				}
				// Devuelve la cadena
				return debug;
		}

		/// <summary>
		///		Añade un espacio de nombres a la colección
		/// </summary>
		public void Add(NameSpaceModel nameSpace)
		{
			NameSpaceGroupModel group = Search(nameSpace.Name);

				if (group == null)
					Add(new NameSpaceGroupModel(nameSpace, nameSpace.Name));
		}

		/// <summary>
		///		Busca un espacio de nombres
		/// </summary>
		public NameSpaceGroupModel Search(string nameSpace)
		{
			return this.FirstOrDefault(group => group.Name.EqualsIgnoreNull(nameSpace));
		}
	}
}
