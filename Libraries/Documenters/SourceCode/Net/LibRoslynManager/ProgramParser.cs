using System;

using Bau.Libraries.LibSourceCode.Models.CompilerSymbols;
using Bau.Libraries.LibRoslynManager.Models.Solutions;
using Bau.Libraries.LibNSharpDoc.Models.Structs;

namespace Bau.Libraries.LibRoslynManager
{
	/// <summary>
	///		Intérprete de un programa
	/// </summary>
	public class ProgramParser : LibNSharpDoc.Models.Interfaces.IDocumenter
	{
		/// <summary>
		///		Interpreta un archivo
		/// </summary>
		public StructDocumentationModel Parse(StructParameterModelDictionary objParameter)
		{	ProgramModel objProgram = ParseProgram(GetFileName(objParameter));

				return new LibSourceCode.Documenter.Common.StructSourceCodeConversor().Parse(objProgram);
		}

		/// <summary>
		///		Obtiene el nombre de archivo de los parámetros
		/// </summary>
		private string GetFileName(StructParameterModelDictionary objParameter)
		{ return objParameter.GetValue("FileName");
		}

		/// <summary>
		///		Interpreta un programa o un archivo
		/// </summary>
		private ProgramModel ParseProgram(string strFileName)
		{ SolutionVisualStudioModel objSolution = new SolutionVisualStudioModel(strFileName);
			ProgramModel objProgram = new ProgramModel(strFileName);
			Parser.CSharpParser objParser = new Parser.CSharpParser();

				// Carga la solución
					objSolution.Load();
				// Interpreta los proyectos
					foreach (ProjectVisualStudioModel objProject in objSolution.Projects)
						foreach (FileVisualStudioModel objFile in objProject.Files)
							if (objFile.FileName.EndsWith(".cs", StringComparison.CurrentCultureIgnoreCase))
								objProgram.CompilationUnits.Add(objParser.ParseFile(objFile.FullFileName));
				// Devuelve el programa interpretado
					return objProgram;
		}
	}
}