using System;

namespace Bau.Libraries.LibSourceCode.Models.CompilerSymbols.Methods
{
	/// <summary>
	///		Clase con los datos de una propiedad
	/// </summary>
	public class PropertyModel : BaseMethodModel
	{
		public PropertyModel(Base.LanguageStructModel parent) : base(StructType.Property, parent) { }

		/// <summary>
		///		Método Get de la propiedad
		/// </summary>
		public MethodModel GetMethod { get; set; }

		/// <summary>
		///		Método Set de la propiedad
		/// </summary>
		public MethodModel SetMethod { get; set; }

		/// <summary>
		///		Indica si es un indizador
		/// </summary>
		public bool IsIndexer { get; set; }

		/// <summary>
		///		Indica si es sólo lectura
		/// </summary>
		public bool IsReadOnly { get; set; }

		/// <summary>
		///		Indica si tiene eventos
		/// </summary>
		public bool IsWithEvents { get; set; }

		/// <summary>
		///		Indica si es sólo escritura
		/// </summary>
		public bool IsWriteOnly { get; set; }
	}
}
