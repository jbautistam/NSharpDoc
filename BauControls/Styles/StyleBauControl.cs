using System;
using System.ComponentModel;
using System.Drawing;

namespace Bau.Controls.Styles
{
	/// <summary>
	///		Estilo de un gradiante
	/// </summary>
	[TypeConverter(typeof(StyleBauControlConverter))]
	public class StyleBauControl : IDisposable
	{ // Delegados
			internal delegate void ChangedStyleHandler(StyleBauControl objSender);
		// Eventos
			internal event ChangedStyleHandler OnChangedStyle;
		// Enumerados
			/// <summary>
			///		Enumerado especial para un gradiante
			///	</summary>
			public enum GroupBoxGradientMode
				{	/// <summary>Sin gradiante</summary>
						None = 4,
					/// <summary>Gradiante de arriba a la derecha abajo a la izquierda</summary>
						BackwardDiagonal = 3,
					/// <summary>Gradiente de arriba a la izquierda abajo a la derecha</summary>
						ForwardDiagonal = 2,
					/// <summary>Gradiante de izquierda a derecha</summary>
						Horizontal = 0,
					/// <summary>Gradiante de arriba abajo</summary>
						Vertical = 1
				}
		// Variables privadas
			private int intRoundCorners = 10;
			private int intAngle = 90;
			private int intShadowThickness = 3;
			private int intBorderWidth = 1;
			private Color clrBorder = Color.Black;
			private Color clrBackground = Color.White;
			private Color clrBackgroundGradient = Color.Gainsboro;
			private Color clrShadow = Color.DarkGray;
			private Color clrFore = Color.Blue;
			private GroupBoxGradientMode intMode = GroupBoxGradientMode.None;
		
		/// <summary>
		///		Limpia los recursos que se estén utilizando.
		/// </summary>
		public void Dispose(bool blnDisposing)
		{
		} 		
		
		/// <summary>
		///		Limpia los recursos que se estén utilizando.
		/// </summary>
		public void Dispose()
		{ Dispose(true);
		}
		
 		/// <summary>
 		///		Lanza el evento de modificación del estilo
 		/// </summary>
 		private void RaiseChangedStyle()
 		{	if (OnChangedStyle != null)
 				OnChangedStyle(this);
 		}

		/// <summary>
		///		Normaliza un valor entre unos límites
		/// </summary>
		private int Normalize(int intValue, int intMin, int intMax)
		{ // Normaliza el valor
				if (intValue < intMin)
					intValue = intMin;
				else if (intValue > intMax)
					intValue = intMax;
			// Devuelve el valor
				return intValue;
		}
		
		[Category("Appearance"), Description("Redondeo de las esquinas")]
		[RefreshProperties(RefreshProperties.Repaint), NotifyParentProperty(true), DefaultValue(10)]
		public int RoundCorners
		{ get { return intRoundCorners; }
			set 
				{ intRoundCorners = value;
					RaiseChangedStyle();
				}
		}
		
		/// <summary>
		///		Angulo de las esquinas
		/// </summary>
		[Category("Appearance"), Browsable(true), Description("Angulo de las esquinas"),
		 RefreshProperties(RefreshProperties.Repaint), NotifyParentProperty(true), DefaultValue(90)]
		public int AngleCorners
		{ get { return intAngle; }
			set
				{ intAngle = value;
					RaiseChangedStyle();
				}
		}

		[Category("Appearance"), Description("Ancho de la sombra")]
		[RefreshProperties(RefreshProperties.Repaint), NotifyParentProperty(true), DefaultValue(3)]
		public int ShadowThickness
		{ get { return intShadowThickness; }
			set 
				{ intShadowThickness = Normalize(value, 1, 15);
					RaiseChangedStyle();
				}
		}

		[Category("Appearance"), Description("Ancho del borde")]
		[RefreshProperties(RefreshProperties.Repaint), NotifyParentProperty(true), DefaultValue(1)]
		public int BorderWidth
		{ get { return intBorderWidth; }
			set 
				{ intBorderWidth = Normalize(value, 1, 5);
					RaiseChangedStyle();
				}
		}
			
		[Category("Appearance"), Description("Color del borde")]
		[RefreshProperties(RefreshProperties.Repaint), NotifyParentProperty(true), DefaultValue(typeof(Color), "Black")]
		public Color BorderColor
		{ get { return clrBorder; }
			set 
				{ clrBorder = value;
					RaiseChangedStyle();
				}
		}

		[Category("Appearance"), Description("Color del texto")]
		[RefreshProperties(RefreshProperties.Repaint), NotifyParentProperty(true), 
		 DefaultValue(typeof(Color), "Blue")]
		public Color ForeColor
		{ get { return clrFore; }
			set 
				{ clrFore = value;
					RaiseChangedStyle();
				}
		}		

		[Category("Appearance"), Description("Color del fondo")]
		[RefreshProperties(RefreshProperties.Repaint), NotifyParentProperty(true), 
		 DefaultValue(typeof(Color), "White")]
		public Color BackgroundColor
		{ get { return clrBackground; }
			set 
				{ clrBackground = value;
					RaiseChangedStyle();
				}
		}		

		[Category("Appearance"), Description("Color final del fondo")]
		[RefreshProperties(RefreshProperties.Repaint), NotifyParentProperty(true), 
		 DefaultValue(typeof(Color), "Gainsboro")]
		public Color BackgroundGradientColor
		{ get { return clrBackgroundGradient; }
			set 
				{ clrBackgroundGradient = value;
					RaiseChangedStyle();
				}
		}		

		[Category("Appearance"), Description("Color de la sombra"),
		 RefreshProperties(RefreshProperties.Repaint), NotifyParentProperty(true), DefaultValue(typeof(Color), "DarkGray")]
		public Color ShadowColor
		{ get { return clrShadow; }
			set 
				{ clrShadow = value;
					RaiseChangedStyle();
				}
		}		

		[Category("Appearance"), Description("Tipo de gradiante")]
		[RefreshProperties(RefreshProperties.Repaint), NotifyParentProperty(true)]
		public GroupBoxGradientMode GradientMode
		{ get { return intMode; }
			set 
				{ intMode = value;
					RaiseChangedStyle();
				}
		}		
	}
}
