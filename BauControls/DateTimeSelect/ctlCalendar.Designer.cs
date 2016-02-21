namespace Bau.Controls.DateTimeSelect
{
	partial class ctlCalendar
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
			this.mntCalendar = new System.Windows.Forms.MonthCalendar();
			this.SuspendLayout();
			// 
			// mntCalendar
			// 
			this.mntCalendar.Dock = System.Windows.Forms.DockStyle.Fill;
			this.mntCalendar.Location = new System.Drawing.Point(0, 0);
			this.mntCalendar.MaxSelectionCount = 1;
			this.mntCalendar.Name = "mntCalendar";
			this.mntCalendar.TabIndex = 0;
			this.mntCalendar.DateSelected += new System.Windows.Forms.DateRangeEventHandler(this.mntCalendar_DateSelected);
			// 
			// ctlCalendar
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.mntCalendar);
			this.Name = "ctlCalendar";
			this.Size = new System.Drawing.Size(226, 161);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.MonthCalendar mntCalendar;
	}
}
