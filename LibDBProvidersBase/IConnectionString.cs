using System;

namespace Bau.Libraries.LibDBProvidersBase
{
	/// <summary>
	///		Interface para los datos de conexión de un proveedor
	/// </summary>
	public interface IConnectionString
	{
		/// <summary>
		///		Compone la cadena de conexión a partir de los parámetros
		/// </summary>
		string ConnectionString { get; set; }
	}
}
