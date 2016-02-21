using System;

namespace Bau.Libraries.LibDBProvidersBase.Providers.OleDB
{
	/// <summary>
	///		Cadena de conexión de OleDB
	/// </summary>
	public class OleDBConnectionString : IConnectionString
	{ 
		public OleDBConnectionString(string strConnectionString)
		{ ConnectionString = strConnectionString;
		}

		/// <summary>
		///		Cadena de conexión
		/// </summary>
		public string ConnectionString { get; set; }
	}
}