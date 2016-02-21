using System;

namespace Bau.Libraries.LibDBSchema.DataSchema
{
	/// <summary>
	///		Colecci�n de tablas
	/// </summary>
	public class SchemaTablesCollection : SchemaItemsCollection<SchemaTable>
	{
		public SchemaTablesCollection(Schema objParent) : base(objParent)
		{
		}
	}
}
