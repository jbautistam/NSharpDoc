using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;

using Bau.Controls.Styles;

namespace Bau.Controls.Helper
{
	/// <summary>
	///		Clase para ayuda en la utilización de GDI+
	/// </summary>
	internal class clsHelperDrawing
	{
		/// <summary>
		///		Dibuja una sombra alrededor de un rectángulo redondeado
		/// </summary>
		public static void DrawRoundedShadow(Graphics grpCanvas, int intLeft, int intTop, int intWidth, int intHeight, 
																				 int intArcWidth, int intArcAngle, SolidBrush brsShadow)
		{ using (GraphicsPath pthShadow = GetRoundedPath(intLeft, intTop, intWidth, intHeight, intArcWidth, intArcAngle))
				grpCanvas.FillPath(brsShadow, pthShadow);
		}

		/// <summary>
		///		Dibuja un rectángulo redondeado
		/// </summary>
		public static void FillRoundedRectangle(Graphics grpCanvas, int intLeft, int intTop, int intWidth, int intHeight, 
																						int intArcWidth, int intArcAngle, SolidBrush brsBackground, 
																						Pen penBorder)
		{ using (GraphicsPath pthRectangle = GetRoundedPath(intLeft, intTop, intWidth, intHeight, intArcWidth, intArcAngle))
				{ // Dibuja el rectángulo con su fondo
						grpCanvas.FillPath(brsBackground, pthRectangle);
					// Dibuja el borde
						grpCanvas.DrawPath(penBorder, pthRectangle);
				}
		}

		/// <summary>
		///		Dibuja un rectángulo redondeado con un gradiante
		/// </summary>
		public static void FillRoundedRectangle(Graphics grpCanvas, int intLeft, int intTop, int intWidth, int intHeight, 
																						int intArcWidth, int intArcAngle, 
																						Color clrBackground, Color clrGradient, LinearGradientMode intMode,
																						Pen penBorder)
		{ using (GraphicsPath pthRectangle = GetRoundedPath(intLeft, intTop, intWidth, intHeight, intArcWidth, intArcAngle))
				using (LinearGradientBrush brsGradient = new LinearGradientBrush(new Rectangle(0, 0, intWidth, intHeight), 
																																				 clrBackground, clrGradient, intMode))
					{ // Dibuja el rectángulo con su fondo
							grpCanvas.FillPath(brsGradient, pthRectangle);
						// Dibuja el borde
							grpCanvas.DrawPath(penBorder, pthRectangle);
					}
		}

		
		/// <summary>
		///		Pinta un panel con sombra y fondo con gradiante
		/// </summary>
		public static void PaintRoundedPanel(Graphics grpCanvas, int intLeft, int intTop, int intWidth, int intHeight,
																				 StyleBauControl objStyle)
		{	// Ajusta el ancho y el alto
				intWidth = intWidth - 1;
				intHeight = intHeight - 1;
			// Si hay una sombra, quita su anchura y altura
				if (objStyle.ShadowColor != Color.Transparent)
					{ intWidth -= objStyle.ShadowThickness;
						intHeight -= objStyle.ShadowThickness;
					}
			// Dibuja la sombra si es necesaria
				if (objStyle.ShadowColor != Color.Transparent)
					using (SolidBrush brsShadow = new SolidBrush(objStyle.ShadowColor))
						{ clsHelperDrawing.DrawRoundedShadow(grpCanvas, intLeft + objStyle.ShadowThickness - 1, intTop + objStyle.ShadowThickness - 1, 
																								 intWidth + objStyle.ShadowThickness - 1, intHeight + objStyle.ShadowThickness - 1, 
																								 objStyle.RoundCorners, objStyle.AngleCorners, brsShadow);
						}
			// Dibuja el rectángulo con el gradiante adecuado
				using (Pen penBorder = new Pen(objStyle.BorderColor, objStyle.BorderWidth))
					{	if (objStyle.GradientMode == StyleBauControl.GroupBoxGradientMode.None)
							using (SolidBrush brsBackGround = new SolidBrush(objStyle.BackgroundColor))
								clsHelperDrawing.FillRoundedRectangle(grpCanvas, intLeft, intTop, intWidth, intHeight, 
																											objStyle.RoundCorners, objStyle.AngleCorners,
																											brsBackGround, penBorder);
						else
							clsHelperDrawing.FillRoundedRectangle(grpCanvas, intLeft, intTop, intWidth, intHeight, objStyle.RoundCorners, 
																										objStyle.AngleCorners, objStyle.BackgroundColor, 
																										objStyle.BackgroundGradientColor, 
																										(LinearGradientMode) objStyle.GradientMode, penBorder);
					}
		}
				
		/// <summary>
		///		Obtiene un path de un rectángulo redondeado
		/// </summary>
		public static GraphicsPath GetRoundedPath(int intLeft, int intTop, int intWidth, int intHeight, int intArcWidth, 
																							int intArcAngle)
		{	GraphicsPath pthRounded = new GraphicsPath();
			int intX2 = intWidth - intArcWidth - 1;
			int intY2 = intHeight - intArcWidth - 1;
		
				// Arco superior izquierdo
					pthRounded.AddArc(intLeft, intTop, intArcWidth, intArcWidth, 180, intArcAngle);
				// Arco superior derecho
					pthRounded.AddArc(intX2, intTop, intArcWidth, intArcWidth, 270, intArcAngle);
				// Arco inferior derecho
					pthRounded.AddArc(intX2, intY2, intArcWidth, intArcWidth, 360, intArcAngle);
				// Arco inferior izquierdo
					pthRounded.AddArc(intLeft, intY2, intArcWidth, intArcWidth, 90, intArcAngle);
				// Cierra el path
					pthRounded.CloseAllFigures();
				// Devuelve el path
					return pthRounded;
		}	
				
		/// <summary>
		///		Obtiene el ancho de un texto en un área de dibujo
		/// </summary>
		public static int GetTextWidth(Graphics grpCanvas, Font fntFont, string strText)
		{ return (grpCanvas.MeasureString(strText, fntFont).ToSize()).Width;
		}
		
		/// <summary>
		///		Obtiene el ancho de un texto en un área de dibujo
		/// </summary>
		public static int GetTextHeight(Graphics grpCanvas, Font fntFont, string strText)
		{ return (grpCanvas.MeasureString(strText, fntFont).ToSize()).Height;
		}
	}
}
