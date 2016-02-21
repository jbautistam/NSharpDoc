using System;
using System.Collections.Generic;

namespace Bau.Libraries.LibHelper.Extensors
{
	/// <summary>
	///		Métodos de extensión para cadenas
	/// </summary>
	public static class StringExtensor
	{	
		/// <summary>
		///		Comprueba si dos cadenas son iguales sin tener en cuenta las mayúsculas y tratando los nulos
		/// </summary>
		public static bool EqualsIgnoreCase(this string strFirst, string strSecond)
		{ if (string.IsNullOrEmpty(strFirst) && !string.IsNullOrEmpty(strSecond))
				return false;
			else if (!string.IsNullOrEmpty(strFirst) && string.IsNullOrEmpty(strSecond))
				return false;
			else if (string.IsNullOrEmpty(strFirst) && string.IsNullOrEmpty(strSecond))
				return true;
			else
				return strFirst.Equals(strSecond, StringComparison.CurrentCultureIgnoreCase);
		}

		/// <summary>
		///		Comprueba si dos cadenas son iguales sin tener en cuenta los nulos
		/// </summary>
		public static bool EqualsIgnoreNull(this string strFirst, string strSecond)
		{ if (string.IsNullOrEmpty(strFirst) && !string.IsNullOrEmpty(strSecond))
				return false;
			else if (!string.IsNullOrEmpty(strFirst) && string.IsNullOrEmpty(strSecond))
				return false;
			else if (string.IsNullOrEmpty(strFirst) && string.IsNullOrEmpty(strSecond))
				return true;
			else
				return strFirst.Equals(strSecond, StringComparison.CurrentCulture);
		}

		/// <summary>
		///		Comprueba si una cadena es nula o está vacía (sin espacios)
		/// </summary>
		public static bool IsEmpty(this string strValue)
		{ return string.IsNullOrEmpty(strValue) || string.IsNullOrEmpty(strValue.Trim());
		}

		/// <summary>
		///		Pasa a mayúsculas el primer carácter de una cadena 
		/// </summary>
		public static string ToUpperFirstLetter(this string strValue)
		{ if (strValue.IsEmpty())
				return "";
			else
				return strValue.Left(1).ToUpper() + strValue.From(1);
		}

		/// <summary>
		///		Comprueba el inicio de una cadena evitando los nulos
		/// </summary>
		public static bool StartsWithIgnoreNull(this string strValue, string strStart, StringComparison intComparison = StringComparison.CurrentCultureIgnoreCase)
		{ return !string.IsNullOrEmpty(strValue) && strValue.StartsWith(strStart, intComparison);
		}

		/// <summary>
		///		Quita los espacios ignorando los valores nulos
		/// </summary>
		public static string TrimIgnoreNull(this string strValue)
		{ if (!IsEmpty(strValue))
				return strValue.Trim();
			else
				return "";
		}

		/// <summary>
		///		Obtiene un valor lógico a partir de una cadena
		/// </summary>
		public static bool GetBool(this string strValue, bool blnDefault = false)
		{ bool blnValue = blnDefault;

				if (strValue.EqualsIgnoreCase("yes"))
					return true;
				else if (strValue.EqualsIgnoreCase("no"))
					return false;
				else if (strValue.IsEmpty() || !bool.TryParse(strValue, out blnValue))
					return blnDefault;
				else
					return blnValue;
		}

		/// <summary>
		///		Obtiene un valor decimal para una cadena
		/// </summary>
		public static double? GetDouble(this string strValue)
		{ double dblValue;
		  
				if (string.IsNullOrEmpty(strValue) || !double.TryParse(strValue.Replace('.',',') , out dblValue))
					return null;
				else
					return dblValue;
		}

		/// <summary>
		///		Obtiene un valor decimal para una cadena
		/// </summary>
		public static double GetDouble(this string strValue, double dblDefault)
		{ return GetDouble(strValue) ?? dblDefault;
		}

		/// <summary>
		///		Obtiene un valor entero para una cadena
		/// </summary>
		public static int? GetInt(this string strValue)
		{ int intValue;
		
				if (string.IsNullOrEmpty(strValue) || !int.TryParse(strValue, out intValue))
					return null;
				else
					return intValue;
		}

		/// <summary>
		///		Obtiene un valor entero para una cadena
		/// </summary>
		public static int GetInt(this string strValue, int intDefault)
		{ return GetInt(strValue) ?? intDefault;
		}

		/// <summary>
		///		Obtiene un valor largo para una cadena
		/// </summary>
		public static long GetLong(this string strValue, long lngDefault)
		{ return GetLong(strValue) ?? lngDefault;
		}

		/// <summary>
		///		Obtiene un valor largo para una cadena
		/// </summary>
		public static long? GetLong(this string strValue)
		{ long lngValue;
		
				if (string.IsNullOrEmpty(strValue) || !long.TryParse(strValue, out lngValue))
					return null;
				else
					return lngValue;
		}

		/// <summary>
		///		Obtiene una fecha para una cadena
		/// </summary>
		public static DateTime? GetDateTime(this string strValue)
		{ DateTime dtmValue;
		
				if (string.IsNullOrEmpty(strValue) || !DateTime.TryParse(strValue, out dtmValue))
					return null;
				else
					return dtmValue;
		}

		/// <summary>
		///		Añade una cadena a otra con un separador si es necesario
		/// </summary>
		public static string AddWithSeparator(this string strValue, string strAdd, string strSeparator, bool blnWithSpace = true)
		{ // Añade el separador
				if (!strValue.IsEmpty())
					strValue += strSeparator + (blnWithSpace ? " " : "");
				else // ... evita los posibles errores si la cadena es NULL
					strValue = "";
			// Devuelve la cadena con el valor añadido
				return strValue + strAdd;
		}

		/// <summary>
		///		Corta una cadena hasta un separador. Devuelve la parte inicial de la cadena antes del separador
		///	y deja en la cadena strTarget, a partir del separador
		/// </summary>
		public static string Cut(this string strSource, string strSeparator, out string strTarget)
		{ int intIndex;
			string strCut = "";
			
				// Inicializa los valores de salida
					strTarget = "";
				// Si hay algo que cortar ...
					if (!strSource.IsEmpty())
						{ // Obtiene el índice donde se encuentra el separador
								intIndex = strSource.IndexOf(strSeparator, StringComparison.CurrentCultureIgnoreCase);
							// Corta la cadena
								if (intIndex < 0)
									strCut = strSource;
								else 
									strCut = strSource.Substring(0, intIndex);
							// Borra al cadena cortada
								if ((strCut + strSeparator).Length - 1 < strSource.Length)
									strTarget = strSource.Substring((strCut + strSeparator).Length);
								else
									strTarget = "";
						}
				// Devuelve la primera parte de la cadena
					return strCut;			
		}
		
		/// <summary>
		///		Corta una cadena por una longitud
		/// </summary>
		/// <param name="strSource">Cadena original</param>
		/// <param name="intLength">Longitud por la que se debe cortar</param>
		/// <param name="strLast">Segunda parte de la cadena subString(intLength + 1) o vacío si no queda suficiente</param>
		/// <returns>Primera parte de la cadena (0..intLength)</returns>
		public static string Cut(this string strSource, int intLength, out string strLast)
		{ // Inicializa la cadena de salida
				strLast = "";
			// Corta la cadena
				if (!strSource.IsEmpty() && strSource.Length > intLength)
					{ strLast = strSource.Substring(intLength);
						strSource = strSource.Substring(0, intLength);
					}
			// Devuelve la primera parte de la cadena
				return strSource;
		}

		/// <summary>
		///		Separa una cadena cuando en partes cuando el separador es una cadena (no se puede utilizar Split)
		/// </summary>
		public static List<string> SplitByString(this string strSource, string strSeparator)
		{ List<string> objColSplit = new List<string>();
				
				// Corta la cadena
					if (!strSource.IsEmpty())
						do
							{ string strPart = "";

									// Corta la cadena
										strSource = strSource.Cut(strSeparator, out strPart);
									// Añade la parte localizada a la colección y continúa con la cadena restante
										if (!strSource.IsEmpty())
											objColSplit.Add(strSource);
									// Pasa al siguiente
										strSource = strPart;
							}
						while (!strSource.IsEmpty());
				// Devuelve la primera parte de la cadena
					return objColSplit;			
		}

		/// <summary>
		///		Elimina una cadena del inicio de otra
		/// </summary>
		public static string RemoveStart(this string strSource, string strStart)
		{ if (strSource.IsEmpty() || !strSource.StartsWith(strStart, StringComparison.CurrentCultureIgnoreCase))
				return strSource;
			else if (strSource.Length == strStart.Length)
				return "";
			else
				return strSource.Substring(strStart.Length);
		}

		/// <summary>
		///		Elimina una cadena al final de otra
		/// </summary>
		public static string RemoveEnd(this string strSource, string strEnd)
		{ if (strSource.IsEmpty() || !strSource.EndsWith(strEnd, StringComparison.CurrentCultureIgnoreCase))
				return strSource;
			else if (strSource.Length == strEnd.Length)
				return "";
			else
				return strSource.Substring(0, strSource.Length - strEnd.Length);
		}

		/// <summary>
		///		Reemplaza una cadena teniendo en cuenta el tipo de comparación
		/// </summary>
		public static string ReplaceWithStringComparison(this string strSource, string strSearch, string strReplace, 
																										 StringComparison intComparison = StringComparison.CurrentCultureIgnoreCase)
		{ int intIndex; 

				// Recorre la cadena sustituyendo los valores
					if (!strSource.IsEmpty() && !strSearch.EqualsIgnoreCase(strReplace))
						do
							{ if ((intIndex = strSource.IndexOf(strSearch, intComparison)) >= 0)
									{ string strLast;
										string strFirst = strSource.Cut(strSearch, out strLast);

											strSource = strFirst + strReplace + strLast;
									} 
							}
						while (intIndex >= 0);
				// Devuelve la cadena
					return strSource;
		}

		/// <summary>
		///		Obtiene la parte izquierda de una cadena
		/// </summary>
		public static string Left(this string strSource, int intLength)
		{ if (IsEmpty(strSource) || intLength <= 0)
				return "";
			else if (intLength > strSource.Length)
				return strSource;
			else
				return strSource.Substring(0, intLength);
		}

		/// <summary>
		///		Obtiene la parte derecha de una cadena
		/// </summary>
		public static string Right(this string strSource, int intLength)
		{ if (IsEmpty(strSource) || intLength <= 0)
				return "";
			else if (intLength > strSource.Length)
				return strSource;
			else
				return strSource.Substring(strSource.Length - intLength, intLength);
		}

		/// <summary>
		///		Obtiene una cadena a partir de un carácter
		/// </summary>
		public static string From(this string strSource, int intFirst)
		{ if (IsEmpty(strSource) || intFirst >= strSource.Length)
				return "";
			else if (intFirst <= 0)
				return strSource;
			else
				return strSource.Substring(intFirst);
		}

		/// <summary>
		///		Obtiene la cadena media
		/// </summary>
		public static string Mid(this string strSource, int intFirst, int intLength)
		{ return strSource.From(intFirst).Left(intLength);
		}

		/// <summary>
		///		Codificar caracteres en Unicode
		/// </summary>
		public static string ToUnicode(this string strValue) 
		{ string strResult = "";

				// Codifica los caracteres
					foreach (char chrChar in strValue) 
						{	if (chrChar > 127) 
								strResult += "\\u" + ((int) chrChar).ToString( "x4" );
							else 
								strResult += chrChar;
						}
				// Devuelve el resultado
					return strResult;
    }

		/// <summary>
		///		Concatena una serie de líneas
		/// </summary>
		public static string Concatenate(this string strValue, List<string> objColStrings, string strCharSeparator = "\r\n",
																		 bool blnWithSpace = false)
		{ string strResult = "";

				// Concatena las cadenas
					if (objColStrings != null)
						foreach (string strString in objColStrings)
							strResult = strResult.AddWithSeparator(strString, strCharSeparator, blnWithSpace);
				// Devuelve el resultado
					return strResult;
		}

		/// <summary>
		///		Separa una serie de cadenas
		/// </summary>
		public static List<string> SplitToList(this string strValue, string strCharSeparator = "\r\n",
																					 bool blnAddEmpty = false)
		{ List<string> objColStrings = new List<string>();
			List<string> objColSource = strValue.SplitByString(strCharSeparator);

				// Añade las cadenas
					foreach (string strSource in objColSource)
						if (blnAddEmpty || (!blnAddEmpty && !strSource.TrimIgnoreNull().IsEmpty()))
							objColStrings.Add(strSource.TrimIgnoreNull());
				// Devuelve las cadenas
					return objColStrings;
		}

		/// <summary>
		///		Normaliza la cadena quitándole los acentos
		/// </summary>
		public static string NormalizeAccents(this string strValue)
		{ int intIndex;
			string strResult = "";
			string strWithAccents = "ÁÉÍÓÚáéíóúÀÈÌÒÙàèìòùÄËÏÖÜäëïöü";
			string strWithOutAccents = "AEIOUaeiouAEIOUaeiouAEIOUaeiou";
			
				// Normaliza la cadena
					if (!strValue.IsEmpty())
						foreach (char chrChar in strValue)
							if ((intIndex = strWithAccents.IndexOf(chrChar)) >= 0)
								strResult += strWithOutAccents[intIndex];
							else
								strResult += chrChar;
				// Devuelve el resultado
					return strResult;
		}

		/// <summary>
		///		Extrae las cadenas que se corresponden con un patrón
		/// </summary>
		public static List<string> Extract(this string strSource, string strStart, string strEnd) 
		{ List<string> objColResults = new List<string>();

				// Obtiene las coincidencias
					if (!strSource.IsEmpty())
						try 
							{	System.Text.RegularExpressions.Match objMatch;

									// Crea la expresión de búsqueda
										objMatch = System.Text.RegularExpressions.Regex.Match(strSource, strStart + "(.|\n)*?" + strEnd, 
																																					System.Text.RegularExpressions.RegexOptions.IgnoreCase | 
																																					System.Text.RegularExpressions.RegexOptions.Compiled, 
																																					TimeSpan.FromSeconds(1));
									// Mientras haya una coincidencia
										while (objMatch.Success)
											{	string strValue = objMatch.Groups[0].Value.TrimIgnoreNull();

													// Quita la cadena inicial y final
														strValue = strValue.RemoveStart(strStart);
														strValue = strValue.RemoveEnd(strEnd);
													// Añade la cadena encontrada
														objColResults.Add(strValue.TrimIgnoreNull());
													// Pasa a la siguiente coincidencia
														objMatch = objMatch.NextMatch();
											}   
							}
						catch (System.Text.RegularExpressions.RegexMatchTimeoutException objException) 
							{	System.Diagnostics.Debug.WriteLine("TimeOut: " + objException.Message);
							}
				// Devuelve las coincidencis
					return objColResults;
		}
	}
}
