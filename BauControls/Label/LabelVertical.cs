using System;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Bau.Controls.Label
{
  /// <summary>
  /// A custom windows control to display text vertically
  /// </summary>
  [ToolboxBitmap(typeof(LabelVertical), "LabelVertical.ico")]
	public partial class LabelVertical : System.Windows.Forms.Control
	{
    /// <summary>
    /// Text Drawing Mode
    /// </summary>
    public enum DrawMode
			{	/// <summary>Text is drawn from bottom - up</summary>
					BottomUp = 1,
				/// <summary>Text is drawn from top to bottom</summary>
					TopBottom
			}
			
		// Variables privadas
			private string strText;
			private DrawMode intDrawMode = DrawMode.BottomUp;
			private bool blnTransparentBG = false;
			System.Drawing.Text.TextRenderingHint intRenderMode = System.Drawing.Text.TextRenderingHint.SystemDefault;
			private System.ComponentModel.Container cntComponents = new System.ComponentModel.Container();

		public LabelVertical()
		{	// Inicializa los estilos
				SetStyle(ControlStyles.DoubleBuffer, true);
				SetStyle(ControlStyles.AllPaintingInWmPaint, true);
				SetStyle(ControlStyles.UserPaint, true);
				SetStyle(ControlStyles.SupportsTransparentBackColor, true);
			// Inicializa las propiedades
				Name = "LabelVertical";
				Text = Name;
				Size = new Size(24, 100);
		}

		protected override void Dispose(bool blnDisposing)
		{	if (blnDisposing && cntComponents != null)
				cntComponents.Dispose();
			base.Dispose(blnDisposing);
		}

		/// <summary>
		///		OnPaint override. This is where the text is rendered vertically.
		/// </summary>
		protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
		{ Color clrColor;

				// Llama al evento base
					base.OnPaint(e);
				// Obtiene el color que se utiliza para el borde y el fondo
					if (blnTransparentBG)
						clrColor = Color.Empty;
					else
						clrColor = BackColor;
				// Dibuja el borde
					using (Pen penBorder = new Pen(clrColor, 0))
						e.Graphics.DrawRectangle(penBorder, 0, 0, Size.Width, Size.Height);
				// Dibuja el fondo
					using (SolidBrush brsBackColor = new SolidBrush(clrColor))
						e.Graphics.FillRectangle(brsBackColor, 0, 0, Size.Width, Size.Height);
				// Asigna los parámetros de dibujo del texto
					e.Graphics.TextRenderingHint = intRenderMode;
					e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
	      // Dibuja el texto
					using (SolidBrush brsForeColor = new SolidBrush(base.ForeColor))
						{	if (TextDrawMode == DrawMode.BottomUp)
								{	e.Graphics.TranslateTransform(0, Size.Height);
									e.Graphics.RotateTransform(270);
									e.Graphics.DrawString(strText, Font, brsForeColor, 0, 0);
								}
							else
								{	// dblTransformX = Size.Width;
									// dblTransformY = Size.Height;
									e.Graphics.TranslateTransform(Size.Width, 0);
									e.Graphics.RotateTransform(90);
									e.Graphics.DrawString(strText, Font, brsForeColor, 0, 0, StringFormat.GenericTypographic);
								}
						}
		}

		//protected override CreateParams CreateParams
		//{	get
		//    {	CreateParams cp = base.CreateParams;
	      
		//        cp.ExStyle |= 0x20;  // Turn on WS_EX_TRANSPARENT
		//        return cp;
		//    }
		//}

		protected override void OnResize(EventArgs e)
		{ Invalidate();
		}

		/// <summary>
		///		Graphics rendering mode. Supprot for antialiasing.
		/// </summary>
		[Category("Properties"), Description("Rendering mode.")]
		public System.Drawing.Text.TextRenderingHint RenderingMode
		{	get { return intRenderMode; }
			set { intRenderMode = value; }
		}
	  
		/// <summary>
		///		The text to be displayed in the control
		/// </summary>
		[Category("LabelVertical"), Description("Text is displayed vertically in container.")]
		public override string Text
		{	get {	return strText; }
			set
				{	strText = value;
					Invalidate();
				}
		}
	  
		/// <summary>
		///		Draw mode
		/// </summary>
		[Category("LabelVertical"), Description("Whether the text will be drawn from Bottom or from Top.")]
		public DrawMode TextDrawMode
		{	get { return intDrawMode; }
			set 
				{ intDrawMode = value; 
					Invalidate();
				}
		}
	  
		[Category("LabelVertical"), Description("Whether the text will be drawn with transparent background or not.")]
		public bool TransparentBackground
		{	get { return blnTransparentBG; }
			set 
				{ blnTransparentBG = value; 
					Invalidate();
				}
		}
	}
}