using System;
using System.Text;

namespace Bau.Libraries.LibMarkupLanguage.Services.XML
{
	/// <summary>
	///		Clase de ayuda para generaci�n de XML
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
			// A�ade la cabecera
				if (blnAddHeader)
					sbXML.Append("<?xml version='1.0' encoding='utf-8'?>" + Environment.NewLine);
			// A�ade los nodos
				Add(0, objFile.Nodes);
		}

		/// <summary>
		///		A�ade los nodos
		/// </summary>
		private void Add(int intIndent, MLNodesCollection objColMLNodes)
		{ foreach (MLNode objMLNode in objColMLNodes)
				Add(intIndent, objMLNode);
		}
		
		/// <summary>
		///		A�ade los datos de un nodo
		/// </summary>
		private void Add(int intIndent, MLNode objMLNode)
		{ // Indentaci�n
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
								{ // A�ade un salto de l�nea
										sbXML.Append(Environment.NewLine);
									// A�ade el CData
										if (objMLNode.Value.IndexOf("<![CDATA[") < 0) // ... si la cadena no ten�a ya un CDATA
											Add(intIndent + 1, "<![CDATA[" + objMLNode.Value + "]]>");
										else
											Add(intIndent + 1, objMLNode.Value);
									// Prepara la l�nea para el cierre
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
		///		A�ade la indentaci�n
		/// </summary>
		private void AddIndent(int intIndent)
		{	for (int intIndex = 0; intIndex < intIndent; intIndex++)
				sbXML.Append("\t");
		}
		
		/// <summary>
		///		A�ade un texto con indentaci�n
		/// </summary>
		private void Add(int intIndent, string strText)
		{ // A�ade la indentaci�n
				AddIndent(intIndent);
			// A�ade el texto
				sbXML.Append(strText);
		}
		
		/// <summary>
		///		A�ade el nombre de un elemento
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
						//strValue = strValue.Replace("�", "&aacute;");
						//strValue = strValue.Replace("�", "&eacute;");
						//strValue = strValue.Replace("�", "&iacute;");
						//strValue = strValue.Replace("�", "&oacute;");
						//strValue = strValue.Replace("�", "&uacute;");
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
