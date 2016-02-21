using System;
using System.Collections.Generic;

namespace Bau.Libraries.LibMarkupLanguage.Services.Tools
{
	/// <summary>
	///		Colección de <see cref="TokenDefinition"/>
	/// </summary>
	internal class TokenDefinitionsCollection : List<TokenDefinition>
	{ 
		/// <summary>
		///		Añade un token a la colección para datos numéricos
		/// </summary>
		internal void Add(int? intType, string strName)
		{ Add(intType, strName, null, null);
		}

		/// <summary>
		///		Añade un token a la colección
		/// </summary>
		internal void Add(int? intType, string strName, string strStart)
		{ Add(intType, strName, strStart, null);
		}

		/// <summary>
		///		Añade un token a la colección
		/// </summary>
		internal void Add(int? intType, string strName, string strStart, string strEnd)
		{ Add(new TokenDefinition(intType, strName, strStart, strEnd));
		}

		/// <summary>
		///		Comprueba si existe algún token para los valores numéricos
		/// </summary>
		internal bool ExistsNumeric()
		{ return SearchNumeric() != null;
		}

		/// <summary>
		///		Obtiene la definición de token para los valores numéricos
		/// </summary>
		internal TokenDefinition SearchNumeric()
		{ // Recorre la colección
				foreach (TokenDefinition objDefinition in this)
					if (objDefinition.IsNumeric)
						return objDefinition;
			// Si ha llegado hasta aquí es porque no ha encontrado ninguna definición para tokens numéricos
				return null;
		}
	}
}
