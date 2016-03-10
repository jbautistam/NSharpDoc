using System;

using Bau.Libraries.LibSourceCode.Models.CompilerSymbols;
using Bau.Libraries.LibNSharpDoc.Models.Structs;

namespace Bau.Libraries.LibVisualBasicParser
{
	/// <summary>
	///		Intérprete de un programa
	/// </summary>
	public class ProgramParser : LibNetParser.Common.AbstractProgramParser
	{
		/// <summary>
		///		Interpreta un archivo
		/// </summary>
		protected override CompilationUnitModel ParseFile(String strFileName)
		{ CompilationUnitModel objUnit = null;

				// Interpreta el archivo
					if (strFileName.EndsWith(".vb", StringComparison.CurrentCultureIgnoreCase))
						objUnit = new Parser.VisualBasicParser().ParseFile(strFileName);
				// Devuelve el archivo interpretado
					return objUnit;
		}
	}
}