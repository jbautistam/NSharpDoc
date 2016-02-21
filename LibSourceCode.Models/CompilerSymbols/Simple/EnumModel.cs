using System;

namespace Bau.Libraries.LibSourceCode.Models.CompilerSymbols.Simple
{
	/// <summary>
	///		Modelo con los datos de un enumerado
	/// </summary>
	public class EnumModel : Base.LanguageStructModel
	{
		public EnumModel(Base.LanguageStructModel objParent) : base(StructType.Enum, objParent) {}

		/// <summary>
		///		Tipo del enumerado
		/// </summary>
		public Base.TypedModel Type { get; set; } = new Base.TypedModel();
	}
}
