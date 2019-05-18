using System;

namespace Bau.Libraries.LibSourceCode.Models.CompilerSymbols.Structs
{
	/// <summary>
	///		Modelo con los datos de un espacio de nombres
	/// </summary>
	public class NameSpaceModel : Base.LanguageStructModel
	{
		public NameSpaceModel(Base.LanguageStructModel parent) : base(StructType.NameSpace, parent) { }
	}
}
