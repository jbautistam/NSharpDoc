using System;

namespace Bau.Libraries.LibDBSchema.DataSchema
{
	/// <summary>
	///		Clase que mantiene los datos de una rutina
	/// </summary>
	public class SchemaRoutine : SchemaItem
	{ // Enumerados
			public enum RoutineType
				{	Unknown,
					Procedure,
					Function
				}

		public SchemaRoutine(Schema objParent) : base(objParent)
		{
		}
		
		/// <summary>
		///		Definición de la rutina
		/// </summary>
		public string Definition { get; set; }
		
		/// <summary>
		///		Tipo de la rutina
		/// </summary>
		public RoutineType Type { get; set; }
	}
}
