namespace Bau.Controls.Files
{
	partial class TextBoxSelectPath
	{
		/// <summary> 
		/// Variable del diseñador requerida.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary> 
		/// Limpiar los recursos que se estén utilizando.
		/// </summary>
		/// <param name="disposing">true si los recursos administrados se deben eliminar; false en caso contrario, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Código generado por el Diseñador de componentes

		/// <summary> 
		/// Método necesario para admitir el Diseñador. No se puede modificar
		/// el contenido del método con el editor de código.
		/// </summary>
		private void InitializeComponent()
		{
			this.dlgPath = new System.Windows.Forms.FolderBrowserDialog();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.txtFileName = new System.Windows.Forms.TextBox();
			this.cmdSearchFile = new System.Windows.Forms.Button();
			this.tableLayoutPanel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 2;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 32F));
			this.tableLayoutPanel1.Controls.Add(this.txtFileName, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.cmdSearchFile, 1, 0);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 1;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(558, 20);
			this.tableLayoutPanel1.TabIndex = 1;
			// 
			// txtFileName
			// 
			this.txtFileName.Dock = System.Windows.Forms.DockStyle.Fill;
			this.txtFileName.Location = new System.Drawing.Point(0, 0);
			this.txtFileName.Margin = new System.Windows.Forms.Padding(0);
			this.txtFileName.Name = "txtFileName";
			this.txtFileName.Size = new System.Drawing.Size(526, 20);
			this.txtFileName.TabIndex = 0;
			// 
			// cmdSearchFile
			// 
			this.cmdSearchFile.Dock = System.Windows.Forms.DockStyle.Fill;
			this.cmdSearchFile.Location = new System.Drawing.Point(526, 0);
			this.cmdSearchFile.Margin = new System.Windows.Forms.Padding(0);
			this.cmdSearchFile.Name = "cmdSearchFile";
			this.cmdSearchFile.Size = new System.Drawing.Size(32, 20);
			this.cmdSearchFile.TabIndex = 1;
			this.cmdSearchFile.Text = "...";
			this.cmdSearchFile.UseVisualStyleBackColor = true;
			this.cmdSearchFile.Click += new System.EventHandler(this.cmdSearchFile_Click);
			// 
			// TextBoxSelectPath
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.tableLayoutPanel1);
			this.Margin = new System.Windows.Forms.Padding(0);
			this.MaximumSize = new System.Drawing.Size(10000, 20);
			this.MinimumSize = new System.Drawing.Size(200, 20);
			this.Name = "TextBoxSelectPath";
			this.Size = new System.Drawing.Size(558, 20);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.FolderBrowserDialog dlgPath;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.TextBox txtFileName;
		private System.Windows.Forms.Button cmdSearchFile;
	}
}
