using System;

namespace Bau.Libraries.LibMarkupLanguage.Services
{
	/// <summary>
	///		Interface para el generador de un archivo o texto de un MLFile
	/// </summary>
	public interface IWriter
	{
		/// <summary>
		///		Graba los datos en un archivo
		/// </summary>
		void Save(MLFile objMLFile, string strFileName);

		/// <summary>
		///		Convierte los datos en una cadena
		/// </summary>
		string ConverToString(MLFile objMLFile, bool blnAddHeader = true);
	}
}
