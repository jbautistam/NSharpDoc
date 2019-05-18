using System;

using Bau.Libraries.LibCommonHelper.Extensors;

namespace Bau.Libraries.LibSourceCode.Models.CompilerSymbols.Base
{
	/// <summary>
	///		Comentarios Xml
	/// </summary>
	public class RemarksXml
	{   
		// Variables privadas
		private string rawXml;

		/// <summary>
		///		Interpreta los comentarios
		/// </summary>
		private void ParseRemarks()
		{ 
			// Limpia los comentarios
			Summary = "";
			Remarks = "";
			Returns = "";
			Parameters = new System.Collections.Generic.Dictionary<string, string>();
			// Interpreta el XML
			if (!RawXml.IsEmpty())
			{	
				// Extrae el resumen y los comentarios
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
		private string Extract(string source, string tag)
		{
			System.Collections.Generic.List<string> parts = source.Extract("<" + tag + ">", "</" + tag + ">");
			string result = "";

				// Añade las cadenas
				foreach (string part in parts)
					result = result.AddWithSeparator(part, Environment.NewLine, false);
				// Devuelve la cadena concatenada
				return result;
		}

		/// <summary>
		///		Extrae los parámetros
		/// </summary>
		private void ExtractParameters(string source)
		{
			System.Collections.Generic.List<string> parts = source.Extract("<param", "</param>");

				// Recorre las cadenas extrayendo los nombres y la descripción de los parámetros
				// El formato de part será del tipo: 'name="disposing">Descripción de disposing'
				foreach (string part in parts)
				{
					string name, description;

					// Corta la cadena hasta el primer ">". En name queda 'name="disposing"' y en description queda la descripción
					name = part.Cut(">", out description);
					// Obtiene el nombre
					name.Cut("\"", out name);
					while (!name.IsEmpty() && name.EndsWith("\""))
						name = name.Left(name.Length - 1);
					// Añade el parámetro
					Parameters.Add(name, description);
				}
		}

		/// <summary>
		///		Obtiene los comentarios de un parámetro
		/// </summary>
		public string GetParameterRemarks(string parameter)
		{
			if (Parameters.TryGetValue(parameter, out string description))
				return description;
			else
				return string.Empty;
		}

		/// <summary>
		///		Xml en crudo de los comentarios de documentación
		/// </summary>
		public string RawXml
		{
			get { return rawXml; }
			set
			{
				rawXml = value;
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
