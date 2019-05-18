using System;

namespace Bau.Libraries.LibSourceCode.Models.CompilerSymbols.Simple
{
	/// <summary>
	///		Modelo con los datos de un enumerado
	/// </summary>
	public class EnumModel : Base.LanguageStructModel
	{
		public EnumModel(Base.LanguageStructModel parent) : base(StructType.Enum, parent) { }

		/// <summary>
		///		Tipo del enumerado
		/// </summary>
		public Base.TypedModel MainType { get; set; } = new Base.TypedModel();
	}
}
