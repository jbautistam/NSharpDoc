using System;

using Bau.Libraries.LibHelper.Extensors;

namespace Bau.Libraries.LibSourceCode.Models.CompilerSymbols.Base
{
	/// <summary>
	///		Comentarios Xml
	/// </summary>
	public class RemarksXml
	{	// Variables privadas
			private string strRawXml;

		/// <summary>
		///		Interpreta los comentarios
		/// </summary>
		private void ParseRemarks()
		{ // Limpia los comentarios
				Summary = "";
				Remarks = "";
				Returns = "";
				Parameters = new System.Collections.Generic.Dictionary<string, string>();
			// Interpreta el XML
				if (!RawXml.IsEmpty())
					{ // Extrae el resumen y los comentarios
							Summary = Extract(RawXml, "summary");
							Remarks = Extract(RawXml, "remarks");
							Returns = Extract(RawXml, "returns");
						// Extrae los parámetros
							ExtractParameters(RawXml);
					}
		}

		/// <summary>
		///		Concatena una colección de cadenas
		/// </summary>
		private string Extract(string strSource, string strTag)
		{ System.Collections.Generic.List<string> objColStrings = strSource.Extract("<" + strTag + ">", "</" + strTag + ">");
			string strResult = "";

				// Añade las cadenas
					foreach (string strString in objColStrings)
						strResult = strResult.AddWithSeparator(strString, Environment.NewLine, false);
				// Devuelve la cadena concatenada
					return strResult;
		}

		/// <summary>
		///		Extrae los parámetros
		/// </summary>
		private void ExtractParameters(string strSource)
		{ System.Collections.Generic.List<string> objColStrings = strSource.Extract("<param", "</param>");

				// Recorre las cadenas extrayendo los nombres y la descripción de los parámetros
				// El formato de strString será del tipo: 'name="disposing">Descripción de disposing'
					foreach (string strString in objColStrings)
						{ string strName, strDescription;
							
								// Corta la cadena hasta el primer ">". En strName queda 'name="disposing"' y en strDescription queda la descripción
									strName = strString.Cut(">", out strDescription);
								// Obtiene el nombre
									strName.Cut("\"", out strName);
									while (!strName.IsEmpty() && strName.EndsWith("\""))
										strName = strName.Left(strName.Length - 1);
								// Añade el parámetro
									Parameters.Add(strName, strDescription);
						}
		}

		/// <summary>
		///		Obtiene los comentarios de un parámetro
		/// </summary>
		public string GetParameterRemarks(string strParameter)
		{ string strDescription;

				if (Parameters.TryGetValue(strParameter, out strDescription))
					return strDescription;
				else
					return "";
		}

		/// <summary>
		///		Xml en crudo de los comentarios de documentación
		/// </summary>
		public string RawXml
		{ get { return strRawXml; }
			set 
				{ strRawXml = value; 
					ParseRemarks();
				}
		}

		/// <summary>
		///		Sección Summary de los comentarios XML
		/// </summary>
		public string Summary { get; private set; }

		/// <summary>
		///		Sección Remarks de los comentarios XML
		/// </summary>
		public string Remarks { get; private set; }

		/// <summary>
		///		Sección Returns de los comentarios XML
		/// </summary>
		public string Returns { get; private set; }

		/// <summary>
		///		Parámetros
		/// </summary>
		public System.Collections.Generic.Dictionary<string, string> Parameters { get; private set; } = new System.Collections.Generic.Dictionary<string, string>();
	}
}
