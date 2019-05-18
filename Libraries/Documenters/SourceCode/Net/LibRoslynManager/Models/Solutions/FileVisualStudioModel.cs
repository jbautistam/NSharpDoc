using System;

namespace Bau.Libraries.LibRoslynManager.Models.Solutions
{
	/// <summary>
	///		Archivo asociado a un proyecto
	/// </summary>
	public class FileVisualStudioModel
	{
		/// <summary>
		///		Nombre del archivo
		/// </summary>
		public string FileName { get; internal set; }

		/// <summary>
		///		Nombre completo del archivo
		/// </summary>
		public string FullFileName { get; internal set; }
	}
}
