using System;
using System.Data;
using System.Data.SqlClient;

using Bau.Libraries.LibDBProvidersBase.Parameters;

namespace Bau.Libraries.LibDBProvidersBase.Providers.SQLServer
{
	/// <summary>
	///		Proveedor para SQL Server
	/// </summary>
	public class SQLServerProvider : DBProviderBase
	{ 
		public SQLServerProvider(SQLServerConnectionString objConnectionString) : this(new SqlConnection(objConnectionString.ConnectionString)) {}

		public SQLServerProvider(IDbConnection objConnection) : base(objConnection) {}

		/// <summary>
		///		Obtiene un comando
		/// </summary>
		protected override IDbCommand GetCommand(string strText)
		{ return new SqlCommand(strText, Connection as SqlConnection);
		}

		/// <summary>
		///		Ejecuta un INSERT sobre la base de datos y obtiene el valor de identidad
		/// </summary>
		public override int? ExecuteGetIdentity(string strText, ParametersDBCollection objColParametersDB, CommandType intCommandType = CommandType.Text)
		{ int? intIdentity = null;

				// Abre la conexión
					Open();
				// Ejecuta sobre la conexión
					switch (intCommandType)
						{ case CommandType.Text:
									intIdentity = (int?) ((decimal?) ExecuteScalar(NormalizeSqlInsert(strText), objColParametersDB, intCommandType));
								break;
							case CommandType.StoredProcedure:
									Execute(strText, objColParametersDB, intCommandType);
									intIdentity = (int?) objColParametersDB["@return_code"].Value;
								break;
							default:
									Execute(strText, objColParametersDB, intCommandType);
									intIdentity = null;
								break;
						}
				// Cierra la conexión
					Close();
				// Devuelve el valor identidad
					return intIdentity;
		}

		/// <summary>
		///		Normaliza una cadena SQL de inserción de datos
		/// </summary>
		private string NormalizeSqlInsert(string strSqlInsert)
		{ // Añade a la cadena de inserción una consulta para obtener el SCOPE_IDENTITY
				if (strSqlInsert.IndexOf("Scope_Identity()", StringComparison.CurrentCultureIgnoreCase) < 0)
					strSqlInsert += "; SELECT SCOPE_IDENTITY()";
			// Devuelve la cadena de inserción
				return strSqlInsert;
		}

		/// <summary>
		///		Obtiene un dataTable
		/// </summary>
		protected override DataTable FillDataTable(IDbCommand objCommand)
		{	DataTable dtTable = new DataTable();
		
				// Rellena la tabla con los datos
					using (SqlDataAdapter objAdapter = new SqlDataAdapter(objCommand as SqlCommand))
						objAdapter.Fill(dtTable);
				// Devuelve la tabla
					return dtTable;
		}

		/// <summary>
		///		Convierte un parámetro
		/// </summary>
		protected override IDataParameter ConvertParameter(ParameterDB objParameter)
		{	if (objParameter.Direction == ParameterDirection.ReturnValue)
				return new SqlParameter(objParameter.Name, SqlDbType.Int);
			else if (objParameter.Value == null)
				return new SqlParameter(objParameter.Name, null);
			else if (objParameter.IsText)
				return new SqlParameter(objParameter.Name, SqlDbType.Text);
			else if (objParameter.Value is bool?)
				return new SqlParameter(objParameter.Name, SqlDbType.Bit);
			else if (objParameter.Value is int?)
				return new SqlParameter(objParameter.Name, SqlDbType.Int);
			else if (objParameter.Value is double?)
				return new SqlParameter(objParameter.Name, SqlDbType.Float);
			else if (objParameter.Value is string)
				return new SqlParameter(objParameter.Name, SqlDbType.VarChar, objParameter.Length);
			else if (objParameter.Value is byte [])
				return new SqlParameter(objParameter.Name, SqlDbType.Image);
			else if (objParameter.Value is DateTime?)
				return  new SqlParameter(objParameter.Name, SqlDbType.DateTime);
			else
				throw new NotSupportedException("Tipo del parámetro " + objParameter.Name + "desconocido");
		}
	}
}