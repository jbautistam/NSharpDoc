using System;
using System.Collections.Generic;
using System.Data;

namespace Bau.Libraries.LibDBProvidersBase.RepositoryData
{	// Delegados públicos de este espacio de nombres
		public delegate object AssignDataCallBack(IDataReader rdoData);

	/// <summary>
	///		Interface para los objetos de repositorio
	/// </summary>
	public interface IProviderRepository : IDisposable
	{ 
		/// <summary>
		///		Obtiene una instancia nueva del objeto
		/// </summary>
		IProviderRepository GetInstance();

		/// <summary>
		///		Carga los datos de una colección
		/// </summary>
		void LoadCollection<TypeData>(IList<TypeData> objColData, string strText, CommandType intCommandType, AssignDataCallBack fncCallBack) 
											where TypeData : new();

		/// <summary>
		///		Carga una colección utilizando genéricos para un procedimiento con un único 
		/// parámetro de entrada alfanumérico
		/// </summary>
		void LoadCollection<TypeData>(IList<TypeData> objColData, string strText, string strParameterName, 
																	string strParameterValue, int intParameterLength, 
																	CommandType intCommandType, AssignDataCallBack fncCallBack) 
											where TypeData : new();
	
		/// <summary>
		///		Carga una colección utilizando genéricos para un procedimiento con un único 
		/// parámetro de entrada numérico
		/// </summary>
		void LoadCollection<TypeData>(IList<TypeData> objColData, string strText, string strParameterName, 
																	int? intParameterValue, CommandType intCommandType, AssignDataCallBack fncCallBack) 
											where TypeData : new();
		
		/// <summary>
		///		Carga una colección utilizando genéricos
		/// </summary>
		void LoadCollection<TypeData>(IList<TypeData> objColData, string strText, Parameters.ParametersDBCollection objColParameters,
																	CommandType intCommandType, AssignDataCallBack fncCallBack)  
											where TypeData : new();
		
		/// <summary>
		///		Carga un objeto utilizando genéricos para un procedimiento con un único parámetro alfanumérico
		/// </summary>
		TypeData LoadObject<TypeData>(string strText, string strParameterName, string strParameterValue, int intParameterLength, 
																	CommandType intCommandType, AssignDataCallBack fncCallBack) where TypeData : new();
		
		/// <summary>
		///		Carga un objeto utilizando genéricos para un procedimiento con un único parámetro numérico
		/// </summary>
		TypeData LoadObject<TypeData>(string strText, string strParameterName, int? intParameterValue,
																	CommandType intCommandType, AssignDataCallBack fncCallBack) where TypeData : new();
		
		/// <summary>
		///		Carga un objeto utilizando genéricos
		/// </summary>
		TypeData LoadObject<TypeData>(string strText, Parameters.ParametersDBCollection objColParametersDB, 
																	CommandType intCommandType, AssignDataCallBack fncCallBack) where TypeData : new();
		
		/// <summary>
		///		Ejecuta una sentencia sobre la conexión
		/// </summary>
		void Execute(string strText, string strParameterName, string strParameterValue, int intParameterLength, CommandType intCommandType);
		
		/// <summary>
		///		Ejecuta una sentencia sobre la conexión
		/// </summary>
		void Execute(string strText, string strParameterName, int? intParameterValue, CommandType intCommandType);
		
		/// <summary>
		///		Ejecuta una sentencia sobre la conexión
		/// </summary>
		void Execute(string strText, Parameters.ParametersDBCollection objColParametersDB, CommandType intCommandType);
		
		/// <summary>
		///		Ejecuta una sentencia sobre la conexión y devuelve un escalar
		/// </summary>
		object ExecuteScalar(string strText, string strParameterName, string strParameterValue, int intParameterLength, CommandType intCommandType);
		
		/// <summary>
		///		Ejecuta una sentencia sobre la conexión y devuelve un escalar
		/// </summary>
		object ExecuteScalar(string strText, string strParameterName, int? intParameterValue, CommandType intCommandType);
		
		/// <summary>
		///		Ejecuta una sentencia sobre la conexión y devuelve un escalar
		/// </summary>
		object ExecuteScalar(string strText, Parameters.ParametersDBCollection objColParametersDB, CommandType intCommandType);		

		/// <summary>
		///		Conexión con la que trabaja el repository
		/// </summary>
		IDBProvider Provider { get; }
	}
}