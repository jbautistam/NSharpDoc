using System;

namespace Bau.Libraries.LibMarkupLanguage.Services.Tools
{
	/// <summary>
	///		Clase de interpretación de cadenas como token
	/// </summary>
	internal class ParserTokenizer
	{ // Constantes privadas
			private const string cnstStrCharsNumeric = "0123456789+-.";
			private static string [] cnstArrStrSpaces = new string[] {" ", "\r", "\n", "\t"};
		// Variables privadas
			private string strContent;
			private int intLine, intCharIndex;

		internal ParserTokenizer()
		{ TokensDefinitions = new TokenDefinitionsCollection();
		}		
		
		/// <summary>
		///		Inicializa los valores a interpretar
		/// </summary>
		internal void Init(string strContent)
		{ // Inicializa la cadena
				this.strContent = strContent;
			// Inicializa las variables
				intCharIndex = 0;
				intLine = 1;
		}
		
		/// <summary>
		///		Obtiene un token
		/// </summary>
		internal Token GetToken()
		{ Token objToken = new Token();
		
				// Quita los espacios
					ConsumeSpaces();
				// Recoge un token
					if (IsEof())
						objToken.SystemType = Token.TokenType.Eof;
					else
						{ bool blnEnd = false;
							TokenDefinition objTokenNumeric = TokensDefinitions.SearchNumeric();
						
								// Inicializa el tipo de token
									objToken.SystemType = Token.TokenType.Unknown;
								// Busca el token
									foreach (TokenDefinition objDefinition in TokensDefinitions)
										if (!blnEnd)
											{ if (objTokenNumeric != null && NextCharIsNumeric())
													{ // Asigna el tipo
															objToken.SystemType = Token.TokenType.Numeric;
															objToken.Definition = objTokenNumeric;
														// Asigna el número
															objToken.Lexema = ConsumeNumeric();
														// Indica que ha terminado
															blnEnd = true;
													}
												else if (!objDefinition.IsNumeric && GetChars(objDefinition.Start.Length) == objDefinition.Start)
													{ // Asigna el tipo
															objToken.SystemType = Token.TokenType.Defined;
															objToken.Definition = objDefinition;
														// Asigna el contenido
															objToken.Lexema = objDefinition.Start;
														// Consume la cadena
															Consume(objDefinition.Start);
														// Busca hasta el final
															if (!string.IsNullOrEmpty(objDefinition.End))
																{	// En el lexema mete el contenido
																		if (objDefinition.End == "\"")
																			objToken.Lexema = ConsumeString();
																		else
																			{ // Obtiene el contenido
																					objToken.Lexema = ConsumeTo(objDefinition.End);
																				// Consume la cadena de fin
																					Consume(objDefinition.End);
																			}
																}
														// Indica que se ha terminado
															blnEnd = true;
													}
											}
								// Si no se ha terminado consume hasta el siguiente espacio
									if (!blnEnd)
										objToken.Lexema = ConsumeTo(cnstArrStrSpaces);
						}
				// Devuelve el token
					return objToken;
		}

		/// <summary>
		///		Comprueba si el siguiente carácter es numérico
		/// </summary>
		private bool NextCharIsNumeric()
		{ return cnstStrCharsNumeric.IndexOf(GetChars(1)) >= 0;
		}

		/// <summary>
		///		Consume una cadena teniendo en cuenta los saltos
		/// </summary>
		private string ConsumeString()
		{ string strValue = "";
			bool blnEnd = false;

				// Busca las comillas finales
					while (!blnEnd && !IsEof())
						{ // Se salta el carácter que vaya después de una \
								if (strContent[intCharIndex] == '\\')
									{ intCharIndex++;
										if (!IsEof())
											{ // Añade el siguiente carácter
													switch (strContent[intCharIndex])
														{ case 'u':
															case 'n':
															case 't':
															case 'r':
																	strValue += "\\" + strContent[intCharIndex];
																break;
															default:
																	strValue += strContent[intCharIndex];
																break;
														}
												// Incrementa el índice del carácter
													intCharIndex++;
											}
									}
								else 
									{ // Comprueba si ha llegado al final o añade el carácter
											if (strContent[intCharIndex] == '\"')
												blnEnd = true;
											else
												strValue += strContent[intCharIndex];
										// Incrementa el índice de caracteres
											intCharIndex++;
									}
						}
				// Devuelve la cadena
					return strValue;
		}
		
		/// <summary>
		///		Devuelve la cadena que se encuentra hasta encontrar otra
		/// </summary>
		private string ConsumeTo(string strValue)
		{ return ConsumeTo(new string[] {strValue});
		}
		
		/// <summary>
		///		Devuelve la cadena que se encuentra hasta encontrar otra
		/// </summary>
		private string ConsumeTo(string [] arrStrValues)
		{ string strValue = "";
			bool blnEnd = false;
		
				// Consume la cadena hasta encontrar alguna de las pasadas como parámetros
					do
						{ // Comprueba si la siguiente cadena es la especificada
								foreach (string strSearchValue in arrStrValues)
									if (GetChars(strSearchValue.Length).Equals(strSearchValue, StringComparison.CurrentCultureIgnoreCase))
										blnEnd = true;
							// Si no se ha terminado añade el siguiente carácter
								if (!blnEnd)
									{	strValue += GetChars(1);
										Consume(1);
									}
						}
					while (!blnEnd && !IsEof());
				// Devuelve la cadena
					return strValue;
		}

		/// <summary>
		///		Devuelve una cadena con números
		/// </summary>
		private string ConsumeNumeric()
		{ string strValue = "";
			bool blnEnd = false;

				// Consume la cadena hasta encontrar algo que no es un dígito
					do
						{ string strNextChar = GetChars(1);

								// Si es numérico, lo pasa a la cadena, si no indica que se ha terminado
									if (cnstStrCharsNumeric.IndexOf(strNextChar) >= 0)
										{ // Asigna el cáracter
												strValue += strNextChar;
											// Lo quita de la cadena pendiente
												Consume(1);
										}
									else
										blnEnd = true;
						}
					while (!blnEnd && !IsEof());
				// Devuelve la cadena
					return strValue;
		}

		/// <summary>
		///		Consume los espacios
		/// </summary>
		private void ConsumeSpaces()
		{ while (!IsEof() && IsSpace(GetChars()))
				{ // Si es un salto de línea incrementa el número de línea
						if (GetChars() == "\n")
							intLine++;
					// Consume el carácter
						Consume(1);
				}
		}
		
		/// <summary>
		///		Obtiene un carácter (pero no lo consume)
		/// </summary>
		private string GetChars()
		{ return GetChars(1);
		}
		
		/// <summary>
		///		Obtiene n caracteres (pero no los consume)
		/// </summary>
		private string GetChars(int intLength)
		{ if (intCharIndex + intLength < strContent.Length)
				return GetCharsSkipBar(strContent, intCharIndex, intLength);
			else
				return GetCharsSkipBar(strContent, intCharIndex, 1);
		}

		/// <summary>
		///		Obtiene una cadena de n caracteres saltándose los caracteres \
		/// </summary>
		private string GetCharsSkipBar(string strContent, int intIndexStart, int intLength)
		{ int intRead = 0;
			string strValue = "";

				// Recorre la cadena obteniendo los caracteres
					while (intRead < intLength && intIndexStart < strContent.Length)
						{ // Si no es un carácter de \ añade el contenido
								if (strContent[intIndexStart] != '\\')
									{ strValue += strContent[intIndexStart];
										intRead++;
									}
							// Pasa al siguiente carácter
								intIndexStart++;
						}
				// Devuelve el valor
					return strValue;
		}

		/// <summary>
		///		Consume los caracteres de la cadena
		/// </summary>
		private void Consume(string strString)
		{ Consume(strString.Length);
		}		
		
		/// <summary>
		///		Consume un número de caracteres
		/// </summary>
		private void Consume(int intLength)
		{ int intRead = 0;

				while (intRead < intLength && intCharIndex < strContent.Length)
					{ // Incrementa el índice de lectura cuando no está en una barra
							if (strContent[intCharIndex] != '\\')
								intRead++;
						// Incrementa el índice global
							intCharIndex ++;
					}
		}
		
		/// <summary>
		///		Indica si se ha llegado al fin de archivo
		/// </summary>
		internal bool IsEof()
		{ return intCharIndex >= strContent.Length;
		} 
		
		/// <summary>
		///		Comprueba si el carácter es un espacio
		/// </summary>
		private bool IsSpace(string strChar)
		{ // Comprueba si es un espacio
				foreach (string strValue in cnstArrStrSpaces)
					if (strChar == strValue)
						return true;
			// Si ha llegado hasta aquí es porque no es un espacio
				return false;
		}
		
		/// <summary>
		///		Definiciones de token
		/// </summary>
		internal TokenDefinitionsCollection TokensDefinitions { get; private set; }		
	}
}
