using System;

namespace Bau.Libraries.LibSourceCode.Models.CompilerSymbols.Structs
{
	/// <summary>
	///		Clase base para los elementos complejos
	/// </summary>
	public abstract class BaseComplexModel : Base.LanguageStructModel
	{
		public BaseComplexModel(StructType intIDType, Base.LanguageStructModel objParent) : base(intIDType, objParent) {}

		/// <summary>
		///		Obtiene la cadena de depuración
		/// </summary>
		public override string Debug(int intIndent)
		{	string strDebug = base.Debug(intIndent);
		
				// Añade los interfaces
					strDebug += new string('\t', intIndent + 1);
					strDebug += "Clase base: [" + BaseClass + "] ";
					if (Interfaces.Count == 0)
						strDebug += " No implementa ningún interface";
					else
						{ strDebug += "Interfaces: ";
							foreach (string strInterface in Interfaces)
								strDebug += strInterface + " - ";
						}
				// Añade los tipos de parámetros
					if (TypeParameters.Count == 0)
						strDebug += " Sin parámetros genéricos";
					else
						{ strDebug += " Genéricos: ";
							foreach (Simple.TypeParameterModel objParameter in TypeParameters)
								strDebug += objParameter.Name + " - ";
						}
				// Devuelve la cadena de depuración
					return strDebug + Environment.NewLine;
		}

		/// <summary>
		///		Clase base
		/// </summary>
		public Base.TypedModel BaseClass { get; set; } = new Base.TypedModel();

		/// <summary>
		///		Interfaces que implementa
		/// </summary>
		public System.Collections.Generic.List<string> Interfaces { get; set; } = new System.Collections.Generic.List<string>();

		/// <summary>
		///		Tipos de los parámetros genéricos
		/// </summary>
		public System.Collections.Generic.List<Simple.TypeParameterModel> TypeParameters { get; set; } = new System.Collections.Generic.List<Simple.TypeParameterModel>();
	}
}
