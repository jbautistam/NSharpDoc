using System;

using Bau.Libraries.LibDbProviders.Odbc;
using Bau.Libraries.LibDbProviders.Base.Schema;

namespace Bau.Libraries.LibDataBaseDocumenter.Providers
{
	/// <summary>
	///		Proveedor de base de datos para Odbc
	/// </summary>
	internal class OdbcSchemaProvider : BaseSchemaProvider
	{
		internal OdbcSchemaProvider(LibNSharpDoc.Models.Structs.StructParameterModelDictionary parameters) : base(parameters) {}

		/// <summary>
		///		Obtiene el esquema de base de datos
		/// </summary>
		internal override SchemaDbModel GetSchema()
		{
			string connectionString = GetParameter("ConnectionString");

				if (string.IsNullOrWhiteSpace(connectionString))
					throw new Exception("Can't find then connectionstring at parameters");
				else
					return new OdbcProvider(new OdbcConnectionString(connectionString)).GetSchema();
		}
	}
}
