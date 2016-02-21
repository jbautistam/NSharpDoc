using System;

using Bau.Libraries.LibDBSchema.DataSchema;

namespace Bau.Libraries.LibDBSchema
{
	/// <summary>
	///		Clase que mantiene los datos de un esquema
	/// </summary>
	public class Schema
	{ 
		public Schema()
		{ Tables = new SchemaTablesCollection(this);
			Triggers = new SchemaTriggersCollection(this);
			Views = new SchemaViewsCollection(this);
			Routines = new SchemaRoutinesCollection(this);
		}
		
		/// <summary>
		///		Limpia los esquemas
		/// </summary>
		public void Clear()
		{ Tables.Clear();
			Triggers.Clear();
			Views.Clear();
			Routines.Clear();
		}
		
		/// <summary>
		///		Servidor
		/// </summary>
		public string Server { get; set; }

		/// <summary>
		///		Base de datos
		/// </summary>
		public string DataBase { get; set; }

		/// <summary>
		///		Tablas
		/// </summary>
		public SchemaTablesCollection Tables { get; private set; }
		
		/// <summary>
		///		Desencadenadores
		/// </summary>
		public SchemaTriggersCollection Triggers { get; private set; }
		
		/// <summary>
		///		Vistas
		/// </summary>
		public SchemaViewsCollection Views { get; private set; }

		/// <summary>
		///		Rutinas
		/// </summary>
		public SchemaRoutinesCollection Routines { get; private set; }
	}
}
