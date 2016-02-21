namespace Bau.Controls.Forms
{
	partial class frmMessage
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
		this.cmdAccept = new System.Windows.Forms.Button();
		this.cmdCancel = new System.Windows.Forms.Button();
		this.lblMessage = new System.Windows.Forms.Label();
		this.txtInput = new System.Windows.Forms.TextBox();
		this.SuspendLayout();
		// 
		// cmdAccept
		// 
		this.cmdAccept.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
		this.cmdAccept.DialogResult = System.Windows.Forms.DialogResult.OK;
		this.cmdAccept.Image = global::Bau.Controls.Properties.Resources.Accept;
		this.cmdAccept.Location = new System.Drawing.Point(213, 60);
		this.cmdAccept.Name = "cmdAccept";
		this.cmdAccept.Size = new System.Drawing.Size(83, 25);
		this.cmdAccept.TabIndex = 2;
		this.cmdAccept.Text = "&Aceptar";
		this.cmdAccept.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
		this.cmdAccept.UseVisualStyleBackColor = true;
		// 
		// cmdCancel
		// 
		this.cmdCancel.Anchor = ((System.Windows.Forms.AnchorStyles) ((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
		this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
		this.cmdCancel.Image = global::Bau.Controls.Properties.Resources.Remove;
		this.cmdCancel.Location = new System.Drawing.Point(300, 60);
		this.cmdCancel.Name = "cmdCancel";
		this.cmdCancel.Size = new System.Drawing.Size(83, 25);
		this.cmdCancel.TabIndex = 3;
		this.cmdCancel.Text = "&Cancelar";
		this.cmdCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
		this.cmdCancel.UseVisualStyleBackColor = true;
		// 
		// lblMessage
		// 
		this.lblMessage.AutoSize = true;
		this.lblMessage.ForeColor = System.Drawing.Color.FromArgb(((int) (((byte) (0)))), ((int) (((byte) (0)))), ((int) (((byte) (192)))));
		this.lblMessage.Location = new System.Drawing.Point(7, 10);
		this.lblMessage.Name = "lblMessage";
		this.lblMessage.Size = new System.Drawing.Size(60, 13);
		this.lblMessage.TabIndex = 0;
		this.lblMessage.Text = "lblMessage";
		// 
		// txtInput
		// 
		this.txtInput.Anchor = ((System.Windows.Forms.AnchorStyles) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
								| System.Windows.Forms.AnchorStyles.Right)));
		this.txtInput.Location = new System.Drawing.Point(12, 31);
		this.txtInput.Name = "txtInput";
		this.txtInput.Size = new System.Drawing.Size(371, 20);
		this.txtInput.TabIndex = 1;
		// 
		// frmMessage
		// 
		this.AcceptButton = this.cmdAccept;
		this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
		this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.CancelButton = this.cmdCancel;
		this.ClientSize = new System.Drawing.Size(390, 89);
		this.Controls.Add(this.txtInput);
		this.Controls.Add(this.lblMessage);
		this.Controls.Add(this.cmdCancel);
		this.Controls.Add(this.cmdAccept);
		this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
		this.MaximizeBox = false;
		this.MinimizeBox = false;
		this.Name = "frmMessage";
		this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		this.Text = "Título";
		this.ResumeLayout(false);
		this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button cmdAccept;
		private System.Windows.Forms.Button cmdCancel;
		private System.Windows.Forms.Label lblMessage;
		private System.Windows.Forms.TextBox txtInput;
	}
}