using System;
using System.Xml;

namespace Bau.Libraries.LibMarkupLanguage.Services.XML
{
	/// <summary>
	///		Interpreta un archivo XML
	/// </summary>
	public class XMLParser : IParser
	{
		public XMLParser(bool blnIncludeComments = false)
		{ IncludeComments = blnIncludeComments;
		}

		/// <summary>
		///		Interpreta un archivo XML
		/// </summary>
		public MLFile Parse(string strFileName)
		{ MLFile objMLFile = new MLFile();
		
				// Lee los datos
					using (XmlReader objReader = Open(strFileName))
						{ XmlDocument objDocument = new XmlDocument();
						
								// Carga los datos
									if (objReader != null) // ... si hay algo en el archivo
										try
											{ // Carga el documento
													objDocument.Load(objReader);
												// Obtiene los nodos
													objMLFile = Load(objDocument);
											}
										catch (Exception objException)
											{ throw new ParserException("Error en la interpretación del archivo XML " + strFileName, objException);
											}
								// Cierra el reader
									objReader.Close();
						}
				// Devuelve el nodo principal
					return objMLFile;
		}

		/// <summary>
		///		Interpreta un texto XML
		/// </summary>
		public MLFile ParseText(string strText)
		{ MLFile objMLFile = new MLFile();
			XmlDocument objDocument = new XmlDocument();
						
				// Carga los datos
					try
						{ // Carga el documento
								objDocument.LoadXml(strText);
							// Obtiene los nodos
								objMLFile = Load(objDocument);
						}
					catch (Exception objException)
						{ throw new ParserException("Error en la interpretación del texto XML", objException);
						}
				// Devuelve el nodo principal
					return objMLFile;
		}

		/// <summary>
		///		Carga los nodos de un archivo XML
		/// </summary>
		public MLFile Load(string strFileName)
		{ return Parse(strFileName);
		}
		
		/// <summary>
		///		Abre un Reader XML sin comprobación de caracteres extraños
		/// </summary>
		private XmlReader Open(string strFileName)
		{ XmlReaderSettings objSettings = new XmlReaderSettings();
					
				// Carga el documento
					objSettings.CheckCharacters = false;
					objSettings.CloseInput = true;
					objSettings.DtdProcessing = DtdProcessing.Ignore;
					objSettings.ValidationType = ValidationType.None;
					objSettings.XmlResolver = null;
				// Devuelve el documento XML abierto
					return XmlReader.Create(strFileName, objSettings);
		}

		/// <summary>
		///		Carga los nodos de un archivo XML
		/// </summary>
		public MLFile Load(XmlDocument objXMLDocument)
		{ MLFile objMLFile = new MLFile();
		
				// Carga los nodos
					objMLFile.Nodes.Add(LoadNodes(objXMLDocument.ChildNodes));
				// Devuelve el archivo
					return objMLFile;
		}

		/// <summary>
		///		Carga una colección de nodos
		/// </summary>
		private MLNodesCollection LoadNodes(XmlNodeList objColXMLNodes)
		{ MLNodesCollection objColNodes = new MLNodesCollection();
		
		    // Lee los nodos
		      foreach (XmlNode objXMLNode in objColXMLNodes)
						if (MustInclude(objXMLNode))
							objColNodes.Add(LoadNode(objXMLNode));
		    // Devuelve la colección de nodos
		      return objColNodes;
		}

		/// <summary>
		///		Carga los datos de un nodo
		/// </summary>
		private MLNode LoadNode(XmlNode objXMLNode)
		{ MLNode objNode = new MLNode();
		
				// Asigna los valores
					objNode.Prefix = objXMLNode.Prefix;
					objNode.Name = objXMLNode.LocalName;
					objNode.Value = Decode(objXMLNode.InnerText);
				// Asigna los atributos
					objNode.Attributes.Add(LoadAttributes(objXMLNode.Attributes));
				// Asigna los espacios de nombres
					objNode.NameSpaces.AddRange(LoadNameSpaces(objXMLNode.Attributes));
				// Carga los nodos
					objNode.Nodes.Add(LoadNodes(objXMLNode.ChildNodes));
				// Devuelve el nodo
					return objNode;
		}

		/// <summary>
		///		Indica si se debe incluir un nodo
		/// </summary>
		private bool MustInclude(XmlNode objNode) 
		{ return objNode.NodeType == XmlNodeType.Element &&
								(IncludeComments || (!IncludeComments && !objNode.Name.Equals("#comment")));
		}

		/// <summary>
		///		Carga los atributos
		/// </summary>
		private MLAttributesCollection LoadAttributes(XmlAttributeCollection objColXMLAttributes)
		{ MLAttributesCollection objColAttributes = new MLAttributesCollection();
		
				// Carga los atributos
					if (objColXMLAttributes != null)
						foreach (XmlAttribute objXMLAttribute in objColXMLAttributes)
							if (objXMLAttribute.Prefix != "xmlns")
								{ MLAttribute objAttribute = objColAttributes.Add(objXMLAttribute.LocalName, 
																																	Decode(objXMLAttribute.InnerText));
								
										// Asigna los valores
											objAttribute.Prefix = objXMLAttribute.Prefix;
								}
				// Devuelve los atributos
					return objColAttributes;
		}

		/// <summary>
		///		Carga los espacios de nombres
		/// </summary>
		private MLNameSpacesCollection LoadNameSpaces(XmlAttributeCollection objColXMLAttributes)
		{ MLNameSpacesCollection objColNameSpaces = new MLNameSpacesCollection();
		
				// Carga los espacios de nombres
					if (objColXMLAttributes != null)
						foreach (XmlAttribute objXMLAttribute in objColXMLAttributes)
							if (objXMLAttribute.Prefix == "xmlns")
								{ MLNameSpace objNameSpace = new MLNameSpace(objXMLAttribute.LocalName, Decode(objXMLAttribute.InnerText));
								
										// Añade el espacio de nombres
											objColNameSpaces.Add(objNameSpace);
								}
				// Devuelve los espacios de nombres
					return objColNameSpaces;
		}
		
		/// <summary>
		///		Decodifica una cadena HTML
		/// </summary>
		private string Decode(string strValue)
		{ // Quita los caracteres raros
				if (!string.IsNullOrEmpty(strValue))
					{	strValue = strValue.Replace("&amp;", "&");
						strValue = strValue.Replace("&lt;", "<");
						strValue = strValue.Replace("&gt;", ">");
						strValue = strValue.Replace("&quot;", "\"");
						strValue = strValue.Replace("&aacute;", "á");
						strValue = strValue.Replace("&eacute;", "é");
						strValue = strValue.Replace("&iacute;", "í");
						strValue = strValue.Replace("&oacute;", "ó");
						strValue = strValue.Replace("&uacute;", "ú");
					}
			// Devuelve la cadena
				return strValue;
		}

		/// <summary>
		///		Indica si se deben incluir los comentarios en los nodos
		/// </summary>
		public bool IncludeComments { get; set; }
	}
}
