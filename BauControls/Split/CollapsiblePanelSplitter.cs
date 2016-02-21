using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Bau.Controls.Split
{
	/// <summary>
	///		Splitter con dos paneles
	/// </summary>
	public class CollapsiblePanelSplitter : SplitContainer
	{
		/// <summary>
		///		Enumerado para definir el estilo visual que se debe aplicar al splitter
		/// </summary>
		public enum VisualStyles
			{	Mozilla = 0,
				XP,
				Win9x,
				DoubleDots,
				Lines
			}
		/// <summary>
		///		Enumerado con el panel que se debe ocultar
		/// </summary>
		public enum CollapseMode
			{ NoCollapse,
				CollapsePanel1,
				CollapsePanel2
			}
		// Variables privadas	
			private bool blnIsMouseOnThumb = false;
			private Color clrBackColorSpliter;
			private VisualStyles intVisualStyle;
			private CollapseMode intCollapseMode;
			private Rectangle rctThumb;

		public CollapsiblePanelSplitter()
		{ SplitterStyle = VisualStyles.Mozilla;
			CollapseAction = CollapseMode.CollapsePanel1;
			BackColorSplitter = BackColor;
			SplitterWidth = 8;
			Panel1MinSize = 0;
			Panel2MinSize = 0;
		}			
		
		/// <summary>
		///		Sobrescribe el evento OnPaint
		/// </summary>
		protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
		{ Graphics g = e.Graphics;
			Point pntPosition;
				
				// Dibuja el fondo del spliter
					g.FillRectangle(new SolidBrush(BackColorSplitter), SplitterRectangle);
				// Obtiene el rectágulo del thumb
					rctThumb = GetRectangleOriented(SplitterRectangle.X, (int) SplitterRectangle.Y + (SplitterRectangle.Height - 115) / 2, 8, 115,
																					(int) SplitterRectangle.X + (SplitterRectangle.Width - 115) / 2, SplitterRectangle.Y, 115, 8);
				// Obtiene la posición donde se deben comenzar a pintar los puntos
					pntPosition = GetPointOriented(rctThumb.X + 3, rctThumb.Y + 14, rctThumb.X + 14, rctThumb.Y + 3);
				// Dibuja el thumb
					DrawButton(g, rctThumb);
				// Dibuja el thumb adecuado dependiendo del estilo
					switch(SplitterStyle)
						{	case VisualStyles.Mozilla:
									DrawControlMozilla(g, pntPosition.X, pntPosition.Y);
								break;
							case VisualStyles.DoubleDots:
									DrawControlDoubleDots(g, pntPosition.X, pntPosition.Y);
								break;
							case VisualStyles.Win9x:
									DrawControlWin9x(g, pntPosition.X, pntPosition.Y);
								break;
							case VisualStyles.XP:
									DrawControlWinXP(g, pntPosition.X, pntPosition.Y);
								break;
							case VisualStyles.Lines:
									DrawControlLines(g, pntPosition.X, pntPosition.Y);
								break;
						}
				// dispose the Graphics object
					//g.Dispose();
		}

		/// <summary>
		///		Sobrescribe el evento OnMouseMove
		/// </summary>
		protected override void OnMouseMove(System.Windows.Forms.MouseEventArgs e)
		{	if (e.X >= rctThumb.X && e.X <= rctThumb.X + rctThumb.Width && 
					e.Y >= rctThumb.Y && e.Y <= rctThumb.Y + rctThumb.Height)
				{	if (!blnIsMouseOnThumb)
						{	blnIsMouseOnThumb = true;
							Invalidate();
						}
				}
			else
				{	if (blnIsMouseOnThumb)
						{	blnIsMouseOnThumb = false;
							Invalidate();;
						}
				}
			base.OnMouseMove(e);
		}

		/// <summary>
		///		Sobrescribe el evento OnMouseLeave
		/// </summary>
		protected override void OnMouseLeave(EventArgs e)
		{	blnIsMouseOnThumb = false;
			Invalidate();
			base.OnMouseLeave(e);
		}
		
		/// <summary>
		///		Sobrescribe el evento OnClick
		/// </summary>
		protected override void OnClick(EventArgs e)
		{	if (CollapseAction != CollapseMode.NoCollapse && Enabled && blnIsMouseOnThumb)
				ToggleSplitter();
			base.OnClick(e);
		}
		
		/// <summary>
		///		Cambia el estado del splitter
		/// </summary>
		private void ToggleSplitter()
		{ // Cambia los anchos / altos de los paneles
				switch (CollapseAction)
					{ case CollapseMode.CollapsePanel1:
								if (Orientation == System.Windows.Forms.Orientation.Vertical)
									{	if (Panel1.Width == 0)
											SplitterDistance = Width / 2;
										else
											SplitterDistance = 0;
									}
								else
									{	if (Panel1.Height == 0)
											SplitterDistance = Height / 2;
										else
											SplitterDistance = 20;
									}
							break;
						case CollapseMode.CollapsePanel2:
								if (Orientation == System.Windows.Forms.Orientation.Vertical)
									{	if (Panel2.Width == 0)
											SplitterDistance = Width / 2;
										else
											SplitterDistance = 0;
									}
								else
									{	if (Panel2.Height == 0)
											SplitterDistance = Height / 2;
										else
											SplitterDistance = 0;
									}
							break;
					}
			// Pinta de nuevo el control
				//Invalidate();
		}
		
		/// <summary>
		///		Dibuja la sección del splitter en el centro que indica si se colapsa a la izquierda o a la derecha
		/// </summary>
		private void DrawButton(Graphics g, Rectangle rr)
		{ // Dibuja el color de fondo del control de cierre de los paneles
				using (SolidBrush brsBackground = new SolidBrush(blnIsMouseOnThumb ? GetHotColor() : BackColorSplitter))
					g.FillRectangle(brsBackground, GetRectangleOriented(rr.X + 1, rr.Y, 6, 115,
																															rr.X, rr.Y + 1, 115, 6));
			// Dibuja las líneas superior e inferior del control de cierre de los paneles
				using (Pen penLines = new Pen(SystemColors.ControlDark, 1))
					{	g.DrawLine(penLines, GetPointOriented(rr.X + 1, rr.Y,
																									rr.X, rr.Y + 1), 
																 GetPointOriented(rr.X + rr.Width - 2, rr.Y,
																									rr.X, rr.Y + rr.Height - 2));
						g.DrawLine(penLines, GetPointOriented(rr.X + 1, rr.Y + rr.Height,
																									rr.X + rr.Width, rr.Y + 1), 
																 GetPointOriented(rr.X + rr.Width - 2, rr.Y + rr.Height,
																									rr.X + rr.Width, rr.Y + rr.Height - 2));
					}
			// Dibuja las flechas
				if (Enabled && CollapseAction != CollapseMode.NoCollapse)
					using (SolidBrush brsArrow = new SolidBrush(SystemColors.ControlDarkDark))
						{ g.FillPolygon(brsArrow, GetPointsArrows(GetPointOriented(rr.X + 2, rr.Y + 3,
																																			 rr.X + 3, rr.Y + 2)));
							g.FillPolygon(brsArrow, GetPointsArrows(GetPointOriented(rr.X + 2, rr.Y + rr.Height - 9,
																																			 rr.X +  rr.Width - 9, rr.Y + 2)));
						}
		}

		/// <summary>
		///		Dibuja las líneas del thumb con estilo Mozilla
		/// </summary>
		private void DrawControlMozilla(Graphics g, int x, int y)
		{ Pen penLight = new Pen(SystemColors.ControlLightLight);
			Pen penDark = new Pen(SystemColors.ControlDarkDark);
			Pen penBackground = new Pen(blnIsMouseOnThumb ? GetHotColor() : BackColorSplitter, 2);

				// Dibuja las líneas
					for (int i = 0; i < 30; i++)
						{	// Punto claro
							  g.DrawLine(penLight, GetPointOriented(x, y + i * 3, 
							                                       x + i * 3, y),
							                       GetPointOriented(x + 1, y + 1 + i * 3,
							                                        x + 1 + i * 3, y + 1));
							// Punto oscuro
							  g.DrawLine(penDark, GetPointOriented(x + 1, y + 1 + i * 3, 
							                                       x + 1 + i * 3, y + 1),
							                      GetPointOriented(x + 2, y + 2 + i * 3,
							                                       x + 2 + i * 3, y + 2));
							// Fondo (dibuja líneas de dos pixels, no simplemente puntos)
							  g.DrawLine(penBackground, GetPointOriented(x + 2, y + 1 + i * 3, 
							                                             x + 1 + i * 3, y + 2),
							                            GetPointOriented(x + 2, y + 2 + i * 3,
							                                             x + 2 + i * 3, y + 2));
						}
				// Libera los pinceles
					penLight.Dispose();
					penDark.Dispose();
					penBackground.Dispose();
		}

		/// <summary>
		///		Dibuja un thumb con puntos dobles
		/// </summary>
		private void DrawControlDoubleDots(Graphics g, int x, int y)
		{ Pen penLight = new Pen(SystemColors.ControlLightLight);
			Pen penDark = new Pen(SystemColors.ControlDarkDark);

				// Dibuja los puntos				
					for (int i = 0; i < 30; i++)
						{ // Punto claro
								g.DrawRectangle(penLight, GetRectangleOriented(x, y + 1 + i * 3, 1, 1,
																															 x + 1 + i * 3, y, 1, 1));
							// Punto oscuro
								g.DrawRectangle(penDark, GetRectangleOriented(x - 1, y + i * 3, 1, 1,
																															x + i * 3, y - 1, 1, 1));
							i++;
							// Punto claro
							  g.DrawRectangle(penLight, GetRectangleOriented(x + 2, y + 1 + i * 3, 1, 1,
							                                                 x + 1 + i * 3, y + 2, 1, 1));
							// Punto oscuro
							  g.DrawRectangle(penDark, GetRectangleOriented(x + 1, y  + i * 3, 1, 1,
							                                                x + i * 3, y + 1, 1, 1));
						}
				// Libera los pinceles
					penLight.Dispose();
					penDark.Dispose();
		}

		/// <summary>
		///		Dibuja un thumb al estilo Win9x
		/// </summary>
		private void DrawControlWin9x(Graphics g, int x, int y)
		{ // Dibuja las líneas claras
				using (Pen penLight = new Pen(SystemColors.ControlLightLight))
					{	// Dibuja las líneas
							g.DrawLine(penLight, GetPointOriented(x, y, x, y),
																	 GetPointOriented(x + 2, y, x, y + 2));
							g.DrawLine(penLight, GetPointOriented(x, y, x, y),
																	 GetPointOriented(x, y + 90, x + 88, y));
						// Libera el pincel
							penLight.Dispose();
					}
			// Dibuja las líneas oscuras
				using (Pen penDark = new Pen(SystemColors.ControlDarkDark))
					{ // Dibuja las líneas
							g.DrawLine(penDark, GetPointOriented(x + 2, y, x, y + 2),
																	GetPointOriented(x + 2, y + 90, x + 88, y + 2));
							g.DrawLine(penDark, GetPointOriented(x, y + 90, x + 88, y),
																	GetPointOriented(x + 2, y + 90, x + 88, y + 2));
						// Libera el pincel
							penDark.Dispose();
					}
		}

		/// <summary>
		///		Dibuja un thumb al estilo WinXP
		/// </summary>
		private void DrawControlWinXP(Graphics g, int x, int y)
		{ Pen penLight = new Pen(SystemColors.ControlLight);
			Pen penUltraLight = new Pen(SystemColors.ControlLightLight);		
			Pen penDark = new Pen(SystemColors.ControlDark);
			Pen penUltraDark = new Pen(SystemColors.ControlDarkDark);
		
				// Pinta el thumb
					for(int i=0; i < 18; i++)
						{	// Punto claro
								g.DrawRectangle(penLight, GetRectangleOriented(x, y + i * 5, 2, 2,
																															 x + i * 5, y, 2, 2));
							// Punto ultraclaro
								g.DrawRectangle(penUltraLight, GetRectangleOriented(x + 1, y + 1 + i * 5, 1, 1,
																																		x + 1 + i * 5, y + 1, 1, 1));
							// Punto ultraoscuro
								g.DrawRectangle(penUltraDark, GetRectangleOriented(x, y + i * 5, 1, 1,
																																	 x + i * 5, y, 1, 1));
							// Relleno oscuro
								g.DrawLine(penDark, GetPointOriented(x, y + i * 5, x + i * 5, y),
																		GetPointOriented(x, y + i * 5 + 1, x + i * 5 + 1, y));
								g.DrawLine(penDark, GetPointOriented(x, y + i * 5, x + i * 5, y),
																		GetPointOriented(x + 1, y + i * 5, x + i * 5, y + 1));
						}
				// Libera los pinceles
					penLight.Dispose();
					penUltraLight.Dispose();
					penDark.Dispose();
					penUltraDark.Dispose();
		}

		/// <summary>
		///		Dibuja un thumb con líneas
		/// </summary>
		private void DrawControlLines(Graphics g, int x, int y)
		{ using (Pen penDark = new Pen(SystemColors.ControlDark))
				{ // Dibuja las líneas
						for (int i = 0; i < 44; i++)
							g.DrawLine(penDark, GetPointOriented(x, y + i * 2, x + i * 2, y),
																	GetPointOriented(x + 2, y + i * 2, x + i * 2, y + 2));
					// Libera el pincel
						penDark.Dispose();
				}
		}

		/// <summary>
		///		Obtiene un rectángulo adecuado según la orientación
		/// </summary>
		private Rectangle GetRectangleOriented(int intX1Vertical, int intY1Vertical, int intX2Vertical, int intY2Vertical,
																					 int intX1Horizontal, int intY1Horizontal, int intX2Horizontal, int intY2Horizontal)
		{ if (Orientation == System.Windows.Forms.Orientation.Vertical)
				return new Rectangle(intX1Vertical, intY1Vertical, intX2Vertical, intY2Vertical);
			else
				return new Rectangle(intX1Horizontal, intY1Horizontal, intX2Horizontal, intY2Horizontal);
		}
		
		/// <summary>
		///		Obtiene un punto adecuado según la orientación
		/// </summary>
		private Point GetPointOriented(int intXVertical, int intYVertical, int intXHorizontal, int intYHorizontal)
		{ if (Orientation == System.Windows.Forms.Orientation.Vertical)
				return new Point(intXVertical, intYVertical);
			else
				return new Point(intXHorizontal, intYHorizontal);
		}
		
		/// <summary>
		///		Obtiene un array de puntos para dibujar un polígono similar a una flecha
		/// </summary>
		private Point[] GetPointsArrows(Point pntPosition)
		{	Point[] arrPoints = new Point[3];
			
				// Obtiene el array que indica los triángulos que se van a dibujar sobre el splitter
					if (Orientation == System.Windows.Forms.Orientation.Vertical)
						{	if (Panel1Collapsed) // flecha a la derecha
								{	arrPoints[0] = new Point(pntPosition.X, pntPosition.Y);
									arrPoints[1] = new Point(pntPosition.X + 3, pntPosition.Y + 3);
									arrPoints[2] = new Point(pntPosition.X, pntPosition.Y + 6);
								}
							else // flecha a la izquierda
								{	arrPoints[0] = new Point(pntPosition.X + 3, pntPosition.Y);
									arrPoints[1] = new Point(pntPosition.X, pntPosition.Y + 3);
									arrPoints[2] = new Point(pntPosition.X + 3, pntPosition.Y + 6);
								}
						}
					else
						{ if (Panel1Collapsed) // flecha hacia arriba
								{	arrPoints[0] = new Point(pntPosition.X, pntPosition.Y);
									arrPoints[1] = new Point(pntPosition.X + 6, pntPosition.Y);
									arrPoints[2] = new Point(pntPosition.X + 3, pntPosition.Y + 3);
								}
							else // flecha hacia abajo
								{	arrPoints[0] = new Point(pntPosition.X + 3, pntPosition.Y);
									arrPoints[1] = new Point(pntPosition.X + 6, pntPosition.Y + 4);
									arrPoints[2] = new Point(pntPosition.X, pntPosition.Y + 4);
								}
						}
				// Devuelve el array de puntos
					return arrPoints;
		}
				
		/// <summary>
		///		Obtiene el color del thumb cuando está el cursor sobre él
		/// </summary>
		private Color GetHotColor()
		{ return CalculateColor(SystemColors.Highlight, SystemColors.Window, 70);
		}

		/// <summary>
		///		Calcula un color sólido aplicando un alpha-blending
		/// </summary>
		private static Color CalculateColor(Color front, Color back, int alpha)
		{	Color frontColor = Color.FromArgb(255, front);
			Color backColor = Color.FromArgb(255, back);
			float frontRed = frontColor.R;
			float frontGreen = frontColor.G;
			float frontBlue = frontColor.B;
			float backRed = backColor.R;
			float backGreen = backColor.G;
			float backBlue = backColor.B;
			float fRed = frontRed * alpha / 255 + backRed * ((float)(255 - alpha) / 255);
			byte newRed = (byte) fRed;
			float fGreen = frontGreen * alpha / 255 + backGreen * ((float) (255 - alpha) / 255);
			byte newGreen = (byte)fGreen;
			float fBlue = frontBlue * alpha / 255 + backBlue * ((float) (255-alpha) / 255);
			byte newBlue = (byte) fBlue;

			return Color.FromArgb(255, newRed, newGreen, newBlue);
		}
				
		/// <summary>
		///		Estilo visual del splitter
		/// </summary>
		[Description("Estilo del splitter")]
		public VisualStyles SplitterStyle 
		{ get { return intVisualStyle; }
			set
				{ if (intVisualStyle != value)
						{ // Asigna el estilo
								intVisualStyle = value;
							// Repinta el control
								Invalidate();
						}
				}
		}
		
		/// <summary>
		///		Forma en que se van a ocultar los paneles
		/// </summary>
		[Description("Forma de ocultar los paneles")]
		public CollapseMode CollapseAction
		{ get { return intCollapseMode; }
			set
				{ if (intCollapseMode != value)
						{ // Asigna el modo
								intCollapseMode = value;
							// Repinta el control
								Invalidate();
						}
				}
		}

		
		/// <summary>
		///		Color de fondo del splitter
		/// </summary>
		[Description("Color de fondo del splitter")]
		public Color BackColorSplitter 
		{ get { return clrBackColorSpliter; }
			set 
				{ clrBackColorSpliter = value; 
					Invalidate();
				}
		}
	}
}
