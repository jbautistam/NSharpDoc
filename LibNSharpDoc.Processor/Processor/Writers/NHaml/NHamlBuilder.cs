using System;

using Bau.Libraries.LibHelper.Extensors;

namespace Bau.Libraries.LibNSharpDoc.Processor.Processor.Writers.NHaml
{
	/// <summary>
	///		Builder de NHaml
	/// </summary>
	internal class NHamlBuilder
	{ // Variables privadas
			private System.Text.StringBuilder sbBuilder;
		
		public NHamlBuilder()
		{ Clear();
		}

		/// <summary>
		///		Limpia el contenido
		/// </summary>
		internal void Clear()
		{ sbBuilder = new System.Text.StringBuilder();
			Indent = 0;
		}

		/// <summary>
		///		Incrementa la indentación
		/// </summary>
		internal void AddIndent()
		{ Indent++;
		}

		/// <summary>
		///		Decrementa la indentación
		/// </summary>
		internal void RemoveIndent()
		{ Indent--;
			if (Indent < 0)
				Indent = 0;
		}

		/// <summary>
		///		Añade una etiqueta con su texto
		/// </summary>
		internal void AddTag(string strTag)
		{ AddTag(strTag, null);
		}

		/// <summary>
		///		Añade una etiqueta con su texto
		/// </summary>
		internal void AddTag(string strTag, string strText)
		{ if (!strTag.IsEmpty())
				{ // Añade el salto de línea y la indentación
						sbBuilder.Append(Environment.NewLine);
						if (Indent > 0)
							sbBuilder.Append(new string('\t', Indent));
					// Añade la etiqueta
						if (!strTag.StartsWith("%") && !strTag.StartsWith("<%"))
							sbBuilder.Append("%");
						sbBuilder.Append(strTag.TrimIgnoreNull());
					// Añade el texto
						if (!string.IsNullOrEmpty(strText))
							sbBuilder.Append(" " + strText.TrimIgnoreNull());
				}
		}
		
		/// <summary>
		///		Añade un texto, presumiblemente una cadena de span (#x value), por eso se le suma uno a la indentación
		/// </summary>
		internal void AddText(string strText)
		{ if (!strText.IsEmpty())
				sbBuilder.Append(" " + strText.TrimIgnoreNull());
		}

		/// <summary>
		///		Obtiene la cadena HTML
		/// </summary>
		public override string ToString()
		{ return sbBuilder.ToString();
		}

		/// <summary>
		///		Indentación
		/// </summary>
		internal int Indent { get; private set; }
	}
}
