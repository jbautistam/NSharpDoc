using System;
using System.Collections.Generic;

namespace Bau.Libraries.LibSourceCode.Models.CompilerSymbols
{
	/// <summary>
	///		Clase con el moduelo de doucmentación de una solución (grupo de <see cref="ProgramModel"/>)
	/// </summary>
	public class SolutionModel
	{
		public SolutionModel(string fileName)
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
				foreach (ProgramModel program in Programs)
					debug += program.Debug() + Environment.NewLine;
				// Devuelve la cadena de depuración
				return debug;
		}

		/// <summary>
		///		Nombre del archivo de solución
		/// </summary>
		public string FileName { get; }

		/// <summary>
		///		Programas asociados a la solución
		/// </summary>
		public List<ProgramModel> Programs { get; } = new List<ProgramModel>();
	}
}
