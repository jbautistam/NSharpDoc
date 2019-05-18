using System;

namespace Bau.Libraries.LibSourceCode.Models.CompilerSymbols.Base
{
	/// <summary>
	///		Clase con los datos de un atributo
	/// </summary>
	public class AttributeModel : LanguageBaseModel
	{
		/// <summary>
		///		Clase a la que se hace referencia
		/// </summary>
		public string ClassReferenced { get; set; }

		/// <summary>
		///		Espacio de nombres al que se hace referencia
		/// </summary>
		public string NameSpaceReferenced { get; set; }

		/// <summary>
		///		Argumentos del atributo
		/// </summary>
		public string Arguments { get; set; }
	}
}
