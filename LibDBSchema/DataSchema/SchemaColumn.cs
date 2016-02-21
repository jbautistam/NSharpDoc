using System;

namespace Bau.Libraries.LibDBSchema.DataSchema
{
	/// <summary>
	///		Clase que almacena los datos de una columna
	/// </summary>
	public class SchemaColumn : SchemaItem
	{ // Enumerados
			public enum ColumnType
				{ Table,
					View
				}

		public SchemaColumn(Schema objParent) : base(objParent)
		{
		}

		public string Table { get; set; }

		public string Description { get; set; }
		
		public int OrdinalPosition { get; set; }
		
		public string Default { get; set; }
		
		public bool IsNullable { get; set; }
		
		public string DataType { get; set; }
		
		public int CharacterMaximumLength { get; set; }
		
		public int NumericPrecision { get; set; }
		
		public int NumericPrecisionRadix { get; set; }
		
		public int NumericScale { get; set; }
		
		public int DateTimePrecision { get; set; }
		
		public string CharacterSetName { get; set; }
		
		public string CollationCatalog { get; set; }
		
		public string CollationSchema { get; set; }
		
		public string CollationName { get; set; }
		
		public bool IsIdentity { get; set; }
	}
}