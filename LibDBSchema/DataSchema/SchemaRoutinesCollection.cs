using System;

namespace Bau.Libraries.LibDBSchema.DataSchema
{
	/// <summary>
	///		Colección de rutinas
	/// </summary>
	public class SchemaRoutinesCollection : SchemaItemsCollection<SchemaRoutine>
	{
		public SchemaRoutinesCollection(Schema objParent) : base(objParent)
		{
		}

		/// <summary>
		///		Obtiene una colección de procedimientos
		/// </summary>
		public SchemaRoutinesCollection SearchProcedures()
		{ return SearchByType(true);
		}

		/// <summary>
		///		Obtiene una colección de funciones
		/// </summary>
		public SchemaRoutinesCollection SearchFunctions()
		{ return SearchByType(false);
		}

		/// <summary>
		///		Obtiene una colección de procedimientos o funciones
		/// </summary>
		private SchemaRoutinesCollection SearchByType(bool blnProcedures)
		{ SchemaRoutinesCollection objColRoutines = new SchemaRoutinesCollection(Parent);
		
				// Recorre la colección buscando los elementos
					foreach (SchemaRoutine objRoutine in this)
						if ((blnProcedures && objRoutine.Type == SchemaRoutine.RoutineType.Procedure) ||
								(!blnProcedures && objRoutine.Type == SchemaRoutine.RoutineType.Function))
							objColRoutines.Add(objRoutine);
				// Devuelve la colección
					return objColRoutines;
		}
	}
}
