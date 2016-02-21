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
	public class LabelRound : System.Windows.Forms.Control
	{ // Constantes privadas
			private Image imgGroup = null;
			private StyleBauControl objStyle;
			
		/// <summary>
		///		Constructor del control
		/// </summary>
		public LabelRound()
		{	// Inicializa los estilos
				SetStyle(ControlStyles.DoubleBuffer, true);
				SetStyle(ControlStyles.AllPaintingInWmPaint, true);
				SetStyle(ControlStyles.UserPaint, true);
				SetStyle(ControlStyles.SupportsTransparentBackColor, true);
			// Inicializa las propiedades
				Name = "LabelRound";
				Text = Name;
				Size = new Size(368, 288);
			// Inicializa los estilos de etiqueta y panel
				Style = new StyleBauControl();
		}

		/// <summary>
		///		Limpia los recursos que se estén utilizando.
		/// </summary>
		protected override void Dispose(bool blnDisposing)
		{	// Elimina los objetos
				if (blnDisposing)
					{	objStyle = null;
					}
			// Llama al base para eliminar los objetos
				base.Dispose(blnDisposing);
		}
		
		/// <summary>
		///		Sobrescribe el método OnPaint
		/// </summary>
		protected override void OnPaint(PaintEventArgs e)
		{	int intLeft = Style.RoundCorners / 2 + (Image != null ? 17 : 1);
			int intWidth = Width;
			int intHeight = Height;
		
				// Antialias
					e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
				// Pinta el fondo
					clsHelperDrawing.PaintRoundedPanel(e.Graphics, 0, 0, Width, Height, Style);
				// Ajusta el ancho y el alto si tiene una sombre
					if (Style.ShadowColor != Color.Transparent)
						{ intWidth -= Style.ShadowThickness;
							intHeight -= Style.ShadowThickness;
						}
				// Pinta el texto
					using (SolidBrush brsText = new SolidBrush(ForeColor))
						e.Graphics.DrawString(base.Text, Font, brsText, new RectangleF(intLeft, 5, intWidth - intLeft, intHeight - 5));
				// Pinta la imagen si existe
					if (Image != null)
						e.Graphics.DrawImage(Image, intLeft - 16, 4, 16, 16);
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
		///		Imagen del título
		///	</summary>
		[Category("Appearance"), Description("Imagen del título")]
		public Image Image
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
		public StyleBauControl Style
		{ get { return objStyle; }
			set 
				{ // Obtiene el estilo
						objStyle = value; 
					// Inicializa la rutina de tratamiento del evento de cambio de estilo
						objStyle.OnChangedStyle += new StyleBauControl.ChangedStyleHandler(Style_OnChangedStyle);
					// Invalida la presentación
						Invalidate();
				}
		}

	}
}
