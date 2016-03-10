namespace Bau.Applications.NSharpDoc
{
	partial class frmMain
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.label4 = new System.Windows.Forms.Label();
			this.cboDocumentType = new Bau.Controls.Combos.ComboBoxExtended();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.label10 = new System.Windows.Forms.Label();
			this.pthTemplates = new Bau.Controls.Files.TextBoxSelectPath();
			this.pthTarget = new Bau.Controls.Files.TextBoxSelectPath();
			this.label7 = new System.Windows.Forms.Label();
			this.label8 = new System.Windows.Forms.Label();
			this.toolStrip1 = new System.Windows.Forms.ToolStrip();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.tabPage1 = new System.Windows.Forms.TabPage();
			this.lswProviders = new Bau.Controls.List.ListUpdatable();
			this.tabPage2 = new System.Windows.Forms.TabPage();
			this.txtLog = new System.Windows.Forms.TextBox();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.chkShowPrivate = new System.Windows.Forms.CheckBox();
			this.chkShowPublic = new System.Windows.Forms.CheckBox();
			this.chkShowProtected = new System.Windows.Forms.CheckBox();
			this.chkShowInternal = new System.Windows.Forms.CheckBox();
			this.cmdNew = new System.Windows.Forms.ToolStripButton();
			this.cmdOpen = new System.Windows.Forms.ToolStripButton();
			this.cmdSave = new System.Windows.Forms.ToolStripButton();
			this.cmdClear = new System.Windows.Forms.ToolStripButton();
			this.cmdSend = new System.Windows.Forms.ToolStripButton();
			this.groupBox1.SuspendLayout();
			this.toolStrip1.SuspendLayout();
			this.tabControl1.SuspendLayout();
			this.tabPage1.SuspendLayout();
			this.tabPage2.SuspendLayout();
			this.groupBox3.SuspendLayout();
			this.SuspendLayout();
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
			this.label4.Location = new System.Drawing.Point(8, 23);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(31, 13);
			this.label4.TabIndex = 0;
			this.label4.Text = "Tipo:";
			// 
			// cboDocumentType
			// 
			this.cboDocumentType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.cboDocumentType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cboDocumentType.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cboDocumentType.ForeColor = System.Drawing.Color.Black;
			this.cboDocumentType.FormattingEnabled = true;
			this.cboDocumentType.Location = new System.Drawing.Point(63, 19);
			this.cboDocumentType.Name = "cboDocumentType";
			this.cboDocumentType.SelectedID = null;
			this.cboDocumentType.Size = new System.Drawing.Size(661, 21);
			this.cboDocumentType.TabIndex = 1;
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox1.Controls.Add(this.label10);
			this.groupBox1.Controls.Add(this.pthTemplates);
			this.groupBox1.Controls.Add(this.pthTarget);
			this.groupBox1.Controls.Add(this.label7);
			this.groupBox1.Controls.Add(this.label8);
			this.groupBox1.Controls.Add(this.label4);
			this.groupBox1.Controls.Add(this.cboDocumentType);
			this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.groupBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
			this.groupBox1.Location = new System.Drawing.Point(8, 36);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(738, 132);
			this.groupBox1.TabIndex = 1;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Parámetros de generación";
			// 
			// label10
			// 
			this.label10.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.label10.AutoSize = true;
			this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label10.ForeColor = System.Drawing.Color.Maroon;
			this.label10.Location = new System.Drawing.Point(296, 104);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(426, 13);
			this.label10.TabIndex = 6;
			this.label10.Text = "Al generar la documentación se elimina el contenido del directorio destino";
			// 
			// pthTemplates
			// 
			this.pthTemplates.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.pthTemplates.BackColor = System.Drawing.Color.White;
			this.pthTemplates.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.pthTemplates.Location = new System.Drawing.Point(63, 48);
			this.pthTemplates.Margin = new System.Windows.Forms.Padding(0);
			this.pthTemplates.MaximumSize = new System.Drawing.Size(11667, 20);
			this.pthTemplates.MinimumSize = new System.Drawing.Size(233, 20);
			this.pthTemplates.Name = "pthTemplates";
			this.pthTemplates.PathName = "";
			this.pthTemplates.Size = new System.Drawing.Size(661, 20);
			this.pthTemplates.TabIndex = 3;
			// 
			// pthTarget
			// 
			this.pthTarget.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.pthTarget.BackColor = System.Drawing.Color.White;
			this.pthTarget.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.pthTarget.Location = new System.Drawing.Point(63, 77);
			this.pthTarget.Margin = new System.Windows.Forms.Padding(0);
			this.pthTarget.MaximumSize = new System.Drawing.Size(11667, 20);
			this.pthTarget.MinimumSize = new System.Drawing.Size(233, 20);
			this.pthTarget.Name = "pthTarget";
			this.pthTarget.PathName = "";
			this.pthTarget.Size = new System.Drawing.Size(661, 20);
			this.pthTarget.TabIndex = 5;
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label7.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
			this.label7.Location = new System.Drawing.Point(8, 81);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(39, 13);
			this.label7.TabIndex = 4;
			this.label7.Text = "Salida:";
			// 
			// label8
			// 
			this.label8.AutoSize = true;
			this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label8.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
			this.label8.Location = new System.Drawing.Point(8, 52);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(51, 13);
			this.label8.TabIndex = 2;
			this.label8.Text = "Plantillas:";
			// 
			// toolStrip1
			// 
			this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmdNew,
            this.cmdOpen,
            this.cmdSave,
            this.toolStripSeparator1,
            this.cmdClear,
            this.toolStripSeparator2,
            this.cmdSend});
			this.toolStrip1.Location = new System.Drawing.Point(0, 0);
			this.toolStrip1.Name = "toolStrip1";
			this.toolStrip1.Size = new System.Drawing.Size(754, 25);
			this.toolStrip1.TabIndex = 0;
			this.toolStrip1.Text = "toolStrip1";
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
			// 
			// tabControl1
			// 
			this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tabControl1.Controls.Add(this.tabPage1);
			this.tabControl1.Controls.Add(this.tabPage2);
			this.tabControl1.Location = new System.Drawing.Point(8, 245);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(738, 423);
			this.tabControl1.TabIndex = 2;
			// 
			// tabPage1
			// 
			this.tabPage1.Controls.Add(this.lswProviders);
			this.tabPage1.Location = new System.Drawing.Point(4, 22);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage1.Size = new System.Drawing.Size(730, 397);
			this.tabPage1.TabIndex = 0;
			this.tabPage1.Text = "Proveedores";
			this.tabPage1.UseVisualStyleBackColor = true;
			// 
			// lswProviders
			// 
			this.lswProviders.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lswProviders.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lswProviders.ForeColor = System.Drawing.Color.Black;
			this.lswProviders.Location = new System.Drawing.Point(3, 3);
			this.lswProviders.MultiSelect = true;
			this.lswProviders.Name = "lswProviders";
			this.lswProviders.ShowItemToolTips = false;
			this.lswProviders.Size = new System.Drawing.Size(724, 391);
			this.lswProviders.TabIndex = 0;
			this.lswProviders.OnUpdateRecord += new Bau.Controls.List.ListUpdatable.UpdateRecordHandler(this.lswProviders_OnUpdateRecord);
			this.lswProviders.OnDeleteRecord += new Bau.Controls.List.ListUpdatable.DeleteRecordHandler(this.lswProviders_OnDeleteRecord);
			// 
			// tabPage2
			// 
			this.tabPage2.Controls.Add(this.txtLog);
			this.tabPage2.Location = new System.Drawing.Point(4, 22);
			this.tabPage2.Name = "tabPage2";
			this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage2.Size = new System.Drawing.Size(730, 397);
			this.tabPage2.TabIndex = 1;
			this.tabPage2.Text = "Resultados";
			this.tabPage2.UseVisualStyleBackColor = true;
			// 
			// txtLog
			// 
			this.txtLog.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
			this.txtLog.Dock = System.Windows.Forms.DockStyle.Fill;
			this.txtLog.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtLog.Location = new System.Drawing.Point(3, 3);
			this.txtLog.Multiline = true;
			this.txtLog.Name = "txtLog";
			this.txtLog.ReadOnly = true;
			this.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.txtLog.Size = new System.Drawing.Size(724, 391);
			this.txtLog.TabIndex = 0;
			// 
			// groupBox3
			// 
			this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox3.Controls.Add(this.chkShowPrivate);
			this.groupBox3.Controls.Add(this.chkShowPublic);
			this.groupBox3.Controls.Add(this.chkShowProtected);
			this.groupBox3.Controls.Add(this.chkShowInternal);
			this.groupBox3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.groupBox3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
			this.groupBox3.Location = new System.Drawing.Point(8, 174);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(738, 66);
			this.groupBox3.TabIndex = 3;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "Parámetros de generación";
			// 
			// chkShowPrivate
			// 
			this.chkShowPrivate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.chkShowPrivate.AutoSize = true;
			this.chkShowPrivate.Checked = true;
			this.chkShowPrivate.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkShowPrivate.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.chkShowPrivate.ForeColor = System.Drawing.Color.Black;
			this.chkShowPrivate.Location = new System.Drawing.Point(531, 42);
			this.chkShowPrivate.Name = "chkShowPrivate";
			this.chkShowPrivate.Size = new System.Drawing.Size(156, 17);
			this.chkShowPrivate.TabIndex = 3;
			this.chkShowPrivate.Text = "Documentar datos privados";
			this.chkShowPrivate.UseVisualStyleBackColor = true;
			// 
			// chkShowPublic
			// 
			this.chkShowPublic.AutoSize = true;
			this.chkShowPublic.Checked = true;
			this.chkShowPublic.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkShowPublic.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.chkShowPublic.ForeColor = System.Drawing.Color.Black;
			this.chkShowPublic.Location = new System.Drawing.Point(31, 19);
			this.chkShowPublic.Name = "chkShowPublic";
			this.chkShowPublic.Size = new System.Drawing.Size(155, 17);
			this.chkShowPublic.TabIndex = 0;
			this.chkShowPublic.Text = "Documentar datos públicos";
			this.chkShowPublic.UseVisualStyleBackColor = true;
			// 
			// chkShowProtected
			// 
			this.chkShowProtected.AutoSize = true;
			this.chkShowProtected.Checked = true;
			this.chkShowProtected.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkShowProtected.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.chkShowProtected.ForeColor = System.Drawing.Color.Black;
			this.chkShowProtected.Location = new System.Drawing.Point(30, 42);
			this.chkShowProtected.Name = "chkShowProtected";
			this.chkShowProtected.Size = new System.Drawing.Size(165, 17);
			this.chkShowProtected.TabIndex = 2;
			this.chkShowProtected.Text = "Documentar datos protegidos";
			this.chkShowProtected.UseVisualStyleBackColor = true;
			// 
			// chkShowInternal
			// 
			this.chkShowInternal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.chkShowInternal.AutoSize = true;
			this.chkShowInternal.Checked = true;
			this.chkShowInternal.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkShowInternal.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.chkShowInternal.ForeColor = System.Drawing.Color.Black;
			this.chkShowInternal.Location = new System.Drawing.Point(531, 19);
			this.chkShowInternal.Name = "chkShowInternal";
			this.chkShowInternal.Size = new System.Drawing.Size(153, 17);
			this.chkShowInternal.TabIndex = 1;
			this.chkShowInternal.Text = "Documentar datos internos";
			this.chkShowInternal.UseVisualStyleBackColor = true;
			// 
			// cmdNew
			// 
			this.cmdNew.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.cmdNew.Image = global::Bau.Applications.NSharpDoc.Properties.Resources.NewItem;
			this.cmdNew.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.cmdNew.Name = "cmdNew";
			this.cmdNew.Size = new System.Drawing.Size(23, 22);
			this.cmdNew.Text = "Nuevo";
			this.cmdNew.Click += new System.EventHandler(this.cmdNew_Click);
			// 
			// cmdOpen
			// 
			this.cmdOpen.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.cmdOpen.Image = global::Bau.Applications.NSharpDoc.Properties.Resources.folder;
			this.cmdOpen.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.cmdOpen.Name = "cmdOpen";
			this.cmdOpen.Size = new System.Drawing.Size(23, 22);
			this.cmdOpen.Text = "Abrir";
			this.cmdOpen.Click += new System.EventHandler(this.cmdOpen_Click);
			// 
			// cmdSave
			// 
			this.cmdSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.cmdSave.Image = global::Bau.Applications.NSharpDoc.Properties.Resources.disk;
			this.cmdSave.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.cmdSave.Name = "cmdSave";
			this.cmdSave.Size = new System.Drawing.Size(23, 22);
			this.cmdSave.Text = "Grabar";
			this.cmdSave.Click += new System.EventHandler(this.cmdSave_Click);
			// 
			// cmdClear
			// 
			this.cmdClear.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.cmdClear.Image = global::Bau.Applications.NSharpDoc.Properties.Resources.cancel;
			this.cmdClear.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.cmdClear.Name = "cmdClear";
			this.cmdClear.Size = new System.Drawing.Size(23, 22);
			this.cmdClear.Text = "Limpiar";
			this.cmdClear.Click += new System.EventHandler(this.cmdClear_Click);
			// 
			// cmdSend
			// 
			this.cmdSend.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.cmdSend.Image = global::Bau.Applications.NSharpDoc.Properties.Resources.world_go;
			this.cmdSend.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.cmdSend.Name = "cmdSend";
			this.cmdSend.Size = new System.Drawing.Size(23, 22);
			this.cmdSend.Text = "Procesar";
			this.cmdSend.Click += new System.EventHandler(this.cmdSend_Click);
			// 
			// frmMain
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(754, 670);
			this.Controls.Add(this.groupBox3);
			this.Controls.Add(this.tabControl1);
			this.Controls.Add(this.toolStrip1);
			this.Controls.Add(this.groupBox1);
			this.Name = "frmMain";
			this.Text = "NSharpDoc";
			this.Load += new System.EventHandler(this.frmMain_Load);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.toolStrip1.ResumeLayout(false);
			this.toolStrip1.PerformLayout();
			this.tabControl1.ResumeLayout(false);
			this.tabPage1.ResumeLayout(false);
			this.tabPage2.ResumeLayout(false);
			this.tabPage2.PerformLayout();
			this.groupBox3.ResumeLayout(false);
			this.groupBox3.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion
		private System.Windows.Forms.Label label4;
		private Bau.Controls.Combos.ComboBoxExtended cboDocumentType;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.ToolStrip toolStrip1;
		private System.Windows.Forms.ToolStripButton cmdOpen;
		private System.Windows.Forms.ToolStripButton cmdSave;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripButton cmdClear;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
		private System.Windows.Forms.ToolStripButton cmdSend;
		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.TabPage tabPage1;
		private System.Windows.Forms.TabPage tabPage2;
		private System.Windows.Forms.TextBox txtLog;
		private Controls.List.ListUpdatable lswProviders;
		private System.Windows.Forms.Label label10;
		private Controls.Files.TextBoxSelectPath pthTemplates;
		private Controls.Files.TextBoxSelectPath pthTarget;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.GroupBox groupBox3;
		private System.Windows.Forms.CheckBox chkShowPrivate;
		private System.Windows.Forms.CheckBox chkShowPublic;
		private System.Windows.Forms.CheckBox chkShowProtected;
		private System.Windows.Forms.CheckBox chkShowInternal;
		private System.Windows.Forms.ToolStripButton cmdNew;
	}
}

