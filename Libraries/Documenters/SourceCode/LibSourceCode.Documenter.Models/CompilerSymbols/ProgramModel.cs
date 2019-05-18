using System;
using System.Collections.Generic;

namespace Bau.Libraries.LibSourceCode.Models.CompilerSymbols
{
	/// <summary>
	///		Clase con el modelo de documentación de un programa
	/// </summary>
	public class ProgramModel
	{
		public ProgramModel(string fileName)
		{
			FileName = fileName;
		}

		/// <summary>
		///		Obtiene la cadena de depuración
		/// </summary>
		public string Debug()
		{
			string debug = FileName + Environment.NewLine;

				// Añade las cadenas de depuración de las unidades de compilación
				foreach (CompilationUnitModel compilation in CompilationUnits)
					debug += compilation.Debug() + Environment.NewLine;
				// Devuelve la cadena de depuración
				return debug;
		}

		/// <summary>
		///		Nombre del archivo 
		/// </summary>
		public string FileName { get; }

		/// <summary>
		///		Elementos de la aplicación
		/// </summary>
		public List<CompilationUnitModel> CompilationUnits { get; } = new List<CompilationUnitModel>();

		/// <summary>
		///		Errores
		/// </summary>
		public List<string> Errors { get; } = new List<string>();
	}
}
