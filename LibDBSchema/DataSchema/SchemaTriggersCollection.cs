using System;

namespace Bau.Libraries.LibDBSchema.DataSchema 
{
	/// <summary>
	///		Colección de <see cref="SchemaTrigger"/>
	/// </summary>
	public class SchemaTriggersCollection : SchemaItemsCollection<SchemaTrigger>
	{
		public SchemaTriggersCollection(Schema objParent) : base(objParent)
		{
		}

		/// <summary>
		///		Busca los triggers de una tabla
		/// </summary>
		public SchemaTriggersCollection SearchByTable(string strTable)
		{ SchemaTriggersCollection objColTriggers = new SchemaTriggersCollection(base.Parent);
		
				// Recorre la colección
					foreach (SchemaTrigger objTrigger in this)
						if (objTrigger.Table.Equals(strTable, StringComparison.CurrentCultureIgnoreCase))
							objColTriggers.Add(objTrigger);
				// Devuelve la colección de triggers encontrados
					return objColTriggers;
		}
	}
}
