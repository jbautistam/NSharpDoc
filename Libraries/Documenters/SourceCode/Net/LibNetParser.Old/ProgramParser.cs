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
			ProgramModel program = ParseProgram(GetFileName(parameter));

				return new LibSourceCode.Documenter.Common.StructSourceCodeConversor().Parse(program);
		}

		/// <summary>
		///		Obtiene el nombre de archivo de los parámetros
		/// </summary>
		private string GetFileName(StructParameterModelDictionary parameter)
		{
			return parameter.GetValue("FileName");
		}

		/// <summary>
		///		Interpreta un programa o un archivo
		/// </summary>
		private ProgramModel ParseProgram(string fileName)
		{
			SolutionVisualStudioModel solution = new SolutionVisualStudioModel(fileName);
			ProgramModel program = new ProgramModel(fileName);

				////? En depuración me salta un error al llamar a ParseFile que indica que "No se puede cargar el ensamblado 'System.Reflection.Metadata'" aunque
				////? la referencia existe en el proyecto. Para obligar a que cargue el ensamblado, simplemente asigno un valor:
				//	System.Reflection.Metadata.PrimitiveTypeCode intID = System.Reflection.Metadata.PrimitiveTypeCode.Boolean;
				////TODO --> Esto habría que borrarlo cuando se solucione el error (posiblemente en la próxima versión de Roslyn
				////? fin del comentario para cargar el ensamblado
				// Carga la solución
				solution.Load();
				// Interpreta los proyectos
				foreach (ProjectVisualStudioModel project in solution.Projects)
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
				// Devuelve el programa interpretado
				return program;
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
