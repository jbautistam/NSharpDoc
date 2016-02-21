namespace Bau.Controls.List
{
	partial class ListUpdatable
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
			this.components = new System.ComponentModel.Container();
			this.tlbList = new System.Windows.Forms.ToolStrip();
			this.cmdNew = new System.Windows.Forms.ToolStripButton();
			this.cmdUpdate = new System.Windows.Forms.ToolStripButton();
			this.cmdDelete = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.cmdUp = new System.Windows.Forms.ToolStripButton();
			this.cmdDown = new System.Windows.Forms.ToolStripButton();
			this.lswItems = new Bau.Controls.List.ListViewExtended();
			this.mnuList = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.mnuNew = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuUpdate = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuDelete = new System.Windows.Forms.ToolStripMenuItem();
			this.imlImages = new System.Windows.Forms.ImageList(this.components);
			this.tlbList.SuspendLayout();
			this.mnuList.SuspendLayout();
			this.SuspendLayout();
			// 
			// tlbList
			// 
			this.tlbList.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.tlbList.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmdNew,
            this.cmdUpdate,
            this.cmdDelete,
            this.toolStripSeparator1,
            this.cmdUp,
            this.cmdDown});
			this.tlbList.Location = new System.Drawing.Point(0, 0);
			this.tlbList.Name = "tlbList";
			this.tlbList.Size = new System.Drawing.Size(389, 25);
			this.tlbList.TabIndex = 0;
			this.tlbList.Text = "toolStrip1";
			// 
			// cmdNew
			// 
			this.cmdNew.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.cmdNew.Image = global::Bau.Controls.Properties.Resources.New;
			this.cmdNew.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.cmdNew.Name = "cmdNew";
			this.cmdNew.Size = new System.Drawing.Size(23, 22);
			this.cmdNew.Text = "Nuevo";
			this.cmdNew.ToolTipText = "Nuevo";
			this.cmdNew.Click += new System.EventHandler(this.cmdNew_Click);
			// 
			// cmdUpdate
			// 
			this.cmdUpdate.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.cmdUpdate.Image = global::Bau.Controls.Properties.Resources.Update;
			this.cmdUpdate.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.cmdUpdate.Name = "cmdUpdate";
			this.cmdUpdate.Size = new System.Drawing.Size(23, 22);
			this.cmdUpdate.Text = "Modificar";
			this.cmdUpdate.ToolTipText = "Modificar";
			this.cmdUpdate.Click += new System.EventHandler(this.cmdUpdate_Click);
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
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
			// 
			// cmdUp
			// 
			this.cmdUp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.cmdUp.Image = global::Bau.Controls.Properties.Resources.ArrowUp;
			this.cmdUp.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.cmdUp.Name = "cmdUp";
			this.cmdUp.Size = new System.Drawing.Size(23, 22);
			this.cmdUp.Text = "Mover arriba";
			this.cmdUp.ToolTipText = "Mover arriba";
			this.cmdUp.Visible = false;
			this.cmdUp.Click += new System.EventHandler(this.cmdUp_Click);
			// 
			// cmdDown
			// 
			this.cmdDown.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.cmdDown.Image = global::Bau.Controls.Properties.Resources.ArrowDown;
			this.cmdDown.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.cmdDown.Name = "cmdDown";
			this.cmdDown.Size = new System.Drawing.Size(23, 22);
			this.cmdDown.Text = "Mover abajo";
			this.cmdDown.Visible = false;
			this.cmdDown.Click += new System.EventHandler(this.cmdDown_Click);
			// 
			// lswItems
			// 
			this.lswItems.ContextMenuStrip = this.mnuList;
			this.lswItems.Dock = System.Windows.Forms.DockStyle.Fill;
			this.lswItems.FullRowSelect = true;
			this.lswItems.HideSelection = false;
			this.lswItems.Location = new System.Drawing.Point(0, 25);
			this.lswItems.Name = "lswItems";
			this.lswItems.SelectedKey = null;
			this.lswItems.Size = new System.Drawing.Size(389, 293);
			this.lswItems.SmallImageList = this.imlImages;
			this.lswItems.TabIndex = 1;
			this.lswItems.UseCompatibleStateImageBehavior = false;
			this.lswItems.View = System.Windows.Forms.View.Details;
			this.lswItems.ItemDoubleClick += new Bau.Controls.List.ListViewExtended.ItemDoubleClickHandler(this.lswItems_ItemDoubleClick);
			this.lswItems.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.lswItems_ItemChecked);
			this.lswItems.SelectedIndexChanged += new System.EventHandler(this.lswItems_SelectedIndexChanged);
			// 
			// mnuList
			// 
			this.mnuList.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuNew,
            this.mnuUpdate,
            this.mnuDelete});
			this.mnuList.Name = "contextMenuStrip1";
			this.mnuList.Size = new System.Drawing.Size(124, 70);
			// 
			// mnuNew
			// 
			this.mnuNew.Image = global::Bau.Controls.Properties.Resources.New;
			this.mnuNew.Name = "mnuNew";
			this.mnuNew.Size = new System.Drawing.Size(123, 22);
			this.mnuNew.Text = "&Nuevo";
			this.mnuNew.Click += new System.EventHandler(this.cmdNew_Click);
			// 
			// mnuUpdate
			// 
			this.mnuUpdate.Image = global::Bau.Controls.Properties.Resources.Update;
			this.mnuUpdate.Name = "mnuUpdate";
			this.mnuUpdate.Size = new System.Drawing.Size(123, 22);
			this.mnuUpdate.Text = "&Modificar";
			this.mnuUpdate.Click += new System.EventHandler(this.cmdUpdate_Click);
			// 
			// mnuDelete
			// 
			this.mnuDelete.Image = global::Bau.Controls.Properties.Resources.Remove;
			this.mnuDelete.Name = "mnuDelete";
			this.mnuDelete.Size = new System.Drawing.Size(123, 22);
			this.mnuDelete.Text = "&Borrar";
			this.mnuDelete.Click += new System.EventHandler(this.cmdDelete_Click);
			// 
			// imlImages
			// 
			this.imlImages.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
			this.imlImages.ImageSize = new System.Drawing.Size(16, 16);
			this.imlImages.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// ListUpdatable
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.lswItems);
			this.Controls.Add(this.tlbList);
			this.Name = "ListUpdatable";
			this.Size = new System.Drawing.Size(389, 318);
			this.tlbList.ResumeLayout(false);
			this.tlbList.PerformLayout();
			this.mnuList.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ToolStrip tlbList;
		private System.Windows.Forms.ToolStripButton cmdNew;
		private System.Windows.Forms.ToolStripButton cmdUpdate;
		private System.Windows.Forms.ToolStripButton cmdDelete;
		private Bau.Controls.List.ListViewExtended lswItems;
		private System.Windows.Forms.ContextMenuStrip mnuList;
		private System.Windows.Forms.ToolStripMenuItem mnuNew;
		private System.Windows.Forms.ToolStripMenuItem mnuUpdate;
		private System.Windows.Forms.ToolStripMenuItem mnuDelete;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripButton cmdUp;
		private System.Windows.Forms.ToolStripButton cmdDown;
		private System.Windows.Forms.ImageList imlImages;
	}
}
