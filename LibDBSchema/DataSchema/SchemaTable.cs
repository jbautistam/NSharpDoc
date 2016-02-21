using System;

namespace Bau.Libraries.LibDBSchema.DataSchema
{
	/// <summary>
	///		Clase con los datos de una tabla
	/// </summary>
	public class SchemaTable : SchemaItem
	{ // Enumerados
			public enum TableType
				{ Unknown,
					Table,
					View
				}

		public SchemaTable(Schema objParent) : base(objParent)
		{ Columns = new SchemaColumnsCollection(objParent);
			Constraints = new SchemaConstraintsCollection(objParent);
		}

		public string Description { get; set; }
		
		public TableType Type { get; set; }

		public DateTime DateCreate { get; set; }
		
		public DateTime DateUpdate { get; set; }
				
		public SchemaColumnsCollection Columns { get; private set; }
		
		public SchemaConstraintsCollection Constraints { get; private set; }
	}
}