using System;

namespace Bau.Libraries.LibSourceCode.Models.CompilerSymbols.Simple
{
	/// <summary>
	///		Modelo con los datos de un miembro de un enumerado
	/// </summary>
	public class EnumMemberModel : Base.LanguageStructModel
	{
		public EnumMemberModel(Base.LanguageStructModel objParent) : base(StructType.EnumMember, objParent) {}

		/// <summary>
		///		Tipo del enumerado
		/// </summary>
		public Base.TypedModel Type { get; internal set; } = new Base.TypedModel();
	}
}
