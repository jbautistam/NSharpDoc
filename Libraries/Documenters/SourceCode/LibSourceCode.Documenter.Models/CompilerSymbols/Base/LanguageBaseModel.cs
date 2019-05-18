using System;

using Bau.Libraries.LibCommonHelper.Extensors;

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
		public virtual string Debug(int indent)
		{
			string debug = new string('\t', indent) + " -> " + Name;

				// Añade el error
				if (HasError)
					debug += "Error " + " -> " + Error + Environment.NewLine;
				// Devuelve la cadena
				return debug;
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
		{
			get { return !Error.IsEmpty(); }
		}
	}
}
