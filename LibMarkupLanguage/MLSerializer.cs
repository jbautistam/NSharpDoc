using System;

namespace Bau.Libraries.LibMarkupLanguage
{
	/// <summary>
	///		Serializador
	/// </summary>
	public class MLSerializer
	{ // Enumerados públicos
			public enum SerializerType
				{ 
					/// <summary>Desconocido. No se debería utilizar</summary>
					Unknown,
					/// <summary>JSON</summary>
					JSON,
					/// <summary>XML</summary>
					XML
				}

		/// <summary>
		///		Interpreta un archivo de un tipo indeterminado
		/// </summary>
		public MLFile Parse(string strFileName, bool blnIncludeComments = false)
		{ if (strFileName.EndsWith(".xml", StringComparison.CurrentCultureIgnoreCase))
				return Parse(SerializerType.XML, strFileName, blnIncludeComments);
			else
				return Parse(SerializerType.JSON, strFileName, blnIncludeComments);
		}

		/// <summary>
		///		Interpreta un archivo de un tipo
		/// </summary>
		public MLFile Parse(SerializerType intType, string strFileName, bool blnIncludeComments = false)
		{ return GetParser(intType, blnIncludeComments).Parse(strFileName);
		}

		/// <summary>
		///		Interpreta un archivo de un tipo indeterminado
		/// </summary>
		public MLFile ParseText(string strText, bool blnIncludeComments = false)
		{ if (strText.StartsWith("<?xml ", StringComparison.CurrentCultureIgnoreCase))
				return ParseText(SerializerType.XML, strText, blnIncludeComments);
			else
				return ParseText(SerializerType.JSON, strText, blnIncludeComments);
		}

		/// <summary>
		///		Interpreta un archivo de un tipo
		/// </summary>
		public MLFile ParseText(SerializerType intType, string strText, bool blnIncludeComments = false)
		{ return GetParser(intType, blnIncludeComments).ParseText(strText);
		}

		/// <summary>
		///		Obtiene un parser determinado
		/// </summary>
		private Services.IParser GetParser(SerializerType intType, bool blnIncludeComments = false)
		{ return new Services.ParserFactory().GetInstance(intType, blnIncludeComments);
		}

		/// <summary>
		///		Graba los datos en un archivo
		/// </summary>
		public void Save(SerializerType intType, MLFile objMLFile, string strFileName)
		{ GetWriter(intType).Save(objMLFile, strFileName);
		}

		/// <summary>
		///		Convierte los datos de un archivo a una cadena
		/// </summary>
		public string ConvertToString(SerializerType intType, MLFile objMLFile)
		{ return GetWriter(intType).ConverToString(objMLFile);
		}

		/// <summary>
		///		Obtiene un parser determinado
		/// </summary>
		private Services.IWriter GetWriter(SerializerType intType)
		{ return new Services.WriterFactory().GetInstance(intType);
		}	
	}
}
