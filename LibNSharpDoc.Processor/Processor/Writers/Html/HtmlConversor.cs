using System;

using Bau.Libraries.LibHelper.Extensors;
using Bau.Libraries.LibMarkupLanguage;

namespace Bau.Libraries.LibNSharpDoc.Processor.Processor.Writers.Html
{
	/// <summary>
	///		Conversor a NHaml
	/// </summary>
	public class HtmlConversor
	{ // Variables privadas
			private HtmlBuilder objBuilder = new HtmlBuilder();

		/// <summary>
		///		Convierte una serie de nodos XML en una cadena NHaml
		/// </summary>
		internal string Convert(string strActualPath, MLIntermedialBuilder objMLBuilder)
		{ // Guarda el generador
				MLBuilder = objMLBuilder;
			// Limpia el contenido
				objBuilder.Clear();
			// Convierte los nodos
				foreach (MLNode objMLNode in MLBuilder.Root.Nodes)
					ConvertNode(strActualPath, objMLNode);
			// Devuelve la cadena
				return objBuilder.GetHtml();
		}

		/// <summary>
		///		Convierte los nodos
		/// </summary>
		private void ConvertNode(string strActualPath, MLNode objMLNode)
		{	if (MLBuilder.CheckIsEmpty(objMLNode))
				{ if (!MLBuilder.CheckIsSpanNode(objMLNode))
						objBuilder.AddTag(GetStartTag(objMLNode, true));
				}
			else if (MLBuilder.CheckIsLinkNode(objMLNode))
				objBuilder.AddTag(GetLinkTag(strActualPath, objMLNode));
			else if (MLBuilder.CheckIsSpanNode(objMLNode))
				objBuilder.AddTag(ConvertSpansText(strActualPath, objMLNode));
			else 
				{ // Crea la etiqueta y le añade la indentación
						objBuilder.AddTag(GetStartTag(objMLNode), objMLNode.Value);
						objBuilder.AddIndent();
					// Crea los nodos hijo
						foreach (MLNode objMLChild in objMLNode.Nodes)
							ConvertNode(strActualPath, objMLChild);
					// Quita la indentación
						objBuilder.RemoveIndent();
						objBuilder.AddTag(GetEndTag(objMLNode.Name));
				}
		}

		/// <summary>
		///		Obtiene una etiqueta de inicio
		/// </summary>
		private string GetStartTag(MLNode objMLNode, bool blnIsAutoClose = false)
		{ string strTag;
			string strAttributes = "";

				// Inicializa la etiqueta
					strTag = "<" + objMLNode.Name;
				// Añade los atributos
					foreach (MLAttribute objMLAttribute in objMLNode.Attributes)
						strAttributes = strAttributes.AddWithSeparator(objMLAttribute.Name + "=\"" + objMLAttribute.Value + "\"", " ", false);
					if (!strAttributes.IsEmpty())
						strTag += " " + strAttributes;
				// Añade el carácter de cierre
					if (blnIsAutoClose)
						strTag += "/>";
					else
						strTag += ">";
				// Devuelve la etiqueta
					return strTag;
		}

		/// <summary>
		///		Obtiene una etiqueta de fin
		/// </summary>
		private string GetEndTag(string strTag)
		{ return "</" + strTag + ">";
		}

		/// <summary>
		///		Obtiene el texto de los spans de un nodo
		/// </summary>
		private string ConvertSpansText(string strActualPath, MLNode objMLNode)
		{ string strText = "";

				// Crea el texto a partir de los nodos de span
					if (objMLNode.Nodes.Count == 0)
						strText = GetSpanText(objMLNode.Value, MLBuilder.CheckIsBold(objMLNode), MLBuilder.CheckIsItalic(objMLNode));
					else
						foreach (MLNode objMLChild in objMLNode.Nodes)
							if (MLBuilder.CheckIsSpanNode(objMLChild))
								strText = strText.AddWithSeparator(GetSpanText(objMLChild.Value, 
																															 MLBuilder.CheckIsBold(objMLNode) || MLBuilder.CheckIsBold(objMLChild),
																															 MLBuilder.CheckIsItalic(objMLNode) || MLBuilder.CheckIsItalic(objMLChild)), 
																									 " ", false);
							else if (MLBuilder.CheckIsLinkNode(objMLChild))
								strText = strText.AddWithSeparator(GetLinkTag(strActualPath, objMLChild), " ", false);
							else
								strText = strText.AddWithSeparator(objMLChild.Value, " ", false);
					// Devuelve el texto convertido
						return strText;
		}

		/// <summary>
		///		Obtiene el texto de un span
		/// </summary>
		private string GetSpanText(string strText, bool blnIsBold, bool blnIsItalic)
		{ string strTagStart = "", strTagEnd = "";

				// Añade las etiquetas de apertura y cierre necesario
					if (!strText.IsEmpty())
						{ if (blnIsBold)
								{	strTagStart += "<strong>";
									strTagEnd = "</strong>" + strTagEnd;
								}
							if (blnIsItalic)
								{ strTagStart += "<em>";
									strTagEnd += "</em>" + strTagEnd;
								}
						}
				// Devuelve el texto
					return strTagStart + strText + strTagEnd;
		}

		/// <summary>
		///		Obtiene la etiqueta de hipervínculo
		/// </summary>
		private string GetLinkTag(string strActualPath, MLNode objMLNode)
		{ return string.Format("<a href='{0}'>{1}</a>", GetHtmlFileName(strActualPath, objMLNode), objMLNode.Value);
		}

		/// <summary>
		///		Obtiene el nombre de archivo HTML de un vínculo
		/// </summary>
		private string GetHtmlFileName(string strActualPath, MLNode objMLNode)
		{	string strHref = System.IO.Path.Combine(MLBuilder.GetHref(objMLNode), GetFileName(objMLNode.Value));

				// Obtiene la Url relativa
					if (!strActualPath.StartsWith("C:", StringComparison.CurrentCultureIgnoreCase))
						strHref = GetUrlRelative(System.IO.Path.Combine(GetFirstPath(strHref), strActualPath) + "\\index.htm", strHref);
					else
						{ string strRootPath = GetFirstPath(strHref);

								strHref = strHref.Right(strHref.Length - strRootPath.Length);
						}
				// Quita las barras iniciales
					while (!strHref.IsEmpty() && strHref.StartsWith("\\"))
						strHref = strHref.Substring(1);
				// Cambia los caracteres de separación
					if (!strHref.IsEmpty())
						strHref = strHref.Replace('\\', '/');
				// Devuelve la URL
					return strHref;
		}

		/// <summary>
		///		Obtiene el primer directorio
		/// </summary>
		private string GetFirstPath(string strPath)
		{ string [] arrStrPath = Split(strPath);

				// Obtiene el primer directorio
					if (arrStrPath.Length > 0)
						return arrStrPath[0];
					else
						return null;
		}

		/// <summary>
		///		Obtiene la Url relativa
		/// </summary>
		private string GetUrlRelative(string strUrlPage, string strUrlTarget)
		{ string [] arrStrURLPage = Split(strUrlPage);
			string [] arrStrURLTarget = Split(strUrlTarget);
			string strURL = "";
			int intIndex = 0, intIndexTarget;
			
				// Quita los directorios iniciales que sean iguales
					while (intIndex < arrStrURLTarget.Length - 1 &&
								 arrStrURLTarget[intIndex].Equals(arrStrURLPage[intIndex], StringComparison.CurrentCultureIgnoreCase))
						intIndex++;
				// Añade todos los .. que sean necesarios
					intIndexTarget = intIndex;
					while (intIndexTarget < arrStrURLPage.Length - 1)
						{ // Añade el salto
								strURL += "../";
							// Incrementa el índice
								intIndexTarget++;
						}
				// Añade los archivos finales
					while (intIndex < arrStrURLTarget.Length)
						{ // Añade el directorio
								strURL += arrStrURLTarget[intIndex];
							// Añade el separador
								if (intIndex < arrStrURLTarget.Length - 1)
									strURL += "/";
							// Incrementa el índice
								intIndex++;
						}
				// Devuelve la URL
					return strURL;
		}

		/// <summary>
		///		Parte una cadena por \ o por /
		/// </summary>
		private string [] Split(string strUrl)
		{ if (strUrl.IsEmpty())
				return new string [] { "" };
			else if (strUrl.IndexOf('/') >= 0)
				return strUrl.Split('/');
			else
				return strUrl.Split('\\');
		}

		/// <summary>
		///		Obtiene el nombre de archivo HTML
		/// </summary>
		internal string GetFileName(string strName)
		{ return LibHelper.Files.HelperFiles.Normalize(strName + ".htm", false);
		}

		/// <summary>
		///		Generador
		/// </summary>
		private MLIntermedialBuilder MLBuilder { get; set; }
	}
}
