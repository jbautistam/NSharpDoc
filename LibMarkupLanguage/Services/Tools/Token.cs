using System;

namespace Bau.Libraries.LibMarkupLanguage.Services.Tools
{
	/// <summary>
	///		Datos de un token
	/// </summary>
	internal class Token
	{ 
		/// <summary>
		///		Tipo de token
		/// </summary>
		internal enum TokenType
		{
			/// <summary>Desconocido. No se debería utilizar</summary>
			Unknown,
			/// <summary>Definido en la colección de definiciones de token</summary>
			Defined,
			/// <summary>Valor numérico</summary>
			Numeric,
			/// <summary>Fin de archivo</summary>
			Eof
		}
			
		/// <summary>
		///		Tipo de token del sistema
		/// </summary>
		internal TokenType SystemType { get; set; }
		
		/// <summary>
		///		Tipo
		/// </summary>
		internal TokenDefinition Definition { get; set; }
		
		/// <summary>
		///		Contenido
		/// </summary>
		internal string Lexema { get; set; }
		
		/// <summary>
		///		Número de línea
		/// </summary>
		internal int Line { get; set; }
	}
}
