using System;

using Bau.Libraries.LibSourceCode.Models.CompilerSymbols.Structs;

namespace Bau.Libraries.LibSourceCode.Models.Groups
{
	/// <summary>
	///		Clase con los datos de un elemento documentado
	/// </summary>
	public class NameSpaceGroupModel
	{
		public NameSpaceGroupModel(NameSpaceModel nameSpace, string name)
		{
			NameSpace = nameSpace;
			Name = name;
		}

		/// <summary>
		///		Nombre del espacio de nombres
		/// </summary>
		public string Name { get; private set; }

		/// <summary>
		///		Espacio de nombres asociado
		/// </summary>
		public NameSpaceModel NameSpace { get; set; }
	}
}
