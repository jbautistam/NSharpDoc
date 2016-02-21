using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

using Bau.Controls.Helper;
using Bau.Controls.Styles;

namespace Bau.Controls.Panels
{
	/// <summary>
	///		Panel con bordes redondeados y gradiante
	///	</summary>
	public class CollapsableGroupBox : System.Windows.Forms.ContainerControl
	{ // Enumerados
			public enum CollapsableGroupBoxMode
				{ CaptionAllWidth,
					CaptionAdjusted
				}
		// Constantes privadas
			private const int cnstIntHeightHeader = 23;
			private const int cnstIntIncrementeCollapse = 5;
		// Variables privadas
			private CollapsableGroupBoxMode intMode = CollapsableGroupBoxMode.CaptionAdjusted;
			private bool blnCollapsable = false;
			private bool blnCollapsed = false;
			private Image imgGroup = null;
			private int intRealHeight;
			private StyleBauControl objStyleLabel;
			private StyleBauControl objStylePanel;
			private Timer	tmrCollapse = new Timer();
			
		/// <summary>
		///		Constructor del control
		/// </summary>
		public CollapsableGroupBox()
		{	// Inicializa los estilos
				SetStyle(ControlStyles.DoubleBuffer, true);
				SetStyle(ControlStyles.AllPaintingInWmPaint, true);
				SetStyle(ControlStyles.UserPaint, true);
				SetStyle(ControlStyles.SupportsTransparentBackColor, true);
			// Inicializa las propiedades
				Name = "GroupBox";
				Text = Name;
				Size = new Size(300, 100);
				Padding = new Padding(5, cnstIntHeightHeader + 2, 10, 10);
			// Inicializa la altura real
				intRealHeight = Size.Height;
			// Inicializa los estilos de etiqueta y panel
				StyleLabel = new StyleBauControl();
				StylePanel = new StyleBauControl();
			// Inicializa el temporizador
				tmrCollapse.Interval = 1;
				tmrCollapse.Enabled = false;
				tmrCollapse.Tick += new EventHandler(tmrCollapse_Tick);
		}

		/// <summary>
		///		Limpia los recursos que se estén utilizando.
		/// </summary>
		protected override void Dispose(bool blnDisposing)
		{	// Elimina los objetos
				if (blnDisposing)
					{	objStyleLabel = null;
						objStylePanel = null;
					}
			// Llama al base para eliminar los objetos
				base.Dispose(blnDisposing);
		}
		
		/// <summary>
		///		Sobrescribe el método OnPaint
		/// </summary>
		protected override void OnPaint(PaintEventArgs e)
		{	// Antialias
				e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
			// Dibuja el fondo
				if (!Collapsable || (Collapsable && !Collapsed) || tmrCollapse.Enabled)
					{	if (Mode == CollapsableGroupBoxMode.CaptionAdjusted)
							clsHelperDrawing.PaintRoundedPanel(e.Graphics, 0, 10, Width, Height, StylePanel);
						else
							clsHelperDrawing.PaintRoundedPanel(e.Graphics, 0, 0, Width, Height, StylePanel);
					}
			// Dibuja el texto
				if (Text != string.Empty)
					PaintGroupText(e.Graphics);
			// Dibuja el botón de cerrar
				if (Collapsable)
					PaintButtomCollapse(e.Graphics);
		}

		/// <summary>
		///		Pinta la etiqueta del título
		/// </summary>
		private void PaintGroupText(Graphics g)
		{	int intLeft = 0; 
			int intWidth = Width;
			int intHeight = cnstIntHeightHeader;

				// Ajusta las coordenadas dependiendo del tipo de grupo
					if (Mode == CollapsableGroupBoxMode.CaptionAdjusted)
						{	// Obtiene la coordenada izquierda y el ancho
								intLeft = StylePanel.RoundCorners / 2 + 1;
								intWidth = clsHelperDrawing.GetTextWidth(g, Font, Text) + intLeft + 15;
							// Ajusta el ancho si tiene una imagen
								if (GroupImage != null)
									intWidth += 18;
						}
				// Ajusta el ancho y el alto si tiene una sombra
					if (StyleLabel.ShadowColor != Color.Transparent)
						{ intWidth += StyleLabel.ShadowThickness;
							intHeight += StyleLabel.ShadowThickness;
						}
				// Pinta el fondo
					clsHelperDrawing.PaintRoundedPanel(g, intLeft, 0, intWidth, intHeight, StyleLabel);
				// Pinta el texto
					using (SolidBrush brsText = new SolidBrush(StyleLabel.ForeColor))
						g.DrawString(base.Text, Font, brsText, intLeft + (GroupImage != null ? 19 : 3), 5);
				// Pinta la imagen si existe
					if (GroupImage != null)
						g.DrawImage(GroupImage, intLeft + 3, 3, 16, 16);
		}
		
		/// <summary>
		///		Dibuja el botón de cerrado
		/// </summary>
		private void PaintButtomCollapse(Graphics grpCanvas)
		{ Image imgButton = global::Bau.Controls.Properties.Resources.ArrowCollapsed;
		
				// Obtiene la imagen del botón
					if (!Collapsed)
						imgButton = global::Bau.Controls.Properties.Resources.ArrowOpened;
				// Dibuja la imagen
					grpCanvas.DrawImage(imgButton, Width - StyleLabel.RoundCorners - 18, 3, 16, 16);
		}
		
		/// <summary>
		///		Comienza el cierre del panel
		/// </summary>
		private void BeginColllapse()
		{ if (!tmrCollapse.Enabled)
				tmrCollapse.Enabled = true;
		}
		
		/// <summary>
		///		Finaliza el cierre o apertura del panel
		/// </summary>
		private void EndCollapse()
		{ tmrCollapse.Enabled = false;
			Collapsed = !Collapsed;
		}
		
		/// <summary>
		///		Trata el temporizado de cierre o apertura del panel
		/// </summary>
		private void tmrCollapse_Tick(object sender, EventArgs e)
		{ // Cambia la altura del control
				if (Collapsed)
					{ // Incrementa la altura
							Height += cnstIntIncrementeCollapse;
						// Si ya ha llegado a su altura real, finaliza el redibujo
							if (Height >= intRealHeight)
								{ Height = intRealHeight;
									EndCollapse();
								}
					}
				else
					{ // Decrementa la altura
							Height -= cnstIntIncrementeCollapse;
						// Si ha llegado a la altura mínima finaliza el redibugo
							if (Height <= cnstIntHeightHeader)
								{ Height = cnstIntHeightHeader;
									EndCollapse();
								}
					}
			// Repinta
				Invalidate();
		}
		
		/// <summary>
		///		Trata el evento MouseClick
		/// </summary>
		protected override void OnMouseClick(MouseEventArgs e)
		{ if (e.Button == MouseButtons.Left && Collapsable)
				BeginColllapse();
			base.OnMouseClick(e);
		}

		/// <summary>
		///		Trata el evento Resize
		/// </summary>
		protected override void OnResize(EventArgs e)
		{ if (!tmrCollapse.Enabled)
				intRealHeight = Size.Height;
		}
		
		/// <summary>
		///		Trata el evento de cambio de estilo
		/// </summary>
		private void Style_OnChangedStyle(StyleBauControl objSender)
		{ Invalidate();
		}

		/// <summary>
		///		Título
		/// </summary>
		[Category("Appearance"), Browsable(true), Description("Título")]
		public override string Text
		{ get { return base.Text; }
			set 
				{ base.Text = value;
					Invalidate();
				}
		}	
		
		/// <summary>
		///		Modo de dibujo
		/// </summary>
		[Category("Appearance"), Browsable(true), Description("Modo de dibujo")]
		public CollapsableGroupBoxMode Mode
		{ get { return intMode; }
			set
				{ intMode = value;
					Invalidate();
				}
		}	
		
		/// <summary>
		///		Indica si se puede cerrar
		/// </summary>
		[Category("Appearance"), Browsable(true), Description("Indica si se puede cerrar"), DefaultValue(false)]
		public bool Collapsable
		{ get { return blnCollapsable; }
			set
				{ blnCollapsable = value;
					Invalidate();
				}
		}	
		
		/// <summary>
		///		Indica si está cerrado
		/// </summary>
		[Category("Appearance"), Browsable(true), Description("Indica si está cerrado"), DefaultValue(false)]
		public bool Collapsed
		{ get { return blnCollapsed; }
			set
				{ blnCollapsed = value;
					if (blnCollapsed)
						Size = new Size(Size.Width, cnstIntHeightHeader);
					else
						Size = new Size(Size.Width, intRealHeight);
					Invalidate();
				}
		}	
						
		/// <summary>
		///		Imagen del título
		///	</summary>
		[Category("Appearance"), Browsable(true), Description("Imagen del título")]
		public Image GroupImage
		{	get	{	return imgGroup; } 
			set
				{ imgGroup = value; 
					Invalidate();
				}
		}

		/// <summary>
		///		Estilo del título
		/// </summary>
		[Category("Appearance"), Browsable(true), DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
		 Description("Estilo del título")]
		public StyleBauControl StyleLabel
		{ get { return objStyleLabel; }
			set 
				{ // Obtiene el estilo
						objStyleLabel = value; 
					// Inicializa la rutina de tratamiento del evento de cambio de estilo
						objStyleLabel.OnChangedStyle += new StyleBauControl.ChangedStyleHandler(Style_OnChangedStyle);
					// Invalida la presentación
						Invalidate();
				}
		}

		/// <summary>
		///		Estilo del panel
		/// </summary>
		[Category("Appearance"), Browsable(true), DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
		 Description("Estilo del panel")]
		public StyleBauControl StylePanel	
		{ get { return objStylePanel; }
			set 
				{ // Obtiene el estilo
						objStylePanel = value; 
					// Inicializa la rutina de tratamiento del evento de cambio de estilo
						objStylePanel.OnChangedStyle += new StyleBauControl.ChangedStyleHandler(Style_OnChangedStyle);
					// Invalida la presentación
						Invalidate();
				}
		}
	}
}