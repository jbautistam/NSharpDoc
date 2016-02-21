using System;

namespace Bau.Libraries.LibSourceCode.Models.CompilerSymbols.Methods
{
	/// <summary>
	///		Clase base para las funciones y propiedades
	/// </summary>
	public abstract class BaseMethodModel : Base.LanguageStructModel
	{
		public BaseMethodModel(StructType intIDType, Base.LanguageStructModel objParent) : base(intIDType, objParent) {}

		/// <summary>
		///		Obtiene la cadena de depuración de los argumentos
		/// </summary>
		internal string DebugArguments(int intIndent)
		{ string strDebug = new string('\t', intIndent);

				// Añade los argumentos
					if (Arguments.Count == 0)
						strDebug += "Sin argumentos" + Environment.NewLine;
					else
						foreach (Simple.ArgumentModel objArgument in Arguments)
							strDebug += objArgument.Debug(0);
				// Devuelve la cadena
					return strDebug;
		}

		/// <summary>
		///		Indica si es abstracta
		/// </summary>
		public bool IsAbstract { get; set; }

		/// <summary>
		///		Indica si está sobrescrito
		/// </summary>
		public bool IsOverride { get; set; }

		/// <summary>
		///		Indica si está sellada
		/// </summary>
		public bool IsSealed { get; set; }

		/// <summary>
		///		Indica si es estática
		/// </summary>
		public bool IsStatic { get; set; }

		/// <summary>
		///		Indica si es virtual
		/// </summary>
		public bool IsVirtual { get; set; }

		/// <summary>
		///		Tipos de los parámetros genéricos
		/// </summary>
		public System.Collections.Generic.List<Simple.TypeParameterModel> TypeParameters { get; set; } = new System.Collections.Generic.List<Simple.TypeParameterModel>();

		/// <summary>
		///		Argumentos
		/// </summary>
		public System.Collections.Generic.List<Simple.ArgumentModel> Arguments { get; set; } = new System.Collections.Generic.List<Simple.ArgumentModel>();
	}
}
