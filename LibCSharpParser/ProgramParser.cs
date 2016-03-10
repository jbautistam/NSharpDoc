using System;

using Bau.Libraries.LibSourceCode.Models.CompilerSymbols;

namespace Bau.Libraries.LibCSharpParser
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
		{ CompilationUnitModel objCompilationUnit = null;

				// Interpreta el archivo
					if (strFileName.EndsWith(".cs", StringComparison.CurrentCultureIgnoreCase))
						objCompilationUnit = new Parser.CSharpParser().ParseFile(strFileName);
				// Devuelve la unidad de compilación
					return objCompilationUnit;
		}
	}
}