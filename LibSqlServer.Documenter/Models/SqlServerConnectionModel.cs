using System;

namespace Bau.Libraries.LibSqlServer.Documenter.Models
{
	/// <summary>
	///		Datos de una conexión
	/// </summary>
	public class SqlServerConnectionModel
	{
		/// <summary>
		///		Servidor
		/// </summary>
		public string Server { get; set; }

		/// <summary>
		///		Base de datos
		/// </summary>
		public string DataBase { get; set; }

		/// <summary>
		///		Archivo de base de datos
		/// </summary>
		public string DataBaseFileName { get; set; }

		/// <summary>
		///		Usuario
		/// </summary>
		public string User { get; set; }

		/// <summary>
		///		Contraseña
		/// </summary>
		public string Password { get; set; }

		/// <summary>
		///		Indica si se debe utilizar la autentificación de Windows
		/// </summary>
		public bool UseWindowsAuthentification { get; set; }
	}
}
