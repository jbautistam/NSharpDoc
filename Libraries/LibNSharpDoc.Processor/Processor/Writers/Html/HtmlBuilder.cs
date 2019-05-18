using System;

namespace Bau.Libraries.LibNSharpDoc.Processor.Processor.Writers.Html
{
	/// <summary>
	///		Builder de HTML
	/// </summary>
	internal class HtmlBuilder
	{ 
		// Variables privadas
		private System.Text.StringBuilder builder;

		/// <summary>
		///		Limpia el contenido
		/// </summary>
		internal void Clear()
		{
			builder = new System.Text.StringBuilder();
			Indent = 0;
		}

		/// <summary>
		///		Incrementa la indentación
		/// </summary>
		internal void AddIndent()
		{
			Indent++;
		}

		/// <summary>
		///		Decrementa la indentación
		/// </summary>
		internal void RemoveIndent()
		{
			Indent--;
			if (Indent < 0)
				Indent = 0;
		}

		/// <summary>
		///		Añade una etiqueta con su texto
		/// </summary>
		internal void AddTag(string tag)
		{
			AddTag(tag, null);
		}

		/// <summary>
		///		Añade una etiqueta con su texto
		/// </summary>
		internal void AddTag(string tag, string text)
		{ 
			// Añade la indentación
			if (Indent > 0)
				builder.Append(new string('\t', Indent));
			// Añade la etiqueta
			builder.Append(tag);
			// Añade el texto
			if (!string.IsNullOrEmpty(text))
				builder.Append(" " + text);
			// Añade un salto de línea
			builder.Append(Environment.NewLine);
		}

		/// <summary>
		///		Obtiene la cadena HTML
		/// </summary>
		public string GetHtml()
		{
			return builder.ToString();
		}

		/// <summary>
		///		Indentación
		/// </summary>
		internal int Indent { get; private set; }
	}
}
