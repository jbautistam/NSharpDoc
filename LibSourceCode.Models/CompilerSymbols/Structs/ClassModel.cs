using System;

namespace Bau.Libraries.LibSourceCode.Models.CompilerSymbols.Structs
{
	/// <summary>
	///		Modelo con los datos de una clase
	/// </summary>
	public class ClassModel : BaseComplexModel
	{
		public ClassModel(Base.LanguageStructModel objParent) : base(StructType.Class, objParent) {}

		/// <summary>
		///		Indica si la clase está marcada como Sealed
		/// </summary>
		public bool IsSealed { get; set; }

		/// <summary>
		///		Indica si la clase está marcada como estática
		/// </summary>
		public bool IsStatic { get; set; }

		/// <summary>
		///		Indica si la clase está marcada como abstracta
		/// </summary>
		public bool IsAbstract { get; set; }
	}
}
