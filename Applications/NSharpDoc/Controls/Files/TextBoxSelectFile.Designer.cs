namespace Bau.Controls.Files
{
	partial class TextBoxSelectFile
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
			this.dlgFileLoad = new System.Windows.Forms.OpenFileDialog();
			this.dlgFileSave = new System.Windows.Forms.SaveFileDialog();
			this.cmdSearchFile = new System.Windows.Forms.Button();
			this.txtFileName = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// dlgFileLoad
			// 
			this.dlgFileLoad.FileName = "openFileDialog1";
			// 
			// cmdSearchFile
			// 
			this.cmdSearchFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.cmdSearchFile.Location = new System.Drawing.Point(216, -1);
			this.cmdSearchFile.Margin = new System.Windows.Forms.Padding(0);
			this.cmdSearchFile.Name = "cmdSearchFile";
			this.cmdSearchFile.Size = new System.Drawing.Size(32, 21);
			this.cmdSearchFile.TabIndex = 2;
			this.cmdSearchFile.Text = "...";
			this.cmdSearchFile.UseVisualStyleBackColor = true;
			this.cmdSearchFile.Click += new System.EventHandler(this.cmdSearchFile_Click);
			// 
			// txtFileName
			// 
			this.txtFileName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtFileName.Location = new System.Drawing.Point(0, 0);
			this.txtFileName.Margin = new System.Windows.Forms.Padding(0);
			this.txtFileName.Name = "txtFileName";
			this.txtFileName.Size = new System.Drawing.Size(214, 20);
			this.txtFileName.TabIndex = 3;
			// 
			// TextBoxSelectFile
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.txtFileName);
			this.Controls.Add(this.cmdSearchFile);
			this.Margin = new System.Windows.Forms.Padding(0);
			this.MaximumSize = new System.Drawing.Size(10000, 20);
			this.MinimumSize = new System.Drawing.Size(100, 20);
			this.Name = "TextBoxSelectFile";
			this.Size = new System.Drawing.Size(250, 20);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.OpenFileDialog dlgFileLoad;
		private System.Windows.Forms.SaveFileDialog dlgFileSave;
		private System.Windows.Forms.Button cmdSearchFile;
		private System.Windows.Forms.TextBox txtFileName;
	}
}
