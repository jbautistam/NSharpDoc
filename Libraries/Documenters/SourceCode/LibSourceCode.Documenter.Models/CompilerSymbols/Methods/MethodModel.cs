using System;

namespace Bau.Libraries.LibSourceCode.Models.CompilerSymbols.Methods
{
	/// <summary>
	///		Clase con los datos de un método
	/// </summary>
	public class MethodModel : BaseMethodModel
	{
		public MethodModel(Base.LanguageStructModel parent) : base(StructType.Method, parent) { }

		/// <summary>
		///		Obtiene la cadena de depuración
		/// </summary>
		public override string Debug(int indent)
		{
			string debug = base.Debug(indent);

				// Añade los datos del tipo de retorno
				return ReturnType.Debug() + Environment.NewLine + DebugArguments(indent);
		}

		/// <summary>
		///		Indica si el método es asíncrono
		/// </summary>
		public bool IsAsync { get; set; }

		/// <summary>
		///		Indica si es un método checked
		/// </summary>
		public bool IsCheckedBuiltin { get; set; }

		/// <summary>
		///		Indica si es una definición
		/// </summary>
		public bool IsDefinition { get; set; }

		/// <summary>
		///		Indica si es un método de extensión
		/// </summary>
		public bool IsExtensionMethod { get; set; }

		/// <summary>
		///		Indica si es externo
		/// </summary>
		public bool IsExtern { get; set; }

		/// <summary>
		///		Indica si es un método genérico
		/// </summary>
		public bool IsGenericMethod { get; set; }

		/// <summary>
		///		Indica si se declara implícitamente
		/// </summary>
		public bool IsImplicitlyDeclared { get; set; }

		/// <summary>
		///		Tipo de retorno
		/// </summary>
		public Base.TypedModel ReturnType { get; set; } = new Base.TypedModel();
	}
}
