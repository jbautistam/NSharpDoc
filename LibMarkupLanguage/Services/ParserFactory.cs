using System;

namespace Bau.Libraries.LibMarkupLanguage.Services
{
	/// <summary>
	///		Factory para los intérpretes
	/// </summary>
	public class ParserFactory
	{
		/// <summary>
		///		Obtiene una instancia de un intérprete
		/// </summary>
		public IParser GetInstance(MLSerializer.SerializerType intType, bool blnIncludeComments = false)
		{ switch (intType)
				{ case MLSerializer.SerializerType.XML:
						return new XML.XMLParser(blnIncludeComments);
					case MLSerializer.SerializerType.JSON:
						return new JSON.JsonParser();
					default:
						throw new ParserException("Tipo de intérprete desconocido");
				}
		}
	}
}
