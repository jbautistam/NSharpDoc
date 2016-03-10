using System;

using Bau.Libraries.LibSourceCode.Models.CompilerSymbols;
using Bau.Libraries.LibNSharpDoc.Models.Structs;
using Bau.Libraries.LibNetParser.Common.LibCSharpParser.Models.Solutions;

namespace Bau.Libraries.LibNetParser.Common
{
	/// <summary>
	///		Intérprete de programas
	/// </summary>
	public abstract class AbstractProgramParser : LibNSharpDoc.Models.Interfaces.IDocumenter
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

				// Carga la solución
					objSolution.Load();
				// Interpreta los proyectos
					foreach (ProjectVisualStudioModel objProject in objSolution.Projects)
						foreach (FileVisualStudioModel objFile in objProject.Files)
							if (!System.IO.File.Exists(objFile.FullFileName))
								objProgram.Errors.Add("No se encuentra el archivo " + objFile.FullFileName);
							else
								{ CompilationUnitModel objUnit = ParseFile(objFile.FullFileName);

										if (objUnit != null)
											objProgram.CompilationUnits.Add(objUnit);
								}
				// Devuelve el programa interpretado
					return objProgram;
		}

		/// <summary>
		///		Interpreta un archivo de texto
		/// </summary>
		protected abstract CompilationUnitModel ParseFile(string strFileName);
	}
}
