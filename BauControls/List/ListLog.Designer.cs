namespace Bau.Controls.List
{
	partial class ListLog
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

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
		this.tlbList = new System.Windows.Forms.ToolStrip();
		this.cmdSave = new System.Windows.Forms.ToolStripButton();
		this.cmdDelete = new System.Windows.Forms.ToolStripButton();
		this.lswLog = new Bau.Controls.List.ListViewExtended();
		this.tlbList.SuspendLayout();
		this.SuspendLayout();
		// 
		// tlbList
		// 
		this.tlbList.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
		this.tlbList.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmdSave,
            this.cmdDelete});
		this.tlbList.Location = new System.Drawing.Point(0, 0);
		this.tlbList.Name = "tlbList";
		this.tlbList.Size = new System.Drawing.Size(624, 25);
		this.tlbList.TabIndex = 1;
		this.tlbList.Text = "toolStrip1";
		// 
		// cmdSave
		// 
		this.cmdSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
		this.cmdSave.Image = global::Bau.Controls.Properties.Resources.Accept;
		this.cmdSave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.cmdSave.Name = "cmdSave";
		this.cmdSave.Size = new System.Drawing.Size(23, 22);
		this.cmdSave.Text = "Nuevo";
		this.cmdSave.ToolTipText = "Grabar";
		// 
		// cmdDelete
		// 
		this.cmdDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
		this.cmdDelete.Image = global::Bau.Controls.Properties.Resources.Remove;
		this.cmdDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.cmdDelete.Name = "cmdDelete";
		this.cmdDelete.Size = new System.Drawing.Size(23, 22);
		this.cmdDelete.Text = "Borrar";
		this.cmdDelete.Click += new System.EventHandler(this.cmdDelete_Click);
		// 
		// lswLog
		// 
		this.lswLog.Anchor = ((System.Windows.Forms.AnchorStyles) ((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
								| System.Windows.Forms.AnchorStyles.Left)
								| System.Windows.Forms.AnchorStyles.Right)));
		this.lswLog.FullRowSelect = true;
		this.lswLog.HideSelection = false;
		this.lswLog.Location = new System.Drawing.Point(0, 28);
		this.lswLog.Name = "lswLog";
		this.lswLog.SelectedKey = null;
		this.lswLog.Size = new System.Drawing.Size(624, 585);
		this.lswLog.TabIndex = 2;
		this.lswLog.UseCompatibleStateImageBehavior = false;
		this.lswLog.View = System.Windows.Forms.View.Details;
		// 
		// ListLog
		// 
		this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
		this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.Controls.Add(this.lswLog);
		this.Controls.Add(this.tlbList);
		this.Name = "ListLog";
		this.Size = new System.Drawing.Size(624, 613);
		this.tlbList.ResumeLayout(false);
		this.tlbList.PerformLayout();
		this.ResumeLayout(false);
		this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ToolStrip tlbList;
		private System.Windows.Forms.ToolStripButton cmdSave;
		private System.Windows.Forms.ToolStripButton cmdDelete;
		private ListViewExtended lswLog;

	}
}
