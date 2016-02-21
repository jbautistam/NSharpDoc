using System;

using Bau.Libraries.LibHelper.Extensors;
using Bau.Libraries.LibDBProvidersBase.Providers.SQLServer;
using Bau.Libraries.LibDBSchema;
using Bau.Libraries.LibDBSchemaProvider.Providers;
using Bau.Libraries.LibNSharpDoc.Models.Structs;

namespace Bau.Libraries.LibSqlServer.Documenter
{
	/// <summary>
	///		Compilador de esquemas de base de datos
	/// </summary>
	public class SchemaDocumenter : LibNSharpDoc.Models.Interfaces.IDocumenter
	{	
		/// <summary>
		///		Interpreta un esquema y genera las estructuras de documentación
		/// </summary>
		public StructDocumentationModel Parse(StructParameterModelDictionary objParameters)
		{ Models.SqlServerConnectionModel objConnection = new Models.SqlServerConnectionModel();

				// Obtiene los parámetros de la conexión
					objConnection.Server = objParameters.GetValue("Server");
					objConnection.DataBase = objParameters.GetValue("DataBase");
					objConnection.User = objParameters.GetValue("User");
					objConnection.Password = objParameters.GetValue("Password");
				// Compila la documentación de la conexión
					return new LibDataBase.Documenter.Common.StructDataBaseConversor().Convert(LoadSchema(objConnection));
		}

		/// <summary>
		///		Compila un esquema de base de datos
		/// </summary>
		private Schema LoadSchema(Models.SqlServerConnectionModel objConnection)
		{ SQLServerConnectionString objConnectionString = new SQLServerConnectionString(objConnection.Server, objConnection.User, 
																																										objConnection.Password, objConnection.DataBase);
			Schema objSchema;

				// Asigna el resto de las propiedades
					if (objConnection.DataBaseFileName.IsEmpty())
						objConnectionString.Type = SQLServerConnectionString.ConnectionType.Normal;
					else
						{ objConnectionString.DataBaseFile = objConnection.DataBaseFileName;
							objConnectionString.Type = SQLServerConnectionString.ConnectionType.File;
						}
					objConnectionString.UseIntegratedSecurity = objConnection.UseWindowsAuthentification;
				// Carga el esquema y le asigna los datos
					objSchema = new SchemaSqlServerProvider().LoadSchema(objConnectionString);
					objSchema.Server = objConnection.Server;
					objSchema.DataBase = objConnection.DataBase;
				// Devuelve el esquema
					return objSchema;
		}
	}
}
