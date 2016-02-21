using System;
using System.Collections.Generic;

using Bau.Libraries.LibHelper.Extensors;

namespace Bau.Libraries.LibNSharpDoc.Models.Structs
{
	/// <summary>
	///		Diccionario de parámetros de una estructura
	/// </summary>
	public class StructParameterModelDictionary
	{	
		/// <summary>
		///		Añade la cadena de depuración
		/// </summary>
		internal string Debug(int intIndent)
		{ string strDebug = "";

				// Añade los parámetros
					if (Parameters.Count > 0)
						{ // Añade la cabecera
								strDebug += new string('\t', intIndent) + "<!-- BeginParameters -->" + Environment.NewLine;
							// Añade el valor del parámetros
								foreach (KeyValuePair<string, StructParameterModel> objParameter in Parameters)
									strDebug += objParameter.Value.Debug(intIndent + 1);
							// Añade el fin de la cadena de parámetros
								strDebug += new string('\t', intIndent) + "<!-- EndParameters -->" + Environment.NewLine;
						}
				// Devuelve la cadena de depuración
					return strDebug;
		}
		
		/// <summary>
		///		Obtiene una cadena con la descripción de los parámetros
		/// </summary>
		public string GetFullParameters()
		{ string strParameters = "";

				// Obtiene los valores
					foreach (KeyValuePair<string, StructParameterModel> objParameter in Parameters)
						strParameters = strParameters.AddWithSeparator($"{objParameter.Value.Key} = {objParameter.Value.Value?.ToString()}", ";");
				// Devuelve la cadena
					return strParameters;
    }

		/// <summary>
		///		Añade un parámetro al diccionario
		/// </summary>
		public StructParameterModel Add(string strKey, object objValue, string strReferenceKey = null)
		{ return Add(new StructParameterModel(strKey, objValue, strReferenceKey));
		}

		/// <summary>
		///		Añade un parámetro al diccionario
		/// </summary>
		public StructParameterModel Add(StructParameterModel objParameter)
		{	// Comprueba si es nulo antes de añadir
				if (objParameter != null)
					{ string strKey = NormalizeKey(objParameter.Key);

						// Añade el parámetro
							if (Parameters.ContainsKey(strKey))
								Parameters[strKey] = objParameter;
							else
								Parameters.Add(strKey, objParameter);
					}
			// Devuelve el parámetro añadido
				return objParameter;
		}

		/// <summary>
		///		Obtiene el valor de un parámetro
		/// </summary>
		public string GetValue(string strKey, string strDefault = "")
		{ StructParameterModel objParameter;

				// Obtiene el valor
					if (Parameters.TryGetValue(NormalizeKey(strKey), out objParameter))
						return objParameter.Value?.ToString();
					else
						return strDefault;
		}

		/// <summary>
		///		Obtiene un parámetro por su clave
		/// </summary>
		public StructParameterModel Search(string strKey)
		{ StructParameterModel objParameter;

				// Obtiene el parámetro
					if (!Parameters.TryGetValue(NormalizeKey(strKey), out objParameter))
						objParameter = new StructParameterModel(strKey, "", null);
				// Devuelve el parámetro
					return objParameter;
		}

		/// <summary>
		///		Normaliza la clave
		/// </summary>
		private string NormalizeKey(string strName)
		{ return strName.TrimIgnoreNull().ToUpper();
		}

		/// <summary>
		///		Diccionario de parámetros
		/// </summary>
		public Dictionary<string, StructParameterModel> Parameters { get; set; } = new Dictionary<string, StructParameterModel>();
	}
}
