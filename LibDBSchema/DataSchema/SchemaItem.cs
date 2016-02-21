using System;

namespace Bau.Libraries.LibDBSchema.DataSchema
{
	/// <summary>
	///		Clase base para los elementos del esquema
	/// </summary>
	public abstract class SchemaItem
	{	
		public SchemaItem(Schema objSchema)
		{ Parent = objSchema;
		}
		
		/// <summary>
		///		Normaliza una cadena
		/// </summary>
		private string Normalize(string strValue)
		{ // Normaliza los valores
				if (!string.IsNullOrEmpty(strValue))
					{ strValue = strValue.Replace(":", "_");
						strValue = strValue.Replace("\\", "_");
						strValue = strValue.Replace(".", "_");
					}
			// Devuelve la cadena normalizada
				return strValue;
		}

		/// <summary>
		///		Esquema al que pertenecen los datos
		/// </summary>
		public Schema Parent { get; private set; }
		
		/// <summary>
		///		Identificador del objeto
		/// </summary>
		public string ID
		{ get { return Normalize(Catalog) + "_" + Schema + "_" + Name; }
		}
		
		/// <summary>
		///		Nombre completo del objeto
		/// </summary>
		public string FullName
		{ get { return Catalog + "." + Schema + "." + Name; }
		}
		
		/// <summary>
		///		Catálogo
		/// </summary>
		public string Catalog { get; set; }
		
		/// <summary>
		///		Nombre de esquema
		/// </summary>
		public string Schema { get ; set; }
		
		/// <summary>
		///		Nombre del elemento
		/// </summary>
		public string Name { get ; set; }
	}
}
