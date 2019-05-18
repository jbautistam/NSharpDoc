using System;
using System.Collections.Generic;

namespace Bau.Libraries.LibSourceCode.Models.CompilerSymbols
{
	/// <summary>
	///		Clase con los datos de una unidad de compilación
	/// </summary>
	public class CompilationUnitModel
	{
		public CompilationUnitModel(string fileName)
		{
			FileName = fileName;
			Root = new Base.LanguageStructModel(Base.LanguageStructModel.StructType.CompilationUnit, null, this);
		}

		/// <summary>
		///		Obtiene la cadena de depuración de una unidad de compilación
		/// </summary>
		public string Debug()
		{
			return "FileName: " + FileName + Environment.NewLine;
		}

		/// <summary>
		///		Obtiene los espacios de nombres de una unidad de compilación
		/// </summary>
		public List<Structs.NameSpaceModel> SearchNameSpaces()
		{
			List<Structs.NameSpaceModel> nameSpaces = new List<Structs.NameSpaceModel>();

				// Obtiene los espacios de nombres
				foreach (Base.LanguageStructModel structDoc in Root.Items)
					if (structDoc is Structs.NameSpaceModel)
						nameSpaces.Add(structDoc as Structs.NameSpaceModel);
				// Devuelve la colección
				return nameSpaces;
		}

		/// <summary>
		///		Obtiene los elementos que no tienen espacios de nombres de una unidad de compilación
		/// </summary>
		public Base.LanguageStructModelCollection SearchNoNameSpaces()
		{
			Base.LanguageStructModelCollection structsDoc = new Base.LanguageStructModelCollection();

				// Añade las estructuras que estén fuera del espacio de nombres a la colección
				foreach (Base.LanguageStructModel structDoc in Root.Items)
					if (structDoc != null && !(structDoc is Structs.NameSpaceModel))
						structsDoc.Add(structDoc);
				// Devuelve la estructura
				return structsDoc;
		}

		/// <summary>
		///		Nombre del archivo
		/// </summary>
		public string FileName { get; }

		/// <summary>
		///		Elemento principal
		/// </summary>
		public Base.LanguageStructModel Root { get; }

		/// <summary>
		///		Cláusulas Using
		/// </summary>
		public List<string> UsingClauses { get; } = new List<string>();
	}
}