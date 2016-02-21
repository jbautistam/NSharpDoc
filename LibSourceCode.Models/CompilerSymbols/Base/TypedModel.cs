using System;

namespace Bau.Libraries.LibSourceCode.Models.CompilerSymbols.Base
{
	/// <summary>
	///		Clase con los elementos de un tipo
	/// </summary>
	public class TypedModel : LanguageBaseModel
	{
		/// <summary>
		///		Obtiene la cadena de depuración del tipo
		/// </summary>
		internal string Debug()
		{ string strDebug = " [Type: " + Name;
		
				// Añade los caracteres que indica si es un array
					if (IsArray)
						strDebug += "[]";
				// Devuelve la cadena de depuración
					return  strDebug + " -- " + NameSpace + "] ";
		}

		/// <summary>
		///		Tipo base
		/// </summary>
		public TypedModel BaseType { get; set; }

		/// <summary>
		///		Espacio de nombres
		/// </summary>
		public string NameSpace { get; set; }

		/// <summary>
		///		Tipo de retorno
		/// </summary>
		public bool IsVoid { get; set; }

		/// <summary>
		///		Indica si es un array
		/// </summary>
		public bool IsArray { get; set; }

		/// <summary>
		///		Dimensiones del array
		/// </summary>
		public int Dimensions { get; set; }

		/// <summary>
		///		Restricciones asociadas al tipo (genéricos)
		/// </summary>
		public System.Collections.Generic.List<TypedModel> Constraints { get; set; } = new System.Collections.Generic.List<TypedModel>();
	}
}
