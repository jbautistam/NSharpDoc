using System;

using Bau.Libraries.LibHelper.Extensors;

namespace Bau.Libraries.LibSourceCode.Models.CompilerSymbols.Base
{
	/// <summary>
	///		Elemento base 
	/// </summary>
	public abstract class LanguageBaseModel 
	{
		/// <summary>
		///		Obtiene la cadena de depuración
		/// </summary>
		public virtual string Debug(int intIndent)
		{ string strDebug = new string('\t', intIndent) + " -> " + Name;

				// Añade el error
					if (HasError)
						strDebug += "Error " + " -> " + Error + Environment.NewLine;
				// Devuelve la cadena
					return strDebug;
		}

		/// <summary>
		///		Nombre del elemento
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		///		Error asociado al elemento
		/// </summary>
		public string Error { get; set; }

		/// <summary>
		///		Indica si hay algún error
		/// </summary>
		public bool HasError 
		{ get { return !Error.IsEmpty(); } 
		}
	}
}
