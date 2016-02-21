using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Bau.Controls.Panels
{
	[ToolboxBitmapAttribute(typeof(GradientPanel),"ExDotNet.ico") ]
	public class GradientPanel : System.Windows.Forms.Panel
	{ // Variables del diseñador
			private System.ComponentModel.Container components = null;
		// Variables privadas
			private Color clrStart = Color.FromKnownColor(KnownColor.Control);
			private Color clrEnd = Color.FromKnownColor(KnownColor.Control);
			private LinearGradientMode m_GradientMode = LinearGradientMode.Vertical;
			private Bau.Controls.BorderStyle m_BorderStyle = Bau.Controls.BorderStyle.Single;
			private int intBorderWidth = 1;
			private bool blnBorder = true;
			private Color clrBorderColor = Color.FromKnownColor(KnownColor.ActiveCaption);
			private string strText = "Panel";
			private bool blnCaption = true;
			private int intCaptionHeight = 24;
			private Color clrTextBeginColor = Color.FromArgb(255, 225, 155);
			private Color clrTextEndColor = Color.FromArgb(255, 165, 78);
			private Color clrTextColor = Color.FromArgb(0 , 0 , 0);
			private bool blnAntialias = true;
			private LinearGradientMode intCaptionGradientMode = LinearGradientMode.Vertical;
			private StringAlignment intStringAlignment = StringAlignment.Near;
			private Font fntCaption;
			private Icon icnIcon = null;
			private bool blnIcon = false;

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
		}
		#endregion

		public GradientPanel()
		{	// Inicializa el componente
				InitializeComponent();
			// Inicializa la fuente del título
				fntCaption = new Font("Microsoft Sans Serif", 10, FontStyle.Bold);
			// Inicializa el estilo de dibujo del control (doble buffer)
				SetStyle(ControlStyles.UserPaint, true);
				SetStyle(ControlStyles.AllPaintingInWmPaint, true);
				SetStyle(ControlStyles.DoubleBuffer, true);
		}

		protected override void Dispose(bool disposing)
		{	if (disposing)
				if (components != null)
					components.Dispose();
			base.Dispose(disposing);
		}

		[Description("The starting color of the gradient background"),Category("_Gradient"),Browsable(true)]
		public Color GradientStart
		{ get { return clrStart; }
			set
				{ clrStart = value;
					Invalidate();
				}
		}

		[Description("The end color of the gradient background"),Category("_Gradient"),Browsable(true)]
		public Color GradientEnd
		{ get { return clrEnd; }
			set
				{ clrEnd = value;
					Invalidate();
				}
		}

		[Description("The gradient direction"),Category("_Gradient"),Browsable(true)]
		public System.Drawing.Drawing2D.LinearGradientMode GradientDirection
		{ get { return m_GradientMode; }
			set
				{ m_GradientMode = value;
					Invalidate();
				}
		}

		[Description("The style of the border"),Category("_Border"),Browsable(true)]
		public Bau.Controls.BorderStyle Style
		{ get { return m_BorderStyle; }
			set
				{ m_BorderStyle = value;
					Invalidate();
				}
		}

		[Description("The width in pixels of the border"),Category("_Border"),Browsable(true)]
		public int BorderWidth
		{ get { return intBorderWidth; }
			set
				{ intBorderWidth = value;
					Invalidate();
				}
		}

		[Description("Enable/Disable border"),Category("_Border"),Browsable(true)]
		public bool Border
		{ get { return blnBorder; }
			set
				{ blnBorder = value;
					Invalidate();
				}
		}

		[Description("The color of the border"),Category("_Border"),Browsable(true)]
		public Color BorderColor
		{ get { return clrBorderColor; }
			set
				{ clrBorderColor = value;
					Invalidate();
				}
		}

		[Description("The gradient direction"),Category("Caption"),Browsable(true)]
		public System.Drawing.StringAlignment CaptionTextAlignment
		{ get { return intStringAlignment; }
			set
				{ intStringAlignment = value;
					Invalidate();
				}
		}

		[Description("The gradient direction"),Category("Caption"),Browsable(true)]
		public System.Drawing.Drawing2D.LinearGradientMode CaptionGradientDirection
		{ get { return intCaptionGradientMode; }
			set
				{ intCaptionGradientMode = value;
					Invalidate();
				}
		}

		[Description("Enable/Disable antialiasing"),Category("Caption"),Browsable(true)]
		public bool TextAntialias
		{ get { return blnAntialias; }
			set
				{ blnAntialias = value;
					Invalidate();
				}
		}

		[Description("The caption"),Category("Caption"),Browsable(true)]
		public override string Text
		{ get { return strText; }
			set
				{ strText = value;
					Invalidate();
				}
		}

		[Description("Enable/Disable the caption"),Category("Caption"),Browsable(true)]
		public bool WithCaption
		{ get { return blnCaption; }
			set
				{ blnCaption = value;
					Invalidate();
				}
		}

		[Description("Change the caption height"),Category("Caption"),Browsable(true)]
		public int CaptionHeight
		{ get { return intCaptionHeight; }
			set
				{ intCaptionHeight = value;
					Invalidate();
				}
		}

		[Description("Change the caption begincolor"),Category("Caption"),Browsable(true)]
		public Color TextBeginColor
		{ get { return clrTextBeginColor; }
			set
				{ clrTextBeginColor = value;
					Invalidate();
				}
		}

		[Description("Change the caption endcolor"),Category("Caption"),Browsable(true)]
		public Color TextEndColor
		{ get { return clrTextEndColor; }
			set
				{ clrTextEndColor = value;
					Invalidate();
				}
		}

		[Description("Change the caption textcolor"),Category("Caption"),Browsable(true)]
		public Color TextColor
		{ get { return clrTextColor; }
			set
				{ clrTextColor = value;
					Invalidate();
				}
		}

		[Description("Change the caption textcolor"),Category("Caption"),Browsable(true)]
		public Font FontText
		{ get { return fntCaption; }
			set
				{ fntCaption = value;
					Invalidate();
				}
		}

		[Description("The icon to display in the title"),Category("_Icon"),Browsable(true)]
		public Icon PanelIcon
		{ get { return icnIcon; }
			set
				{ icnIcon = value;
					Invalidate();
				}
		}

		[Description("Enable/Disable the icon"),Category("_Icon"),Browsable(true)]
		public bool IconVisible
		{ get { return blnIcon; }
			set
				{ blnIcon = value;
					Invalidate();
				}
		}

		protected override void OnPaint(PaintEventArgs e)
		{	// The painting with shadow is slightly different...
			if (m_BorderStyle == Bau.Controls.BorderStyle.Shadow)
			{
				// fill the background
				System.Drawing.Drawing2D.LinearGradientBrush brsh = new System.Drawing.Drawing2D.LinearGradientBrush(new Rectangle(0 , 0 , this.Width - 5 , this.Height - 5) , clrStart, clrEnd, m_GradientMode);
				e.Graphics.FillRectangle(brsh, 0 , 0 , this.Width - 5 , this.Height - 5);

				// draw the border
				System.Drawing.Pen pen = new Pen(clrBorderColor);
				for(int i=0 ; i<intBorderWidth ; i++)
				{
					e.Graphics.DrawRectangle(pen , i , i , this.Width - 6 - (i*2) , this.Height - 6 - (i*2));
				}

				// draw the caption bar
				if (blnCaption)
				{
					if (blnAntialias) e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
					System.Drawing.Drawing2D.LinearGradientBrush brshCaption = new System.Drawing.Drawing2D.LinearGradientBrush(new Rectangle(intBorderWidth , intBorderWidth , this.Width - 5 - (intBorderWidth*2) , intCaptionHeight) , clrTextBeginColor , clrTextEndColor , intCaptionGradientMode);
					e.Graphics.FillRectangle(brshCaption , intBorderWidth , intBorderWidth  , this.Width - 5 - (intBorderWidth*2) , intCaptionHeight);
					StringFormat format = new StringFormat();
					format.FormatFlags = StringFormatFlags.NoWrap;
					format.LineAlignment = StringAlignment.Center;
					format.Alignment = intStringAlignment;
					format.Trimming = StringTrimming.EllipsisCharacter;
					e.Graphics.DrawString(
						strText , 
						this.FontText , 
						new SolidBrush(clrTextColor) , 
						new Rectangle(
						// LEFT
						(m_BorderStyle != Bau.Controls.BorderStyle.None) ? 
						(blnIcon ? 
						intBorderWidth + icnIcon.Width + ((intCaptionHeight / 2) - (icnIcon.Height / 2)) 
						: 
						intBorderWidth)
						: 
						(blnIcon ? 
						icnIcon.Width + ((intCaptionHeight / 2) - (icnIcon.Height / 2)) 
						: 
						0) , 
						// TOP
						(m_BorderStyle != Bau.Controls.BorderStyle.None) ?  
					intBorderWidth 
						: 
						0 ,
						// WIDTH
						(m_BorderStyle != Bau.Controls.BorderStyle.None) ?  
						(blnIcon ? 
						this.Width - (intBorderWidth*2) - ((intCaptionHeight / 2) - (icnIcon.Height / 2)) - icnIcon.Width -5
						: 
						this.Width - (intBorderWidth*2))-5
						:
						(blnIcon ?
						this.Width - ((intCaptionHeight / 2) - (icnIcon.Height / 2)) - icnIcon.Width-5
						:
						this.Width)-5 , 
						// HEIGHT
						intCaptionHeight) 
						, format);
				}

				// draw the shadow
				Pen pen1 = new Pen(Color.FromArgb(142 , 142 , 142) , 1.0f);
				Pen pen2 = new Pen(Color.FromArgb(171 , 171 , 171) , 1.0f);
				Pen pen3 = new Pen(Color.FromArgb(212 , 212 , 212) , 1.0f);
				Pen pen4 = new Pen(Color.FromArgb(241 , 241 , 241) , 1.0f);

				e.Graphics.DrawLine(pen1 , this.Width - 5 , 2 , this.Width - 5 , this.Height - 5);
				e.Graphics.DrawLine(pen2 , this.Width - 4 , 4 , this.Width - 4 , this.Height - 4);
				e.Graphics.DrawLine(pen3 , this.Width - 3 , 6 , this.Width - 3 , this.Height - 3);
				e.Graphics.DrawLine(pen4 , this.Width - 2 , 8 , this.Width - 2 , this.Height - 2);

				e.Graphics.DrawLine(pen1 , 2 , this.Height - 5 , this.Width - 5 , this.Height - 5);
				e.Graphics.DrawLine(pen2 , 4 , this.Height - 4 , this.Width - 4 , this.Height - 4);
				e.Graphics.DrawLine(pen3 , 6 , this.Height - 3 , this.Width - 3 , this.Height - 3);
				e.Graphics.DrawLine(pen4 , 8 , this.Height - 2 , this.Width - 2 , this.Height - 2);
			}
			else
			{
				// fill the background
				System.Drawing.Drawing2D.LinearGradientBrush brsh = new System.Drawing.Drawing2D.LinearGradientBrush(new Rectangle(0 , 0 , this.Width , this.Height) , clrStart , clrEnd , m_GradientMode);
				e.Graphics.FillRectangle(brsh , 0 , 0 , this.Width , this.Height);

				// draw the border
				switch(m_BorderStyle)
				{
					case Bau.Controls.BorderStyle.Single:
					{
						System.Drawing.Pen pen = new Pen(clrBorderColor);
						for(int i=0 ; i<intBorderWidth ; i++)
						{
							e.Graphics.DrawRectangle(pen , i , i , this.Width - 1 - (i*2) , this.Height - 1 - (i*2));
						}
						break;
					}
					case Bau.Controls.BorderStyle.Raised3D:
					{
						break;
					}
				}

				// draw caption bar
				if (blnCaption)
				{
					if (blnAntialias) e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
					System.Drawing.Drawing2D.LinearGradientBrush brshCaption = new System.Drawing.Drawing2D.LinearGradientBrush(new Rectangle((m_BorderStyle != Bau.Controls.BorderStyle.None) ?  intBorderWidth : 0 , (m_BorderStyle != Bau.Controls.BorderStyle.None) ?  intBorderWidth : 0 , (m_BorderStyle != Bau.Controls.BorderStyle.None) ?  this.Width - (intBorderWidth*2) : this.Width , intCaptionHeight) , clrTextBeginColor , clrTextEndColor , intCaptionGradientMode);
					e.Graphics.FillRectangle(brshCaption , (m_BorderStyle != Bau.Controls.BorderStyle.None) ?  intBorderWidth : 0 , (m_BorderStyle != Bau.Controls.BorderStyle.None) ?  intBorderWidth : 0 , (m_BorderStyle != Bau.Controls.BorderStyle.None) ?  this.Width - (intBorderWidth*2) : this.Width , intCaptionHeight);
					StringFormat format = new StringFormat();
					format.FormatFlags = StringFormatFlags.NoWrap;
					format.LineAlignment = StringAlignment.Center;
					format.Alignment = intStringAlignment;
					format.Trimming = StringTrimming.EllipsisCharacter;
					e.Graphics.DrawString(
						strText , 
						this.FontText , 
						new SolidBrush(clrTextColor) , 
						new Rectangle(
							// LEFT
							(m_BorderStyle != Bau.Controls.BorderStyle.None) ? 
							(blnIcon ? 
								intBorderWidth + icnIcon.Width + ((intCaptionHeight / 2) - (icnIcon.Height / 2)) 
								: 
								intBorderWidth)
							: 
							(blnIcon ? 
								icnIcon.Width + ((intCaptionHeight / 2) - (icnIcon.Height / 2)) 
								: 
								0) , 
							// TOP
							(m_BorderStyle != Bau.Controls.BorderStyle.None) ?  
								intBorderWidth 
								: 
								0 ,
							// WIDTH
							(m_BorderStyle != Bau.Controls.BorderStyle.None) ?  
								(blnIcon ? 
									this.Width - (intBorderWidth*2) - ((intCaptionHeight / 2) - (icnIcon.Height / 2)) - icnIcon.Width 
									: 
									this.Width - (intBorderWidth*2))
								:
								(blnIcon ?
									this.Width - ((intCaptionHeight / 2) - (icnIcon.Height / 2)) - icnIcon.Width
									:
									this.Width) , 
							// HEIGHT
							intCaptionHeight) 
							, format);
				}
			}

			// draw the icon
			if (blnIcon && blnCaption)
				e.Graphics.DrawIcon(icnIcon , ((m_BorderStyle != Bau.Controls.BorderStyle.None) ? intBorderWidth : 0) + ((intCaptionHeight / 2) - (icnIcon.Height / 2)) , ((m_BorderStyle != Bau.Controls.BorderStyle.None) ? intBorderWidth : 0) + ((intCaptionHeight / 2) - (icnIcon.Height / 2)));
			// Llama al componente base
				base.OnPaint(e);
		}

		protected override void OnResize(EventArgs e)
		{ Invalidate();
			base.OnResize (e);
		}
	}
}