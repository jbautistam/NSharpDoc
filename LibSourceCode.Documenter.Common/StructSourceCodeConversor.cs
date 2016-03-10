using System;

using Bau.Libraries.LibSourceCode.Models.CompilerSymbols;
using Bau.Libraries.LibSourceCode.Models.Groups;
using Bau.Libraries.LibNSharpDoc.Models.Structs;

namespace Bau.Libraries.LibSourceCode.Documenter.Common
{
	/// <summary>
	///		Manager para la generación de estructuras de documentación a partir de una estructura de programa
	/// </summary>
	public class StructSourceCodeConversor
	{
		/// <summary>
		///		Interpreta un archivo
		/// </summary>
		public StructDocumentationModel Parse(ProgramModel objProgram)
		{ StructDocumentationModel objStruct = new StructDocumentationModel(null, StructDocumentationModel.ScopeType.Global,
																																				System.IO.Path.GetFileNameWithoutExtension(objProgram.FileName),
																																				"Program", 0);

				// Añade las estructuras de documentación
					objStruct.Childs.AddRange(Generate(objProgram));
				// Devuelve las estructuras de documentación
					return objStruct;
		}

		/// <summary>
		///		Genera las estructuras de un programa
		/// </summary>
		private StructDocumentationModelCollection Generate(ProgramModel objProgram)
		{	NameSpaceGroupModelCollection objColGroups = new Prepare.NameSpaceGroupGenerator().Generate(objProgram);
			StructDocumentationModelCollection objColStructs = new Prepare.StructConversor().Convert(objColGroups);

				// Cambia las referencias y los vínculos
					new Prepare.StructReferencesConversor().ConvertReferences(objColStructs);
				// Devuelve las estructuras de documentación
					return objColStructs;
		}
	}
}
