using System;
using System.Collections.Generic;

namespace Bau.Libraries.LibSourceCode.Models.CompilerSymbols
{
	/// <summary>
	///		Clase con el modelo de documentación de un programa
	/// </summary>
	public class ProgramModel
	{
		public ProgramModel(string strFileName)
		{ FileName = strFileName;
		}

		/// <summary>
		///		Obtiene la cadena de depuración
		/// </summary>
		public string Debug()
		{ string strDebug = FileName + Environment.NewLine;

				// Añade las cadenas de depuración de las unidades de compilación
					foreach (CompilationUnitModel objCompilation in CompilationUnits)
						strDebug += objCompilation.Debug() + Environment.NewLine;
				// Devuelve la cadena de depuración
					return strDebug;
		}

		/// <summary>
		///		Nombre del archivo 
		/// </summary>
		public string FileName { get; }

		/// <summary>
		///		Elementos de la aplicación
		/// </summary>
		public List<CompilationUnitModel> CompilationUnits { get; } = new List<CompilationUnitModel>();
	}
}
