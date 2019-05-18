using System;

using Bau.Libraries.LibNSharpDoc.Models.Structs;

namespace Bau.Libraries.LibDataBaseDocumenter.Providers
{
	/// <summary>
	///		Factory para selección de un proveedor de base de datos
	/// </summary>
	internal class ProviderFactory
	{
		/// <summary>
		///		Obtiene el proveedor de base de daots adecuado
		/// </summary>
		internal BaseSchemaProvider GetInstance(StructDataBaseConversor.DataBaseType type, StructParameterModelDictionary parameters)
		{
			switch (type)
			{
				case StructDataBaseConversor.DataBaseType.Odbc:
					return new OdbcSchemaProvider(parameters);
				case StructDataBaseConversor.DataBaseType.SqlServer:
					return new SqlServerSchemaProvider(parameters);
				default:
					throw new NotImplementedException($"Database provider {type} unknown");
			}
		}
	}
}
