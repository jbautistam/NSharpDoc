using System;
using System.Globalization;

namespace Bau.Libraries.LibHelper.Extensors
{
	/// <summary>
	///		Proporciona métodos para generar e interpretar la información de fecha y hora
	/// </summary>
	/// <remarks>
	///     Ver <a href="http://www.ietf.org/rfc/rfc0822.txt">RFC #822</a> el estándar para mensajes de texto de ARPA
	///     y <a href="http://www.ietf.org/rfc/rfc3339.txt">RFC #3339</a>: fecha y hora en Internet para más información
	///     sobre los formatos de fecha y hora implementados
	/// </remarks>
	public static class DateTimeHelper
	{
		/// <summary>
		///		Convierte la fecha en el formato RFC-3339
		/// </summary>
		public static string ToStringRfc3339(DateTime dtmValue)
		{ DateTimeFormatInfo fmtDateTime = CultureInfo.InvariantCulture.DateTimeFormat;

				// Devuelve la cadena con la fecha formateada en RFC-3339
					if (dtmValue.Kind == DateTimeKind.Local)
						return dtmValue.ToString("yyyy'-'MM'-'dd'T'HH:mm:ss.ffzzz", fmtDateTime);
					else
						return dtmValue.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss.ff'Z'", fmtDateTime);
		}
		
		/// <summary>
		///		Convierte una representación de una fecha formateada con RFC-3339 en un DateTime
		/// </summary>
		public static DateTime ParseRfc3339(string strValue)
		{ DateTime dtmResult = DateTime.MinValue;

				// Interpreta la fecha formateada en RFC-3339
					if (DateTimeHelper.TryParseRfc3339(strValue, out dtmResult))
						return dtmResult;
					else
						throw new FormatException();
		}

		/// <summary>
		///		Convierte una cadena en formato RFC-3339 en una fecha
		///		Ejemplo de cadena en el formato: Thu, 10 Sep 2009 10:51:36 -0800
		/// </summary>
		public static bool TryParseRfc3339(string strValue, out DateTime dtmResult)
		{	DateTimeFormatInfo fmtDateTime = CultureInfo.InvariantCulture.DateTimeFormat;
			string[] arrStrFormats = { fmtDateTime.SortableDateTimePattern,
																 fmtDateTime.UniversalSortableDateTimePattern,
																 "yyyy'-'MM'-'dd'T'HH:mm:ss'Z'",
																 "yyyy'-'MM'-'dd'T'HH:mm:ss.f'Z'",
																 "yyyy'-'MM'-'dd'T'HH:mm:ss.ff'Z'",
																 "yyyy'-'MM'-'dd'T'HH:mm:ss.fff'Z'",
																 "yyyy'-'MM'-'dd'T'HH:mm:ss.ffff'Z'",
																 "yyyy'-'MM'-'dd'T'HH:mm:ss.fffff'Z'",
																 "yyyy'-'MM'-'dd'T'HH:mm:ss.ffffff'Z'",
																 "yyyy'-'MM'-'dd'T'HH:mm:szz",
																 "yyyy'-'MM'-'dd'T'HH:mm:ss.ffzzz",
																 "yyyy'-'MM'-'dd'T'HH:mm:ss.fffzzz",
																 "yyyy'-'MM'-'dd'T'HH:mm:ss.ffffzzz",
																 "yyyy'-'MM'-'dd'T'HH:mm:ss.fffffzzz",
																 "yyyy'-'MM'-'dd'T'HH:mm:ss.ffffffzzz"
																 
																 // 2004-03-24T10:17:34.5000000-08:00
																};

					// Inicializa el valor de salida
						dtmResult = DateTime.MinValue;
					// Quita el incremento Grenwich sobre la fecha (si existe)
						if (strValue.Length > 6 && (strValue.Substring(strValue.Length - 6, 1) == "-" ||
																				strValue.Substring(strValue.Length - 6, 1) == "+"))
							strValue = strValue.Substring(0, strValue.Length - 6);
					// Comprueba el parámetro
						if (string.IsNullOrEmpty(strValue))
							return false;
						else
							return DateTime.TryParseExact(strValue, arrStrFormats, fmtDateTime, 
																						DateTimeStyles.AssumeUniversal, out dtmResult);
		}
	  
		/// <summary>
		///		Reemplaza el componente de la hora RFC-822 con su desplazamiento horario equivalente
		/// </summary>
		private static string ReplaceRfc822TimeZoneWithOffset(string strValue)
		{	// Quita los espacios
				strValue = strValue.Trim();
			// Obtiene el desplazamiento horario
				if (strValue.EndsWith("UT", StringComparison.OrdinalIgnoreCase))
					return String.Format(null, "{0}GMT", strValue.TrimEnd("UT".ToCharArray()));
				else if (strValue.EndsWith("EST", StringComparison.OrdinalIgnoreCase))
					return String.Format(null, "{0}-05:00", strValue.TrimEnd("EST".ToCharArray()));
				else if (strValue.EndsWith("EDT", StringComparison.OrdinalIgnoreCase))
					return String.Format(null, "{0}-04:00", strValue.TrimEnd("EDT".ToCharArray()));
				else if (strValue.EndsWith("CST", StringComparison.OrdinalIgnoreCase))
					return String.Format(null, "{0}-06:00", strValue.TrimEnd("CST".ToCharArray()));
				else if (strValue.EndsWith("CDT", StringComparison.OrdinalIgnoreCase))
					return String.Format(null, "{0}-05:00", strValue.TrimEnd("CDT".ToCharArray()));
				else if (strValue.EndsWith("MST", StringComparison.OrdinalIgnoreCase))
					return String.Format(null, "{0}-07:00", strValue.TrimEnd("MST".ToCharArray()));
				else if (strValue.EndsWith("MDT", StringComparison.OrdinalIgnoreCase))
					return String.Format(null, "{0}-06:00", strValue.TrimEnd("MDT".ToCharArray()));
				else if (strValue.EndsWith("PST", StringComparison.OrdinalIgnoreCase))
					return String.Format(null, "{0}-08:00", strValue.TrimEnd("PST".ToCharArray()));
				else if (strValue.EndsWith("PDT", StringComparison.OrdinalIgnoreCase))
					return String.Format(null, "{0}-07:00", strValue.TrimEnd("PDT".ToCharArray()));
				else if (strValue.EndsWith("Z", StringComparison.OrdinalIgnoreCase))
					return String.Format(null, "{0}GMT", strValue.TrimEnd("Z".ToCharArray()));
				else if (strValue.EndsWith("A", StringComparison.OrdinalIgnoreCase))
					return String.Format(null, "{0}-01:00", strValue.TrimEnd("A".ToCharArray()));
				else if (strValue.EndsWith("M", StringComparison.OrdinalIgnoreCase))
					return String.Format(null, "{0}-12:00", strValue.TrimEnd("M".ToCharArray()));
				else if (strValue.EndsWith("N", StringComparison.OrdinalIgnoreCase))
					return String.Format(null, "{0}+01:00", strValue.TrimEnd("N".ToCharArray()));
				else if (strValue.EndsWith("Y", StringComparison.OrdinalIgnoreCase))
					return String.Format(null, "{0}+12:00", strValue.TrimEnd("Y".ToCharArray()));
				else
					return strValue;
		}

		/// <summary>
		///		Convierte el valor de la fecha en la cadena correspondiente en el formato RFC-822
		/// </summary>
		public static string ToStringRfc822(DateTime dtmValue)
		{	DateTimeFormatInfo fmtDateTime = CultureInfo.InvariantCulture.DateTimeFormat;

				return dtmValue.ToString(fmtDateTime.RFC1123Pattern, fmtDateTime);
		}

		/// <summary>
		///		Convierte una cadena en formato RFC-822 a una fecha
		/// </summary>
		public static DateTime ParseRfc822(string strValue)
		{	DateTime dtmResult = DateTime.MinValue;

				if (TryParseRfc822(strValue, out dtmResult))
					return dtmResult;
				else
					throw new FormatException(string.Format(null, "'{0}' no es un valor correcto en el formato RFC-822.", 
																									strValue));
		}

		/// <summary>
		///		Convierte la cadena en el formato RFC-822 en una fecha
		/// </summary>
		public static bool TryParseRfc822(string strValue, out DateTime dtmResult)
		{	DateTimeFormatInfo fmtDateTime = CultureInfo.InvariantCulture.DateTimeFormat;
			string [] arrStrFormats = { fmtDateTime.RFC1123Pattern,
																	"ddd',' d MMM yyyy HH:mm:ss zzz",
																	"ddd',' dd MMM yyyy HH:mm:ss zzz"
																};

				// Inicializa el resultado
					dtmResult = DateTime.MinValue;
				// Comprueba el parámetro y, si es correcto, convierte el resultado
					if (string.IsNullOrEmpty(strValue))
						{	dtmResult = DateTime.MinValue;
							return false;
						}
					else
						return DateTime.TryParseExact(ReplaceRfc822TimeZoneWithOffset(strValue), 
																					arrStrFormats, fmtDateTime, 
																					DateTimeStyles.None, out dtmResult);
		}
		
		/// <summary>
		///		Interpreta una fecha utilizando los diferentes formatos hasta que encuentra uno correcto
		/// </summary>
		public static bool TryParseRfc(string strValue, out DateTime dtmResult)
		{ return TryParseRfc3339(strValue, out dtmResult) || TryParseRfc822(strValue, out dtmResult);
		}
		
		/// <summary>
		///		Interpreta una fecha utilizando los diferentes formatos hasta que encuentra uno correcto
		/// </summary>
		public static DateTime ParseRfc(string strValue)
		{ DateTime dtmResult;
		
				if (!string.IsNullOrEmpty(strValue) && 
						(TryParseRfc3339(strValue, out dtmResult) || TryParseRfc822(strValue, out dtmResult)))
					return dtmResult;
				else
					return DateTime.MinValue;
		}
	}
}
