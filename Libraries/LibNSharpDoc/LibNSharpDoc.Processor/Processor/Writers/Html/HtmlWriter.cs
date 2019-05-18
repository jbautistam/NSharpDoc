using System;
using System.Text;
using System.Text.RegularExpressions;

using Bau.Libraries.LibCommonHelper.Extensors;
using Bau.Libraries.LibNSharpDoc.Processor.Models.Documents;

namespace Bau.Libraries.LibNSharpDoc.Processor.Processor.Writers.Html
{
	/// <summary>
	///		Generador de un archivo HTML para documentación
	/// </summary>
	public class HtmlWriter : IDocumentWriter
	{ 
		// Variables privadas
		private HtmlConversor conversor = new HtmlConversor();

		/// <summary>
		///		Graba la documentación Html en un archivo
		/// </summary>
		public void Save(DocumentFileModel document, MLIntermedialBuilder builderML, string rootFileNameTemplate, string path)
		{
			Save(document.Name, 
				 ConvertMLBuilder(document, path, document.Name, document.Name, builderML, rootFileNameTemplate),
				 System.IO.Path.Combine(path, document.GetPathLocal()));
		}

		/// <summary>
		///		Convierte una estructura XML en HTML
		/// </summary>
		private string ConvertMLBuilder(DocumentFileModel document, string rootPath,
										string title, string description, MLIntermedialBuilder builderML, string fileNameTemplate)
		{
			string result = new Repository.Templates.TemplateRepository().LoadTextRootTemplate(fileNameTemplate);
			string html = conversor.Convert(rootPath, builderML);

				// Convierte los vínculos del cuerpo HTML	
				html = UpdateLinksBody(System.IO.Path.Combine(rootPath, "filler.htm"), html);
				// Reemplaza los datos de la plantilla de entrada
				if (result.IsEmpty())
					result = html;
				else
				{ 
					// Asigna el título y la descripción
					result = result.ReplaceWithStringComparison("{{Title}}", title);
					result = result.ReplaceWithStringComparison("{{Description}}", description);
					// Crea el vínculo a la página superior
					if (document != null && document.Parent != null)
						result = result.ReplaceWithStringComparison("{{TopPage}}", string.Format("<a href='{0}'>{1}</a>",
																	GetUrl(document.Parent, ""),
																	document.Parent.Name));
					else
						result = result.ReplaceWithStringComparison("{{TopPage}}", "");
					// Cambia los vínculos
					result = UpdateLinks(System.IO.Path.Combine(rootPath, "filler.htm"), result);
					// Asigna el cuerpo
					result = result.ReplaceWithStringComparison("{{Body}}", html);
				}
				// Devuelve el texto
				return result;
		}

		/// <summary>
		///		Obtiene la Url a un documento
		/// </summary>
		private string GetUrl(DocumentFileModel document, string rootPath)
		{
			string url = document.GetUrl(rootPath);

				// Añade el nombre de página
				url = System.IO.Path.Combine(url, conversor.GetFileName(document.Name));
				// Devuelve la URL cambiando los separadores
				return url.Replace('\\', '/');
		}

		/// <summary>
		///		Modifica los vínculos
		/// </summary>
		private string UpdateLinks(string urlActualDocument, string content)
		{
			StringBuilder builder = new StringBuilder();
			Match match = Regex.Match(content, @"\s*(href|src)\s*=\s*(?:[""'](?<1>[^""']*)[""']|(?<1>\S+))"); ;
			int lastPosition = 0;

				// Mientras se encuentre una cadena
				while (match.Success)
				{   
					// Añade a la cadena de salida lo anterior
					builder.Append(content.Substring(lastPosition, match.Index - lastPosition));
					// Añade el contenido
					builder.Append(ReplaceInner(urlActualDocument, match.Value));
					// Guarda la posición actual
					lastPosition = match.Index + match.Length;
					// Obtiene la siguiente coincidencia
					match = match.NextMatch();
				}
				// Añade el resto de la cadena
				if (lastPosition < content.Length)
					builder.Append(content.Substring(lastPosition));
				// Devuelve el contenido
				return builder.ToString();
		}

		/// <summary>
		///		Reemplaza la cadena interna
		/// </summary>
		private string ReplaceInner(string urlActualDocument, string content)
		{
			Match match = Regex.Match(content, @"[""'](?<1>[^""']*)[""']");
			string result = null;

				// Cambia la cadena entre comillas
				if (match.Success)
				{
					string urlTarget = match.Value.Trim().Substring(1, match.Value.Trim().Length - 2);

						// Añade el contenido anterior a la Url
						result = content.Substring(0, match.Index);
						// Transforma la URL
						result += ConvertUrl(urlActualDocument, urlTarget);
						// Añade el contenido siguiente a la Url
						result += content.Substring(match.Index + match.Length);
				}
				// Devuelve el resultado
				if (!result.IsEmpty())
					return result;
				else
					return content;
		}

		/// <summary>
		///		Modifica los vínculos del cuerpo
		/// </summary>
		private string UpdateLinksBody(string urlActualDocument, string html)
		{
			// Reemplaza los vínculos
			foreach (string url in html.Extract("\"##", "##\""))
				html = html.ReplaceWithStringComparison($"\"##{url}##\"", ConvertUrl(urlActualDocument, url));
			// Devuelve la cadena convertida
			return html;
		}

		/// <summary>
		///		Convierte una URL
		/// </summary>
		private string ConvertUrl(string urlActualDocument, string urlAbsolute)
		{
			if (urlAbsolute.StartsWith("http:", StringComparison.CurrentCultureIgnoreCase) ||
					urlAbsolute.StartsWith("https:", StringComparison.CurrentCultureIgnoreCase))
				return $"\"{urlAbsolute}\"";
			else
				return "\"" + conversor.GetUrlRelative(urlActualDocument, urlAbsolute) + "\"";
		}

		/// <summary>
		///		Graba un archivo HTML
		/// </summary>
		private void Save(string name, string text, string path)
		{
			string fileName = System.IO.Path.Combine(path, conversor.GetFileName(name));

				// Crea el directorio
				LibCommonHelper.Files.HelperFiles.MakePath(path);
				// Graba el archivo de documentación
				LibCommonHelper.Files.HelperFiles.SaveTextFile(fileName, text);
		}
	}
}
