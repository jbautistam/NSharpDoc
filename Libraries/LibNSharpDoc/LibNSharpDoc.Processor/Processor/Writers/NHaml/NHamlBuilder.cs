using System;

using Bau.Libraries.LibCommonHelper.Extensors;

namespace Bau.Libraries.LibNSharpDoc.Processor.Processor.Writers.NHaml
{
	/// <summary>
	///		Builder de NHaml
	/// </summary>
	internal class NHamlBuilder
	{ 
		// Variables privadas
		private System.Text.StringBuilder builder;

		public NHamlBuilder()
		{
			Clear();
		}

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
			if (!tag.IsEmpty())
			{ 
				// Añade el salto de línea y la indentación
				builder.Append(Environment.NewLine);
				if (Indent > 0)
					builder.Append(new string('\t', Indent));
				// Añade la etiqueta
				if (!tag.StartsWith("%") && !tag.StartsWith("<%"))
					builder.Append("%");
				builder.Append(tag.TrimIgnoreNull());
				// Añade el texto
				if (!string.IsNullOrEmpty(text))
					builder.Append(" " + text.TrimIgnoreNull());
			}
		}

		/// <summary>
		///		Añade un texto, presumiblemente una cadena de span (#x value), por eso se le suma uno a la indentación
		/// </summary>
		internal void AddText(string text)
		{
			if (!text.IsEmpty())
				builder.Append(" " + text.TrimIgnoreNull());
		}

		/// <summary>
		///		Obtiene la cadena HTML
		/// </summary>
		public override string ToString()
		{
			return builder.ToString();
		}

		/// <summary>
		///		Indentación
		/// </summary>
		internal int Indent { get; private set; }
	}
}
