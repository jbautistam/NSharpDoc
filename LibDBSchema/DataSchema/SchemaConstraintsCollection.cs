using System;

namespace Bau.Libraries.LibDBSchema.DataSchema
{
	/// <summary>
	///		Colección de restricciones de una tabla
	/// </summary>
	public class SchemaConstraintsCollection : SchemaItemsCollection<SchemaConstraint>
	{
		public SchemaConstraintsCollection(Schema objParent) : base(objParent)
		{
		}
	}
}
