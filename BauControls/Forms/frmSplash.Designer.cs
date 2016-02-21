namespace Bau.Controls.Forms
{
	partial class frmSplash
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

		#region Código generado por el Diseñador de Windows Forms

		/// <summary>
		/// Método necesario para admitir el Diseñador. No se puede modificar
		/// el contenido del método con el editor de código.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSplash));
			this.imgImage = new System.Windows.Forms.PictureBox();
			this.lblMessage = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.imgImage)).BeginInit();
			this.SuspendLayout();
			// 
			// imgImage
			// 
			this.imgImage.Dock = System.Windows.Forms.DockStyle.Fill;
			this.imgImage.Image = ((System.Drawing.Image)(resources.GetObject("imgImage.Image")));
			this.imgImage.Location = new System.Drawing.Point(0, 0);
			this.imgImage.Name = "imgImage";
			this.imgImage.Size = new System.Drawing.Size(513, 406);
			this.imgImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.imgImage.TabIndex = 0;
			this.imgImage.TabStop = false;
			// 
			// lblMessage
			// 
			this.lblMessage.BackColor = System.Drawing.Color.White;
			this.lblMessage.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.lblMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblMessage.ForeColor = System.Drawing.Color.Black;
			this.lblMessage.Location = new System.Drawing.Point(0, 386);
			this.lblMessage.Name = "lblMessage";
			this.lblMessage.Size = new System.Drawing.Size(513, 20);
			this.lblMessage.TabIndex = 1;
			this.lblMessage.Text = "lblMessage";
			// 
			// frmSplash
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(513, 406);
			this.Controls.Add(this.lblMessage);
			this.Controls.Add(this.imgImage);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Name = "frmSplash";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Splash";
			((System.ComponentModel.ISupportInitialize)(this.imgImage)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.PictureBox imgImage;
		private System.Windows.Forms.Label lblMessage;
	}
}