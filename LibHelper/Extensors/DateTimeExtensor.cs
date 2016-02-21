using System;

namespace Bau.Libraries.LibHelper.Extensors 
{
	/// <summary>
	///		Métodos de extensión para fechas
	/// </summary>
	public static class DateTimeExtensor 
	{
		/// <summary>
		///		Convierte una fecha a UTC para que incluya la zona horaria (el método de DateTime no
		///	funciona con fechas que pueden ser nulas)
		/// </summary>
		public static DateTime? ConvertToUniversalTime(this DateTime? dtmValue)
		{ //! Devuelve la fecha convertida en UTC, nunca será NULL por la condición inicial 
			//! por eso nunca se utiliza DateTime.Now pero necesitamos utilizar el ?? porque si no .NET no
			//! nos ofrece la opción de ToUniversalTime.
				if (dtmValue == null)
					return null;
				else 
					return (dtmValue ?? DateTime.Now).ToUniversalTime();
		}
	}
}
