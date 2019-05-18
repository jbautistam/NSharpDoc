using System;

namespace Bau.Libraries.LibDataBaseDocumenter.Providers
{
	/// <summary>
	///		Proveedor de base de datos
	/// </summary>
	internal abstract class BaseSchemaProvider
	{
		protected BaseSchemaProvider(LibNSharpDoc.Models.Structs.StructParameterModelDictionary parameters)
		{
			Parameters = parameters;
		}

		/// <summary>
		///		Obtiene el valor de un parámetro
		/// </summary>
		protected string GetParameter(string key)
		{
			return Parameters.GetValue(key);
		}

		/// <summary>
		///		Obtiene el esquema de la base de datos
		/// </summary>
		internal abstract LibDbProviders.Base.Schema.SchemaDbModel GetSchema();

		/// <summary>
		///		Parámetros de acceso a la base de datos
		/// </summary>
		protected LibNSharpDoc.Models.Structs.StructParameterModelDictionary Parameters { get; }

		/// <summary>
		///		Base de datos
		/// </summary>
		internal string DataBase { get ; set; }

		/// <summary>
		///		Servidor
		/// </summary>
		internal string Server { get; set; }
	}
}
