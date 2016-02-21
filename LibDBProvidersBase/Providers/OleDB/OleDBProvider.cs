using System;
using System.Data;
using System.Data.OleDb;

using Bau.Libraries.LibDBProvidersBase.Parameters;

namespace Bau.Libraries.LibDBProvidersBase.Providers.OleDB
{
	/// <summary>
	///		Proveedor para OleDB
	/// </summary>
	public class OleDBProvider : DBProviderBase
	{ 
		public OleDBProvider(OleDBConnectionString objConnectionString) : this(new OleDbConnection(objConnectionString.ConnectionString)) {}

		public OleDBProvider(IDbConnection objConnection) : base(objConnection) {}

		/// <summary>
		///		Obtiene un comando
		/// </summary>
		protected override IDbCommand GetCommand(string strText)
		{ return new OleDbCommand(strText, Connection as OleDbConnection);
		}

		/// <summary>
		///		Obtiene un dataTable
		/// </summary>
		protected override DataTable FillDataTable(IDbCommand objCommand)
		{	DataTable dtTable = new DataTable();
		
				// Rellena la tabla con los datos
					using (OleDbDataAdapter objAdapter = new OleDbDataAdapter(objCommand as OleDbCommand))
						objAdapter.Fill(dtTable);
				// Devuelve la tabla
					return dtTable;
		}

		/// <summary>
		///		Ejecuta un INSERT sobre la base de datos y devuelve el ID de IDENTITY
		/// </summary>
		public override int? ExecuteGetIdentity(string strText, ParametersDBCollection objColParametersDB, CommandType intCommandType)
		{ throw new NotImplementedException();
		}

		/// <summary>
		///		Convierte un parámetro
		/// </summary>
		protected override IDataParameter ConvertParameter(ParameterDB objParameter)
		{	if (objParameter.Direction == ParameterDirection.ReturnValue)
				return new OleDbParameter(objParameter.Name, OleDbType.Integer);
			else if (objParameter.IsText)
				return new OleDbParameter(objParameter.Name, OleDbType.VarChar);
			else if (objParameter.Value is bool)
				return new OleDbParameter(objParameter.Name, OleDbType.Boolean);
			else if (objParameter.Value is int)
				return new OleDbParameter(objParameter.Name, OleDbType.Integer);
			else if (objParameter.Value is double)
				return new OleDbParameter(objParameter.Name, OleDbType.Double);
			else if (objParameter.Value is string)
				return new OleDbParameter(objParameter.Name, OleDbType.VarChar, objParameter.Length);
			else if (objParameter.Value is byte [])
				return new OleDbParameter(objParameter.Name, OleDbType.Binary);
			else if (objParameter.Value is DateTime)
				return new OleDbParameter(objParameter.Name, OleDbType.Date);
			else
				throw new NotSupportedException("Tipo del parámetro " + objParameter.Name + "desconocido");
		}
	}
}