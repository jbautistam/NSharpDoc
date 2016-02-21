using System;
using System.Data;

namespace Bau.Libraries.LibDBProvidersBase.Parameters
{
	/// <summary>
	///		Clase que almacena los par�metros de un comando
	/// </summary>
	public class ParametersDBCollection : System.Collections.Generic.List<ParameterDB>
	{ 
		/// <summary>
		///		A�ade un par�metro a la colecci�n de par�metros del comando
		/// </summary>
		public void Add(string strName, string strValue, int intLength, ParameterDirection intDirection = ParameterDirection.Input)
		{ // Corta la cadena si supera la longitud m�xima
				if (!string.IsNullOrEmpty(strValue) && strValue.Length > intLength)
					strValue = strValue.Substring(0, intLength);
			// A�ade el par�metro
				Add(new ParameterDB(strName, strValue, intDirection, intLength));
		}

		/// <summary>
		///		A�ade un par�metro a la colecci�n de par�metros del comando
		/// </summary>
		public void Add(string strName, object objValue, ParameterDirection intDirection = ParameterDirection.Input, bool blnSkipValueZero = true)
		{ if (objValue is Enum)
				{ int? intValue = (int) ((object) objValue);

						// Convierte el valor 0 del enumerado en un valor NULL
							if (blnSkipValueZero && (intValue ?? 0) == 0)
								intValue = null;
						// A�ade el valor del par�metro
							Add(new ParameterDB(strName, intValue, intDirection));
				}		
			else
				Add(new ParameterDB(strName, objValue, intDirection));
		}		
		
		/// <summary>
		///		A�ade un par�metro a la colecci�n de par�metros del comando
		/// </summary>
		public void Add(string strName, byte [] arrBytBuffer, ParameterDirection intDirection = ParameterDirection.Input)
		{ Add(new ParameterDB(strName, arrBytBuffer, intDirection));
		}

		/// <summary>
		///		A�ade un par�metro de tipo Text
		/// </summary>
		public void AddText(string strName, string strValue, ParameterDirection intDirection = ParameterDirection.Input)
		{ ParameterDB objParameter = new ParameterDB(strName, strValue, intDirection);

				// Indica que es un par�metro de texto
					objParameter.IsText = true;
				// A�ade el par�metro a la colecci�n
					Add(objParameter);
		}
		
		/// <summary>
		///		Obtiene un par�metro de la colecci�n a partir del nombre, si no lo encuentra devuelve un par�metro vac�o
		/// </summary>
		public ParameterDB Search(string strName)
		{ // Recorre la colecci�n de par�metros buscando el elemento adecuado
				foreach (ParameterDB objParameter in this)
					if (objParameter.Name.Equals(strName, StringComparison.CurrentCultureIgnoreCase))
						return objParameter;
			// Devuelve un objeto vac�o
				return new ParameterDB(strName, null, ParameterDirection.Input);
		}
		
		/// <summary>
		///		Obtiene el valor de un par�metro o un valor predeterminado si es null
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
		///		Obtiene una fecha normalizada al inicio o fin del d�a
		/// </summary>
		private DateTime? GetNormalizedDate(DateTime? dtmValue, int intHour, int intMinute, int intSecond) 
		{ DateTime dtmNormalized = dtmValue ?? DateTime.Now;

				// Devuelve la fecha normalizada
					return new DateTime(dtmNormalized.Year, dtmNormalized.Month, dtmNormalized.Day, intHour, intMinute, intSecond);
		}

		/// <summary>
		///		Indizador de la colecci�n por el nombre de par�metro
		/// </summary>
		public ParameterDB this[string strName]
		{ get { return Search(strName); }
		}
	}
}