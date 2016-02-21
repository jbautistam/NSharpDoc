using System;
using System.Data;

namespace Bau.Libraries.LibDBProvidersBase.Parameters
{
	/// <summary>
	///		Clase que almacena los parámetros de un comando
	/// </summary>
	public class ParametersDBCollection : System.Collections.Generic.List<ParameterDB>
	{ 
		/// <summary>
		///		Añade un parámetro a la colección de parámetros del comando
		/// </summary>
		public void Add(string strName, string strValue, int intLength, ParameterDirection intDirection = ParameterDirection.Input)
		{ // Corta la cadena si supera la longitud máxima
				if (!string.IsNullOrEmpty(strValue) && strValue.Length > intLength)
					strValue = strValue.Substring(0, intLength);
			// Añade el parámetro
				Add(new ParameterDB(strName, strValue, intDirection, intLength));
		}

		/// <summary>
		///		Añade un parámetro a la colección de parámetros del comando
		/// </summary>
		public void Add(string strName, object objValue, ParameterDirection intDirection = ParameterDirection.Input, bool blnSkipValueZero = true)
		{ if (objValue is Enum)
				{ int? intValue = (int) ((object) objValue);

						// Convierte el valor 0 del enumerado en un valor NULL
							if (blnSkipValueZero && (intValue ?? 0) == 0)
								intValue = null;
						// Añade el valor del parámetro
							Add(new ParameterDB(strName, intValue, intDirection));
				}		
			else
				Add(new ParameterDB(strName, objValue, intDirection));
		}		
		
		/// <summary>
		///		Añade un parámetro a la colección de parámetros del comando
		/// </summary>
		public void Add(string strName, byte [] arrBytBuffer, ParameterDirection intDirection = ParameterDirection.Input)
		{ Add(new ParameterDB(strName, arrBytBuffer, intDirection));
		}

		/// <summary>
		///		Añade un parámetro de tipo Text
		/// </summary>
		public void AddText(string strName, string strValue, ParameterDirection intDirection = ParameterDirection.Input)
		{ ParameterDB objParameter = new ParameterDB(strName, strValue, intDirection);

				// Indica que es un parámetro de texto
					objParameter.IsText = true;
				// Añade el parámetro a la colección
					Add(objParameter);
		}
		
		/// <summary>
		///		Obtiene un parámetro de la colección a partir del nombre, si no lo encuentra devuelve un parámetro vacío
		/// </summary>
		public ParameterDB Search(string strName)
		{ // Recorre la colección de parámetros buscando el elemento adecuado
				foreach (ParameterDB objParameter in this)
					if (objParameter.Name.Equals(strName, StringComparison.CurrentCultureIgnoreCase))
						return objParameter;
			// Devuelve un objeto vacío
				return new ParameterDB(strName, null, ParameterDirection.Input);
		}
		
		/// <summary>
		///		Obtiene el valor de un parámetro o un valor predeterminado si es null
		/// </summary>
		public object iisNull(string strName)
		{ ParameterDB objParameter = Search(strName);
		
				if (objParameter.Value == DBNull.Value)
					return null;
				else
					return objParameter.Value;
		}

		/// <summary>
		///		Normaliza las fechas
		/// </summary>
		public void NormalizeDates(ref DateTime? dtmStart, ref DateTime? dtmEnd) 
		{ // Cambia las fechas
				SwapDates(ref dtmStart, ref dtmEnd);
			// Normaliza las fechas de inicio o fin
				if (dtmStart != null)
					dtmStart = GetNormalizedDate(dtmStart, 0, 0, 0);
				if (dtmEnd != null)
					dtmEnd = GetNormalizedDate(dtmEnd, 23, 59, 59);
		}
		
		/// <summary>
		///		Intercambia dos fechas para un filtro
		/// </summary>
		private void SwapDates(ref DateTime? dtmFrom, ref DateTime? dtmTo)
		{ if (dtmFrom != null && dtmTo != null && dtmFrom > dtmTo)
		    { DateTime? dtmValue = dtmFrom;
				
		        dtmFrom = dtmTo;
		        dtmTo = dtmValue;
		    }
		}

		/// <summary>
		///		Obtiene una fecha normalizada al inicio o fin del día
		/// </summary>
		private DateTime? GetNormalizedDate(DateTime? dtmValue, int intHour, int intMinute, int intSecond) 
		{ DateTime dtmNormalized = dtmValue ?? DateTime.Now;

				// Devuelve la fecha normalizada
					return new DateTime(dtmNormalized.Year, dtmNormalized.Month, dtmNormalized.Day, intHour, intMinute, intSecond);
		}

		/// <summary>
		///		Indizador de la colección por el nombre de parámetro
		/// </summary>
		public ParameterDB this[string strName]
		{ get { return Search(strName); }
		}
	}
}