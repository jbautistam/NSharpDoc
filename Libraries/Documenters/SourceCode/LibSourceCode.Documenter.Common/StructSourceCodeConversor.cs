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
		///		Interpreta una solución
		/// </summary>
		public StructDocumentationModel Parse(SolutionModel solution)
		{
			StructDocumentationModel structDoc = new StructDocumentationModel(null, StructDocumentationModel.ScopeType.Global,
																			  System.IO.Path.GetFileNameWithoutExtension(solution.FileName),
																			  "Solution", 0);

				// Añade las estructuras de los programas
				foreach (ProgramModel program in solution.Programs)
					structDoc.Childs.Add(Parse(program));
				// Devuelve las estructuras de documentación
				return structDoc;
		}

		/// <summary>
		///		Interpreta un programa
		/// </summary>
		public StructDocumentationModel Parse(ProgramModel program)
		{
			StructDocumentationModel structDoc = new StructDocumentationModel(null, StructDocumentationModel.ScopeType.Global,
																			  System.IO.Path.GetFileNameWithoutExtension(program.FileName),
																			  "Program", 0);

				// Añade las estructuras de documentación
				structDoc.Childs.AddRange(Generate(program));
				// Devuelve las estructuras de documentación
				return structDoc;
		}

		/// <summary>
		///		Genera las estructuras de un programa
		/// </summary>
		private StructDocumentationModelCollection Generate(ProgramModel program)
		{
			NameSpaceGroupModelCollection groups = new Prepare.NameSpaceGroupGenerator().Generate(program);
			StructDocumentationModelCollection structsDoc = new Prepare.StructConversor().Convert(groups);

				// Cambia las referencias y los vínculos
				new Prepare.StructReferencesConversor().ConvertReferences(structsDoc);
				// Devuelve las estructuras de documentación
				return structsDoc;
		}
	}
}
