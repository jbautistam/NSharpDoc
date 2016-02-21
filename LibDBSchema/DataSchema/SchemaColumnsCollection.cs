using System;

namespace Bau.Libraries.LibDBSchema.DataSchema
{
	/// <summary>
	///		Colección de columnas
	/// </summary>
	public class SchemaColumnsCollection : SchemaItemsCollection<SchemaColumn>
	{
		public SchemaColumnsCollection(Schema objParent) : base(objParent)
		{
		}

		/// <summary>
		///		Ordena las columnas por su posición
		/// </summary>
		public void SortByOrdinalPosition()
		{ Sort((objFirst, objSecond) => objFirst.OrdinalPosition.CompareTo(objSecond.OrdinalPosition));
		}
	}
}