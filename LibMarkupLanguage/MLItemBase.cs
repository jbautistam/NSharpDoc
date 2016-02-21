using System;

namespace Bau.Libraries.LibMarkupLanguage
{
	/// <summary>
	///		Base para nodos y atributos
	/// </summary>
	public class MLItemBase
	{ // Constantes privadas
			internal const string cnstStrYes = "yes";
			internal const string cnstStrNo = "no";
			internal const string cnstStrTrue = "true";
			internal const string cnstStrFalse = "false";
			
		public MLItemBase()
		{
		}

		public MLItemBase(string strName) : this(null, strName, null, false) {}
		
		public MLItemBase(string strName, string strValue) : this(null, strName, strValue, true) {}
		
		public MLItemBase(string strName, bool blnValue) : this(null, strName, blnValue) {}
		
		public MLItemBase(string strName, double dblValue) : this(null, strName, dblValue) {}
								
		public MLItemBase(string strName, DateTime dtmValue) : this(null, strName, dtmValue) {}
		
		public MLItemBase(string strName, string strValue, bool blnIsCData) : this(null, strName, strValue, blnIsCData) {}
		
		public MLItemBase(string strPrefix, string strName, double dblValue) :
								this(strPrefix, strName, Format(dblValue), false) {}
								
		public MLItemBase(string strPrefix, string strName, DateTime dtmValue) :
								this(strPrefix, strName, Format(dtmValue), false) {}
								
		public MLItemBase(string strPrefix, string strName, string strValue, bool blnIsCData)
		{ Prefix = strPrefix;
			Name = strName;
			Value = strValue;
			IsCData = blnIsCData;
		}

		/// <summary>
		///		Obtiene un valor lógico
		/// </summary>
		public bool GetValue(bool blnDefault)
		{ if (string.IsNullOrEmpty(Value))
				return blnDefault;
			else if (Value.Trim().Equals(MLItemBase.cnstStrTrue, StringComparison.CurrentCultureIgnoreCase) || 
							 Value.Trim().Equals(MLItemBase.cnstStrYes, StringComparison.CurrentCultureIgnoreCase))
				return true;
			else if (Value.Trim().Equals(MLItemBase.cnstStrFalse, StringComparison.CurrentCultureIgnoreCase) || 
							 Value.Trim().Equals(MLItemBase.cnstStrNo, StringComparison.CurrentCultureIgnoreCase))
				return false;
			else
				return blnDefault;
		}
				
		/// <summary>
		/// 	Obtiene el valor de un nodo 
		/// </summary>
		public DateTime GetValue(DateTime dtmDefault)
		{ DateTime dtmParse;
		
				if (string.IsNullOrEmpty(Value) || !DateTime.TryParse(Value, out dtmParse))
					return Tools.DateTimeHelper.ParseRfc(Value, dtmDefault);	
				else 
					return dtmParse;
		}

		/// <summary>
		/// 	Obtiene el valor de un nodo 
		/// </summary>
		public int GetValue(int intDefault)
		{ int intParse;
		
				if (string.IsNullOrEmpty(Value) || !int.TryParse(Value, out intParse))
					return intDefault;	
				else
					return intParse;
		}

		/// <summary>
		///		Obtiene el valor numérico de un nodo
		/// </summary>
		public long GetValue(long lngDefault)
		{ long lngParse;
		
				if (string.IsNullOrEmpty(Value) || !long.TryParse(Value, out lngParse))
					return lngDefault;
				else
					return lngParse;
		}
		
		/// <summary>
		/// 	Obtiene el valor de un nodo 
		/// </summary>
		public double GetValue(double dblDefault)
		{ double dblParse;
		
				if (string.IsNullOrEmpty(Value) || !double.TryParse(Value, out dblParse))
					return dblDefault;
				else
					return dblParse;
		}
			
		/// <summary>
		///		Formatea un valor lógico
		/// </summary>
		internal static string Format(bool blnValue)
		{ if (blnValue)
				return cnstStrYes;
			else
				return cnstStrNo;
		}
		
		/// <summary>
		///		Formatea un valor numérico
		/// </summary>
		internal static string Format(double? dblValue)
		{ if (dblValue == null)
				return "";
			else
				return dblValue.ToString().Replace(',', '.');
		}
		
		/// <summary>
		///		Formatea un valor de tipo fecha
		/// </summary>
		internal static string Format(DateTime dtmValue)
		{ return string.Format("{0:yyyy-MM-dd HH:mm:ss}", dtmValue);
		}
		
		/// <summary>
		///		Prefijo del espacio de nombres
		/// </summary>
		public string Prefix { get; set; }
		
		/// <summary>
		///		Nombre del elemento
		/// </summary>
		public string Name { get; set; }
		
		/// <summary>
		///		Valor del elemento
		/// </summary>
		public string Value { get; set; }
		
		/// <summary>
		///		Indica si se debe tratar como CData
		/// </summary>
		public bool IsCData { get; set; }
	}
}
