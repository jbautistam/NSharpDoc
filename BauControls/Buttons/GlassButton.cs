using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using PushButtonState = System.Windows.Forms.VisualStyles.PushButtonState;

namespace Bau.Controls.Buttons
{
  /// <summary>
  /// Represents a glass button control.
  /// </summary>
  public class GlassButton : Button
  { // Controles privados
			private Timer timer = new Timer();
			
		/// <summary>
		/// Initializes a new instance of the GlassButton class.
		/// </summary>
		public GlassButton()
		{ timer.Tick += new EventHandler(timer_Tick);
			timer.Interval = animationLength / framesCount;
			base.BackColor = Color.Transparent;
			BackColor = Color.Black;
			ForeColor = Color.White;
			OuterBorderColor = Color.White;
			InnerBorderColor = Color.Black;
			ShineColor = Color.White;
			GlowColor = Color.FromArgb(unchecked((int)(0xFF8DBDFF)));
			SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.SupportsTransparentBackColor | ControlStyles.UserPaint, true);
			SetStyle(ControlStyles.Opaque, false);
		}

		private Color backColor;
		/// <summary>
		/// Gets or sets the background color of the control.
		/// </summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> value representing the background color.</returns>
		[DefaultValue(typeof(Color), "Black")]
		public new Color BackColor
		{
			get { return backColor; }
			set
			{
					if (!backColor.Equals(value))
					{
							backColor = value;
							UseVisualStyleBackColor = false;
							OnBackColorChanged(EventArgs.Empty);
					}
			}
		}

		/// <summary>
		/// Gets or sets the foreground color of the control.
		/// </summary>
		/// <returns>The foreground <see cref="T:System.Drawing.Color" /> of the control.</returns>
		[DefaultValue(typeof(Color), "White")]
		public new Color ForeColor
		{
			get { return base.ForeColor; }
			set { base.ForeColor = value; }
		}

		private Color innerBorderColor;
		/// <summary>
		/// Gets or sets the inner border color of the control.
		/// </summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> value representing the color of the inner border.</returns>
		[DefaultValue(typeof(Color), "Black"), Category("Appearance"), Description("The inner border color of the control.")]
		public Color InnerBorderColor
		{
			get { return innerBorderColor; }
			set
			{
					if (innerBorderColor != value)
					{
							innerBorderColor = value;
							if (IsHandleCreated)
							{
									Invalidate();
							}
					}
			}
		}

		private Color outerBorderColor;
		/// <summary>
		/// Gets or sets the outer border color of the control.
		/// </summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> value representing the color of the outer border.</returns>
		[DefaultValue(typeof(Color), "White"), Category("Appearance"), Description("The outer border color of the control.")]
		public Color OuterBorderColor
		{
			get { return outerBorderColor; }
			set
			{
					if (outerBorderColor != value)
					{
							outerBorderColor = value;
							if (IsHandleCreated)
							{
									Invalidate();
							}
					}
			}
		}

		private Color shineColor;
		/// <summary>
		/// Gets or sets the shine color of the control.
		/// </summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> value representing the shine color.</returns>
		[DefaultValue(typeof(Color), "White"), Category("Appearance"), Description("The shine color of the control.")]
		public Color ShineColor
		{
			get { return shineColor; }
			set
			{
					if (shineColor != value)
					{
							shineColor = value;
							if (IsHandleCreated)
							{
									Invalidate();
							}
					}
			}
		}

		private Color glowColor;
		/// <summary>
		/// Gets or sets the glow color of the control.
		/// </summary>
		/// <returns>A <see cref="T:System.Drawing.Color" /> value representing the glow color.</returns>
		[DefaultValue(typeof(Color), "255,141,189,255"), Category("Appearance"), Description("The glow color of the control.")]
		public Color GlowColor
		{
			get { return glowColor; }
			set
			{
					if (glowColor != value)
					{
							glowColor = value;
							if (IsHandleCreated)
							{
									Invalidate();
							}
					}
			}
		}

		private bool isHovered;
		private bool isFocused;
		private bool isFocusedByKey;
		private bool isKeyDown;
		private bool isMouseDown;
		private bool isPressed { get { return isKeyDown || (isMouseDown && isHovered); } }

		/// <summary>
		/// Gets the state of the button control.
		/// </summary>
		/// <value>The state of the button control.</value>
		[Browsable(false)]
		public PushButtonState State
		{
			get
			{
					if (!Enabled)
					{
							return PushButtonState.Disabled;
					}
					if (isPressed)
					{
							return PushButtonState.Pressed;
					}
					if (isHovered)
					{
							return PushButtonState.Hot;
					}
					if (isFocused || IsDefault)
					{
							return PushButtonState.Default;
					}
					return PushButtonState.Normal;
			}
		}

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.Click" /> event.
		/// </summary>
		/// <param name="e">The <see cref="System.EventArgs" /> instance containing the event data.</param>
		protected override void OnClick(EventArgs e)
		{
			isKeyDown = isMouseDown = false;
			base.OnClick(e);
		}

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.Enter" /> event.
		/// </summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
		protected override void OnEnter(EventArgs e)
		{
			isFocused = isFocusedByKey = true;
			base.OnEnter(e);
		}

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.Leave" /> event.
		/// </summary>
		/// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
		protected override void OnLeave(EventArgs e)
		{
			base.OnLeave(e);
			isFocused = isFocusedByKey = isKeyDown = isMouseDown = false;
			Invalidate();
		}

		/// <summary>
		/// Raises the <see cref="M:System.Windows.Forms.ButtonBase.OnKeyUp(System.Windows.Forms.KeyEventArgs)" /> event.
		/// </summary>
		/// <param name="kevent">A <see cref="T:System.Windows.Forms.KeyEventArgs" /> that contains the event data.</param>
		protected override void OnKeyDown(KeyEventArgs kevent)
		{
			if (kevent.KeyCode == Keys.Space)
			{
					isKeyDown = true;
					Invalidate();
			}
			base.OnKeyDown(kevent);
		}

		/// <summary>
		/// Raises the <see cref="M:System.Windows.Forms.ButtonBase.OnKeyUp(System.Windows.Forms.KeyEventArgs)" /> event.
		/// </summary>
		/// <param name="kevent">A <see cref="T:System.Windows.Forms.KeyEventArgs" /> that contains the event data.</param>
		protected override void OnKeyUp(KeyEventArgs kevent)
		{
			if (isKeyDown && kevent.KeyCode == Keys.Space)
			{
					isKeyDown = false;
					Invalidate();
			}
			base.OnKeyUp(kevent);
		}

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.MouseDown" /> event.
		/// </summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.MouseEventArgs" /> that contains the event data.</param>
		protected override void OnMouseDown(MouseEventArgs e)
		{
			//HideToolTip();
			if (!isMouseDown && e.Button == MouseButtons.Left)
			{
					isMouseDown = true;
					isFocusedByKey = false;
					Invalidate();
			}
			base.OnMouseDown(e);
		}

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.MouseUp" /> event.
		/// </summary>
		/// <param name="e">A <see cref="T:System.Windows.Forms.MouseEventArgs" /> that contains the event data.</param>
		protected override void OnMouseUp(MouseEventArgs e)
		{
			if (isMouseDown)
			{
					isMouseDown = false;
					Invalidate();
			}
			base.OnMouseUp(e);
		}

		/// <summary>
		/// Raises the <see cref="M:System.Windows.Forms.Control.OnMouseMove(System.Windows.Forms.MouseEventArgs)" /> event.
		/// </summary>
		/// <param name="mevent">A <see cref="T:System.Windows.Forms.MouseEventArgs" /> that contains the event data.</param>
		protected override void OnMouseMove(MouseEventArgs mevent)
		{
			base.OnMouseMove(mevent);
			if (mevent.Button != MouseButtons.None)
			{
					if (!ClientRectangle.Contains(mevent.X, mevent.Y))
					{
							if (isHovered)
							{
									isHovered = false;
									Invalidate();
							}
					}
					else if (!isHovered)
					{
							isHovered = true;
							Invalidate();
					}
			}
		}

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.MouseEnter" /> event.
		/// </summary>
		/// <param name="e">The <see cref="System.EventArgs" /> instance containing the event data.</param>
		protected override void OnMouseEnter(EventArgs e)
		{
			isHovered = true;
			FadeIn();
			Invalidate();
			/*if (!DesignMode && toolTip != null)
			{
					timerPop.Interval = toolTip.InitialDelay;
					timerPop.Enabled = true;
			}*/
			base.OnMouseEnter(e);
		}

		/// <summary>
		/// Raises the <see cref="E:System.Windows.Forms.Control.MouseLeave" /> event.
		/// </summary>
		/// <param name="e">The <see cref="System.EventArgs" /> instance containing the event data.</param>
		protected override void OnMouseLeave(EventArgs e)
		{
			isHovered = false;
			//HideToolTip();
			FadeOut();
			Invalidate();
			base.OnMouseLeave(e);
		}

		/// <summary>
		/// Raises the <see cref="M:System.Windows.Forms.ButtonBase.OnPaint(System.Windows.Forms.PaintEventArgs)" /> event.
		/// </summary>
		/// <param name="pevent">A <see cref="T:System.Windows.Forms.PaintEventArgs" /> that contains the event data.</param>
		protected override void OnPaint(PaintEventArgs pevent)
		{
			SmoothingMode sm = pevent.Graphics.SmoothingMode;
			pevent.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
			DrawButtonBackground(pevent.Graphics, (float)currentFrame / (framesCount - 1f));
			DrawButtonForeground(pevent.Graphics);
			pevent.Graphics.SmoothingMode = sm;
		}

		private void DrawButtonBackground(Graphics g, float glowOpacity)
		{
			Rectangle rect = ClientRectangle;
			rect.Width--;
			rect.Height--;
			using (GraphicsPath bw = CreateRoundRectangle(rect, 4))
			{
					using (Pen p = new Pen(outerBorderColor))
					{
							g.DrawPath(p, bw);
					}
			}
			rect.X++;
			rect.Y++;
			rect.Width -= 2;
			rect.Height -= 2;
			Rectangle rect2 = rect;
			rect2.Height >>= 1;

			using (GraphicsPath bb = CreateRoundRectangle(rect, 4))
			{
					int opacity = isPressed ? 0xcc : 0x7f;
					using (Brush br = new SolidBrush(Color.FromArgb(opacity, backColor)))
					{
							g.FillPath(br, bb);
					}
			}

			if ((isHovered || isAnimating) && !isPressed)
			{
					using (GraphicsPath clip = CreateRoundRectangle(rect, 4))
					{
							g.SetClip(clip, CombineMode.Intersect);
							using (GraphicsPath brad = CreateBottomRadialPath(rect))
							{
									using (PathGradientBrush pgr = new PathGradientBrush(brad))
									{
											unchecked
											{
													int opacity = (int)(0xB2 * glowOpacity + .5f);
													pgr.CenterColor = Color.FromArgb(opacity, glowColor);
													pgr.SurroundColors = new Color[] { Color.FromArgb(0, glowColor) };
											}
											g.FillPath(pgr, brad);
									}
							}
							g.ResetClip();
					}
			}

			if (rect2.Width > 0 && rect2.Height > 0)
			{
					using (GraphicsPath bh = CreateTopRoundRectangle(rect2, 4))
					{
							int opacity = 0x99;
							if (isPressed) opacity = (int)(.4f * opacity + .5f);
							using (Brush br = new LinearGradientBrush(rect2, Color.FromArgb(opacity, shineColor), Color.FromArgb(opacity / 3, shineColor), LinearGradientMode.Vertical))
							{
									g.FillPath(br, bh);
							}
					}
			}

			using (GraphicsPath bb = CreateRoundRectangle(rect, 4))
			{
					using (Pen p = new Pen(innerBorderColor))
					{
							g.DrawPath(p, bb);
					}
			}
		}

		private void DrawButtonForeground(Graphics g)
		{
			Rectangle rect = ClientRectangle;
			rect.Inflate(-6, -4);
			if (Padding.Top > 0)
			{
					rect.Y += Padding.Top;
					rect.Height -= Padding.Top;
			}
			if (Padding.Bottom > 0)
			{
					rect.Height -= Padding.Bottom;
			}
			if (Padding.Left > 0)
			{
					rect.X += Padding.Left;
					rect.Width -= Padding.Left;
			}
			if (Padding.Right > 0)
			{
					rect.Width -= Padding.Right;
			}
			TextFormatFlags textFormatFlags = TextFormatFlags.ExpandTabs | TextFormatFlags.NoPadding | TextFormatFlags.GlyphOverhangPadding | TextFormatFlags.WordBreak | TextFormatFlags.TextBoxControl;
			//StringFormat format = new StringFormat();
			switch (RtlTranslateContent(TextAlign))
			{
					case ContentAlignment.BottomCenter:
							textFormatFlags |= TextFormatFlags.Bottom | TextFormatFlags.HorizontalCenter;
							//format.Alignment = StringAlignment.Center;
							//format.LineAlignment = StringAlignment.Far;
							break;
					case ContentAlignment.BottomLeft:
							textFormatFlags |= TextFormatFlags.Bottom | TextFormatFlags.Left;
							//format.Alignment = StringAlignment.Near;
							//format.LineAlignment = StringAlignment.Far;
							break;
					case ContentAlignment.BottomRight:
							textFormatFlags |= TextFormatFlags.Bottom | TextFormatFlags.Right;
							//format.Alignment = StringAlignment.Far;
							//format.LineAlignment = StringAlignment.Far;
							break;
					case ContentAlignment.MiddleCenter:
							textFormatFlags |= TextFormatFlags.VerticalCenter | TextFormatFlags.HorizontalCenter;
							//format.Alignment = StringAlignment.Center;
							//format.LineAlignment = StringAlignment.Center;
							break;
					case ContentAlignment.MiddleLeft:
							textFormatFlags |= TextFormatFlags.VerticalCenter | TextFormatFlags.Left;
							//format.Alignment = StringAlignment.Near;
							//format.LineAlignment = StringAlignment.Center;
							break;
					case ContentAlignment.MiddleRight:
							textFormatFlags |= TextFormatFlags.VerticalCenter | TextFormatFlags.Right;
							//format.Alignment = StringAlignment.Far;
							//format.LineAlignment = StringAlignment.Center;
							break;
					case ContentAlignment.TopCenter:
							textFormatFlags |= TextFormatFlags.Top | TextFormatFlags.HorizontalCenter;
							//format.Alignment = StringAlignment.Center;
							//format.LineAlignment = StringAlignment.Near;
							break;
					case ContentAlignment.TopLeft:
							textFormatFlags |= TextFormatFlags.Top | TextFormatFlags.Left;
							//format.Alignment = StringAlignment.Near;
							//format.LineAlignment = StringAlignment.Near;
							break;
					case ContentAlignment.TopRight:
							textFormatFlags |= TextFormatFlags.Top | TextFormatFlags.Right;
							//format.Alignment = StringAlignment.Far;
							//format.LineAlignment = StringAlignment.Near;
							break;
			}
			if (AutoEllipsis)
			{
					//format.Trimming = StringTrimming.EllipsisCharacter;
					textFormatFlags |= TextFormatFlags.EndEllipsis;
			}
			//format.HotkeyPrefix = UseMnemonic ? HotkeyPrefix.Show : HotkeyPrefix.None;
			if (!UseMnemonic)
			{
					textFormatFlags |= TextFormatFlags.NoPrefix;
			}
			if (!ShowKeyboardCues)
			{
					textFormatFlags |= TextFormatFlags.HidePrefix;
			}
			if (RightToLeft == RightToLeft.Yes)
			{
					textFormatFlags |= TextFormatFlags.RightToLeft;
			}
			//using (Brush textBrush = new SolidBrush(textColor))
			{
					//VControlPaint.DrawStringGlow(graphics, Text, Font, textBrush, Color.White, rect, format);
					TextRenderer.DrawText(g, Text, Font, rect, ForeColor, textFormatFlags);
			}
			if (Focused && ShowFocusCues)
			{
					rect = ClientRectangle;
					rect.Inflate(-4, -4);
					ControlPaint.DrawFocusRectangle(g, rect);
			}
		}

		private GraphicsPath CreateRoundRectangle(Rectangle rectangle, int radius)
		{
			GraphicsPath path = new GraphicsPath();
			int l = rectangle.Left;
			int t = rectangle.Top;
			int w = rectangle.Width;
			int h = rectangle.Height;
			int d = radius << 1;
			path.AddArc(l, t, d, d, 180, 90); // topleft
			path.AddLine(l + radius, t, l + w - radius, t); // top
			path.AddArc(l + w - d, t, d, d, 270, 90); // topright
			path.AddLine(l + w, t + radius, l + w, t + h - radius); // right
			path.AddArc(l + w - d, t + h - d, d, d, 0, 90); // bottomright
			path.AddLine(l + w - radius, t + h, l + radius, t + h); // bottom
			path.AddArc(l, t + h - d, d, d, 90, 90); // bottomleft
			path.AddLine(l, t + h - radius, l, t + radius); // left
			path.CloseFigure();
			return path;
		}

		private GraphicsPath CreateTopRoundRectangle(Rectangle rectangle, int radius)
		{
			GraphicsPath path = new GraphicsPath();
			int l = rectangle.Left;
			int t = rectangle.Top;
			int w = rectangle.Width;
			int h = rectangle.Height;
			int d = radius << 1;
			path.AddArc(l, t, d, d, 180, 90); // topleft
			path.AddLine(l + radius, t, l + w - radius, t); // top
			path.AddArc(l + w - d, t, d, d, 270, 90); // topright
			path.AddLine(l + w, t + radius, l + w, t + h); // right
			path.AddLine(l + w, t + h, l, t + h); // bottom
			path.AddLine(l, t + h, l, t + radius); // left
			path.CloseFigure();
			return path;
		}

		private GraphicsPath CreateBottomRadialPath(Rectangle rectangle)
		{
			GraphicsPath path = new GraphicsPath();
			/*<ScaleTransform ScaleX="1.702" ScaleY="2.243"/>
			<SkewTransform AngleX="0" AngleY="0"/>
			<RotateTransform Angle="0"/>
			<TranslateTransform X="-0.368" Y="-0.152"/>*/
			RectangleF rect = rectangle;
			rect.X -= rectangle.Width * .35f;
			rect.Y -= rectangle.Height * .15f;
			rect.Width *= 1.7f;
			rect.Height *= 2.3f;
			path.AddEllipse(rect);
			path.CloseFigure();
			return path;
		}

		/// <summary>This property is not relevant for this class.</summary>
		/// <returns>This property is not relevant for this class.</returns>
		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), EditorBrowsable(EditorBrowsableState.Never)]
		public override Image BackgroundImage
		{
			get { return base.BackgroundImage; }
			set { base.BackgroundImage = value; }
		}

		/// <summary>This property is not relevant for this class.</summary>
		/// <returns>This property is not relevant for this class.</returns>
		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), EditorBrowsable(EditorBrowsableState.Never)]
		public override ImageLayout BackgroundImageLayout
		{
			get { return base.BackgroundImageLayout; }
			set { base.BackgroundImageLayout = value; }
		}

		/// <summary>This property is not relevant for this class.</summary>
		/// <returns>This property is not relevant for this class.</returns>
		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), EditorBrowsable(EditorBrowsableState.Never)]
		public new FlatButtonAppearance FlatAppearance
		{
			get { return base.FlatAppearance; }
		}

		/// <summary>This property is not relevant for this class.</summary>
		/// <returns>This property is not relevant for this class.</returns>
		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), EditorBrowsable(EditorBrowsableState.Never)]
		public new FlatStyle FlatStyle
		{
			get { return base.FlatStyle; }
			set { base.FlatStyle = value; }
		}

		/// <summary>This property is not relevant for this class.</summary>
		/// <returns>This property is not relevant for this class.</returns>
		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), EditorBrowsable(EditorBrowsableState.Never)]
		public new Image Image
		{
			get { return base.Image; }
			set { base.Image = value; }
		}

		/// <summary>This property is not relevant for this class.</summary>
		/// <returns>This property is not relevant for this class.</returns>
		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), EditorBrowsable(EditorBrowsableState.Never)]
		public new ContentAlignment ImageAlign
		{
			get { return base.ImageAlign; }
			set { base.ImageAlign = value; }
		}

		/// <summary>This property is not relevant for this class.</summary>
		/// <returns>This property is not relevant for this class.</returns>
		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), EditorBrowsable(EditorBrowsableState.Never)]
		public new int ImageIndex
		{
			get { return base.ImageIndex; }
			set { base.ImageIndex = value; }
		}

		/// <summary>This property is not relevant for this class.</summary>
		/// <returns>This property is not relevant for this class.</returns>
		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), EditorBrowsable(EditorBrowsableState.Never)]
		public new string ImageKey
		{
			get { return base.ImageKey; }
			set { base.ImageKey = value; }
		}

		/// <summary>This property is not relevant for this class.</summary>
		/// <returns>This property is not relevant for this class.</returns>
		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), EditorBrowsable(EditorBrowsableState.Never)]
		public new ImageList ImageList
		{
			get { return base.ImageList; }
			set { base.ImageList = value; }
		}

		/// <summary>This property is not relevant for this class.</summary>
		/// <returns>This property is not relevant for this class.</returns>
		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), EditorBrowsable(EditorBrowsableState.Never)]
		public new TextImageRelation TextImageRelation
		{
			get { return base.TextImageRelation; }
			set { base.TextImageRelation = value; }
		}

		/// <summary>This property is not relevant for this class.</summary>
		/// <returns>This property is not relevant for this class.</returns>
		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), EditorBrowsable(EditorBrowsableState.Never)]
		public new bool UseCompatibleTextRendering
		{
			get { return base.UseCompatibleTextRendering; }
			set { base.UseCompatibleTextRendering = value; }
		}

		/// <summary>This property is not relevant for this class.</summary>
		/// <returns>This property is not relevant for this class.</returns>
		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), EditorBrowsable(EditorBrowsableState.Never)]
		public new bool UseVisualStyleBackColor
		{
			get { return base.UseVisualStyleBackColor; }
			set { base.UseVisualStyleBackColor = value; }
		}

		private const int animationLength = 300;
		private const int framesCount = 10;
		private int currentFrame;
		private int direction;

		private bool isAnimating
		{
			get
			{
					return direction != 0;
			}
		}

		private void FadeIn()
		{
			direction = 1;
			timer.Enabled = true;
		}

		private void FadeOut()
		{
			direction = -1;
			timer.Enabled = true;
		}

		private void timer_Tick(object sender, EventArgs e)
		{
			if (!timer.Enabled)
			{
					return;
			}
			Refresh();
			currentFrame += direction;
			if (currentFrame == -1)
			{
					currentFrame = 0;
					timer.Enabled = false;
					direction = 0;
					return;
			}
			if (currentFrame == framesCount)
			{
					currentFrame = framesCount - 1;
					timer.Enabled = false;
					direction = 0;
			}
		}
  }
}
