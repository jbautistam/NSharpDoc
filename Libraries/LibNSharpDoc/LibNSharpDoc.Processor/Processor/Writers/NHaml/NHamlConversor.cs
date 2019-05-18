using System;

using Bau.Libraries.LibCommonHelper.Extensors;
using Bau.Libraries.LibMarkupLanguage;

namespace Bau.Libraries.LibNSharpDoc.Processor.Processor.Writers.NHaml
{
	/// <summary>
	///		Conversor a NHaml
	/// </summary>
	public class NHamlConversor
	{ 
		// Variables privadas
		private NHamlBuilder builder = new NHamlBuilder();

		/// <summary>
		///		Convierte una serie de nodos XML en una cadena NHaml
		/// </summary>
		internal string Convert(MLIntermedialBuilder builderML)
		{	
			// Guarda el generador
			MLBuilder = builderML;
			// Limpia el contenido
			builder.Clear();
			// Convierte los nodos
			foreach (MLNode nodeML in MLBuilder.Root.Nodes)
				ConvertNode(nodeML);
			// Devuelve la cadena
			return builder.ToString();
		}

		/// <summary>
		///		Convierte los nodos
		/// </summary>
		private void ConvertNode(MLNode nodeML)
		{
			if (MLBuilder.CheckIsEmpty(nodeML))
			{
				if (!MLBuilder.CheckIsSpanNode(nodeML))
					AddNodeTag(nodeML, null);
			}
			else if (MLBuilder.CheckIsLinkNode(nodeML))
				builder.AddText(GetNodeLink(nodeML));
			else if (MLBuilder.CheckIsSpanNode(nodeML))
				AddNodeSpan(nodeML);
			else
			{ 
				// Crea la etiqueta y le añade la indentación
				AddNodeTag(nodeML, nodeML.Value);
				builder.AddIndent();
				// Crea los nodos hijo
				foreach (MLNode childML in nodeML.Nodes)
					ConvertNode(childML);
				// Quita la indentación
				builder.RemoveIndent();
			}
		}

		/// <summary>
		///		Añade una etiqueta de un nodo con sus atributos
		/// </summary>
		private void AddNodeTag(MLNode nodeML, string text)
		{
			string tag = "%" + nodeML.Name;

				// Añade los atributos
				tag += GetAttributes(nodeML);
				// Añade el texto al generador
				builder.AddTag(tag, text);
		}

		/// <summary>
		///		Obtiene los atributos de un nodo formateados para NHaml
		/// </summary>
		private string GetAttributes(MLNode nodeML)
		{
			string attributes = "";

				// Añade los atributos del nodo
				foreach (MLAttribute attributeML in nodeML.Attributes)
					attributes = attributes.AddWithSeparator(ConvertAttribute(attributeML), " ", false);
				// Añade las llaves
				if (!attributes.IsEmpty())
					attributes = " {" + attributes + " }";
				// Devuelve los atributos
				return attributes;
		}

		/// <summary>
		///		Obtiene el texto de los spans de un nodo
		/// </summary>
		private void AddNodeSpan(MLNode nodeML)
		{
			string text = "";

				// Crea el texto a partir de los nodos span
				if (nodeML.Nodes.Count == 0)
					text = ConvertSpanText(nodeML);
				else
					foreach (MLNode childML in nodeML.Nodes)
						if (MLBuilder.CheckIsSpanNode(childML))
							text = text.AddWithSeparator(ConvertSpanText(childML), " ", false);
						else if (MLBuilder.CheckIsLinkNode(childML))
							text = text.AddWithSeparator(GetNodeLink(childML), " ", false);
						else
							text = text.AddWithSeparator(childML.Value, " ", false);
				// Escribe el texto
				builder.AddText(text);
		}

		/// <summary>
		///		Convierte el texto del span
		/// </summary>
		private string ConvertSpanText(MLNode nodeML)
		{
			string startTag = "";
			string endTag = "";
			string attributes = "";

				// Añade el texto que indica si está en negrita o en cursiva
				if (MLBuilder.CheckIsBold(nodeML) || MLBuilder.CheckIsItalic(nodeML))
				{ 
					// Añade las etiquetas para el inicio y el fin
					if (MLBuilder.CheckIsBold(nodeML))
					{
						startTag = startTag.AddWithSeparator("#b", " ", false);
						endTag = endTag.AddWithSeparator("#", " ", false);
					}
					if (MLBuilder.CheckIsItalic(nodeML))
					{
						startTag = startTag.AddWithSeparator("#em", " ", false);
						endTag = endTag.AddWithSeparator("#", " ", false);
					}
				}
				// Añade los atributos
				foreach (MLAttribute attributeML in nodeML.Attributes)
					if (!MLBuilder.CheckIsBold(attributeML) && !MLBuilder.CheckIsItalic(attributeML))
						attributes = attributes.AddWithSeparator(ConvertAttribute(attributeML), " ", false);
				// Añade las llaves a los atributos
				if (!attributes.IsEmpty())
					attributes = " { " + attributes + " } ";
				// Devuelve el texto
				return (startTag + attributes + " " + nodeML.Value + " " + endTag).TrimIgnoreNull();
		}

		/// <summary>
		///		Añade un nodo con el vínculo
		/// </summary>
		private string GetNodeLink(MLNode nodeML)
		{
			string attributes = "";

				// Asigna los atributos
				foreach (MLAttribute attributeML in nodeML.Attributes)
					if (!MLBuilder.CheckIsHref(attributeML))
						attributes = attributes.AddWithSeparator(ConvertAttribute(attributeML), " ", false);
				// Añade el atributo con la referencia
				attributes = attributes.AddWithSeparator(ConvertAttribute("href", MLBuilder.GetHref(nodeML)), " ", false);
				// Añade las llaves de los atributos
				if (!attributes.IsEmpty())
					attributes = "{ " + attributes + " }";
				// Añade el nodo con el vínculo
				return string.Format("#a {0} {1}#", attributes, nodeML.Value);
		}

		/// <summary>
		///		Convierte un atributo
		/// </summary>
		private string ConvertAttribute(MLAttribute attributeML)
		{
			return ConvertAttribute(attributeML.Name, attributeML.Value);
		}

		/// <summary>
		///		Convierte un atributo
		/// </summary>
		private string ConvertAttribute(string name, string value)
		{
			return string.Format("{0} = \"{1}\"", name, value);
		}

		/// <summary>
		///		Generador
		/// </summary>
		private MLIntermedialBuilder MLBuilder { get; set; }
	}
}
