using System;

using Bau.Libraries.LibSourceCode.Models.CompilerSymbols;
using Bau.Libraries.LibNSharpDoc.Models.Structs;
using Bau.Libraries.LibNetParser.Models.Solutions;

namespace Bau.Libraries.LibNetParser
{
	/// <summary>
	///		Intérprete de programas
	/// </summary>
	public class ProgramParser : LibNSharpDoc.Models.Interfaces.IDocumenter
	{
		/// <summary>
		///		Interpreta un archivo
		/// </summary>
		public StructDocumentationModel Parse(StructParameterModelDictionary parameter)
		{
			SolutionModel solution = ParseSolution(GetFileName(parameter));

				return new LibSourceCode.Documenter.Common.StructSourceCodeConversor().Parse(solution);
		}

		/// <summary>
		///		Obtiene el nombre de archivo de los parámetros
		/// </summary>
		private string GetFileName(StructParameterModelDictionary parameter)
		{
			return parameter.GetValue("FileName");
		}

		/// <summary>
		///		Interpreta una solución
		/// </summary>
		private SolutionModel ParseSolution(string fileName)
		{
			SolutionVisualStudioModel solutionVisual = new SolutionVisualStudioModel(fileName);
			SolutionModel solution = new SolutionModel(fileName);

				// Carga la solución
				solutionVisual.Load();
				// Interpreta los proyectos
				foreach (ProjectVisualStudioModel project in solutionVisual.Projects)
					if (project.Files.Count > 0) // ... los proyectos sin archivos se corresponden a las carpetas de solución
					{
						ProgramModel program = new ProgramModel(project.FullFileNameSource);

							// Interpreta los archivos del proyecto
							foreach (FileVisualStudioModel file in project.Files)
								if (!System.IO.File.Exists(file.FullFileName))
									program.Errors.Add($"No se encuentra el archivo {file.FullFileName}");
								else if (!file.FullFileName.EndsWith(".vb", StringComparison.CurrentCultureIgnoreCase) ||
										 file.FullFileName.EndsWith(".cs", StringComparison.CurrentCultureIgnoreCase))
								{
									CompilationUnitModel unit = ParseFile(file.FullFileName);

										if (unit != null)
											program.CompilationUnits.Add(unit);
								}
							// Añade el proyecto a la solución
							solution.Programs.Add(program);
					}
				// Devuelve el programa interpretado
				return solution;
		}

		/// <summary>
		///		Interpreta un archivo de texto
		/// </summary>
		private CompilationUnitModel ParseFile(string fileName)
		{
			string text = LibCommonHelper.Files.HelperFiles.LoadTextFile(fileName);

				// Interpreta el archivo
				if (fileName.EndsWith(".vb", StringComparison.CurrentCultureIgnoreCase))
					return new Parser.VisualBasicParser().ParseText(fileName, text);
				else if (fileName.EndsWith(".cs", StringComparison.CurrentCultureIgnoreCase))
					return new Parser.CSharpParser().ParseText(fileName, text);
				else
					return null;
		}
	}
}
