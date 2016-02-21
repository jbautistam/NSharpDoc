using System;

namespace Bau.Libraries.LibMarkupLanguage.Services
{
	/// <summary>
	///		Factory para las servicios de escritura
	/// </summary>
	public class WriterFactory
	{
		/// <summary>
		///		Obtiene una instancia de un servicio de escritura
		/// </summary>
		public IWriter GetInstance(MLSerializer.SerializerType intType, bool blnIncludeComments = false)
		{ switch (intType)
				{ case MLSerializer.SerializerType.XML:
						return new XML.XMLWriter();
					case MLSerializer.SerializerType.JSON:
						return null; // new JSON.JsonParser();
					default:
						throw new ParserException("Tipo de intérprete desconocido");
				}
		}
	}
}
