namespace Bau.Applications.NSharpDoc
{
	partial class frmProvider
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmProvider));
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.optDataBase = new System.Windows.Forms.RadioButton();
			this.optSourceCode = new System.Windows.Forms.RadioButton();
			this.label3 = new System.Windows.Forms.Label();
			this.fnSolution = new Bau.Controls.Files.TextBoxSelectFile();
			this.txtUser = new System.Windows.Forms.TextBox();
			this.txtRepeatPassword = new System.Windows.Forms.TextBox();
			this.txtPassword = new System.Windows.Forms.TextBox();
			this.txtServer = new System.Windows.Forms.TextBox();
			this.label5 = new System.Windows.Forms.Label();
			this.txtDataBase = new System.Windows.Forms.TextBox();
			this.label6 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.cmdCancel = new System.Windows.Forms.Button();
			this.cmdAccept = new System.Windows.Forms.Button();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.pthAdditional = new Bau.Controls.Files.TextBoxSelectPath();
			this.label9 = new System.Windows.Forms.Label();
			this.optXMLStructs = new System.Windows.Forms.RadioButton();
			this.fnXMLStructs = new Bau.Controls.Files.TextBoxSelectFile();
			this.label7 = new System.Windows.Forms.Label();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox1.Controls.Add(this.optXMLStructs);
			this.groupBox1.Controls.Add(this.optDataBase);
			this.groupBox1.Controls.Add(this.optSourceCode);
			this.groupBox1.Controls.Add(this.label7);
			this.groupBox1.Controls.Add(this.label3);
			this.groupBox1.Controls.Add(this.fnXMLStructs);
			this.groupBox1.Controls.Add(this.fnSolution);
			this.groupBox1.Controls.Add(this.txtUser);
			this.groupBox1.Controls.Add(this.txtRepeatPassword);
			this.groupBox1.Controls.Add(this.txtPassword);
			this.groupBox1.Controls.Add(this.txtServer);
			this.groupBox1.Controls.Add(this.label5);
			this.groupBox1.Controls.Add(this.txtDataBase);
			this.groupBox1.Controls.Add(this.label6);
			this.groupBox1.Controls.Add(this.label4);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.groupBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
			this.groupBox1.Location = new System.Drawing.Point(7, 7);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(632, 282);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Datos de proveedor";
			// 
			// optDataBase
			// 
			this.optDataBase.AutoSize = true;
			this.optDataBase.ForeColor = System.Drawing.Color.Navy;
			this.optDataBase.Location = new System.Drawing.Point(13, 85);
			this.optDataBase.Name = "optDataBase";
			this.optDataBase.Size = new System.Drawing.Size(187, 17);
			this.optDataBase.TabIndex = 3;
			this.optDataBase.TabStop = true;
			this.optDataBase.Text = "Base de datos (SQL Server):";
			this.optDataBase.UseVisualStyleBackColor = true;
			this.optDataBase.CheckedChanged += new System.EventHandler(this.optSourceCode_CheckedChanged);
			// 
			// optSourceCode
			// 
			this.optSourceCode.AutoSize = true;
			this.optSourceCode.ForeColor = System.Drawing.Color.Navy;
			this.optSourceCode.Location = new System.Drawing.Point(13, 25);
			this.optSourceCode.Name = "optSourceCode";
			this.optSourceCode.Size = new System.Drawing.Size(136, 17);
			this.optSourceCode.TabIndex = 0;
			this.optSourceCode.TabStop = true;
			this.optSourceCode.Text = "Código fuente (C#):";
			this.optSourceCode.UseVisualStyleBackColor = true;
			this.optSourceCode.CheckedChanged += new System.EventHandler(this.optSourceCode_CheckedChanged);
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
			this.label3.Location = new System.Drawing.Point(33, 51);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(51, 13);
			this.label3.TabIndex = 1;
			this.label3.Text = "Solución:";
			// 
			// fnSolution
			// 
			this.fnSolution.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.fnSolution.BackColorEdit = System.Drawing.SystemColors.Window;
			this.fnSolution.FileName = "";
			this.fnSolution.Filter = "Archivos de solución (*.sln)|*.sln|Archivos de proyecto (*.csproj)|*.csproj";
			this.fnSolution.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.fnSolution.ForeColor = System.Drawing.Color.Black;
			this.fnSolution.Location = new System.Drawing.Point(117, 48);
			this.fnSolution.Margin = new System.Windows.Forms.Padding(0);
			this.fnSolution.MaximumSize = new System.Drawing.Size(11667, 20);
			this.fnSolution.MinimumSize = new System.Drawing.Size(117, 20);
			this.fnSolution.Name = "fnSolution";
			this.fnSolution.Size = new System.Drawing.Size(506, 20);
			this.fnSolution.TabIndex = 2;
			this.fnSolution.Type = Bau.Controls.Files.TextBoxSelectFile.FileSelectType.Load;
			// 
			// txtUser
			// 
			this.txtUser.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtUser.BackColor = System.Drawing.Color.White;
			this.txtUser.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtUser.ImeMode = System.Windows.Forms.ImeMode.On;
			this.txtUser.Location = new System.Drawing.Point(117, 161);
			this.txtUser.Name = "txtUser";
			this.txtUser.Size = new System.Drawing.Size(506, 20);
			this.txtUser.TabIndex = 9;
			// 
			// txtRepeatPassword
			// 
			this.txtRepeatPassword.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtRepeatPassword.BackColor = System.Drawing.Color.White;
			this.txtRepeatPassword.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtRepeatPassword.ImeMode = System.Windows.Forms.ImeMode.On;
			this.txtRepeatPassword.Location = new System.Drawing.Point(412, 187);
			this.txtRepeatPassword.Name = "txtRepeatPassword";
			this.txtRepeatPassword.PasswordChar = '*';
			this.txtRepeatPassword.Size = new System.Drawing.Size(211, 20);
			this.txtRepeatPassword.TabIndex = 13;
			// 
			// txtPassword
			// 
			this.txtPassword.BackColor = System.Drawing.Color.White;
			this.txtPassword.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtPassword.ImeMode = System.Windows.Forms.ImeMode.On;
			this.txtPassword.Location = new System.Drawing.Point(117, 188);
			this.txtPassword.Name = "txtPassword";
			this.txtPassword.PasswordChar = '*';
			this.txtPassword.Size = new System.Drawing.Size(176, 20);
			this.txtPassword.TabIndex = 11;
			// 
			// txtServer
			// 
			this.txtServer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtServer.BackColor = System.Drawing.Color.White;
			this.txtServer.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtServer.ImeMode = System.Windows.Forms.ImeMode.On;
			this.txtServer.Location = new System.Drawing.Point(117, 108);
			this.txtServer.Name = "txtServer";
			this.txtServer.Size = new System.Drawing.Size(506, 20);
			this.txtServer.TabIndex = 5;
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
			this.label5.Location = new System.Drawing.Point(33, 164);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(46, 13);
			this.label5.TabIndex = 8;
			this.label5.Text = "Usuario:";
			// 
			// txtDataBase
			// 
			this.txtDataBase.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtDataBase.BackColor = System.Drawing.Color.White;
			this.txtDataBase.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtDataBase.ImeMode = System.Windows.Forms.ImeMode.On;
			this.txtDataBase.Location = new System.Drawing.Point(117, 135);
			this.txtDataBase.Name = "txtDataBase";
			this.txtDataBase.Size = new System.Drawing.Size(506, 20);
			this.txtDataBase.TabIndex = 7;
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
			this.label6.Location = new System.Drawing.Point(302, 191);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(108, 13);
			this.label6.TabIndex = 12;
			this.label6.Text = "Repita la contraseña:";
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
			this.label4.Location = new System.Drawing.Point(33, 191);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(64, 13);
			this.label4.TabIndex = 10;
			this.label4.Text = "Contraseña:";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
			this.label2.Location = new System.Drawing.Point(33, 111);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(49, 13);
			this.label2.TabIndex = 4;
			this.label2.Text = "Servidor:";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
			this.label1.Location = new System.Drawing.Point(33, 138);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(78, 13);
			this.label1.TabIndex = 6;
			this.label1.Text = "Base de datos:";
			// 
			// cmdCancel
			// 
			this.cmdCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cmdCancel.Image = ((System.Drawing.Image)(resources.GetObject("cmdCancel.Image")));
			this.cmdCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.cmdCancel.Location = new System.Drawing.Point(556, 355);
			this.cmdCancel.Name = "cmdCancel";
			this.cmdCancel.Size = new System.Drawing.Size(83, 26);
			this.cmdCancel.TabIndex = 4;
			this.cmdCancel.Text = "&Cancelar";
			this.cmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			this.cmdCancel.UseVisualStyleBackColor = true;
			// 
			// cmdAccept
			// 
			this.cmdAccept.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.cmdAccept.Image = ((System.Drawing.Image)(resources.GetObject("cmdAccept.Image")));
			this.cmdAccept.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.cmdAccept.Location = new System.Drawing.Point(470, 355);
			this.cmdAccept.Name = "cmdAccept";
			this.cmdAccept.Size = new System.Drawing.Size(83, 26);
			this.cmdAccept.TabIndex = 3;
			this.cmdAccept.Text = "&Aceptar";
			this.cmdAccept.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			this.cmdAccept.UseVisualStyleBackColor = true;
			this.cmdAccept.Click += new System.EventHandler(this.cmdAccept_Click);
			// 
			// groupBox2
			// 
			this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox2.Controls.Add(this.pthAdditional);
			this.groupBox2.Controls.Add(this.label9);
			this.groupBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.groupBox2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
			this.groupBox2.Location = new System.Drawing.Point(7, 297);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(632, 54);
			this.groupBox2.TabIndex = 2;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Directorios de generación";
			// 
			// pthAdditional
			// 
			this.pthAdditional.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.pthAdditional.BackColor = System.Drawing.Color.White;
			this.pthAdditional.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.pthAdditional.Location = new System.Drawing.Point(141, 21);
			this.pthAdditional.Margin = new System.Windows.Forms.Padding(0);
			this.pthAdditional.MaximumSize = new System.Drawing.Size(11667, 20);
			this.pthAdditional.MinimumSize = new System.Drawing.Size(233, 20);
			this.pthAdditional.Name = "pthAdditional";
			this.pthAdditional.PathName = "";
			this.pthAdditional.Size = new System.Drawing.Size(482, 20);
			this.pthAdditional.TabIndex = 6;
			// 
			// label9
			// 
			this.label9.AutoSize = true;
			this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label9.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
			this.label9.Location = new System.Drawing.Point(8, 25);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(130, 13);
			this.label9.TabIndex = 5;
			this.label9.Text = "Documentación adicional:";
			// 
			// optXMLStructs
			// 
			this.optXMLStructs.AutoSize = true;
			this.optXMLStructs.ForeColor = System.Drawing.Color.Navy;
			this.optXMLStructs.Location = new System.Drawing.Point(13, 220);
			this.optXMLStructs.Name = "optXMLStructs";
			this.optXMLStructs.Size = new System.Drawing.Size(298, 17);
			this.optXMLStructs.TabIndex = 3;
			this.optXMLStructs.TabStop = true;
			this.optXMLStructs.Text = "Archivo de estructuras de documentación (XML)";
			this.optXMLStructs.UseVisualStyleBackColor = true;
			this.optXMLStructs.CheckedChanged += new System.EventHandler(this.optSourceCode_CheckedChanged);
			// 
			// fnXMLStructs
			// 
			this.fnXMLStructs.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.fnXMLStructs.BackColorEdit = System.Drawing.SystemColors.Window;
			this.fnXMLStructs.FileName = "";
			this.fnXMLStructs.Filter = "Archivos de estructuras (*.xml)|*.xml|Todos los archivos|*.*";
			this.fnXMLStructs.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.fnXMLStructs.ForeColor = System.Drawing.Color.Black;
			this.fnXMLStructs.Location = new System.Drawing.Point(117, 246);
			this.fnXMLStructs.Margin = new System.Windows.Forms.Padding(0);
			this.fnXMLStructs.MaximumSize = new System.Drawing.Size(11667, 20);
			this.fnXMLStructs.MinimumSize = new System.Drawing.Size(117, 20);
			this.fnXMLStructs.Name = "fnXMLStructs";
			this.fnXMLStructs.Size = new System.Drawing.Size(506, 20);
			this.fnXMLStructs.TabIndex = 2;
			this.fnXMLStructs.Type = Bau.Controls.Files.TextBoxSelectFile.FileSelectType.Load;
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label7.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
			this.label7.Location = new System.Drawing.Point(33, 249);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(46, 13);
			this.label7.TabIndex = 1;
			this.label7.Text = "Archivo:";
			// 
			// frmProvider
			// 
			this.AcceptButton = this.cmdAccept;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.cmdCancel;
			this.ClientSize = new System.Drawing.Size(646, 387);
			this.Controls.Add(this.cmdCancel);
			this.Controls.Add(this.cmdAccept);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.groupBox1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "frmProvider";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Proveedor";
			this.Load += new System.EventHandler(this.frmProvider_Load);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.TextBox txtServer;
		private System.Windows.Forms.TextBox txtDataBase;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button cmdCancel;
		private System.Windows.Forms.Button cmdAccept;
		private System.Windows.Forms.RadioButton optDataBase;
		private System.Windows.Forms.RadioButton optSourceCode;
		private Controls.Files.TextBoxSelectFile fnSolution;
		private System.Windows.Forms.TextBox txtUser;
		private System.Windows.Forms.TextBox txtPassword;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox txtRepeatPassword;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.GroupBox groupBox2;
		private Controls.Files.TextBoxSelectPath pthAdditional;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.RadioButton optXMLStructs;
		private System.Windows.Forms.Label label7;
		private Controls.Files.TextBoxSelectFile fnXMLStructs;
	}
}