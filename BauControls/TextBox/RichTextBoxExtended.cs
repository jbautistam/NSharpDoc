using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Bau.Controls.TextBox
{
	/// <summary>
	///		Control para manejo de un RichTextBox
	/// </summary>
	public class RichTextBoxExtended : System.Windows.Forms.UserControl
	{	// Controles
			private ToolStrip tlbFormat;
			private ToolStripButton tlbBold;
			private ToolStripButton tlbItalic;
			private ToolStripButton tlbUnderline;
			private Panel panel1;
			private ExRichTextBox rtfTextBox;
			private ToolStripButton tlbColor;
			private ColorDialog dlgColor;
			private ContextMenuStrip ctxMenuEdit;
			private ToolStripMenuItem mnuEditCopy;
			private ToolStripMenuItem mnuEditCut;
			private ToolStripMenuItem mnuEditPaste;
			private ToolStripSeparator toolStripMenuItem1;
			private ToolStripMenuItem mnuEditRemove;
		private TableLayoutPanel tableLayoutPanel1;
			private IContainer components;
		// Delegados
			public delegate void SelectionChangedHandler(object sender, System.EventArgs e);
		// Eventos
			public event SelectionChangedHandler SelectionChanged;

		#region Código generado por el Diseñador de componentes
		/// <summary> 
		/// Método necesario para admitir el Diseñador. No se puede modificar 
		/// el contenido del método con el editor de código.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RichTextBoxExtended));
			this.tlbFormat = new System.Windows.Forms.ToolStrip();
			this.tlbBold = new System.Windows.Forms.ToolStripButton();
			this.tlbItalic = new System.Windows.Forms.ToolStripButton();
			this.tlbUnderline = new System.Windows.Forms.ToolStripButton();
			this.tlbColor = new System.Windows.Forms.ToolStripButton();
			this.panel1 = new System.Windows.Forms.Panel();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.rtfTextBox = new Bau.Controls.TextBox.ExRichTextBox();
			this.ctxMenuEdit = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.mnuEditCopy = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuEditCut = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuEditPaste = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
			this.mnuEditRemove = new System.Windows.Forms.ToolStripMenuItem();
			this.dlgColor = new System.Windows.Forms.ColorDialog();
			this.tlbFormat.SuspendLayout();
			this.panel1.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			this.ctxMenuEdit.SuspendLayout();
			this.SuspendLayout();
			// 
			// tlbFormat
			// 
			this.tlbFormat.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.tlbFormat.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tlbBold,
            this.tlbItalic,
            this.tlbUnderline,
            this.tlbColor});
			this.tlbFormat.Location = new System.Drawing.Point(0, 0);
			this.tlbFormat.Name = "tlbFormat";
			this.tlbFormat.Size = new System.Drawing.Size(345, 24);
			this.tlbFormat.TabIndex = 1;
			this.tlbFormat.Text = "toolStrip1";
			// 
			// tlbBold
			// 
			this.tlbBold.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.tlbBold.Image = ((System.Drawing.Image)(resources.GetObject("tlbBold.Image")));
			this.tlbBold.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tlbBold.Name = "tlbBold";
			this.tlbBold.Size = new System.Drawing.Size(23, 21);
			this.tlbBold.Text = "toolStripButton3";
			this.tlbBold.ToolTipText = "Negrita";
			this.tlbBold.Click += new System.EventHandler(this.tlbBold_Click);
			// 
			// tlbItalic
			// 
			this.tlbItalic.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.tlbItalic.Image = ((System.Drawing.Image)(resources.GetObject("tlbItalic.Image")));
			this.tlbItalic.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tlbItalic.Name = "tlbItalic";
			this.tlbItalic.Size = new System.Drawing.Size(23, 21);
			this.tlbItalic.Text = "toolStripButton4";
			this.tlbItalic.ToolTipText = "Cursiva";
			this.tlbItalic.Click += new System.EventHandler(this.tlbItalic_Click);
			// 
			// tlbUnderline
			// 
			this.tlbUnderline.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.tlbUnderline.Image = ((System.Drawing.Image)(resources.GetObject("tlbUnderline.Image")));
			this.tlbUnderline.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tlbUnderline.Name = "tlbUnderline";
			this.tlbUnderline.Size = new System.Drawing.Size(23, 21);
			this.tlbUnderline.Text = "toolStripButton5";
			this.tlbUnderline.ToolTipText = "Subrayado";
			this.tlbUnderline.Click += new System.EventHandler(this.tlbUnderline_Click);
			// 
			// tlbColor
			// 
			this.tlbColor.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.tlbColor.Image = ((System.Drawing.Image)(resources.GetObject("tlbColor.Image")));
			this.tlbColor.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.tlbColor.Name = "tlbColor";
			this.tlbColor.Size = new System.Drawing.Size(23, 21);
			this.tlbColor.Text = "toolStripButton1";
			this.tlbColor.ToolTipText = "Color del texto";
			this.tlbColor.Click += new System.EventHandler(this.tlbColor_Click);
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.tableLayoutPanel1);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel1.Location = new System.Drawing.Point(0, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(345, 339);
			this.panel1.TabIndex = 3;
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 1;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Controls.Add(this.tlbFormat, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.rtfTextBox, 0, 1);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 2;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 24F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(345, 339);
			this.tableLayoutPanel1.TabIndex = 4;
			// 
			// rtfTextBox
			// 
			this.rtfTextBox.AcceptsTab = true;
			this.rtfTextBox.ContextMenuStrip = this.ctxMenuEdit;
			this.rtfTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.rtfTextBox.HighlightColor = Bau.Controls.TextBox.RtfColor.White;
			this.rtfTextBox.Location = new System.Drawing.Point(3, 27);
			this.rtfTextBox.Name = "rtfTextBox";
			this.rtfTextBox.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
			this.rtfTextBox.ShowSelectionMargin = true;
			this.rtfTextBox.Size = new System.Drawing.Size(339, 309);
			this.rtfTextBox.TabIndex = 3;
			this.rtfTextBox.Text = "";
			this.rtfTextBox.TextColor = Bau.Controls.TextBox.RtfColor.Black;
			// 
			// ctxMenuEdit
			// 
			this.ctxMenuEdit.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuEditCopy,
            this.mnuEditCut,
            this.mnuEditPaste,
            this.toolStripMenuItem1,
            this.mnuEditRemove});
			this.ctxMenuEdit.Name = "ctxMenuEdit";
			this.ctxMenuEdit.Size = new System.Drawing.Size(117, 98);
			this.ctxMenuEdit.Opening += new System.ComponentModel.CancelEventHandler(this.ctxMenuEdit_Opening);
			// 
			// mnuEditCopy
			// 
			this.mnuEditCopy.Image = global::Bau.Controls.Properties.Resources.Copy;
			this.mnuEditCopy.Name = "mnuEditCopy";
			this.mnuEditCopy.Size = new System.Drawing.Size(116, 22);
			this.mnuEditCopy.Text = "&Copiar";
			this.mnuEditCopy.Click += new System.EventHandler(this.mnuEditCopy_Click);
			// 
			// mnuEditCut
			// 
			this.mnuEditCut.Image = global::Bau.Controls.Properties.Resources.Cut;
			this.mnuEditCut.Name = "mnuEditCut";
			this.mnuEditCut.Size = new System.Drawing.Size(116, 22);
			this.mnuEditCut.Text = "C&ortar";
			this.mnuEditCut.Click += new System.EventHandler(this.mnuEditCut_Click);
			// 
			// mnuEditPaste
			// 
			this.mnuEditPaste.Image = global::Bau.Controls.Properties.Resources.Paste;
			this.mnuEditPaste.Name = "mnuEditPaste";
			this.mnuEditPaste.Size = new System.Drawing.Size(116, 22);
			this.mnuEditPaste.Text = "&Pegar";
			this.mnuEditPaste.Click += new System.EventHandler(this.mnuEditPaste_Click);
			// 
			// toolStripMenuItem1
			// 
			this.toolStripMenuItem1.Name = "toolStripMenuItem1";
			this.toolStripMenuItem1.Size = new System.Drawing.Size(113, 6);
			// 
			// mnuEditRemove
			// 
			this.mnuEditRemove.Image = global::Bau.Controls.Properties.Resources.Remove;
			this.mnuEditRemove.Name = "mnuEditRemove";
			this.mnuEditRemove.Size = new System.Drawing.Size(116, 22);
			this.mnuEditRemove.Text = "&Eliminar";
			this.mnuEditRemove.Click += new System.EventHandler(this.mnuEditRemove_Click);
			// 
			// RichTextBoxExtended
			// 
			this.Controls.Add(this.panel1);
			this.Name = "RichTextBoxExtended";
			this.Size = new System.Drawing.Size(345, 339);
			this.tlbFormat.ResumeLayout(false);
			this.tlbFormat.PerformLayout();
			this.panel1.ResumeLayout(false);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.ctxMenuEdit.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		public RichTextBoxExtended()
		{	// Llamada necesaria para el Diseñador de formularios Windows.Forms.
				InitializeComponent();
		}
		
		/// <summary> 
		///		Limpia los recursos que se estén utilizando
		/// </summary>
		protected override void Dispose(bool blnDisposing)
		{	if (blnDisposing)
				if (components != null)
					components.Dispose();
			base.Dispose(blnDisposing);
		}

		/// <summary>
		///		Añade el texto
		/// </summary>
		public void AddText(string strText)
		{ rtfTextBox.InsertTextAsRtf(strText);
		}
		
		public void SetFont(bool blnBold, bool blnItalic, bool blnUnderline)
		{ if (rtfTextBox.SelectionFont != null)
				SetFont(rtfTextBox.SelectionFont.Name, rtfTextBox.SelectionFont.Size,
								blnBold, blnItalic, blnUnderline);
		}
		
		public void SetFont(string strFontName, float fltSize, bool blnBold, bool blnItalic, bool blnUnderline)
		{ FontStyle intStyle = FontStyle.Regular;
		
				// Asigna el estilo
					if (blnBold)
						intStyle |= FontStyle.Bold;
					if (blnItalic)
						intStyle |= FontStyle.Italic;
					if (blnUnderline)
						intStyle |= FontStyle.Underline;
				// Cambia la fuente del texto seleccionado
					rtfTextBox.SelectionFont = new Font(strFontName, fltSize, intStyle);
		}
		
		public void SetBold(bool blnBold)
		{ if (rtfTextBox.SelectionFont != null)
				SetFont(blnBold, rtfTextBox.SelectionFont.Italic, rtfTextBox.SelectionFont.Underline);
		}
		
		public void SetItalic(bool blnItalic)
		{ if (rtfTextBox.SelectionFont != null)
				SetFont(rtfTextBox.SelectionFont.Bold, blnItalic, rtfTextBox.SelectionFont.Underline);
		}
		
		public void SetUnderline(bool blnUnderline)
		{ if (rtfTextBox.SelectionFont != null)
				SetFont(rtfTextBox.SelectionFont.Bold, rtfTextBox.SelectionFont.Italic, blnUnderline);
		}

		/// <summary>
		///		Modifica el color de la selección
		/// </summary>
		public void SetColor(Color clrColor)
		{ if (clrColor != Color.Transparent)
				rtfTextBox.SelectionColor = clrColor;
		} 
		
		/// <summary>
		///		Obtiene el color seleccionado por el usuario
		/// </summary>
		private Color GetDialogColor()
		{ // Devuelve el color seleccionado en el cuadro de diálogo
				if (dlgColor.ShowDialog() == DialogResult.OK)
					return dlgColor.Color;
			// Si no ha seleccionado nada, devuelve el color transparente
				return Color.Transparent;
		}
		
		/// <summary>
		///		Convierte el texto RTF en HTML
		/// </summary>
		private string getHTML()
		{ bool blnBold = false, blnUnderline = false, blnStrikeThru = false;
			bool blnItalic = false;
			string strLastFont = "";
			Color colLastColor = Color.Black;
			HorizontalAlignment intLastAlignment = HorizontalAlignment.Left;
			string strHTML = "";
		
				for (int intIndex = 0; intIndex < rtfTextBox.TextLength; intIndex++)
					{ // Selecciona el carácter correspondiente
							rtfTextBox.Select(intIndex, 1);
						// Alineación
							if (intLastAlignment != rtfTextBox.SelectionAlignment)
								{	// Crea un párrafo con la alineación adecuada
										switch (rtfTextBox.SelectionAlignment)
											{ case HorizontalAlignment.Left:
														strHTML += "<p align = left>";
													break;
												case HorizontalAlignment.Center:
														strHTML += "<p align = center>";
													break;
												case HorizontalAlignment.Right:
														strHTML += "<p align = right>";
													break;
											}
									// Guarda la alineación
										intLastAlignment = rtfTextBox.SelectionAlignment;
								}
						// Negrita
							if (blnBold != rtfTextBox.SelectionFont.Bold)
								{ // Abre / cierra el tag HTML
										if (rtfTextBox.SelectionFont.Bold)
											strHTML += "<b>";
										else
											strHTML += "</b>";
									// Guarda el estado
										blnBold = rtfTextBox.SelectionFont.Bold;
								}
						// Cursiva
							if (blnItalic != rtfTextBox.SelectionFont.Italic)
								{ if (blnItalic = rtfTextBox.SelectionFont.Italic)
										strHTML += "<i>";
									else
										strHTML += "</i>";
								}
						// Subrayado
							if (blnUnderline != rtfTextBox.SelectionFont.Underline)
								{ if (blnUnderline = rtfTextBox.SelectionFont.Underline)
										strHTML += "<u>";
									else
										strHTML += "</u>";
								}
						// Tachado
							if (blnStrikeThru != rtfTextBox.SelectionFont.Strikeout)
								{ if (blnStrikeThru = rtfTextBox.SelectionFont.Strikeout)
										strHTML += "<s>";
									else
										strHTML += "</s>";
								}
						// Fuente
							if (strLastFont != rtfTextBox.SelectionFont.Name)
								{ if (strLastFont != "")
										strHTML += "</font>";
									strLastFont = rtfTextBox.SelectionFont.Name;
									strHTML += "<font face='" + strLastFont + "'>";
								}
						// Color
		//					if (colLastColor != rtfTextBox.SelectionColor)
		//            lngLastFontColor& = rtbRichTextBox.SelColor
		//            
		//            ''Get hexidecimal value of color
		//            strHex$ = Hex(rtbRichTextBox.SelColor)
		//            strHex$ = String$(6 - Len(strHex$), "0") & strHex$
		//            strHex$ = Right$(strHex$, 2) & Mid$(strHex$, 3, 2) & Left$(strHex$, 2)
		//            
		//            strHTML$ = strHTML$ + "<font color=#" & strHex$ & ">"
		//        End If
						// Añade el texto seleccionado a la cadena de salida
							strHTML += rtfTextBox.SelectedText;
					}
				// Devuelve la cadena HTML
					return strHTML;
		}
		
		/// <summary>
		///		Actualiza la barra de herramientas para que refleje el texto seleccionado
		/// </summary>
		private void RefreshToolBar()
		{ tlbBold.Checked = rtfTextBox.SelectionFont.Bold;
			tlbItalic.Checked = rtfTextBox.SelectionFont.Italic;
			tlbUnderline.Checked = rtfTextBox.SelectionFont.Underline;
		}

		/// <summary>
		///		Copia el texto seleccionado en el portapapeles
		/// </summary>
		public void Copy()
		{	if (rtfTextBox.SelectedText != null && rtfTextBox.SelectedText.Trim() != "")
				Clipboard.SetText(rtfTextBox.SelectedText);
		}

		/// <summary>
		///		Copia el texto seleccionado en el portapapeles
		/// </summary>
		public void Cut()
		{ // Copia
				Copy();
			// ... elimina el texto seleccionado
				rtfTextBox.SelectedText = "";
		}	
		
		/// <summary>
		///		Pega el texto del portapapeles
		/// </summary>
		public void Paste()
		{ rtfTextBox.InsertTextAsRtf(Clipboard.GetText());
		}
		
		public Font SelectionFont
		{ get { return rtfTextBox.SelectionFont; }
		}		

		public string HTML
		{ get { return getHTML(); }
		}

		public string PlainText
		{ get { return rtfTextBox.Text; }
			set { rtfTextBox.Text = value; }
		}
		
		public override string Text
		{ get { return rtfTextBox.Rtf; }
			set 
				{ try
						{	rtfTextBox.Rtf = value; 
						}
					catch 
						{ rtfTextBox.Text = value;
						}
				}
		}

		public string SelectedText
		{ get { return rtfTextBox.SelectedText; }
			set { rtfTextBox.SelectedText = ""; }
		}		
		
		private void rtfTextBox_SelectionChanged(object sender, System.EventArgs e)
		{	// Llama a los controles de herramientas para que se modifiquen las barras de herramientas
				if (SelectionChanged != null)
					SelectionChanged(sender, e);
			// Modifica la barra de herramientas
				RefreshToolBar();
		}

		private void tlbBold_Click(object sender, EventArgs e)
		{ tlbBold.Checked = !tlbBold.Checked;
			SetBold(tlbBold.Checked);
		}

		private void tlbItalic_Click(object sender, EventArgs e)
		{ tlbItalic.Checked = !tlbItalic.Checked;
			SetItalic(tlbItalic.Checked);
		}

		private void tlbUnderline_Click(object sender, EventArgs e)
		{ tlbUnderline.Checked = !tlbUnderline.Checked;
			SetUnderline(tlbUnderline.Checked);
		}

		private void tlbColor_Click(object sender, EventArgs e)
		{ SetColor(GetDialogColor());
		}

		private void mnuEditCopy_Click(object sender, EventArgs e)
		{ Copy();
		}

		private void mnuEditCut_Click(object sender, EventArgs e)
		{ Cut();
		}

		private void mnuEditPaste_Click(object sender, EventArgs e)
		{ Paste();
		}

		private void mnuEditRemove_Click(object sender, EventArgs e)
		{ rtfTextBox.SelectedText = "";
		}

		private void ctxMenuEdit_Opening(object sender, CancelEventArgs e)
		{ mnuEditCopy.Enabled = rtfTextBox.SelectedText != null && rtfTextBox.SelectedText != "";
			mnuEditRemove.Enabled = mnuEditCut.Enabled = mnuEditCopy.Enabled;
			mnuEditPaste.Enabled = Clipboard.ContainsText();
		}
	}
}
