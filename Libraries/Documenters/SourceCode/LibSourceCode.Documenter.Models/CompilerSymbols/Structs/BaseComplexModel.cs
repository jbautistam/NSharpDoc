using System;

namespace Bau.Libraries.LibSourceCode.Models.CompilerSymbols.Structs
{
	/// <summary>
	///		Clase base para los elementos complejos
	/// </summary>
	public abstract class BaseComplexModel : Base.LanguageStructModel
	{
		public BaseComplexModel(StructType typeId, Base.LanguageStructModel parent) : base(typeId, parent) { }

		/// <summary>
		///		Obtiene la cadena de depuración
		/// </summary>
		public override string Debug(int indent)
		{
			string debug = base.Debug(indent);

				// Añade los interfaces
				debug += new string('\t', indent + 1);
				debug += "Clase base: [" + BaseClass + "] ";
				if (Interfaces.Count == 0)
					debug += " No implementa ningún interface";
				else
				{
					debug += "Interfaces: ";
					foreach (string strInterface in Interfaces)
						debug += strInterface + " - ";
				}
				// Añade los tipos de parámetros
				if (TypeParameters.Count == 0)
					debug += " Sin parámetros genéricos";
				else
				{
					debug += " Genéricos: ";
					foreach (Simple.TypeParameterModel parameter in TypeParameters)
						debug += parameter.Name + " - ";
				}
				// Devuelve la cadena de depuración
				return debug + Environment.NewLine;
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
