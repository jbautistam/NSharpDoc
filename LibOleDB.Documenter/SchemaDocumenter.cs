using System;
using System.Data;
using System.Data.OleDb;

using Bau.Libraries.LibHelper.Extensors;
using Bau.Libraries.LibDBSchema;
using Bau.Libraries.LibDBSchemaProvider.Providers;
using Bau.Libraries.LibNSharpDoc.Models.Structs;

namespace Bau.Libraries.LibOleDB.Documenter
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
		{ Schema objSchema = new Schema();

				// Abre la conexión a OleDB
					using (OleDbConnection objConnection = new OleDbConnection(objParameters.GetValue("ConnectionString")))
						{ // Abre la conexión
								objConnection.Open();
							// Obtiene los datos
								LoadTables(objConnection, objSchema);
							// Cierra la conexión
								objConnection.Close();
						}
				// Devuelve la documentación del esquema
					return new LibDataBase.Documenter.Common.StructDataBaseConversor().Convert(objSchema);
		}

		/// <summary>
		///		Obtiene el esquema de tablas
		/// </summary>
		private void LoadTables(OleDbConnection objConnection, Schema objSchema)
		{ using (DataTable objDBTable = objConnection.GetSchema(""))
				{ 
				}
		}
	}
}
