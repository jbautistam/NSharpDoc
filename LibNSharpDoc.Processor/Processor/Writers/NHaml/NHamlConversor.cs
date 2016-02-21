using System;

using Bau.Libraries.LibHelper.Extensors;
using Bau.Libraries.LibMarkupLanguage;

namespace Bau.Libraries.LibNSharpDoc.Processor.Processor.Writers.NHaml
{
	/// <summary>
	///		Conversor a NHaml
	/// </summary>
	public class NHamlConversor
	{ // Variables privadas
			private NHamlBuilder objBuilder = new NHamlBuilder();

		/// <summary>
		///		Convierte una serie de nodos XML en una cadena NHaml
		/// </summary>
		internal string Convert(MLIntermedialBuilder objMLBuilder)
		{ // Guarda el generador
				MLBuilder = objMLBuilder;
			// Limpia el contenido
				objBuilder.Clear();
			// Convierte los nodos
				foreach (MLNode objMLNode in MLBuilder.Root.Nodes)
					ConvertNode(objMLNode);
			// Devuelve la cadena
				return objBuilder.ToString();
		}

		/// <summary>
		///		Convierte los nodos
		/// </summary>
		private void ConvertNode(MLNode objMLNode)
		{	if (MLBuilder.CheckIsEmpty(objMLNode))
				{ if (!MLBuilder.CheckIsSpanNode(objMLNode))
						AddNodeTag(objMLNode, null);
				}
			else if (MLBuilder.CheckIsLinkNode(objMLNode))
				objBuilder.AddText(GetNodeLink(objMLNode));
			else if (MLBuilder.CheckIsSpanNode(objMLNode))
				AddNodeSpan(objMLNode);
			else
				{ // Crea la etiqueta y le añade la indentación
						AddNodeTag(objMLNode, objMLNode.Value);
						objBuilder.AddIndent();
					// Crea los nodos hijo
						foreach (MLNode objMLChild in objMLNode.Nodes)
							ConvertNode(objMLChild);
					// Quita la indentación
						objBuilder.RemoveIndent();
				}
		}

		/// <summary>
		///		Añade una etiqueta de un nodo con sus atributos
		/// </summary>
		private void AddNodeTag(MLNode objMLNode, string strText)
		{ string strTag = "%" + objMLNode.Name;

				// Añade los atributos
					strTag += GetAttributes(objMLNode);
				// Añade el texto al generador
					objBuilder.AddTag(strTag, strText);
		}

		/// <summary>
		///		Obtiene los atributos de un nodo formateados para NHaml
		/// </summary>
		private string GetAttributes(MLNode objMLNode)
		{ string strAttributes = "";

				// Añade los atributos del nodo
					foreach (MLAttribute objMLAttribute in objMLNode.Attributes)
						strAttributes = strAttributes.AddWithSeparator(ConvertAttribute(objMLAttribute), " ", false);
				// Añade las llaves
					if (!strAttributes.IsEmpty())
						strAttributes = " {" + strAttributes + " }";
				// Devuelve los atributos
					return strAttributes;
		}

		/// <summary>
		///		Obtiene el texto de los spans de un nodo
		/// </summary>
		private void AddNodeSpan(MLNode objMLNode)
		{ string strText = "";

				// Crea el texto a partir de los nodos span
					if (objMLNode.Nodes.Count == 0)
						strText = ConvertSpanText(objMLNode);
					else
						foreach (MLNode objMLChild in objMLNode.Nodes)
							if (MLBuilder.CheckIsSpanNode(objMLChild))
								strText = strText.AddWithSeparator(ConvertSpanText(objMLChild), " ", false);
							else if (MLBuilder.CheckIsLinkNode(objMLChild))
								strText = strText.AddWithSeparator(GetNodeLink(objMLChild), " ", false);
							else
								strText = strText.AddWithSeparator(objMLChild.Value, " ", false);
				// Escribe el texto
					objBuilder.AddText(strText);
		}

		/// <summary>
		///		Convierte el texto del span
		/// </summary>
		private string ConvertSpanText(MLNode objMLNode)
		{ string strStartTag = "";
			string strEndTag = "";
			string strAttributes = "";

				// Añade el texto que indica si está en negrita o en cursiva
					if (MLBuilder.CheckIsBold(objMLNode) || MLBuilder.CheckIsItalic(objMLNode))
						{ // Añade las etiquetas para el inicio y el fin
								if (MLBuilder.CheckIsBold(objMLNode))
									{ strStartTag = strStartTag.AddWithSeparator("#b", " ", false);
										strEndTag = strEndTag.AddWithSeparator("#", " ", false);
									}
								if (MLBuilder.CheckIsItalic(objMLNode))
									{ strStartTag = strStartTag.AddWithSeparator("#em", " ", false);
										strEndTag = strEndTag.AddWithSeparator("#", " ", false);
									}
						}
				// Añade los atributos
					foreach (MLAttribute objMLAttribute in objMLNode.Attributes)
						if (!MLBuilder.CheckIsBold(objMLAttribute) && !MLBuilder.CheckIsItalic(objMLAttribute))
							strAttributes = strAttributes.AddWithSeparator(ConvertAttribute(objMLAttribute), " ", false);
				// Añade las llaves a los atributos
					if (!strAttributes.IsEmpty())
						strAttributes = " { " + strAttributes + " } ";
				// Devuelve el texto
					return (strStartTag + strAttributes + " " + objMLNode.Value + " " + strEndTag).TrimIgnoreNull();
		}

		/// <summary>
		///		Añade un nodo con el vínculo
		/// </summary>
		private string GetNodeLink(MLNode objMLNode)
		{ string strAttributes = "";

				// Asigna los atributos
					foreach (MLAttribute objMLAttribute in objMLNode.Attributes)
						if (!MLBuilder.CheckIsHref(objMLAttribute))
							strAttributes = strAttributes.AddWithSeparator(ConvertAttribute(objMLAttribute), " ", false);
				// Añade el atributo con la referencia
					strAttributes = strAttributes.AddWithSeparator(ConvertAttribute("href", MLBuilder.GetHref(objMLNode)), " ", false);
				// Añade las llaves de los atributos
					if (!strAttributes.IsEmpty())
						strAttributes = "{ " + strAttributes + " }";
				// Añade el nodo con el vínculo
					return string.Format("#a {0} {1}#", strAttributes, objMLNode.Value);
		}

		/// <summary>
		///		Convierte un atributo
		/// </summary>
		private string ConvertAttribute(MLAttribute objMLAttribute)
		{ return ConvertAttribute(objMLAttribute.Name, objMLAttribute.Value);
		}

		/// <summary>
		///		Convierte un atributo
		/// </summary>
		private string ConvertAttribute(string strName, string strValue)
		{	return string.Format("{0} = \"{1}\"", strName, strValue);
		}

		/// <summary>
		///		Generador
		/// </summary>
		private MLIntermedialBuilder MLBuilder { get; set; }
	}
}
