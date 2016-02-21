using System;
using System.Text;

namespace Bau.Libraries.LibMarkupLanguage.Services.XML
{
	/// <summary>
	///		Clase de ayuda para generación de XML
	/// </summary>
	public class XMLWriter : IWriter
	{ // Variables privadas
			private StringBuilder sbXML;

		/// <summary>
		///		Graba los datos de un MLFile en un archivo
		/// </summary>
		public void Save(MLFile objMLFile, string strFileName)
		{ using (System.IO.StreamWriter stmFile = new System.IO.StreamWriter(strFileName, false, System.Text.Encoding.UTF8))
				{ // Escribe la cadena
						stmFile.Write(ConverToString(objMLFile));
					// Cierra el stream
						stmFile.Close();
				}
		}

		/// <summary>
		///		Convierte los datos de un MLFile en una cadena
		/// </summary>
		public string ConverToString(MLFile objMLFile, bool blnAddHeader = true)
		{ // Crea el stringBuilder del archivo
				Create(objMLFile, blnAddHeader);
			// Devuelve la cadena
				return sbXML.ToString();
		}
		
		/// <summary>
		///		Crea el texto de un archivo
		/// </summary>
		private void Create(MLFile objFile, bool blnAddHeader)
		{ // Limpia el contenido
				sbXML = new StringBuilder();
			// Añade la cabecera
				if (blnAddHeader)
					sbXML.Append("<?xml version='1.0' encoding='utf-8'?>" + Environment.NewLine);
			// Añade los nodos
				Add(0, objFile.Nodes);
		}

		/// <summary>
		///		Añade los nodos
		/// </summary>
		private void Add(int intIndent, MLNodesCollection objColMLNodes)
		{ foreach (MLNode objMLNode in objColMLNodes)
				Add(intIndent, objMLNode);
		}
		
		/// <summary>
		///		Añade los datos de un nodo
		/// </summary>
		private void Add(int intIndent, MLNode objMLNode)
		{ // Indentación
				AddIndent(intIndent);
			// Cabecera
				sbXML.Append("<");
			// Nombre
				AddName(objMLNode);
			// Espacios de nombres
				Add(objMLNode.NameSpaces);
			// Atributos
				Add(objMLNode.Attributes);
			// Final y datos del nodo (en su caso)
				if (IsAutoClose(objMLNode))
					sbXML.Append("/>" + Environment.NewLine);
				else
					{ // Cierre de la etiqueta de apertura
							sbXML.Append(">");
						// Datos
							if (objMLNode.Nodes.Count > 0)
								{ sbXML.Append(Environment.NewLine);
									Add(intIndent + 1, objMLNode.Nodes);
								}
							else if (objMLNode.IsCData)
								{ // Añade un salto de línea
										sbXML.Append(Environment.NewLine);
									// Añade el CData
										if (objMLNode.Value.IndexOf("<![CDATA[") < 0) // ... si la cadena no tenía ya un CDATA
											Add(intIndent + 1, "<![CDATA[" + objMLNode.Value + "]]>");
										else
											Add(intIndent + 1, objMLNode.Value);
									// Prepara la línea para el cierre
										sbXML.Append(Environment.NewLine);
										AddIndent(intIndent);
								}
							else if (!string.IsNullOrEmpty(objMLNode.Value))
								sbXML.Append(objMLNode.Value);
						// Cierre
							sbXML.Append("</");
							AddName(objMLNode);
							sbXML.Append(">" + Environment.NewLine);
					}
		}

		/// <summary>
		///		Añade la indentación
		/// </summary>
		private void AddIndent(int intIndent)
		{	for (int intIndex = 0; intIndex < intIndent; intIndex++)
				sbXML.Append("\t");
		}
		
		/// <summary>
		///		Añade un texto con indentación
		/// </summary>
		private void Add(int intIndent, string strText)
		{ // Añade la indentación
				AddIndent(intIndent);
			// Añade el texto
				sbXML.Append(strText);
		}
		
		/// <summary>
		///		Añade el nombre de un elemento
		/// </summary>
		private void AddName(MLItemBase objMLNode)
		{	// Espacio de nombres
				if (!string.IsNullOrEmpty(objMLNode.Prefix))
					sbXML.Append(objMLNode.Prefix + ":");
			// Nombre
				sbXML.Append(objMLNode.Name);
		}

		/// <summary>
		///		Espacios de nombres
		/// </summary>
		private void Add(MLNameSpacesCollection objColNameSpaces)
		{ foreach (MLNameSpace objNameSpace in objColNameSpaces)
				{ // Nombre
						sbXML.Append(" xmlns");
						if (!string.IsNullOrEmpty(objNameSpace.Prefix))
							sbXML.Append(":" + objNameSpace.Prefix);
					// Atributos
						sbXML.Append(" = \"" + objNameSpace.NameSpace + "\" ");
				}
		}

		/// <summary>
		///		Atributos
		/// </summary>
		private void Add(MLAttributesCollection objColAttributes)
		{ foreach (MLAttribute objAttribute in objColAttributes)
				sbXML.Append(" " + objAttribute.Name + " = \"" + EncodeHTML(objAttribute.Value) + "\" ");
		}
		
		/// <summary>
		///		Codifica una cadena HTML quitando los caracteres raros
		/// </summary>
		private string EncodeHTML(string strValue)
		{ // Quita los caracteres raros
				if (!string.IsNullOrEmpty(strValue))
					{	strValue = strValue.Replace("&", "&amp;");
						strValue = strValue.Replace("<", "&lt;");
						strValue = strValue.Replace(">", "&gt;");
						strValue = strValue.Replace("\"", "&quot;");
						//strValue = strValue.Replace("á", "&aacute;");
						//strValue = strValue.Replace("é", "&eacute;");
						//strValue = strValue.Replace("í", "&iacute;");
						//strValue = strValue.Replace("ó", "&oacute;");
						//strValue = strValue.Replace("ú", "&uacute;");
					}
			// Devuelve la cadena
				return strValue;
		}

		/// <summary>
		///		Indica que es un nodo que se debe autocerrar
		/// </summary>
		private bool IsAutoClose(MLNode objMLNode)
		{ return string.IsNullOrEmpty(objMLNode.Value) && (objMLNode.Nodes == null || objMLNode.Nodes.Count == 0);
		}		
	}
}
