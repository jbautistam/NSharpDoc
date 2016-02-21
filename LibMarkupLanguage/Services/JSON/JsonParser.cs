using System;

using Bau.Libraries.LibMarkupLanguage.Services.Tools;

namespace Bau.Libraries.LibMarkupLanguage.Services.JSON
{
	/// <summary>
	///		Intérprete de Json
	/// </summary>
	public class JsonParser : IParser
	{ // Enumerados privados
			/// <summary>
			///		Tipos de token
			/// </summary>
			private enum TokenType
				{ 
					/// <summary>Desconocido. No se debería utilizar</summary>
					Unknown,
					/// <summary>Llave de apertura</summary>
					BracketOpen,
					/// <summary>Llave de cierre</summary>
					BracketClose,
					/// <summary>Cadena</summary>
					String,
					/// <summary>Dos puntos</summary>
					Colon,
					/// <summary>Coma</summary>
					Comma,
					/// <summary>Corchete de apertura</summary>
					BraceOpen,
					/// <summary>Corchete de cierre</summary>
					BraceClose,
					/// <summary>Cadena con el valor lógico "True"</summary>
					True,
					/// <summary>Cadena con el valor lógico "False"</summary>
					False,
					/// <summary>Valor numérico</summary>
					Numeric,
					/// <summary>Cadena con el valor "null"</summary>
					Null
				}
		// Variables privadas
			private Token objActualToken;
			private TokenType intIDActualType;

		/// <summary>
		///		Interpreta un archivo
		/// </summary>
		public MLFile Parse(string strFileName)
		{ return ParseText(LibMarkupLanguage.Tools.FileHelper.LoadTextFile(strFileName));
		}

		/// <summary>
		///		Interpreta un texto
		/// </summary>
		public MLFile ParseText(string strText)
		{ ParserTokenizer objTokenizer = InitTokenizer();
			MLFile objMLFile = new MLFile();

				// Inicializa el contenido
					objTokenizer.Init(strText);
				// Interpreta el archivo
					objMLFile.Nodes.Add(ParseNode("Root", objTokenizer, true));
				// Devuelve el archivo
					return objMLFile;
		}

		/// <summary>
		///		Interpreta un nodo
		/// </summary>
		private MLNode ParseNode(string strName, ParserTokenizer objTokenizer, bool blnSearchBracket)
		{ MLNode objMLNode = new MLNode(strName);

				// Captura el siguiente nodo
					if (!objTokenizer.IsEof())
						{ if (blnSearchBracket)
								{	// Obtiene el siguiente token
										GetNextToken(objTokenizer);
									// Debería ser una llave de apertura
										if (intIDActualType == TokenType.BraceOpen)
											objMLNode.Nodes.Add(ParseNodesArray(objTokenizer));
										else if (intIDActualType != TokenType.BracketOpen)
											throw new ParserException("Se esperaba una llave de apertura");
										else
											ParseNodeAttributes(objTokenizer, objMLNode);
								}
							else
								ParseNodeAttributes(objTokenizer, objMLNode);
						}
				// Devuelve el nodo interpretado
					return objMLNode;
		}

		/// <summary>
		///		Interpreta los atributos de un nodo "id":"value","id":"value", ... ó "id":{object} ó "id":[array]
		/// </summary>
		private void ParseNodeAttributes(ParserTokenizer objTokenizer, MLNode objMLNodeParent)
		{ bool blnEnd = false;

				// Obtiene los nodos
					while (!objTokenizer.IsEof() && !blnEnd)		
						{ // Lee el siguiente Token, debería ser un identificador
								GetNextToken(objTokenizer);
							// Comprueba que sea correcto
								if (intIDActualType == TokenType.BracketClose) // ... es un objeto vacío
									blnEnd = true;
								else if (intIDActualType != TokenType.String) // ... no se ha encontrado el identificador
									throw new ParserException("Se esperaba el identificador del elemento");
								else
									{ MLAttribute objMLAttribute = new MLAttribute();

											// Asigna el código del atributo
												objMLAttribute.Name = objActualToken.Lexema;
											// Lee el siguiente token. Deberían ser dos puntos
												GetNextToken(objTokenizer);
											// Comprueba que sea correcto
												if (intIDActualType != TokenType.Colon)
													throw new ParserException("Se esperaban dos puntos (separador de identificador / valor)");
												else
													{ // Lee el siguiente token...
															GetNextToken(objTokenizer);
														// Interpreta el valor
															switch (intIDActualType)
																{ case TokenType.String:
																	case TokenType.True:
																	case TokenType.False:
																	case TokenType.Numeric:
																	case TokenType.Null:
																			// Asigna el valor al atributo
																				switch (intIDActualType)
																					{ case TokenType.Null:
																								objMLAttribute.Value = "";
																							break;
																						case TokenType.String:
																								objMLAttribute.Value = ParseUnicode(objActualToken.Lexema);
																							break;
																						default:
																								objMLAttribute.Value = objActualToken.Lexema;
																							break;
																					}
																			// Añade el atributo al nodo
																				objMLNodeParent.Attributes.Add(objMLAttribute);
																		break;
																	case TokenType.BracketOpen: // ... definición de objeto
																			MLNode objMLNode = ParseNode(objMLAttribute.Name, objTokenizer, false);

																				// Añade el nodo como objeto
																					objMLNodeParent.Nodes.Add(objMLNode);
																		break;
																	case TokenType.BraceOpen: // ... definición de array
																			objMLNodeParent.Nodes.Add(ParseNodesArray(objMLAttribute.Name, objTokenizer));
																		break;
																	default:
																		throw new ParserException("Cadena desconocida. " + objActualToken.Lexema);
																}
													}
											// Lee el siguiente token
												GetNextToken(objTokenizer);
											// Si es una coma, seguir con el siguiente atributo del nodo, si es una llave de cierre, terminar
												switch (intIDActualType)
													{ case TokenType.Comma:
																// ... no hace nada, simplemente pasa a la creación del siguiente nodo
															break;
														case TokenType.BracketClose:
																blnEnd = true;
															break;
														default:
															throw new ParserException("Cadena desconocida. " + objActualToken.Lexema);
													}
									}
						}
		}

		/// <summary>
		///		Interpreta los nodos de un array
		/// </summary>
		private MLNode ParseNodesArray(ParserTokenizer objTokenizer)
		{ return ParseNodesArray("Array", objTokenizer);
		}

		/// <summary>
		///		Interpreta los nodos de un array
		/// </summary>
		private MLNode ParseNodesArray(string strNodeParent, ParserTokenizer objTokenizer)
		{ MLNode objMLNode = new MLNode(strNodeParent);
			bool blnEnd = false;
			int intIndex = 0;

				// Obtiene el siguiente token (puede que se trate de un array vacío)
					while (!objTokenizer.IsEof() && !blnEnd)
						{ // Obtiene el siguiente token
								GetNextToken(objTokenizer);
							// Interpreta el nodo
								switch (intIDActualType)
									{ case TokenType.BracketOpen:
												objMLNode.Nodes.Add(ParseNode("Struct", objTokenizer, false));
											break;
										case TokenType.BraceOpen:
												objMLNode.Nodes.Add(ParseNodesArray(objTokenizer));
											break;
										case TokenType.String:
										case TokenType.Numeric:
										case TokenType.True:
										case TokenType.False:
										case TokenType.Null:
												objMLNode.Nodes.Add("Item", objActualToken.Lexema);
											break;
										case TokenType.Comma: // ... no hace nada, simplemente pasa al siguiente incrementando el índice
												intIndex++;
											break;
										case TokenType.BraceClose: // ... corchete de cierre, indica que ha terminado
												blnEnd = true;
											break;
										default:
											throw new NotImplementedException("No se ha encontrado un token válido ('" + objActualToken.Lexema + "')");
									}
						}
				// Si no se ha encontrado un corchete, lanza una excepción
					if (!blnEnd)
						throw new ParserException("No se ha encontrado el carácter de fin del array ']'");
				// Devuelve la colección de nodos
					return objMLNode;
		}

		/// <summary>
		///		Obtiene los datos del siguiente token
		/// </summary>
		private void GetNextToken(ParserTokenizer objTokenizer)
		{ objActualToken = objTokenizer.GetToken();
			intIDActualType = GetIDType(objActualToken);
		}

		/// <summary>
		///		Obtiene el tipo de un token
		/// </summary>
		private TokenType GetIDType(Token objToken)
		{	if (objToken != null && objToken.Definition != null)
				return (TokenType) (objToken.Definition.Type ?? 0);
			else
				return TokenType.Unknown;
		}		

		/// <summary>
		///		Interpreta una cadena Unicode
		/// </summary>
		private string ParseUnicode(string strValue)
		{ int intIndexOf;
			bool blnEnd = false;

				// Convierte los caracteres Unicode de la cadena
					do
						{ // Indica que ya se ha terminado
								blnEnd = true;
							// Obtiene el índice de la cadena Unicode
								intIndexOf = strValue.IndexOf("\\u", StringComparison.CurrentCultureIgnoreCase);
							// Si se ha obtenido algo, comprueba que se puedan coger cuatro caracteres
								if (intIndexOf >= 0 && intIndexOf + 6 <= strValue.Length)
									{ string strUnicode = strValue.Substring(intIndexOf, 6);
										string strFirst;

											// Obtiene la parte izquierda de la cadena (con el carácter Unicode convertido)
												strFirst = strValue.Substring(0, intIndexOf) + 
																		(char) int.Parse(strUnicode.Substring(2), System.Globalization.NumberStyles.HexNumber);
											// Obtiene la parte derecha de la cadena
												if (strValue.Length >= intIndexOf + 6)
													strValue = strValue.Substring(intIndexOf + 6);
											// Obtiene la cadena completa
												strValue = strFirst + strValue;
											// Indica que debe continuar
												blnEnd = false;
									}
						}
					while (!blnEnd);
				// Convierte los saltos de línea
					strValue = strValue.Replace("\n", Environment.NewLine);
				// Devuelve la cadena convertida		
					return strValue;
		}

		/// <summary>
		///		Inicializa el objeto de creación de tokens
		/// </summary>
		private ParserTokenizer InitTokenizer()
		{ ParserTokenizer objTokenizer = new ParserTokenizer();

				// Asigna los tokens
					objTokenizer.TokensDefinitions.Add(GetTokenDefinition(TokenType.BracketOpen, "{"));
					objTokenizer.TokensDefinitions.Add(GetTokenDefinition(TokenType.BracketClose, "}"));
					objTokenizer.TokensDefinitions.Add(GetTokenDefinition(TokenType.String, "\"", "\""));
					objTokenizer.TokensDefinitions.Add(GetTokenDefinition(TokenType.Colon, ":"));
					objTokenizer.TokensDefinitions.Add(GetTokenDefinition(TokenType.Comma, ","));
					objTokenizer.TokensDefinitions.Add(GetTokenDefinition(TokenType.BraceOpen, "["));
					objTokenizer.TokensDefinitions.Add(GetTokenDefinition(TokenType.BraceClose, "]"));
					objTokenizer.TokensDefinitions.Add(GetTokenDefinition(TokenType.True, "true"));
					objTokenizer.TokensDefinitions.Add(GetTokenDefinition(TokenType.False, "false"));
					objTokenizer.TokensDefinitions.Add(GetTokenDefinition(TokenType.Null, "null"));
					objTokenizer.TokensDefinitions.Add((int) TokenType.Numeric, "Numeric");
				// Devuelve el objeto de creación de tokens
					return objTokenizer;
		}

		/// <summary>
		///		Definición del token
		/// </summary>
		private TokenDefinition GetTokenDefinition(TokenType intIDToken, string strStart, string strEnd = null)
		{ return new TokenDefinition((int) intIDToken, intIDToken.ToString(), strStart, strEnd);
		}
	}
}
