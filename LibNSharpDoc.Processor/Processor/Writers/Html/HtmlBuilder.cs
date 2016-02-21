using System;

namespace Bau.Libraries.LibNSharpDoc.Processor.Processor.Writers.Html
{
	/// <summary>
	///		Builder de HTML
	/// </summary>
	internal class HtmlBuilder
	{ // Variables privadas
			private System.Text.StringBuilder sbBuilder;

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
		{ // Añade la indentación
				if (Indent > 0)
					sbBuilder.Append(new string('\t', Indent));
			// Añade la etiqueta
				sbBuilder.Append(strTag);
			// Añade el texto
				if (!string.IsNullOrEmpty(strText))
					sbBuilder.Append(" " + strText);
			// Añade un salto de línea
				sbBuilder.Append(Environment.NewLine);
		}

		/// <summary>
		///		Obtiene la cadena HTML
		/// </summary>
		public string GetHtml()
		{ return sbBuilder.ToString();
		}

		/// <summary>
		///		Indentación
		/// </summary>
		internal int Indent { get; private set; }
	}
}
