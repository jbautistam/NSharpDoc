using System;

namespace Bau.Libraries.LibDBSchema.DataSchema
{
	/// <summary>
	///		Clase con los datos de una vista
	/// </summary>
	public class SchemaView : SchemaItem
	{ 
		public SchemaView(Schema objParent) : base(objParent)
		{ Columns = new SchemaColumnsCollection(objParent);
		}
		
		/// <summary>
		///		SQL que define la vista
		/// </summary>
		public string Definition { get; set; }
		
		/// <summary>
		///		Opción check
		/// </summary>
		public string CheckOption { get; set; }
		
		/// <summary>
		///		Indica si es modificable
		/// </summary>
		public bool IsUpdatable { get; set; }
		
		/// <summary>
		///		Columnas
		/// </summary>
		public SchemaColumnsCollection Columns { get; set; }
	}
}
