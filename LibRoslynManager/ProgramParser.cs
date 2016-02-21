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
		{	if (strFileName.EndsWith(".sln", StringComparison.CurrentCultureIgnoreCase) ||
					strFileName.EndsWith(".prj", StringComparison.CurrentCultureIgnoreCase))
				return ParseSolution(strFileName);
			else
				return ParseText(LibHelper.Files.HelperFiles.LoadTextFile(strFileName));
		}

		/// <summary>
		///		Interpreta una solución
		/// </summary>
		private ProgramModel ParseSolution(string strFileName)
		{ SolutionVisualStudioModel objSolution = new SolutionVisualStudioModel(strFileName);
			ProgramModel objProgram = new ProgramModel(strFileName);
			Parser.FileParser objParser = new Parser.FileParser();

				// Carga la solución
					objSolution.Load();
				// Interpreta los proyectos
					foreach (ProjectVisualStudioModel objProject in objSolution.Projects)
						foreach (FileVisualStudioModel objFile in objProject.Files)
							objProgram.CompilationUnits.Add(objParser.ParseFile(objFile.FullFileName));
				// Devuelve el programa interpretado
					return objProgram;
		}

		/// <summary>
		///		Interpreta el contenido de un texto
		/// </summary>
		private ProgramModel ParseText(string strText)
		{ ProgramModel objProgram = new ProgramModel("SinNombreArchivo");
				
				// Interpreta el texto
					objProgram.CompilationUnits.Add(new Parser.FileParser().ParseText(strText));
				// Devuelve el programa
					return objProgram;
		}
	}
}