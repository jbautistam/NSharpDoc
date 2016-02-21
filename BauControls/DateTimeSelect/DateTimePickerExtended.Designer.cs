namespace Bau.Controls.DateTimeSelect
{
	partial class DateTimePickerExtended
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
		this.txtDate = new System.Windows.Forms.TextBox();
		this.cmdShowCalendar = new System.Windows.Forms.PictureBox();
		this.txtHour = new Bau.Controls.TextBox.NumericUpDowExtended();
		this.txtMinute = new Bau.Controls.TextBox.NumericUpDowExtended();
		((System.ComponentModel.ISupportInitialize)(this.cmdShowCalendar)).BeginInit();
		((System.ComponentModel.ISupportInitialize)(this.txtHour)).BeginInit();
		((System.ComponentModel.ISupportInitialize)(this.txtMinute)).BeginInit();
		this.SuspendLayout();
		// 
		// txtDate
		// 
		this.txtDate.Location = new System.Drawing.Point(0, 1);
		this.txtDate.Name = "txtDate";
		this.txtDate.Size = new System.Drawing.Size(65, 20);
		this.txtDate.TabIndex = 0;
		this.txtDate.TextChanged += new System.EventHandler(this.txtDate_TextChanged);
		// 
		// cmdShowCalendar
		// 
		this.cmdShowCalendar.BackColor = System.Drawing.Color.Transparent;
		this.cmdShowCalendar.Image = global::Bau.Controls.Properties.Resources.calendar;
		this.cmdShowCalendar.Location = new System.Drawing.Point(65, 2);
		this.cmdShowCalendar.Name = "cmdShowCalendar";
		this.cmdShowCalendar.Size = new System.Drawing.Size(19, 19);
		this.cmdShowCalendar.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
		this.cmdShowCalendar.TabIndex = 4;
		this.cmdShowCalendar.TabStop = false;
		this.cmdShowCalendar.Click += new System.EventHandler(this.cmdShowCalendar_Click);
		// 
		// txtHour
		// 
		this.txtHour.Location = new System.Drawing.Point(87, 1);
		this.txtHour.Maximum = new decimal(new int[] {
            59,
            0,
            0,
            0});
		this.txtHour.Name = "txtHour";
		this.txtHour.Size = new System.Drawing.Size(39, 20);
		this.txtHour.TabIndex = 1;
		this.txtHour.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtHour.ValueChanged += new System.EventHandler(this.txtDate_TextChanged);
		// 
		// txtMinute
		// 
		this.txtMinute.Location = new System.Drawing.Point(130, 1);
		this.txtMinute.Maximum = new decimal(new int[] {
            59,
            0,
            0,
            0});
		this.txtMinute.Name = "txtMinute";
		this.txtMinute.Size = new System.Drawing.Size(39, 20);
		this.txtMinute.TabIndex = 2;
		this.txtMinute.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtMinute.ValueChanged += new System.EventHandler(this.txtDate_TextChanged);
		// 
		// DateTimePickerExtended
		// 
		this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
		this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.Controls.Add(this.txtMinute);
		this.Controls.Add(this.txtHour);
		this.Controls.Add(this.cmdShowCalendar);
		this.Controls.Add(this.txtDate);
		this.Name = "DateTimePickerExtended";
		this.Size = new System.Drawing.Size(168, 23);
		((System.ComponentModel.ISupportInitialize)(this.cmdShowCalendar)).EndInit();
		((System.ComponentModel.ISupportInitialize)(this.txtHour)).EndInit();
		((System.ComponentModel.ISupportInitialize)(this.txtMinute)).EndInit();
		this.ResumeLayout(false);
		this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox txtDate;
		private System.Windows.Forms.PictureBox cmdShowCalendar;
		private TextBox.NumericUpDowExtended txtHour;
		private TextBox.NumericUpDowExtended txtMinute;
	}
}
