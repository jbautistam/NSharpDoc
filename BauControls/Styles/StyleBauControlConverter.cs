using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;

namespace Bau.Controls.Styles
{
	/// <summary>
	///		Conversor para los estilos de control
	/// </summary>
	public class StyleBauControlConverter : ExpandableObjectConverter
	{
		/// <summary>
		///		Comprueba si se puede convertir a un tipo
		/// </summary>
		public override bool CanConvertTo(ITypeDescriptorContext cntContext, Type intDestinationType)
		{	if (intDestinationType == typeof(string))
				return true;
			else
				return base.CanConvertTo(cntContext, intDestinationType);
		}	

		/// <summary>
		///		Convierte un valor a una cadena
		/// </summary>
		public override object ConvertTo(ITypeDescriptorContext cntContext, CultureInfo culCulture, 
																		 object objValue, Type intDestinationType)
		{	if (intDestinationType == typeof(string))
				return ConvertToString(objValue);
			else
				return base.ConvertTo(cntContext, culCulture, objValue, intDestinationType);
		}

		/// <summary>
		///		Convierte un valor a una cadena
		/// </summary>
		public new string ConvertToString(object objValue)
		{ StyleBauControl objStyle = (StyleBauControl) objValue;
			ColorConverter objConverter = new ColorConverter();
			string strConvert;
			
					// Obtiene la cadena formateada con los valores numéricos
						strConvert = objStyle.RoundCorners.ToString() + ",";
						strConvert = objStyle.AngleCorners.ToString() + ",";
						strConvert += objStyle.ShadowThickness.ToString() + ",";
						strConvert += objStyle.BorderWidth.ToString() + ",";
					// Obtiene la cadena formateada con los colores
						strConvert += objConverter.ConvertToString(objStyle.BorderColor) + ",";
						strConvert += objConverter.ConvertToString(objStyle.BackgroundColor) + ",";
						strConvert += objConverter.ConvertToString(objStyle.BackgroundGradientColor) + ",";
						strConvert += objConverter.ConvertToString(objStyle.ShadowColor) + ",";
						strConvert += objConverter.ConvertToString(objStyle.ForeColor) + ",";
					// Obtiene la cadena formateada con el modo del gradiante
						strConvert += ((int) objStyle.GradientMode).ToString();
					// Devuelve la cadena
						return strConvert;
		}
		
		/// <summary>
		///		Comprueba si se puede convertir a partir de un valor
		/// </summary>
		public override bool CanConvertFrom(ITypeDescriptorContext cntContext, Type intSourceType)
		{ if (intSourceType == typeof(string))
				return true;
			else
				return base.CanConvertFrom(cntContext, intSourceType);
		}

		/// <summary>
		///		Convierte un objeto a partir de un valor
		/// </summary>
		public override object ConvertFrom(ITypeDescriptorContext cntContext, CultureInfo culCulture, object objValue)
		{	if (objValue is string)
				return ConvertFromString(objValue);
			else
				return base.ConvertFrom(cntContext, culCulture, objValue);
		}

		/// <summary>
		///		Obtiene un estilo a partir de una cadena
		/// </summary>
		private StyleBauControl ConvertFromString(object objValue)
		{	string [] arrStrValues = ((string) objValue).Split(',');
			ColorConverter objConverter = new ColorConverter();
			StyleBauControl objStyle = new StyleBauControl();
		
				try
					{	// Obtiene la cadena formateada con los valores numéricos
							objStyle.RoundCorners = int.Parse(arrStrValues[0]);
							objStyle.AngleCorners = int.Parse(arrStrValues[1]);
							objStyle.ShadowThickness = int.Parse(arrStrValues[2]);
							objStyle.BorderWidth = int.Parse(arrStrValues[3]);
						// Obtiene la cadena formateada con los colores
							objStyle.BorderColor = (Color) objConverter.ConvertFromString(arrStrValues[4]);
							objStyle.BackgroundColor = (Color) objConverter.ConvertFromString(arrStrValues[5]);
							objStyle.BackgroundGradientColor = (Color) objConverter.ConvertFromString(arrStrValues[6]);
							objStyle.ShadowColor = (Color) objConverter.ConvertFromString(arrStrValues[7]);
							objStyle.ForeColor = (Color) objConverter.ConvertFromString(arrStrValues[8]);
						// Obtiene la cadena formateada con el modo del gradiante
							objStyle.GradientMode = (StyleBauControl.GroupBoxGradientMode) int.Parse(arrStrValues[9]);
						// Devuelve el estilo
							return objStyle;
					}
				catch
					{	throw new ArgumentException("Could not convert the value");
					}
		}	
	}
}
