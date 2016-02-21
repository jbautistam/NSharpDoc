namespace Bau.Controls.ColorSelection
{
	partial class ColorSelect
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
		this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
		this.cmdSearchColor = new System.Windows.Forms.Button();
		this.lblColor = new System.Windows.Forms.Label();
		this.dlgColor = new System.Windows.Forms.ColorDialog();
		this.tableLayoutPanel1.SuspendLayout();
		this.SuspendLayout();
		// 
		// tableLayoutPanel1
		// 
		this.tableLayoutPanel1.ColumnCount = 2;
		this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
		this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 32F));
		this.tableLayoutPanel1.Controls.Add(this.cmdSearchColor, 1, 0);
		this.tableLayoutPanel1.Controls.Add(this.lblColor, 0, 0);
		this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
		this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
		this.tableLayoutPanel1.Name = "tableLayoutPanel1";
		this.tableLayoutPanel1.RowCount = 1;
		this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
		this.tableLayoutPanel1.Size = new System.Drawing.Size(320, 20);
		this.tableLayoutPanel1.TabIndex = 0;
		// 
		// cmdSearchColor
		// 
		this.cmdSearchColor.Dock = System.Windows.Forms.DockStyle.Fill;
		this.cmdSearchColor.Location = new System.Drawing.Point(288, 0);
		this.cmdSearchColor.Margin = new System.Windows.Forms.Padding(0);
		this.cmdSearchColor.Name = "cmdSearchColor";
		this.cmdSearchColor.Size = new System.Drawing.Size(32, 20);
		this.cmdSearchColor.TabIndex = 1;
		this.cmdSearchColor.Text = "...";
		this.cmdSearchColor.UseVisualStyleBackColor = true;
		this.cmdSearchColor.Click += new System.EventHandler(this.cmdSearchColor_Click);
		// 
		// lblColor
		// 
		this.lblColor.AutoSize = true;
		this.lblColor.BackColor = System.Drawing.Color.White;
		this.lblColor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.lblColor.Dock = System.Windows.Forms.DockStyle.Fill;
		this.lblColor.Location = new System.Drawing.Point(0, 0);
		this.lblColor.Margin = new System.Windows.Forms.Padding(0);
		this.lblColor.Name = "lblColor";
		this.lblColor.Size = new System.Drawing.Size(288, 20);
		this.lblColor.TabIndex = 2;
		// 
		// ColorSelect
		// 
		this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
		this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.Controls.Add(this.tableLayoutPanel1);
		this.Name = "ColorSelect";
		this.Size = new System.Drawing.Size(320, 20);
		this.tableLayoutPanel1.ResumeLayout(false);
		this.tableLayoutPanel1.PerformLayout();
		this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.Button cmdSearchColor;
		private System.Windows.Forms.Label lblColor;
		private System.Windows.Forms.ColorDialog dlgColor;
	}
}
