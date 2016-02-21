using System;

namespace Bau.Libraries.LibNSharpDoc.Models.Structs
{
	/// <summary>
	///		Parámetro de una estructura
	/// </summary>
	public class StructParameterModel
	{
		public StructParameterModel(string strKey, object objValue, string strReferenceKey)
		{ Key = strKey;
			Value = objValue;
			ReferenceKey = strReferenceKey;
		}

		/// <summary>
		///		Obtiene la cadena de depuración
		/// </summary>
		internal string Debug(int intIndent)
		{ string strDebug = new string('\t', intIndent);

				// Añade los datos del parámetro
					strDebug += $"Key: {Key} - Value: {Value} - ReferenceKey: {ReferenceKey}" + Environment.NewLine;
					strDebug += Parameters.Debug(intIndent + 1);
				// Devuelve la cadena de depuración
					return strDebug;
		}

		/// <summary>
		///		Clave
		/// </summary>
		public string Key { get; }

		/// <summary>
		///		Valor
		/// </summary>
		public object Value { get; }

		/// <summary>
		///		Clave de referencia
		/// </summary>
		public string ReferenceKey { get; }

		/// <summary>
		///		Referencia de estructura
		/// </summary>
		public StructDocumentationModel Reference { get; set; }

		/// <summary>
		///		Parámetros de la referencia
		/// </summary>
		public StructParameterModelDictionary Parameters { get; } = new StructParameterModelDictionary();
	}
}
