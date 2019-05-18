using System;

using Bau.Libraries.LibCommonHelper.Extensors;
using Bau.Libraries.LibDbProviders.Base.Schema;
using Bau.Libraries.LibDbProviders.SqlServer;

namespace Bau.Libraries.LibDataBaseDocumenter.Providers
{
	/// <summary>
	///		Proveedor de base de datos para SqlServer
	/// </summary>
	internal class SqlServerSchemaProvider : BaseSchemaProvider
	{
		internal SqlServerSchemaProvider(LibNSharpDoc.Models.Structs.StructParameterModelDictionary parameters) : base(parameters) {}

		/// <summary>
		///		Obtiene el esquema de base de datos
		/// </summary>
		internal override SchemaDbModel GetSchema()
		{
			string user, password;
			bool integratedSecurity;

				// Obtiene los parámetros
				DataBase = GetParameter("Database");
				Server = GetParameter("Server");
				user = GetParameter("User");
				password = GetParameter("Password");
				integratedSecurity = GetParameter("UseIntegratedSecurity").GetBool();
				// Carga el esquema
				if (string.IsNullOrWhiteSpace(DataBase) || string.IsNullOrWhiteSpace(Server) || string.IsNullOrWhiteSpace(user))
					throw new NotImplementedException("Can't find all parameters when connect to SqlServer");
				else
					return new SqlServerProvider(new SqlServerConnectionString(Server, user, password, DataBase, integratedSecurity)).GetSchema();
		}
	}
}
