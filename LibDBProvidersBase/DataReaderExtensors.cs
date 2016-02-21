using System;
using System.Data;

namespace Bau.Libraries.LibDBProvidersBase
{
	/// <summary>
	///		Funciones de extensión de <see cref="IDataReader"/>
	/// </summary>
	public static class DataReaderExtensors
	{
		/// <summary>
		///		Obtiene el valor de un campo de un IDataReader
		/// </summary>
		public static object IisNull(this IDataReader rdoReader, string strField, object objDefault = null)
		{ if (rdoReader.IsDBNull(rdoReader.GetOrdinal(strField)))
				return objDefault;
			else
				return rdoReader.GetValue(rdoReader.GetOrdinal(strField));
		}

		/// <summary>
		///		Obtiene el valor de un campo de un IDataReader
		/// </summary>
		public static bool IisNull(this IDataReader rdoReader, string strField, bool blnDefault)
		{ if (rdoReader.IsDBNull(rdoReader.GetOrdinal(strField)))
				return blnDefault;
			else
				return rdoReader.GetBoolean(rdoReader.GetOrdinal(strField));
		}

		/// <summary>
		///		Obtiene el valor de un campo de un IDataReader
		/// </summary>
		public static int IisNull(this IDataReader rdoReader, string strField, int intDefault)
		{ if (rdoReader.IsDBNull(rdoReader.GetOrdinal(strField)))
				return intDefault;
			else
				return rdoReader.GetInt32(rdoReader.GetOrdinal(strField));
		}

		/// <summary>
		///		Obtiene el valor de un campo de un IDataReader
		/// </summary>
		public static double IisNull(this IDataReader rdoReader, string strField, double dblDefault)
		{ if (rdoReader.IsDBNull(rdoReader.GetOrdinal(strField)))
				return dblDefault;
			else
				return rdoReader.GetDouble(rdoReader.GetOrdinal(strField));
		}

		/// <summary>
		///		Obtiene el valor de un campo de un IDataReader
		/// </summary>
		public static DateTime IisNull(this IDataReader rdoReader, string strField, DateTime dtmDefault)
		{ if (rdoReader.IsDBNull(rdoReader.GetOrdinal(strField)))
				return dtmDefault;
			else
				return rdoReader.GetDateTime(rdoReader.GetOrdinal(strField));
		}
	}
}
