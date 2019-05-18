using System;
using System.Collections.Generic;

using Bau.Libraries.LibCommonHelper.Extensors;
using Bau.Libraries.LibMarkupLanguage;
using Bau.Libraries.LibNSharpDoc.Processor.Models.Documents;

namespace Bau.Libraries.LibNSharpDoc.Processor.Processor.Writers
{
	/// <summary>
	///		Clase interna de generación en nodos ML
	/// </summary>
	public class MLIntermedialBuilder
	{ 
		// Constantes privadas
		private const string TagRoot = "Page";
		private const string TagSpan = "Part";
		private const string TagBold = "IsBold";
		private const string TagFormat = "Format";
		private const string TagItalic = "IsItalic";
		private const string TagLink = "Link";
		private const string TagSearchLink = "SearchLink";
		private const string TagHref = "Ref";
		private const string FormatMarkdown = "Markdown";

		/// <summary>
		///		Limpia el constructor
		/// </summary>
		public void Clear()
		{
			Root = new MLNode(TagRoot);
		}

		/// <summary>
		///		Comprueba si un nodo está vacío
		/// </summary>
		public bool CheckIsEmpty(MLNode nodeML)
		{
			return nodeML.Nodes.Count == 0 && nodeML.Value.IsEmpty();
		}

		/// <summary>
		///		Comprueba si un nodo es complejo
		/// </summary>
		public bool CheckIsComplex(MLNode nodeML)
		{
			return nodeML.Nodes.Count > 0;
		}

		/// <summary>
		///		Comprueba si es un nodo de span
		/// </summary>
		public bool CheckIsSpanNode(MLNode nodeML)
		{
			return nodeML.Name == TagSpan;
		}

		/// <summary>
		///		Comprueba si es un nodo de span
		/// </summary>
		public bool CheckIsLinkNode(MLNode nodeML)
		{
			return nodeML.Name == TagLink;
		}

		/// <summary>
		///		Comprueba si un nodo de span está definido como negrita
		/// </summary>
		public bool CheckIsBold(MLNode nodeML)
		{
			return nodeML.Attributes[TagBold].Value.GetBool();
		}

		/// <summary>
		///		Obtiene los formatos asociado a un nodo
		/// </summary>
		public string[] GetFormats(MLNode nodeML)
		{
			string format = nodeML.Attributes[TagFormat].Value;

				if (string.IsNullOrWhiteSpace(format))
					return new string[0];
				else
					return format.Split('|');
		}

		/// <summary>
		///		Comprueba si un atributo se corresponde con la negrita
		/// </summary>
		public bool CheckIsBold(MLAttribute attributeML)
		{
			return attributeML.Name == TagBold;
		}

		/// <summary>
		///		Comprueba si un nodo de span está definido como cursiva
		/// </summary>
		public bool CheckIsItalic(MLNode nodeML)
		{
			return nodeML.Attributes[TagItalic].Value.GetBool();
		}

		/// <summary>
		///		Comprueba si un atributo se corresponde con la cursiva
		/// </summary>
		public bool CheckIsItalic(MLAttribute attributeML)
		{
			return attributeML.Name == TagItalic;
		}

		/// <summary>
		///		Comprueba si un atributo es una referencia
		/// </summary>
		public bool CheckIsHref(MLAttribute attributeML)
		{
			return attributeML.Name == TagHref;
		}

		/// <summary>
		///		Obtiene un span
		/// </summary>
		public MLNode GetSpan(string text, string format = null, bool bold = false, bool italic = false)
		{
			MLNode spanML = new MLNode(TagSpan, text);

				// Añade los atributos
				spanML.Attributes.Add(TagBold, bold);
				spanML.Attributes.Add(TagItalic, italic);
				spanML.Attributes.Add(TagFormat, format);
				// Devuelve el nodo
				return spanML;
		}

		/// <summary>
		///		Obtiene un vínculo
		/// </summary>
		public MLNode GetLink(string title, string url)
		{
			MLNode nodeML = new MLNode(TagLink, title);

				// Añade la referencia
				nodeML.Attributes.Add(TagHref, url);
				// Devuelve el nodo
				return nodeML;
		}

		/// <summary>
		///		Aplica los formatos al texto
		/// </summary>
		public string FormatText(string text, string[] formats)
		{
			// Aplica los formatos de texto
			if (!string.IsNullOrWhiteSpace(text))
				foreach (string format in formats)
					if (!string.IsNullOrWhiteSpace(format))
					{
						if (format.EqualsIgnoreCase(FormatMarkdown))
							text = ConvertMarkdown(text);
					}
			// Devuelve el texto formateado
			return text;
		}

		/// <summary>
		///		Convierte el texto a HTML utilizando Markdown
		/// </summary>
		private string ConvertMarkdown(string text)
		{
			try
			{
				return Markdig.Markdown.ToHtml(RemoveFirstTab(text));
			}
			catch (Exception exception)
			{
				System.Diagnostics.Debug.WriteLine($"Error when parse: {text}. {exception.Message}");
				return text;
			}
		}

		/// <summary>
		///		Cuando la cadena está en formato Markdown, le quita el primer tabulador a todas
		///	las líneas si la primera línea comienza por un tabulador
		/// </summary>
		private string RemoveFirstTab(string text)
		{
			return text.ReplaceWithStringComparison("\n\t", "\n");
		}

		/// <summary>
		///		Obtiene un vínculo para un elemento que se va a sustituir por otro en el postproceso
		/// </summary>
		/// <returns>
		///		Cuando se añade información como un nombre de tipo, nos interesa poder generar un
		///	hipervínculo pero aún no sabemos en qué documento se ha creado la información, por eso
		///	creamos un nodo de "vínculo a postprocesar" para modificarlo una vez generados todos los
		///	documentos
		/// </returns>
		public MLNode GetSearchLink(string title, string urlSearch)
		{
			MLNode nodeML = new MLNode(TagSearchLink, title);

				// Añade la referencia
				nodeML.Attributes.Add(TagHref, urlSearch);
				// Devuelve el nodo
				return nodeML;
		}

		/// <summary>
		///		Obtiene el valor del atributo href de un hipervínculo
		/// </summary>
		public string GetHref(MLNode nodeML)
		{
			return nodeML.Attributes[TagHref].Value;
		}

		/// <summary>
		///		Transforma los vínculos de búsqueda
		/// </summary>
		internal void TransformSeachLinks(DocumentFileModel document, Dictionary<string, DocumentFileModel> links, string pathBase)
		{
			foreach (MLNode nodeML in Root.Nodes)
				TransformSeachLinks(document, links, nodeML, pathBase);
		}

		/// <summary>
		///		Transforma los vínculos de búsqueda
		/// </summary>
		private void TransformSeachLinks(DocumentFileModel document, Dictionary<string, DocumentFileModel> links, MLNode nodeML, string pathBase)
		{
			if (nodeML.Name == TagSearchLink)
			{
				string tagLink = nodeML.Attributes[TagHref].Value;
				DocumentFileModel documentTarget;

					// Obtiene la referencia
					if (links.TryGetValue(tagLink, out documentTarget))
					{
						nodeML.Name = TagLink;
						nodeML.Attributes[TagHref].Value = documentTarget.GetUrl(pathBase);
					}
					else
						nodeML.Name = TagSpan;
			}
			else
				foreach (MLNode childML in nodeML.Nodes)
					TransformSeachLinks(document, links, childML, pathBase);
		}

		/// <summary>
		///		Nodo raíz
		/// </summary>
		public MLNode Root { get; private set; } = new MLNode(TagRoot);
	}
}
