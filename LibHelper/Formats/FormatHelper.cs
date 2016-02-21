using System;

namespace Bau.Libraries.LibHelper.Formats
{
	/// <summary>
	///		Helper para el formato de datos
	/// </summary>
	public static class FormatHelper
	{
		/// <summary>
		///		Formatea una fecha
		/// </summary>
		public static string Format(DateTime? dtmValue, bool blnWithHour = true)
		{ if (dtmValue == null)
				return "-";
			else if (blnWithHour)
				return string.Format("{0:dd-MM-yyyy HH:mm}", dtmValue);
			else
				return string.Format("{0:dd-MM-yyyy}", dtmValue);
		}

		/// <summary>
		///		Formatea un valor lógico
		/// </summary>
		public static string Format(bool? blnValue)
		{ if (blnValue == null)
				return "-";
			else if (blnValue ?? false)
				return "Sí";
			else
				return "No";
		}
	}
}
