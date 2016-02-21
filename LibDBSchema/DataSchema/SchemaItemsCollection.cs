using System;
using System.Collections.Generic;

namespace Bau.Libraries.LibDBSchema.DataSchema
{
	/// <summary>
	///		Colecci�n base de esquema
	/// </summary>
	public abstract class SchemaItemsCollection<TypeData> : List<TypeData> where TypeData : SchemaItem
	{ 
		public SchemaItemsCollection(Schema objParent)
		{ Parent = objParent;
		}
		
		/// <summary>
		///		Busca un elemento
		/// </summary>
		public virtual SchemaItem Search(string strFullName)
		{ // Busca el elemento
				foreach (TypeData objItem in this)
					if (objItem.FullName == strFullName)
						return objItem;
			// Si ha llegado hasta aqu� es porque no ha encontrado nada
				return null;
		}
		
		/// <summary>
		///		Esquema al que pertenece la colecci�n
		/// </summary>
		public Schema Parent { get; private set; }
	}
}
