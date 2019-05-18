using System;

using Bau.Libraries.LibCommonHelper.Extensors;
using Bau.Libraries.LibMarkupLanguage;

namespace Bau.Libraries.LibNSharpDoc.Processor.Processor.Writers.Html
{
	/// <summary>
	///		Conversor a NHaml
	/// </summary>
	public class HtmlConversor
	{ 
		// Variables privadas
		private HtmlBuilder builder = new HtmlBuilder();

		/// <summary>
		///		Convierte una serie de nodos XML en una cadena NHaml
		/// </summary>
		internal string Convert(string actualPath, MLIntermedialBuilder builderML)
		{ 
			// Guarda el generador
			MLBuilder = builderML;
			// Limpia el contenido
			builder.Clear();
			// Convierte los nodos
			foreach (MLNode nodeML in MLBuilder.Root.Nodes)
				ConvertNode(actualPath, nodeML);
			// Devuelve la cadena
			return builder.GetHtml();
		}

		/// <summary>
		///		Convierte los nodos
		/// </summary>
		private void ConvertNode(string actualPath, MLNode nodeML)
		{
			if (MLBuilder.CheckIsEmpty(nodeML))
			{
				if (!MLBuilder.CheckIsSpanNode(nodeML))
					builder.AddTag(GetStartTag(nodeML, true));
			}
			else if (MLBuilder.CheckIsLinkNode(nodeML))
				builder.AddTag(GetLinkTag(actualPath, nodeML));
			else if (MLBuilder.CheckIsSpanNode(nodeML))
				builder.AddTag(ConvertSpansText(actualPath, nodeML));
			else
			{ 
				// Crea la etiqueta y le añade la indentación
				builder.AddTag(GetStartTag(nodeML), nodeML.Value);
				builder.AddIndent();
				// Crea los nodos hijo
				foreach (MLNode childML in nodeML.Nodes)
					ConvertNode(actualPath, childML);
				// Quita la indentación
				builder.RemoveIndent();
				builder.AddTag(GetEndTag(nodeML.Name));
			}
		}

		/// <summary>
		///		Obtiene una etiqueta de inicio
		/// </summary>
		private string GetStartTag(MLNode nodeML, bool isAutoClose = false)
		{
			string tag;
			string attributes = "";

				// Inicializa la etiqueta
				tag = "<" + nodeML.Name;
				// Añade los atributos
				foreach (MLAttribute attributeML in nodeML.Attributes)
					attributes = attributes.AddWithSeparator(attributeML.Name + "=\"" + attributeML.Value + "\"", " ", false);
				if (!attributes.IsEmpty())
					tag += " " + attributes;
				// Añade el carácter de cierre
				if (isAutoClose)
					tag += "/>";
				else
					tag += ">";
				// Devuelve la etiqueta
				return tag;
		}

		/// <summary>
		///		Obtiene una etiqueta de fin
		/// </summary>
		private string GetEndTag(string tag)
		{
			return "</" + tag + ">";
		}

		/// <summary>
		///		Obtiene el texto de los spans de un nodo
		/// </summary>
		private string ConvertSpansText(string actualPath, MLNode nodeML)
		{
			string text = "";

				// Crea el texto a partir de los nodos de span
				if (nodeML.Nodes.Count == 0)
					text = GetSpanText(nodeML.Value, MLBuilder.GetFormats(nodeML), 
									   MLBuilder.CheckIsBold(nodeML), MLBuilder.CheckIsItalic(nodeML));
				else
					foreach (MLNode childML in nodeML.Nodes)
						if (MLBuilder.CheckIsSpanNode(childML))
							text = text.AddWithSeparator(GetSpanText(childML.Value, MLBuilder.GetFormats(childML),
																	 MLBuilder.CheckIsBold(nodeML) || MLBuilder.CheckIsBold(childML),
																	 MLBuilder.CheckIsItalic(nodeML) || MLBuilder.CheckIsItalic(childML)),
														 " ", false);
						else if (MLBuilder.CheckIsLinkNode(childML))
							text = text.AddWithSeparator(GetLinkTag(actualPath, childML), " ", false);
						else
							text = text.AddWithSeparator(childML.Value, " ", false);
				// Devuelve el texto convertido
				return text;
		}

		/// <summary>
		///		Obtiene el texto de un span
		/// </summary>
		private string GetSpanText(string text, string[] formats, bool isBold, bool isItalic)
		{
			string tagStart = string.Empty, tagEnd = string.Empty;

				// Añade las etiquetas de apertura y cierre necesario
				if (!text.IsEmpty())
				{
					// Formatea el texto
					text = MLBuilder.FormatText(text, formats);
					// Añade las etiquetas de principio y fin si es negrita
					if (isBold)
					{
						tagStart += "<strong>";
						tagEnd = "</strong>" + tagEnd;
					}
					// Añade las etiquetas de principio y fin si es cursiva
					if (isItalic)
					{
						tagStart += "<em>";
						tagEnd += "</em>" + tagEnd;
					}
				}
				// Devuelve el texto
				return tagStart + text + tagEnd;
		}

		/// <summary>
		///		Obtiene la etiqueta de hipervínculo
		/// </summary>
		private string GetLinkTag(string actualPath, MLNode nodeML)
		{
			return string.Format("<a href='{0}'>{1}</a>", GetHtmlFileName(actualPath, nodeML), nodeML.Value);
		}

		/// <summary>
		///		Obtiene el nombre de archivo HTML de un vínculo
		/// </summary>
		private string GetHtmlFileName(string actualPath, MLNode nodeML)
		{
			string href = System.IO.Path.Combine(MLBuilder.GetHref(nodeML), GetFileName(nodeML.Value));

				// Obtiene la Url relativa
				if (!actualPath.StartsWith("C:", StringComparison.CurrentCultureIgnoreCase))
					href = GetUrlRelative(System.IO.Path.Combine(GetFirstPath(href), actualPath) + "\\index.htm", href);
				else
				{
					string rootPath = GetFirstPath(href);

						href = href.Right(href.Length - rootPath.Length);
				}
				// Quita las barras iniciales
				while (!href.IsEmpty() && href.StartsWith("\\"))
					href = href.Substring(1);
				// Cambia los caracteres de separación
				if (!href.IsEmpty())
					href = href.Replace('\\', '/');
				// Devuelve la URL
				return href;
		}

		/// <summary>
		///		Obtiene el primer directorio
		/// </summary>
		private string GetFirstPath(string path)
		{
			string[] paths = Split(path);

				// Obtiene el primer directorio
				if (paths.Length > 0)
					return paths[0];
				else
					return null;
		}

		/// <summary>
		///		Obtiene la Url relativa
		/// </summary>
		internal string GetUrlRelative(string urlPage, string urlTarget)
		{
			string[] urlPages = Split(urlPage);
			string[] urlTargets = Split(urlTarget);
			string url = "";
			int index = 0, indexTarget;

				// Quita los directorios iniciales que sean iguales
				while (index < urlTargets.Length - 1 &&
							 urlTargets[index].Equals(urlPages[index], StringComparison.CurrentCultureIgnoreCase))
					index++;
				// Añade todos los .. que sean necesarios
				indexTarget = index;
				while (indexTarget < urlPages.Length - 1)
				{ 
					// Añade el salto
					url += "../";
					// Incrementa el índice
					indexTarget++;
				}
				// Añade los archivos finales
				while (index < urlTargets.Length)
				{ 
					// Añade el directorio
					url += urlTargets[index];
					// Añade el separador
					if (index < urlTargets.Length - 1)
						url += "/";
					// Incrementa el índice
					index++;
				}
				// Devuelve la URL
				return url;
		}

		/// <summary>
		///		Parte una cadena por \ o por /
		/// </summary>
		private string[] Split(string url)
		{
			if (url.IsEmpty())
				return new string[] { "" };
			else if (url.IndexOf('/') >= 0)
				return url.Split('/');
			else
				return url.Split('\\');
		}

		/// <summary>
		///		Obtiene el nombre de archivo HTML
		/// </summary>
		internal string GetFileName(string name)
		{
			return LibCommonHelper.Files.HelperFiles.Normalize(name + ".htm", false);
		}

		/// <summary>
		///		Generador
		/// </summary>
		private MLIntermedialBuilder MLBuilder { get; set; }
	}
}
